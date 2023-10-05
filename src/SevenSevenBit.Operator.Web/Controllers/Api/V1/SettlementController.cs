namespace SevenSevenBit.Operator.Web.Controllers.Api.V1;

using System.Numerics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SevenSevenBit.Operator.Application.DTOs.Entities;
using SevenSevenBit.Operator.Application.DTOs.Internal.Settlement;
using SevenSevenBit.Operator.Application.Interfaces.Services;
using SevenSevenBit.Operator.Application.Interfaces.Services.Signatures;
using SevenSevenBit.Operator.Application.Interfaces.Services.StarkExServices;
using SevenSevenBit.Operator.Domain.Entities;
using SevenSevenBit.Operator.Domain.Enums;
using SevenSevenBit.Operator.Domain.ValueObjects;
using SevenSevenBit.Operator.Infrastructure.Identity.Auth;
using SevenSevenBit.Operator.Web.Attributes.Routing;
using SevenSevenBit.Operator.Web.Models.Api.Settlement;
using Swashbuckle.AspNetCore.Annotations;

[ApiController]
[Authorize]
[ApiRoute("settlements")]
[ApiVersion("1.0")]
public class SettlementController : ApiControllerBase
{
    private readonly ILogger<SettlementController> logger;

    private readonly IVaultService vaultService;
    private readonly ISettlementService settlementService;

    private readonly IStarkExEncodingService starkExEncodingService;
    private readonly IStarkExSignatureService starkExSignatureService;

    public SettlementController(
        ILogger<SettlementController> logger,
        IVaultService vaultService,
        ISettlementService settlementService,
        IStarkExEncodingService starkExEncodingService,
        IStarkExSignatureService starkExSignatureService)
    {
        this.logger = logger;

        this.vaultService = vaultService;
        this.settlementService = settlementService;

        this.starkExEncodingService = starkExEncodingService;
        this.starkExSignatureService = starkExSignatureService;
    }

