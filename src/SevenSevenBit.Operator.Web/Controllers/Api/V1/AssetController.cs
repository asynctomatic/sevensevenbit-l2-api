namespace SevenSevenBit.Operator.Web.Controllers.Api.V1;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SevenSevenBit.Operator.Application.DTOs.Entities;
using SevenSevenBit.Operator.Application.DTOs.Pagination;
using SevenSevenBit.Operator.Application.Interfaces.Services;
using SevenSevenBit.Operator.Domain.Enums;
using SevenSevenBit.Operator.Infrastructure.Identity.Auth;
using SevenSevenBit.Operator.SharedKernel.Extensions;
using SevenSevenBit.Operator.Web.Attributes.Routing;
using SevenSevenBit.Operator.Web.Models.Admin;
using SevenSevenBit.Operator.Web.Models.Api;
using Swashbuckle.AspNetCore.Annotations;

[ApiController]
[Authorize]
[ApiRoute("assets")]
[ApiVersion("1.0")]
public class AssetController : ApiControllerBase
{
    private readonly ILogger<AssetController> logger;
    private readonly IAssetService assetService;
    private readonly IEthereumService ethereumService;

    public AssetController(
        ILogger<AssetController> logger,
        IEthereumService ethereumService,
        IAssetService assetService)
    {
        this.logger = logger;
        this.assetService = assetService;
        this.ethereumService = ethereumService;
    }

    [HttpPost]
    [Authorize(Policies.WriteAssets)]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<AssetDto>> Index(
        [FromBody] RegisterAssetModel registerAssetModel,
        CancellationToken cancellationToken)
    {
        if (registerAssetModel.Type!.Value.IsAToken() &&
            !await ethereumService.IsAddressASmartContractAsync(registerAssetModel.Address))
        {
            ProblemDetails problemDetails = new()
            {
                Title = "Asset address is not a Smart Contract",
                Detail = $"Asset address {registerAssetModel.Address} is not a Smart Contract.",
                Status = StatusCodes.Status400BadRequest,
                Type = ((int)ErrorCodes.AssetAddressIsNotAContract).ToString(),
                Instance = HttpContext.Request.Path,
            };

            logger.LogWarning(
                "Asset address {Address} is not a Smart Contract",
                registerAssetModel.Address);

            return BadRequest(problemDetails);
        }

        var isAssetRegistered = await assetService.IsAssetRegisteredAsync(
            registerAssetModel.Type.Value,
            registerAssetModel.Address,
            registerAssetModel.Quantum);

        if (isAssetRegistered)
        {
            ProblemDetails problemDetails = new()
            {
                Title = "Asset already registered",
                Detail = $"Asset {registerAssetModel.Address} is already registered.",
                Status = StatusCodes.Status400BadRequest,
                Type = ((int)ErrorCodes.AssetAlreadyRegistered).ToString(),
                Instance = HttpContext.Request.Path,
            };

            logger.LogWarning(
                "Asset {Address} is already registered on the Db",
                registerAssetModel.Address);

            return BadRequest(problemDetails);
        }

        var newAsset = await assetService.RegisterAssetAsync(
            registerAssetModel.Type.Value,
            registerAssetModel.Address,
            registerAssetModel.Name,
            registerAssetModel.Symbol,
            registerAssetModel.Quantum,
            cancellationToken);

        return ApiCreated($"assets/{newAsset.Id}", new AssetDto(newAsset));
    }

    [HttpGet]
    [Authorize(Policies.ReadAssets)]
    [SwaggerOperation(
        Summary = "Get All Assets",
        Description = "This endpoint fetches all assets in the system, with support for filters and pagination.",
        OperationId = "GetAllAssets",
        Tags = new[] { "Asset" })]
    [SwaggerResponse(StatusCodes.Status200OK, "Returns all assets in the system (paginated).", typeof(PaginatedResponseDto<AssetDto>))]
    public async Task<ActionResult<PaginatedResponseDto<AssetDto>>> Index(
            [FromQuery] GetAssetsQueryModel queryModel,
            CancellationToken cancellationToken)
    {
        var assets = await assetService.GetAssetsAsync(
            new(queryModel.PageNumber, queryModel.PageSize),
            queryModel.AssetType,
            null,
            cancellationToken);

        return Ok(assets);
    }

    [HttpGet("{assetId:guid:required}")]
    [Authorize(Policies.ReadAssets)]
    [SwaggerOperation(
        Summary = "Get Asset",
        Description = "This endpoint fetches a specific asset by ID.",
        OperationId = "GetAsset",
        Tags = new[] { "Asset" })]
    [SwaggerResponse(StatusCodes.Status200OK, "Returns an asset in the system.", typeof(AssetDto))]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Not Found.", typeof(ProblemDetails))]
    public async Task<ActionResult<AssetDto>> Index(
        [FromRoute, SwaggerParameter("The asset ID.", Required = true)] Guid assetId,
        CancellationToken cancellationToken)
    {
        var asset = await assetService.GetAssetAsync(assetId, cancellationToken);

        if (asset is null)
        {
            ProblemDetails problemDetails = new()
            {
                Title = "Asset not found",
                Detail = $"Asset {assetId} does not exist",
                Status = StatusCodes.Status404NotFound,
                Type = ((int)ErrorCodes.AssetDisabled).ToString(),
                Instance = HttpContext.Request.Path,
            };

            return NotFound(problemDetails);
        }

        return Ok(new AssetDto(asset));
    }
}