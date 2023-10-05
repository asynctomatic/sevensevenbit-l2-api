namespace SevenSevenBit.Operator.Web.Controllers.Api.V1;

using System.Numerics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SevenSevenBit.Operator.Application.DTOs;
using SevenSevenBit.Operator.Application.DTOs.Internal;
using SevenSevenBit.Operator.Application.Interfaces.Services;
using SevenSevenBit.Operator.Domain.Entities;
using SevenSevenBit.Operator.Domain.Enums;
using SevenSevenBit.Operator.Infrastructure.Identity.Auth;
using SevenSevenBit.Operator.SharedKernel.Extensions;
using SevenSevenBit.Operator.Web.Attributes.Routing;
using SevenSevenBit.Operator.Web.Models;
using SevenSevenBit.Operator.Web.Models.Api.Mint;
using StarkEx.Crypto.SDK.Enums;
using Swashbuckle.AspNetCore.Annotations;

[ApiController]
[Authorize]
[ApiRoute("mint")]
[ApiVersion("1.0")]
public class MintController : ApiControllerBase
{
    private readonly ILogger<MintController> logger;
    private readonly IAssetService assetService;
    private readonly IMintService mintService;
    private readonly IUsersService userService;

    public MintController(
        ILogger<MintController> logger,
        IAssetService assetService,
        IMintService mintService,
        IUsersService userService)
    {
        this.logger = logger;
        this.assetService = assetService;
        this.mintService = mintService;
        this.userService = userService;
    }

