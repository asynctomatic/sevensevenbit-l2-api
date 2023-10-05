namespace SevenSevenBit.Operator.Web.Controllers.Api.V1;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SevenSevenBit.Operator.Application.DTOs;
using SevenSevenBit.Operator.Application.DTOs.Details;
using SevenSevenBit.Operator.Application.DTOs.Entities;
using SevenSevenBit.Operator.Application.Interfaces.Services;
using SevenSevenBit.Operator.Application.Interfaces.Services.Signatures;
using SevenSevenBit.Operator.Application.Interfaces.Services.StarkExServices;
using SevenSevenBit.Operator.Domain.Entities;
using SevenSevenBit.Operator.Domain.Enums;
using SevenSevenBit.Operator.Domain.ValueObjects;
using SevenSevenBit.Operator.Infrastructure.Identity.Auth;
using SevenSevenBit.Operator.SharedKernel.Extensions;
using SevenSevenBit.Operator.Web.Attributes.Routing;
using SevenSevenBit.Operator.Web.Models.Api;
using Swashbuckle.AspNetCore.Annotations;

[ApiController]
[Authorize]
[ApiRoute("transfers")]
[ApiVersion("1.0")]
public class TransferController : ApiControllerBase
{
    private readonly ILogger<TransferController> logger;

    private readonly IAssetService assetService;
    private readonly IFeeService feeService;
    private readonly IUsersService userService;
    private readonly IVaultService vaultService;
    private readonly ITransferService transferService;
    private readonly INonceService nonceService;

    private readonly IStarkExEncodingService starkExEncodingService;
    private readonly IStarkExSignatureService starkExSignatureService;
    private readonly ITimestampService timestampService;

    public TransferController(
        ILogger<TransferController> logger,
        IAssetService assetService,
        IFeeService feeService,
        IUsersService userService,
        IVaultService vaultService,
        ITransferService transferService,
        INonceService nonceService,
        IStarkExEncodingService starkExEncodingService,
        IStarkExSignatureService starkExSignatureService,
        ITimestampService timestampService)
    {
        this.logger = logger;

        this.assetService = assetService;
        this.feeService = feeService;
        this.userService = userService;
        this.vaultService = vaultService;
        this.transferService = transferService;
        this.nonceService = nonceService;

        this.starkExEncodingService = starkExEncodingService;
        this.starkExSignatureService = starkExSignatureService;
        this.timestampService = timestampService;
    }