    [HttpPost]
    [Authorize(Policies.WriteSettlements)]
    [SwaggerOperation(
        Summary = "Submit Settlement",
        Description = "This endpoint submits an order settlement.",
        OperationId = "SubmitSettlement",
        Tags = new[] { "Settlement" })]
    [SwaggerResponse(StatusCodes.Status200OK, "Returns the vaults updated by the settlement operation.", typeof(IEnumerable<VaultDto>))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "The settlement request was invalid.", typeof(ProblemDetails))]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Not Found.", typeof(ProblemDetails))]
    public async Task<ActionResult<IEnumerable<VaultDto>>> SubmitSettlementAsync(
        [FromBody, SwaggerRequestBody("The settlement request.", Required = true)]
        SubmitSettlementModel submitSettlementModel,
        CancellationToken cancellationToken)
    {
        var vaultsExistsResult = await AllVaultsExists(submitSettlementModel, cancellationToken);
        if (vaultsExistsResult != null)
        {
            return vaultsExistsResult;
        }

        var orderASellVault = await vaultService.GetVaultAsync(submitSettlementModel.OrderA.SellVaultId, cancellationToken);
        var orderABuyVault = await vaultService.GetVaultAsync(submitSettlementModel.OrderA.BuyVaultId, cancellationToken);
        var orderBSellVault = await vaultService.GetVaultAsync(submitSettlementModel.OrderB.SellVaultId, cancellationToken);
        var orderBBuyVault = await vaultService.GetVaultAsync(submitSettlementModel.OrderB.BuyVaultId, cancellationToken);
        var orderAFeeDestinationVault = submitSettlementModel.SettlementInfo?.OrderAFeeDestinationVaultId is null
            ? null
            : await vaultService.GetVaultAsync(submitSettlementModel.SettlementInfo.OrderAFeeDestinationVaultId.Value, cancellationToken);
        var orderBFeeDestinationVault = submitSettlementModel.SettlementInfo?.OrderBFeeDestinationVaultId is null
            ? null
            : await vaultService.GetVaultAsync(submitSettlementModel.SettlementInfo.OrderBFeeDestinationVaultId.Value, cancellationToken);
        var orderAFeeVault = submitSettlementModel.OrderA.FeeVaultId.HasValue
            ? await vaultService.GetVaultAsync(submitSettlementModel.OrderA.FeeVaultId.Value, cancellationToken)
            : null;
        var orderBFeeVault = submitSettlementModel.OrderB.FeeVaultId.HasValue
            ? await vaultService.GetVaultAsync(submitSettlementModel.OrderB.FeeVaultId.Value, cancellationToken)
            : null;

        // Check for ownership of BuyVault, SellVault and FeeVault
        if ((!orderABuyVault.UserId.Equals(orderASellVault.UserId) || (orderAFeeVault is not null && !orderABuyVault.UserId.Equals(orderAFeeVault.UserId))) ||
            (!orderBBuyVault.UserId.Equals(orderBSellVault.UserId) || (orderBFeeVault is not null && !orderBBuyVault.UserId.Equals(orderBFeeVault.UserId))))
        {
            ProblemDetails problemDetails = new()
            {
                Title = "Conflicting vault owners",
                Detail = "The vaults have conflicting owners.",
                Status = StatusCodes.Status400BadRequest,
                Type = ((int)ErrorCodes.ConflictingVaultOwners).ToString(),
                Instance = HttpContext.Request.Path,
            };

            return BadRequest(problemDetails);
        }

        var areAssetsOk = CheckVaultAssets(
            orderABuyVault,
            orderBSellVault,
            nameof(submitSettlementModel.OrderA.BuyVaultId),
            nameof(submitSettlementModel.OrderB.SellVaultId));

        if (!areAssetsOk)
        {
            ProblemDetails problemDetails = new()
            {
                Title = "Conflicting vault assets",
                Detail = "The vaults have conflicting assets.",
                Status = StatusCodes.Status400BadRequest,
                Type = ((int)ErrorCodes.ConflictingVaultAssets).ToString(),
                Instance = HttpContext.Request.Path,
            };

            return BadRequest(problemDetails);
        }

        if (orderABuyVault.Asset is not { Enabled: true } || orderBBuyVault.Asset is not { Enabled: true })
        {
            ProblemDetails problemDetails = new()
            {
                Title = "Asset disabled",
                Detail = $"Asset {orderABuyVault.AssetId} or {orderBBuyVault.AssetId} disabled",
                Status = StatusCodes.Status400BadRequest,
                Type = ((int)ErrorCodes.AssetDisabled).ToString(),
                Instance = HttpContext.Request.Path,
            };

            logger.LogWarning(
                "Asset {AssetIdA} or {AssetIdB} disabled",
                orderABuyVault.AssetId.ToString(),
                orderBBuyVault.AssetId.ToString());

            return BadRequest(problemDetails);
        }

        var areVaultAssetsOk = CheckVaultAssets(
            orderBBuyVault,
            orderASellVault,
            nameof(submitSettlementModel.OrderB.BuyVaultId),
            nameof(submitSettlementModel.OrderA.SellVaultId));

        if (!areVaultAssetsOk)
        {
            ProblemDetails problemDetails = new()
            {
                Title = "Conflicting vault assets",
                Detail = "The vaults have conflicting assets.",
                Status = StatusCodes.Status400BadRequest,
                Type = ((int)ErrorCodes.ConflictingVaultAssets).ToString(),
                Instance = HttpContext.Request.Path,
            };

            return BadRequest(problemDetails);
        }

        // Fee vaults have enough balance to cover the transfer
        // Fees can be paid with an asset that is not from the settlement (ie. trade USDC/BTC and pay fees in TST)
        var (checkOrderAFeeVaultResult, orderABalanceToCheck) = CheckFeeVaultBalancesAsync(submitSettlementModel.OrderA, orderAFeeVault);
        if (checkOrderAFeeVaultResult != null)
        {
            return checkOrderAFeeVaultResult;
        }

        var (checkOrderBFeeVaultResult, orderBBalanceToCheck) = CheckFeeVaultBalancesAsync(submitSettlementModel.OrderB, orderBFeeVault);
        if (checkOrderBFeeVaultResult != null)
        {
            return checkOrderBFeeVaultResult;
        }

        // Sell Vaults have enough balance to cover the transfer
        var checkOrderASellVaultResult = CheckSellVaultBalances(orderASellVault, orderABalanceToCheck, nameof(submitSettlementModel.OrderA.SellQuantizedAmount));
        if (checkOrderASellVaultResult != null)
        {
            return checkOrderASellVaultResult;
        }

        var checkOrderBSellVaultResult = CheckSellVaultBalances(orderBSellVault, orderBBalanceToCheck, nameof(submitSettlementModel.OrderB.SellQuantizedAmount));
        if (checkOrderBSellVaultResult != null)
        {
            return checkOrderBSellVaultResult;
        }

        // encode transfer for validation
        var orderAMessageHash = starkExEncodingService.EncodeLimitOrder(
            orderASellVault,
            orderABuyVault.VaultChainId,
            orderBSellVault.AssetStarkExId(),
            submitSettlementModel.OrderA.SellQuantizedAmount,
            submitSettlementModel.OrderA.BuyQuantizedAmount,
            submitSettlementModel.OrderA.ExpirationTimestamp,
            submitSettlementModel.OrderA.Nonce,
            orderAFeeVault,
            submitSettlementModel.OrderA.FeeQuantizedAmount);

        // validate signature
        var validOrderASignature = starkExSignatureService.ValidateStarkExSignature(
            orderAMessageHash, orderASellVault!.User.StarkKey, new StarkSignature(submitSettlementModel.OrderA.Signature.R, submitSettlementModel.OrderA.Signature.S));
        if (!validOrderASignature)
        {
            ProblemDetails problemDetails = new()
            {
                Title = "Invalid signature",
                Detail = "The signature is invalid.",
                Status = StatusCodes.Status400BadRequest,
                Type = ((int)ErrorCodes.StarkSignatureInvalid).ToString(),
                Instance = HttpContext.Request.Path,
            };

            logger.LogWarning("Failed to validate signature");

            return BadRequest(problemDetails);
        }

        // encode transfer for validation
        var orderBMessageHash = starkExEncodingService.EncodeLimitOrder(
            orderBSellVault,
            orderBBuyVault.VaultChainId,
            orderASellVault.AssetStarkExId(),
            submitSettlementModel.OrderB.SellQuantizedAmount,
            submitSettlementModel.OrderB.BuyQuantizedAmount,
            submitSettlementModel.OrderB.ExpirationTimestamp,
            submitSettlementModel.OrderB.Nonce,
            orderBFeeVault,
            submitSettlementModel.OrderB.FeeQuantizedAmount);

        // validate signature
        var validOrderBSignature = starkExSignatureService.ValidateStarkExSignature(
            orderBMessageHash, orderBSellVault!.User.StarkKey, new StarkSignature(submitSettlementModel.OrderB.Signature.R, submitSettlementModel.OrderB.Signature.S));
        if (!validOrderBSignature)
        {
            ProblemDetails problemDetails = new()
            {
                Title = "Invalid signature",
                Detail = "The signature is invalid.",
                Status = StatusCodes.Status400BadRequest,
                Type = ((int)ErrorCodes.StarkSignatureInvalid).ToString(),
                Instance = HttpContext.Request.Path,
            };

            logger.LogWarning("Failed to validate signature");

            return BadRequest(problemDetails);
        }

        var settlementData = CreateSettlementDataModel(
            submitSettlementModel,
            orderASellVault,
            orderABuyVault,
            orderAFeeVault,
            orderBSellVault,
            orderBBuyVault,
            orderBFeeVault,
            orderAFeeDestinationVault,
            orderBFeeDestinationVault);

        var vaults = await settlementService.SubmitSettlementAsync(settlementData, cancellationToken);

        return Ok(vaults.Select(v => new VaultDto(v)));
    }

    private static SettlementDataDto CreateSettlementDataModel(
        SubmitSettlementModel submitSettlementModel,
        Vault orderASellVault,
        Vault orderABuyVault,
        Vault orderAFeeVault,
        Vault orderBSellVault,
        Vault orderBBuyVault,
        Vault orderBFeeVault,
        Vault orderAFeeDestinationVault,
        Vault orderBFeeDestinationVault)
    {
        return new SettlementDataDto
        {
            OrderA = new SettlementOrderDataDto
            {
                Nonce = submitSettlementModel.OrderA.Nonce,
                SellVault = orderASellVault,
                BuyVault = orderABuyVault,
                FeeVault = orderAFeeVault,
                ExpirationTimestamp = submitSettlementModel.OrderA.ExpirationTimestamp,
                BuyQuantizedAmount = submitSettlementModel.OrderA.BuyQuantizedAmount,
                SellQuantizedAmount = submitSettlementModel.OrderA.SellQuantizedAmount,
                FeeQuantizedAmount = submitSettlementModel.OrderA.FeeQuantizedAmount,
                SignatureR = submitSettlementModel.OrderA.Signature.R,
                SignatureS = submitSettlementModel.OrderA.Signature.S,
            },
            OrderB = new SettlementOrderDataDto
            {
                Nonce = submitSettlementModel.OrderB.Nonce,
                SellVault = orderBSellVault,
                BuyVault = orderBBuyVault,
                FeeVault = orderBFeeVault,
                ExpirationTimestamp = submitSettlementModel.OrderB.ExpirationTimestamp,
                BuyQuantizedAmount = submitSettlementModel.OrderB.BuyQuantizedAmount,
                SellQuantizedAmount = submitSettlementModel.OrderB.SellQuantizedAmount,
                FeeQuantizedAmount = submitSettlementModel.OrderB.FeeQuantizedAmount,
                SignatureR = submitSettlementModel.OrderB.Signature.R,
                SignatureS = submitSettlementModel.OrderB.Signature.S,
            },
            SettlementInfo = new SettlementInformationDto
            {
                OrderASold = submitSettlementModel.OrderA.SellQuantizedAmount,
                OrderBSold = submitSettlementModel.OrderB.SellQuantizedAmount,
                OrderAInfo = CreateFeeInfoSettlementModel(submitSettlementModel.SettlementInfo?.OrderAFeeAmount, orderAFeeDestinationVault),
                OrderBInfo = CreateFeeInfoSettlementModel(submitSettlementModel.SettlementInfo?.OrderBFeeAmount, orderBFeeDestinationVault),
            },
        };
    }

    private static FeeInfoSettlementDto CreateFeeInfoSettlementModel(BigInteger? feeAmount, Vault orderAFeeDestinationVault)
    {
        if (feeAmount is null || orderAFeeDestinationVault is null)
        {
            return null;
        }

        return new FeeInfoSettlementDto
        {
            FeeTaken = feeAmount.Value,
            FeeDestinationVault = orderAFeeDestinationVault,
        };
    }

    private async Task<ActionResult> AllVaultsExists(SubmitSettlementModel submitSettlementModel, CancellationToken cancellationToken)
    {
        ActionResult result;

        result = await CheckVaultExistence(submitSettlementModel.OrderA.BuyVaultId, "OrderA.BuyVaultId", cancellationToken);
        if (result != null)
        {
            return result;
        }

        result = await CheckVaultExistence(submitSettlementModel.OrderA.SellVaultId, "OrderA.SellVaultId", cancellationToken);
        if (result != null)
        {
            return result;
        }

        result = await CheckVaultExistence(submitSettlementModel.OrderB.BuyVaultId, "OrderB.BuyVaultId", cancellationToken);
        if (result != null)
        {
            return result;
        }

        result = await CheckVaultExistence(submitSettlementModel.OrderB.SellVaultId, "OrderB.SellVaultId", cancellationToken);
        if (result != null)
        {
            return result;
        }

        if (submitSettlementModel.OrderA.FeeVaultId.HasValue)
        {
            result = await CheckVaultExistence(submitSettlementModel.OrderA.FeeVaultId.Value, "OrderA.FeeVaultId", cancellationToken);
            if (result != null)
            {
                return result;
            }
        }

        if (submitSettlementModel.OrderB.FeeVaultId.HasValue)
        {
            result = await CheckVaultExistence(submitSettlementModel.OrderB.FeeVaultId.Value, "OrderB.FeeVaultId", cancellationToken);
            if (result != null)
            {
                return result;
            }
        }

        if (submitSettlementModel.SettlementInfo?.OrderAFeeDestinationVaultId != null)
        {
            result = await CheckVaultExistence(submitSettlementModel.SettlementInfo.OrderAFeeDestinationVaultId!.Value, "SettlementInfo.OrderAFeeDestinationVaultId", cancellationToken);
            if (result != null)
            {
                return result;
            }
        }

        if (submitSettlementModel.SettlementInfo?.OrderBFeeDestinationVaultId != null)
        {
            result = await CheckVaultExistence(submitSettlementModel.SettlementInfo.OrderBFeeDestinationVaultId!.Value, "SettlementInfo.OrderBFeeDestinationVaultId", cancellationToken);
            if (result != null)
            {
                return result;
            }
        }

        return null;
    }

    private async Task<ActionResult> CheckVaultExistence(Guid vaultId, string vaultName, CancellationToken cancellationToken)
    {
        var vaultExists = await vaultService.DoesVaultExistsAsync(vaultId, cancellationToken);
        if (!vaultExists)
        {
            ProblemDetails problemDetails = new()
            {
                Title = "Vault not found",
                Detail = $"Vault {vaultName} not found",
                Status = StatusCodes.Status404NotFound,
                Type = ((int)ErrorCodes.VaultNotRegistered).ToString(),
                Instance = HttpContext.Request.Path,
            };

            logger.LogWarning("{VaultName} {VaultId} not found", vaultName, vaultId);
            return NotFound(problemDetails);
        }

        return null;
    }

    private ActionResult CheckSellVaultBalances(Vault sellVault, BigInteger balanceToCheck, string balancePropertyName)
    {
        if (sellVault!.QuantizedAvailableBalance < balanceToCheck)
        {
            ProblemDetails problemDetails = new()
            {
                Title = "Insufficient balance",
                Detail = $"Insufficient balance in {balancePropertyName}",
                Status = StatusCodes.Status400BadRequest,
                Type = ((int)ErrorCodes.InsufficientBalance).ToString(),
                Instance = HttpContext.Request.Path,
            };

            logger.LogWarning("Insufficient vault balance {Balance}", sellVault.QuantizedAvailableBalance);
            return BadRequest(problemDetails);
        }

        return null;
    }

    private (ActionResult Result, BigInteger Balance) CheckFeeVaultBalancesAsync(
        SettlementOrderModel order,
        Vault feeVault)
    {
        var balanceToCheck = feeVault is null ? order.SellQuantizedAmount : order.FeeQuantizedAmount!.Value;
        if (order.FeeVaultId.HasValue && order.FeeVaultId.Value.Equals(order.SellVaultId))
        {
            balanceToCheck += order.SellQuantizedAmount;
        }

        if (feeVault is not null && feeVault!.QuantizedAvailableBalance < balanceToCheck)
        {
            ProblemDetails problemDetails = new()
            {
                Title = "Insufficient balance",
                Detail = $"Insufficient balance in {nameof(feeVault)}",
                Status = StatusCodes.Status400BadRequest,
                Type = ((int)ErrorCodes.InsufficientBalance).ToString(),
                Instance = HttpContext.Request.Path,
            };

            logger.LogWarning("Insufficient fee vault balance {Balance}", feeVault.QuantizedAvailableBalance);
            return (BadRequest(problemDetails), balanceToCheck);
        }

        return (null, balanceToCheck);
    }

    private bool CheckVaultAssets(
        Vault buyVault,
        Vault sellVault,
        string buyVaultPropertyName,
        string sellVaultPropertyName)
    {
        if (buyVault.QuantizedAccountingBalance > 0 && !buyVault.AssetStarkExId().Equals(sellVault.AssetStarkExId()))
        {
            logger.LogWarning(
                "Vault {BuyVault} asset {BuyVaultAsset} does not match with Vault {SellVault} asset {SellVaultAsset}",
                buyVaultPropertyName,
                buyVault.AssetStarkExId(),
                sellVaultPropertyName,
                sellVault.AssetStarkExId());

            return false;
        }

        if (!buyVault.AssetId.Equals(sellVault.AssetId))
        {
            logger.LogWarning(
                "Vault {BuyVault} asset {BuyVaultAsset} does not match with Vault {SellVault} asset {SellVaultAsset}",
                buyVaultPropertyName,
                buyVault.AssetId,
                sellVaultPropertyName,
                sellVault.AssetId);

            return false;
        }

        return true;
    }
}