    [HttpPost]
    [Authorize(Policies.MintAssets)]
    [SwaggerOperation(
        Summary = "Mint Assets",
        Description = "This endpoint allows for the minting of fungible and non-fungible assets.",
        OperationId = "MintAssets",
        Tags = new[] { "Mint" })]
    [SwaggerResponse(StatusCodes.Status202Accepted, "Returns the transaction ID that was created by the mint operation.", typeof(TransactionIdDto))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "The mint request was invalid.", typeof(ProblemDetails))]
    public async Task<ActionResult<TransactionIdDto>> IndexAsync(
        [FromBody, SwaggerRequestBody("The assets to mint for each user.", Required = true)]
        BatchMintRequestModel batchMintRequestModel,
        CancellationToken cancellationToken)
    {
        // Select all the user IDs from the request.
        var userIds = batchMintRequestModel.Mints
            .Select(x => x.UserId.Value).Distinct().ToList();

        // Get all the users from the database and validate that they exist.
        var users = (await userService.GetUsersAsync(userIds, cancellationToken)).ToList();
        var userValidation = ValidateUsers(users, userIds);
        if (!userValidation.IsValid)
        {
            return userValidation.Result;
        }

        // Select all the asset IDs from the request.
        var assetIds = batchMintRequestModel.Mints
            .Select(x => x.AssetId.Value).Distinct().ToList();

        var assets = (await assetService.GetAssetsAsync(assetIds, cancellationToken)).ToList();
        var assetValidation = ValidateMintableAssets(batchMintRequestModel, assets, assetIds);

        if (!assetValidation.IsValid)
        {
            return assetValidation.Result;
        }

        var mintableAssets = GetMintAssetsDataDto(batchMintRequestModel, users, assets);

        if (mintableAssets.Count > 1)
        {
            var transactionId = await mintService.MintAssetsAsync(
                mintableAssets,
                cancellationToken);

            return Accepted(new TransactionIdDto(transactionId));
        }

        var mintableAsset = mintableAssets.First();
        var mintTransactionId = await mintService.MintAssetAsync(mintableAsset, cancellationToken);

        return Accepted(new TransactionIdDto(mintTransactionId));
    }

    private static IEnumerable<(Guid AssetId, BigInteger Amount)> GetUnquantizableAmountsPerAssetType(
        BatchMintRequestModel batchMintRequest,
        IEnumerable<Asset> assets)
    {
        var assetsDict = assets.ToDictionary(tenantAsset => tenantAsset.Id, a => a);

        return batchMintRequest.Mints
            .Select(x => (Asset: assetsDict[x.AssetId.Value], x.Amount))
            .Where(assetAmountPair => !assetAmountPair.Amount.IsQuantizableBy(assetAmountPair.Asset.Quantum))
            .Select(assetAmountPair => (assetAmountPair.Asset.Id, assetAmountPair.Amount));
    }

    private static IList<MintAssetDataDto> GetMintAssetsDataDto(
        BatchMintRequestModel batchMintRequest,
        IList<User> users,
        IList<Asset> assets)
    {
        return (from mintRequest in batchMintRequest.Mints
            let user = users.Single(x => x.Id.Equals(mintRequest.UserId))
            let asset = assets.Single(x => x.Id.Equals(mintRequest.AssetId))
            select new MintAssetDataDto
            {
                User = user,
                Asset = asset,
                MintingBlob = asset.Type.HasTokenId() ? mintRequest.TokenId : "0x00",   // TODO: Remove this when we have a proper token id
                QuantizedAmount = mintRequest.Amount.ToQuantized(asset.Quantum),
                DataAvailabilityMode = DataAvailabilityModes.Validium,
            }).ToList();
    }

    private static bool IsInvalidAmount((Asset Asset, BigInteger Amount) pair)
    {
        return pair.Asset.Type switch
        {
            AssetType.MintableErc20 => pair.Amount <= 0,
            AssetType.MintableErc721 => pair.Amount != 1,
            AssetType.MintableErc1155 => pair.Amount <= 0,
            _ => false,
        };
    }

    private List<(Guid AssetId, BigInteger Amount)> GetInvalidAmountsPerAssetType(
        BatchMintRequestModel batchMintRequest,
        IList<Asset> assets)
    {
        // Filter invalid amounts for the respective asset types
        return batchMintRequest.Mints
            .Select(x => (Asset: assets.SingleOrDefault(a => a.Id.Equals(x.AssetId)), x.Amount))
            .Where(IsInvalidAmount)
            .Select(x => (x.Asset.Id, x.Amount))
            .ToList();
    }

    private StateValidationDto ValidateUsers(
        IEnumerable<User> users,
        IEnumerable<Guid> userIds)
    {
        var unregisteredUsers = userIds.
            Except((users ?? Enumerable.Empty<User>()).Select(x => x.Id));

        if (users is not null && !unregisteredUsers.Any())
        {
            return new StateValidationDto();
        }

        ProblemDetails problemDetails = new()
        {
            Title = "User not found",
            Detail = "User not found",
            Status = StatusCodes.Status404NotFound,
            Type = ((int)ErrorCodes.UserIdNotFound).ToString(),
        };

        logger.LogWarning("Users {UserIds} not found", unregisteredUsers);

        return new StateValidationDto(NotFound(problemDetails));
    }

    private StateValidationDto ValidateMintableAssets(
        BatchMintRequestModel batchMintRequest,
        IList<Asset> assets,
        IEnumerable<Guid> assetsIds)
    {
        // Check for mint requests with a missing tokenId.
        var missingTokenIds =
            (from asset in assets
                where asset.Type.HasTokenId()
                join mint in batchMintRequest.Mints on asset.Id equals mint.AssetId into gj
                from subMint in gj.DefaultIfEmpty()
                where subMint == null || string.IsNullOrEmpty(subMint.TokenId)
                select asset.Id).ToList();
        if (missingTokenIds.Any())
        {
            ProblemDetails problemDetails = new()
            {
                Title = "TokenId required",
                Detail = $"TokenId required for asset {missingTokenIds.First()}",
                Status = StatusCodes.Status400BadRequest,
                Type = ((int)ErrorCodes.TokenIdRequired).ToString(),
                Instance = HttpContext.Request.Path,
            };

            logger.LogWarning("Missing tokenId for asset of type {AssetType}", missingTokenIds.First());

            return new StateValidationDto(BadRequest(problemDetails));
        }

        // Check for assets that are disabled in the system.
        var disabledAssets = assetsIds
            .Except(assets
                .Where(x => x.Enabled)
                .Select(x => x.Id));
        if (disabledAssets.Any())
        {
            ProblemDetails problemDetails = new()
            {
                Title = "Asset disabled",
                Detail = "Asset disabled",
                Status = StatusCodes.Status404NotFound,
                Type = ((int)ErrorCodes.AssetDisabled).ToString(),
            };

            logger.LogWarning(
                "Assets {BlacklistedAssets} are disabled",
                disabledAssets);

            return new StateValidationDto(NotFound(problemDetails));
        }

        // Check for assets that are not mintable.
        var invalidAssetTypes = assets
            .Where(x => !x.Type.IsMintable())
            .Select(x => x.Id);
        if (invalidAssetTypes.Any())
        {
            ProblemDetails problemDetails = new()
            {
                Title = "Asset not mintable",
                Detail = "Asset not mintable",
                Status = StatusCodes.Status400BadRequest,
                Type = ((int)ErrorCodes.InvalidAssetType).ToString(),
                Instance = HttpContext.Request.Path,
            };

            logger.LogWarning(
                "Assets {Assets} are not mintable",
                invalidAssetTypes);

            return new StateValidationDto(BadRequest(problemDetails));
        }

        // Check for invalid mint amounts.
        var invalidAmountsPerAssetType = GetInvalidAmountsPerAssetType(batchMintRequest, assets);
        if (invalidAmountsPerAssetType.Any())
        {
            ProblemDetails problemDetails = new()
            {
                Title = "Invalid mint amount",
                Detail = "Invalid mint amount",
                Status = StatusCodes.Status400BadRequest,
                Type = ((int)ErrorCodes.OperationAmountInvalid).ToString(),
                Instance = HttpContext.Request.Path,
            };

            logger.LogWarning(
                "Mint amounts {Amounts} for {AssetId} are invalid",
                invalidAmountsPerAssetType.Select(x => x.Amount),
                invalidAmountsPerAssetType.Select(x => x.AssetId));

            return new StateValidationDto(BadRequest(problemDetails));
        }

        // Check for unquantizable mint amounts.
        var unquantizableAmountsPerAssetType = GetUnquantizableAmountsPerAssetType(batchMintRequest, assets).ToList();
        if (unquantizableAmountsPerAssetType.Any())
        {
            ProblemDetails problemDetails = new()
            {
                Title = "Unquantizable mint amount",
                Detail = "Unquantizable mint amount",
                Status = StatusCodes.Status400BadRequest,
                Type = ((int)ErrorCodes.OperationAmountUnquantizable).ToString(),
                Instance = HttpContext.Request.Path,
            };

            logger.LogWarning(
                "Mint amounts {Amounts} for {AssetId} are unquantizable",
                unquantizableAmountsPerAssetType.Select(x => x.Amount),
                unquantizableAmountsPerAssetType.Select(x => x.AssetId));

            return new StateValidationDto(BadRequest(problemDetails));
        }

        // MintingBlob has to be unique for MERC721
        var assetsDict = assets.ToDictionary(tenantAsset => tenantAsset.Id, a => a);
        foreach (var mintDataModel in batchMintRequest.Mints)
        {
            var asset = assetsDict[mintDataModel.AssetId!.Value];

            if (asset.Type == AssetType.MintableErc721 &&
                asset.MintingBlobs is not null &&
                asset.MintingBlobs.Any(assetMintingBlob => assetMintingBlob.MintingBlobHex == mintDataModel.TokenId))
            {
                ProblemDetails problemDetails = new()
                {
                    Title = "Existing minting blob",
                    Detail = "Existing minting blob",
                    Status = StatusCodes.Status400BadRequest,
                    Type = ((int)ErrorCodes.ExistingMintingBlob).ToString(),
                    Instance = HttpContext.Request.Path,
                };

                logger.LogWarning(
                    "Minting blob {MintingBlob} for asset {AssetId} already exists",
                    mintDataModel.TokenId,
                    mintDataModel.AssetId);

                return new StateValidationDto(BadRequest(problemDetails));
            }
        }

        return new StateValidationDto();
    }
}