    [HttpPost]
    [Authorize(Policies.WriteTransfers)]
    [SwaggerOperation(
        Summary = "Transfer Asset",
        Description = "This endpoint allows for transferring assets between users.",
        OperationId = "Transfer",
        Tags = new[] { "Transfer" })]
    [SwaggerResponse(StatusCodes.Status200OK, "Returns the vaults updated by the transfer operation.", typeof(IEnumerable<VaultDto>))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "The transfer request was invalid.", typeof(ProblemDetails))]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Not Found.", typeof(ProblemDetails))]
    public async Task<ActionResult<IEnumerable<VaultDto>>> TransferAsync(
        [FromBody, SwaggerRequestBody("The transfer request.", Required = true)] TransferModel transferModel,
        CancellationToken cancellationToken)
    {
        // TODO Shouldn't the model also include a feeAmount prop?
        // Or do we trust that the signable payload was generated with the correct fee amount? This can be hard to debug if it happens
        var asset = await assetService.GetAssetAsync(transferModel.AssetId!.Value, cancellationToken);

        if (asset is not { Enabled: true })
        {
            ProblemDetails problemDetails = new()
            {
                Title = "Asset not found",
                Detail = $"Asset {transferModel.AssetId!.Value} does not exist",
                Status = StatusCodes.Status404NotFound,
                Type = ((int)ErrorCodes.AssetDisabled).ToString(),
                Instance = HttpContext.Request.Path,
            };

            logger.LogWarning("Asset {AssetId} not found", transferModel.AssetId!.Value);

            return NotFound(problemDetails);
        }

        var fromUser = await userService.GetUserAsync(transferModel.FromUserId!.Value, cancellationToken);
        var tokenId = GetTokenIdAndMintingBlob(transferModel.TokenId, asset, out var mintingBlob);

        if (asset.Type.IsMintable() && mintingBlob == null)
        {
            ProblemDetails problemDetails = new()
            {
                Title = "MintingBlob not found",
                Detail = $"MintingBlob {transferModel.TokenId.ConvertToHex()} not found for asset {asset.Id}",
                Status = StatusCodes.Status400BadRequest,
                Type = ((int)ErrorCodes.MintingBlobNotFound).ToString(),
                Instance = HttpContext.Request.Path,
            };

            logger.LogWarning("Minting blob {MintingBlob} not found for asset of type {AssetType}", transferModel.TokenId.ConvertToHex(), asset.Type);

            return BadRequest(problemDetails);
        }

        var fromVault = vaultService.GetVault(fromUser, asset, tokenId, mintingBlob);

        if (fromVault == null)
        {
            ProblemDetails problemDetails = new()
            {
                Title = "Sender vault not found",
                Detail = $"Sender vault {transferModel.FromUserId} not found",
                Status = StatusCodes.Status404NotFound,
                Type = ((int)ErrorCodes.VaultNotRegistered).ToString(),
                Instance = HttpContext.Request.Path,
            };

            logger.LogWarning("Sender vault {VaultId} not found", transferModel.FromUserId);
            return NotFound(problemDetails);
        }

        var toUser = await userService.GetUserAsync(transferModel.ToUserId!.Value, cancellationToken);
        var destinationVault = vaultService.GetVault(toUser, asset, tokenId, mintingBlob);

        if (destinationVault == null)
        {
            ProblemDetails problemDetails = new()
            {
                Title = "Receiver vault not found",
                Detail = $"Receiver vault {transferModel.ToUserId} not found",
                Status = StatusCodes.Status404NotFound,
                Type = ((int)ErrorCodes.VaultNotRegistered).ToString(),
                Instance = HttpContext.Request.Path,
            };

            logger.LogWarning("Receiver vault {VaultId} not found", transferModel.ToUserId);

            return NotFound(problemDetails);
        }

        // Check if both vaults contains the same asset
        // Newly allocated vaults will not have an asset yet, so we skip this check for them
        if (destinationVault.AssetStarkExId() is not null && !fromVault.AssetStarkExId().Equals(destinationVault.AssetStarkExId()))
        {
            ProblemDetails problemDetails = new()
            {
                Title = "Conflicting vault assets",
                Detail = $"Sender vault asset {fromVault.AssetStarkExId()} does not match receiver vault asset {destinationVault.AssetStarkExId()}",
                Status = StatusCodes.Status400BadRequest,
                Type = ((int)ErrorCodes.ConflictingVaultAssets).ToString(),
                Instance = HttpContext.Request.Path,
            };

            logger.LogWarning(
                "Sender vault asset {SenderAsset} does not match receiver vault asset {ReceiverAsset}",
                fromVault.AssetStarkExId(),
                destinationVault.AssetStarkExId());

            return BadRequest(problemDetails);
        }

        if (!transferModel.Amount.IsQuantizableBy(asset.Quantum))
        {
            ProblemDetails problemDetails = new()
            {
                Title = "Amount unquantizable",
                Detail = $"Amount {transferModel.Amount} is unquantizable for asset {asset.Id}",
                Status = StatusCodes.Status400BadRequest,
                Type = ((int)ErrorCodes.OperationAmountUnquantizable).ToString(),
                Instance = HttpContext.Request.Path,
            };

            logger.LogWarning(
                "Transfer details amount {Amount} is unquantizable for Asset quantum {Quantum}",
                transferModel.Amount,
                asset.Quantum);

            return BadRequest(problemDetails);
        }

        var amountQuantized = transferModel.Amount.ToQuantized(asset.Quantum);

        // get transfer fee (default to 0)
        // TODO calculate fee based on quantizedAmount - To be implemented in Fee PR
        var fee = await feeService.GetFeeAsync(
            FeeAction.Transfer,
            fromVault.Asset,
            amountQuantized,
            cancellationToken);

        // check if sender has enough balance to cover the transfer + fees
        if (fromVault.QuantizedAvailableBalance < amountQuantized + fee.QuantizedAmount)
        {
            ProblemDetails problemDetails = new()
            {
                Title = "Insufficient balance",
                Detail = $"Insufficient balance in sender vault {fromVault.Id}",
                Status = StatusCodes.Status400BadRequest,
                Type = ((int)ErrorCodes.InsufficientBalance).ToString(),
                Instance = HttpContext.Request.Path,
            };

            logger.LogWarning("Insufficient vault balance {Balance}", fromVault.QuantizedAvailableBalance);

            return BadRequest(problemDetails);
        }

        // encode transfer for validation
        var messageHash = starkExEncodingService.EncodeTransferWithFees(
            fromVault,
            destinationVault,
            amountQuantized,
            fee.QuantizedAmount,
            transferModel.ExpirationTimestamp,
            transferModel.Nonce);

        // validate signature
        var validSignature = starkExSignatureService.ValidateStarkExSignature(
            messageHash, fromVault.User.StarkKey, new StarkSignature(transferModel.Signature.R, transferModel.Signature.S));

        if (!validSignature)
        {
            ProblemDetails problemDetails = new()
            {
                Title = "Invalid signature",
                Detail = $"Invalid signature for transfer {transferModel}",
                Status = StatusCodes.Status400BadRequest,
                Type = ((int)ErrorCodes.StarkSignatureInvalid).ToString(),
                Instance = HttpContext.Request.Path,
            };

            logger.LogWarning("Failed to validate signature");

            return BadRequest(problemDetails);
        }

        var transactionId = await transferService.TransferAsync(
            fromVault,
            destinationVault,
            amountQuantized,
            fromVault,
            destinationVault,  // TODO should be an operator controlled vault - To be implemented in Fee PR
            fee.QuantizedAmount,
            transferModel.ExpirationTimestamp,
            new StarkSignature(transferModel.Signature.R, transferModel.Signature.S),
            transferModel.Nonce,
            cancellationToken);

        return Ok(new TransactionIdDto(transactionId));
    }

    [HttpGet]
    [Route("signable")]
    [Authorize(Policies.WriteTransfers)]
    [SwaggerOperation(
        Summary = "Get Signable Details",
        Description = "This endpoint allows for fetching details of a transfer to be signed",
        OperationId = "TransferSignable",
        Tags = new[] { "Transfer" })]
    [SwaggerResponse(StatusCodes.Status200OK, "Returns the signable transfer details.", typeof(TransferSignableDto))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "The signable transfer request was invalid.", typeof(ProblemDetails))]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Not Found.", typeof(ProblemDetails))]
    public async Task<ActionResult<TransferSignableDto>> GetTransferSignableAsync(
        [FromQuery, SwaggerRequestBody("The signable transfer details request.", Required = true)] TransferDetailsModel transferDetailsModel,
        CancellationToken cancellationToken)
    {
        var sender = await userService.GetUserAsync(transferDetailsModel.FromUserId!.Value, cancellationToken);

        if (sender == null)
        {
            ProblemDetails problemDetails = new()
            {
                Title = "Sender not found",
                Detail = $"Sender {transferDetailsModel.FromUserId} not found",
                Status = StatusCodes.Status404NotFound,
                Type = ((int)ErrorCodes.UserIdNotFound).ToString(),
                Instance = HttpContext.Request.Path,
            };

            logger.LogWarning("User {UserId} not found", transferDetailsModel.FromUserId);

            return NotFound(problemDetails);
        }

        var receiver = await userService.GetUserAsync(transferDetailsModel.ToUserId!.Value, cancellationToken);

        if (receiver == null)
        {
            ProblemDetails problemDetails = new()
            {
                Title = "Receiver not found",
                Detail = $"Receiver {transferDetailsModel.ToUserId} not found",
                Status = StatusCodes.Status404NotFound,
                Type = ((int)ErrorCodes.UserIdNotFound).ToString(),
                Instance = HttpContext.Request.Path,
            };

            logger.LogWarning("User {UserId} not found", transferDetailsModel.ToUserId);

            return NotFound(problemDetails);
        }

        var asset = await assetService.GetAssetAsync(transferDetailsModel.AssetId!.Value, cancellationToken);

        if (asset is not { Enabled: true })
        {
            ProblemDetails problemDetails = new()
            {
                Title = "Asset not found",
                Detail = $"Asset {transferDetailsModel.AssetId} does not exist",
                Status = StatusCodes.Status404NotFound,
                Type = ((int)ErrorCodes.AssetDisabled).ToString(),
                Instance = HttpContext.Request.Path,
            };

            logger.LogWarning("Asset {AssetId} not found", transferDetailsModel.AssetId);

            return NotFound(problemDetails);
        }

        if (asset.Type.HasTokenId() && transferDetailsModel.TokenId == null)
        {
            ProblemDetails problemDetails = new()
            {
                Title = "TokenId required",
                Detail = $"TokenId required for asset {asset.Id}",
                Status = StatusCodes.Status400BadRequest,
                Type = ((int)ErrorCodes.TokenIdRequired).ToString(),
                Instance = HttpContext.Request.Path,
            };

            logger.LogWarning("Missing tokenId for asset of type {AssetType}", asset.Type);

            return BadRequest(problemDetails);
        }

        if (asset.Type.IsMintable() && transferDetailsModel.TokenId.ConvertToHex() == null)
        {
            ProblemDetails problemDetails = new()
            {
                Title = "MintingBlob required",
                Detail = $"MintingBlob required for asset {asset.Id}",
                Status = StatusCodes.Status400BadRequest,
                Type = ((int)ErrorCodes.MintingBlobRequired).ToString(),
                Instance = HttpContext.Request.Path,
            };

            logger.LogWarning("Missing mintingBlob for asset of type {AssetType}", asset.Type);

            return BadRequest(problemDetails);
        }

        if (!transferDetailsModel.Amount.IsQuantizableBy(asset.Quantum))
        {
            ProblemDetails problemDetails = new()
            {
                Title = "Amount unquantizable",
                Detail = $"Amount {transferDetailsModel.Amount} is unquantizable for asset {asset.Id}",
                Status = StatusCodes.Status400BadRequest,
                Type = ((int)ErrorCodes.OperationAmountUnquantizable).ToString(),
                Instance = HttpContext.Request.Path,
            };

            logger.LogWarning(
                "Transfer details amount {Amount} is unquantizable for Asset quantum {Quantum}",
                transferDetailsModel.Amount,
                asset.Quantum);

            return BadRequest(problemDetails);
        }

        var tokenId = GetTokenIdAndMintingBlob(transferDetailsModel.TokenId, asset, out var mintingBlob);

        if (asset.Type.IsMintable() && mintingBlob == null)
        {
            ProblemDetails problemDetails = new()
            {
                Title = "MintingBlob not found",
                Detail = $"MintingBlob {transferDetailsModel.TokenId.ConvertToHex()} not found for asset {asset.Id}",
                Status = StatusCodes.Status400BadRequest,
                Type = ((int)ErrorCodes.MintingBlobNotFound).ToString(),
                Instance = HttpContext.Request.Path,
            };

            logger.LogWarning("Minting blob {MintingBlob} not found for asset of type {AssetType}", transferDetailsModel.TokenId.ConvertToHex(), asset.Type);

            return BadRequest(problemDetails);
        }

        var senderVault = vaultService.GetVault(
            sender,
            asset,
            tokenId,
            mintingBlob);

        if (senderVault == null)
        {
            ProblemDetails problemDetails = new()
            {
                Title = "Sender vault not found",
                Detail = $"Sender vault not found for asset {asset.Id}",
                Status = StatusCodes.Status404NotFound,
                Type = ((int)ErrorCodes.VaultNotRegistered).ToString(),
                Instance = HttpContext.Request.Path,
            };

            logger.LogWarning("Sender vault not found");

            return NotFound(problemDetails);
        }

        var receiverVault = vaultService.GetVault(
            receiver,
            asset,
            tokenId,
            mintingBlob);

        // Allocate a new vault if receiver does not have a vault for the asset
        receiverVault ??= await vaultService.AllocateVaultAsync(
            receiver,
            asset,
            cancellationToken,
            transferDetailsModel.TokenId);

        // check for transfer into the same vault
        if (senderVault.VaultChainId == receiverVault.VaultChainId)
        {
            ProblemDetails problemDetails = new()
            {
                Title = "Transfer into same vault",
                Detail = $"Transfer into same vault {senderVault.Id}",
                Status = StatusCodes.Status400BadRequest,
                Type = ((int)ErrorCodes.TransferIntoSameVault).ToString(),
                Instance = HttpContext.Request.Path,
            };

            logger.LogWarning("Transfer into same vault {VaultId}", senderVault.Id);

            return BadRequest(problemDetails);
        }

        // get transfer fee (default to 0)
        var amountQuantized = transferDetailsModel.Amount.ToQuantized(asset.Quantum);

        // TODO calculate fee based on quantizedAmount - To be implemented in Fee PR
        var fee = await feeService.GetFeeAsync(
            FeeAction.Transfer,
            senderVault.Asset,
            amountQuantized,
            cancellationToken);

        if (senderVault.QuantizedAvailableBalance < amountQuantized + fee.QuantizedAmount)
        {
            ProblemDetails problemDetails = new()
            {
                Title = "Insufficient balance",
                Detail = $"Insufficient balance {senderVault.QuantizedAvailableBalance}",
                Status = StatusCodes.Status400BadRequest,
                Type = ((int)ErrorCodes.InsufficientBalance).ToString(),
                Instance = HttpContext.Request.Path,
            };

            logger.LogWarning("Insufficient vault balance {Balance}", senderVault.QuantizedAvailableBalance);

            return BadRequest(problemDetails);
        }

        // calculate expiration timestamp
        var expirationTimestamp = timestampService.GetTargetExpirationTimestamp();
        var nonce = nonceService.GetRandomNonce();

        // encode transfer payload
        var messageHash = starkExEncodingService.EncodeTransferWithFees(
            senderVault,
            receiverVault,
            amountQuantized,
            fee.QuantizedAmount,
            expirationTimestamp,
            nonce);

        // We send the amounts quantized in the transfer details so that users can verify the signable payload
        var transferDetails = new TransferSignableDto
        {
            Signable = messageHash,
            ExpirationTimestamp = expirationTimestamp,
            Nonce = nonce,
        };

        return Ok(transferDetails);
    }

    private static string GetTokenIdAndMintingBlob(string requestTokenId, Asset asset, out string mintingBlob)
    {
        string tokenId = null;
        mintingBlob = null;

        if (asset.Type.HasTokenId())
        {
            tokenId = requestTokenId;
        }
        else
        {
            mintingBlob = requestTokenId is null ?
                null :
                asset.MintingBlobs.FirstOrDefault(m => m.MintingBlobHex == requestTokenId.ConvertToHex())?.MintingBlobHex.ToString();
        }

        return tokenId;
    }
}