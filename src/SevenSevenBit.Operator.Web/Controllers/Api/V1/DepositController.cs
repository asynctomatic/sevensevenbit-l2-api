namespace SevenSevenBit.Operator.Web.Controllers.Api.V1;

using System.Numerics;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SevenSevenBit.Operator.Application.DTOs.Details;
using SevenSevenBit.Operator.Application.Interfaces.Services;
using SevenSevenBit.Operator.Application.UseCases.Deposit.GetSignableDeposit;
using SevenSevenBit.Operator.Domain.Enums;
using SevenSevenBit.Operator.Infrastructure.Identity.Auth;
using SevenSevenBit.Operator.SharedKernel.Extensions;
using SevenSevenBit.Operator.Web.Attributes.Routing;
using SevenSevenBit.Operator.Web.Models.Api;
using StarkEx.Crypto.SDK.Enums;
using Swashbuckle.AspNetCore.Annotations;

[ApiController]
[Authorize]
[ApiRoute("deposits")]
[ApiVersion("1.0")]
public class DepositController : ApiControllerBase
{
    private readonly ILogger<DepositController> logger;
    private readonly IAssetService assetService;
    private readonly IUsersService userService;
    private readonly IVaultService vaultService;
    private readonly IMediator mediator;

    public DepositController(
        ILogger<DepositController> logger,
        IAssetService assetService,
        IUsersService userService,
        IVaultService vaultService,
        IMediator mediator)
    {
        this.logger = logger;
        this.assetService = assetService;
        this.userService = userService;
        this.vaultService = vaultService;
        this.mediator = mediator;
    }

    [HttpPost("signable")]
    [Authorize(Policies.ReadWriteVaults)]
    [SwaggerOperation(
        Summary = "Returns the deposit details for a given asset.",
        Description = "This endpoint returns the deposit details for a given user, asset, and data availability mode.",
        OperationId = "GetSignableDeposit",
        Tags = new[] { "Deposit" })]
    [SwaggerResponse(StatusCodes.Status200OK, "Returns the deposit details.", typeof(DepositDetailsDto))]
    public async Task<ActionResult<DepositDetailsDto>> GetSignableDepositAsync(
        [FromBody, SwaggerRequestBody("The signable deposit request.", Required = true)] SignableDepositModel model,
        CancellationToken cancellationToken)
    {
        var user = await userService.GetUserAsync(model.UserId!.Value, cancellationToken);
        if (user is null)
        {
            ProblemDetails problemDetails = new()
            {
                Title = "User not found",
                Detail = $"User {model.UserId} not found",
                Status = StatusCodes.Status404NotFound,
                Type = ((int)ErrorCodes.UserIdNotFound).ToString(),
                Instance = HttpContext.Request.Path,
            };

            logger.LogWarning("User {UserId} not found", model.UserId);

            return NotFound(problemDetails);
        }

        var asset = await assetService.GetAssetAsync(model.AssetId!.Value, cancellationToken);
        if (asset is null)
        {
            ProblemDetails problemDetails = new()
            {
                Title = "Asset not found",
                Detail = $"Asset {model.AssetId} not found",
                Status = StatusCodes.Status404NotFound,
                Type = ((int)ErrorCodes.AssetNotRegistered).ToString(),
                Instance = HttpContext.Request.Path,
            };

            return NotFound(problemDetails);
        }

        if (!asset.Enabled)
        {
            ProblemDetails problemDetails = new()
            {
                Title = "Asset disabled",
                Detail = $"Asset {model.AssetId} is disabled",
                Status = StatusCodes.Status400BadRequest,
                Type = ((int)ErrorCodes.AssetDisabled).ToString(),
                Instance = HttpContext.Request.Path,
            };

            logger.LogWarning("Asset {AssetId} is disabled", model.AssetId);

            return BadRequest(problemDetails);
        }

        if (asset.Type.IsMintable())
        {
            ProblemDetails problemDetails = new()
            {
                Title = "Mintable Asset",
                Detail = $"Mintable Asset {model.AssetId} can't be deposited",
                Status = StatusCodes.Status400BadRequest,
                Type = ((int)ErrorCodes.InvalidAssetType).ToString(),
                Instance = HttpContext.Request.Path,
            };

            logger.LogWarning("Mintable Asset {AssetId} can't be deposited", model.AssetId);

            return BadRequest(problemDetails);
        }

        if (asset.Type.HasTokenId() && string.IsNullOrWhiteSpace(model.TokenId))
        {
            ProblemDetails problemDetails = new()
            {
                Title = "TokenId required",
                Detail = $"TokenId is required for Asset {model.AssetId}",
                Status = StatusCodes.Status400BadRequest,
                Type = ((int)ErrorCodes.TokenIdRequired).ToString(),
                Instance = HttpContext.Request.Path,
            };

            return BadRequest(problemDetails);
        }

        if (asset.Type is AssetType.Erc721 && model.Amount != BigInteger.One)
        {
            ProblemDetails problemDetails = new()
            {
                Title = "Invalid amount",
                Detail = $"Invalid amount for ERC721 Asset {model.AssetId} can't be deposited",
                Status = StatusCodes.Status400BadRequest,
                Type = ((int)ErrorCodes.OperationAmountInvalid).ToString(),
                Instance = HttpContext.Request.Path,
            };

            logger.LogWarning("Invalid amount for ERC721 Asset {AssetId} can't be deposited", model.AssetId);

            return BadRequest(problemDetails);
        }

        if (!model.Amount.IsQuantizableBy(asset.Quantum))
        {
            ProblemDetails problemDetails = new()
            {
                Title = "Invalid amount",
                Detail = $"Asset {model.AssetId} is not quantizable by {model.Amount}",
                Status = StatusCodes.Status400BadRequest,
                Type = ((int)ErrorCodes.OperationAmountUnquantizable).ToString(),
                Instance = HttpContext.Request.Path,
            };

            logger.LogWarning("Asset {AssetId} is not quantizable by {Amount}", model.AssetId, model.Amount);

            return BadRequest(problemDetails);
        }

        var vault = vaultService.GetVault(
            user, asset, model.TokenId);

        // If vault is null, allocate a new vault
        vault ??= await vaultService.AllocateVaultAsync(
            user,
            asset,
            cancellationToken,
            model.TokenId);

        var command = new GetSignableDepositCommand(user, asset, vault, model.Amount);
        var result = await mediator.Send(command, cancellationToken);

        return result.Match<ActionResult<DepositDetailsDto>>(
            signableDeposit =>
        {
            var depositDetailsDto = new DepositDetailsDto
            {
                Metadata = new DepositDetailsMetadata
                {
                    Nonce = signableDeposit.Nonce,
                    GasLimit = signableDeposit.GasLimit,
                    MaxPriorityFeePerGas = signableDeposit.MaxPriorityFeePerGas,
                    MaxFeePerGas = signableDeposit.MaxFeePerGas,
                    To = signableDeposit.To,
                    Value = signableDeposit.Value,
                    Data = signableDeposit.Data,
                },
                Signable = "0x0",   // TODO
            };

            return Ok(depositDetailsDto);
        },
            _ => StatusCode(StatusCodes.Status500InternalServerError));
    }
}