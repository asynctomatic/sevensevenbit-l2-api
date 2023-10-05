namespace SevenSevenBit.Operator.Web.Controllers.Api.V1;

using System.Numerics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SevenSevenBit.Operator.Application.DTOs.Details;
using SevenSevenBit.Operator.Application.DTOs.Entities;
using SevenSevenBit.Operator.Application.Interfaces.Services;
using SevenSevenBit.Operator.Domain.Enums;
using SevenSevenBit.Operator.Infrastructure.Identity.Auth;
using SevenSevenBit.Operator.SharedKernel.Extensions;
using SevenSevenBit.Operator.Web.Attributes.Routing;
using SevenSevenBit.Operator.Web.Models.Api;
using StarkEx.Crypto.SDK.Enums;
using Swashbuckle.AspNetCore.Annotations;

[ApiController]
[Authorize]
[ApiRoute("vaults")]
[ApiVersion("1.0")]
public class VaultController : ApiControllerBase
{
    private readonly ILogger<VaultController> logger;

    private readonly IVaultService vaultService;
    private readonly IWithdrawService withdrawService;

    public VaultController(
        ILogger<VaultController> logger,
        IVaultService vaultService,
        IWithdrawService withdrawService)
    {
        this.logger = logger;

        this.vaultService = vaultService;
        this.withdrawService = withdrawService;
    }

    [HttpPost]
    [Route("withdraw")]
    [Authorize(Policies.WriteVaults)]
    [SwaggerOperation(
        Summary = "Withdraw Asset",
        Description = "This endpoint allows for withdrawing assets from SevenSevenBit.",
        OperationId = "Withdraw",
        Tags = new[] { "Withdraw" })]
    [SwaggerResponse(StatusCodes.Status200OK, "Returns the details of the withdraw operation.", typeof(WithdrawDetailsDto))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "The withdraw request was invalid.", typeof(ProblemDetails))]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Not Found.", typeof(ProblemDetails))]
    public async Task<ActionResult<VaultDto>> WithdrawAsync(
        [FromBody, SwaggerRequestBody("The withdraw request.", Required = true)] WithdrawModel model,
        CancellationToken cancellationToken)
    {
        var vault = await vaultService.GetVaultAsync(model.VaultId!.Value, cancellationToken);

        if (vault is null)
        {
            ProblemDetails problemDetails = new()
            {
                Title = "Vault not found",
                Detail = $"Vault {model.VaultId} not found",
                Status = StatusCodes.Status404NotFound,
                Type = ((int)ErrorCodes.VaultNotRegistered).ToString(),
                Instance = HttpContext.Request.Path,
            };

            logger.LogWarning("Vault {VaultId} not found", model.VaultId);

            return NotFound(problemDetails);
        }

        if (!model.Amount.IsQuantizableBy(vault.Asset.Quantum))
        {
            ProblemDetails problemDetails = new()
            {
                Title = "Invalid amount",
                Detail = $"Withdraw amount {model.Amount} is unquantizable for Asset quantum {vault.Asset.Quantum}",
                Status = StatusCodes.Status400BadRequest,
                Type = ((int)ErrorCodes.OperationAmountUnquantizable).ToString(),
                Instance = HttpContext.Request.Path,
            };

            logger.LogWarning(
                "Withdraw amount {Amount} is unquantizable for Asset quantum {Quantum}",
                model.Amount,
                vault.Asset.Quantum);

            return BadRequest(problemDetails);
        }

        if (vault.Asset.Type is AssetType.Erc721 && model.Amount != BigInteger.One)
        {
            ProblemDetails problemDetails = new()
            {
                Title = "Invalid amount",
                Detail = $"Withdraw amount {model.Amount} is invalid for ERC721 Asset",
                Status = StatusCodes.Status400BadRequest,
                Type = ((int)ErrorCodes.OperationAmountInvalid).ToString(),
                Instance = HttpContext.Request.Path,
            };

            logger.LogWarning("Invalid amount for ERC721 Asset can't be withdrawn for Vault {VaultId}", model.VaultId);

            return BadRequest(problemDetails);
        }

        var quantizedAmount = model.Amount.ToQuantized(vault.Asset.Quantum);

        if (quantizedAmount > vault.QuantizedAvailableBalance)
        {
            ProblemDetails problemDetails = new()
            {
                Title = "Insufficient balance",
                Detail = $"Insufficient balance {vault.QuantizedAvailableBalance} for withdraw amount {quantizedAmount}",
                Status = StatusCodes.Status400BadRequest,
                Type = ((int)ErrorCodes.InsufficientBalance).ToString(),
                Instance = HttpContext.Request.Path,
            };

            logger.LogWarning("Insufficient vault balance {Balance}", vault.QuantizedAvailableBalance);

            return BadRequest(problemDetails);
        }

        await withdrawService.WithdrawAsync(vault, quantizedAmount, cancellationToken);

        var vaultDto = new VaultDto(vault);

        var withdrawDetails = new WithdrawDetailsDto
        {
            Vault = vaultDto,
            WithdrawFunction = vault.Asset.Type.GetWithdrawFunctionName(),
            StarkKey = vault.User.StarkKey,
            AssetType = vault.Asset.StarkExType,
            TokenId = vault.TokenId,
            MintingBlob = vault.BaseMintingBlob?.MintingBlobHex,
        };

        return Ok(withdrawDetails);
    }
}