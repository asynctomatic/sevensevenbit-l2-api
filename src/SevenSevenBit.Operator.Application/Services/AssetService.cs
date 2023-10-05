namespace SevenSevenBit.Operator.Application.Services;

using System.Numerics;
using SevenSevenBit.Operator.Application.Common.Interfaces;
using SevenSevenBit.Operator.Application.DTOs.Entities;
using SevenSevenBit.Operator.Application.DTOs.Pagination;
using SevenSevenBit.Operator.Application.Helpers;
using SevenSevenBit.Operator.Application.Interfaces.Services;
using SevenSevenBit.Operator.Application.Interfaces.Services.StarkExServices;
using SevenSevenBit.Operator.Domain.Entities;
using SevenSevenBit.Operator.Domain.Enums;
using SevenSevenBit.Operator.Domain.ValueObjects;
using SevenSevenBit.Operator.SharedKernel.Extensions;
using StarkEx.Crypto.SDK.Encoding;
using StarkEx.Crypto.SDK.Enums;

public class AssetService : IAssetService
{
    private readonly IUnitOfWork unitOfWork;
    private readonly IStarkExContractService starkExContractService;

    public AssetService(
        IUnitOfWork unitOfWork,
        IStarkExContractService starkExContractService)
    {
        this.unitOfWork = unitOfWork;
        this.starkExContractService = starkExContractService;
    }

    public async Task<bool> IsAssetRegisteredAsync(
        AssetType type,
        BlockchainAddress address,
        BigInteger quantum)
    {
        var assetType = AssetEncoder
            .GetAssetType(type, quantum: quantum.ToBouncyCastle(), address: address);

        return await unitOfWork.Repository<Asset>().AnyAsync(asset => assetType == asset.StarkExType);
    }

    public async Task<Asset> RegisterAssetAsync(
        AssetType type,
        BlockchainAddress address,
        string name,
        string symbol,
        BigInteger quantum,
        CancellationToken cancellationToken)
    {
        // compute StarkEx asset identifiers
        var assetType = AssetEncoder
            .GetAssetType(type, quantum: quantum.ToBouncyCastle(), address: address);

        var isAssetRegisteredOnChain = await IsAssetRegisteredOnChainAsync(type, address, quantum);
        if (!isAssetRegisteredOnChain)
        {
            // TODO: thrown exception
        }

        var asset = new Asset
        {
            StarkExType = assetType,
            Type = type,
            Address = address,
            Name = name,
            Symbol = symbol,
            Quantum = quantum,
            Enabled = true,
        };

        await unitOfWork.Repository<Asset>().InsertAsync(asset, cancellationToken);
        await unitOfWork.SaveAsync(cancellationToken);

        return asset;
    }

    public async Task<Asset> GetAssetAsync(
        Guid assetId,
        CancellationToken cancellationToken)
    {
        return await unitOfWork.Repository<Asset>().GetByIdAsync(assetId, cancellationToken);
    }

    public async Task<IEnumerable<Asset>> GetAssetsAsync(CancellationToken cancellationToken)
    {
        return await unitOfWork.Repository<Asset>().GetAsync(cancellationToken: cancellationToken);
    }

    public async Task<IEnumerable<Asset>> GetAssetsAsync(IEnumerable<Guid> assetIds, CancellationToken cancellationToken)
    {
        return await unitOfWork.Repository<Asset>().GetAsync(
            filter: asset => assetIds.Contains(asset.Id),
            cancellationToken: cancellationToken);
    }

    public async Task<PaginatedResponseDto<AssetDto>> GetAssetsAsync(
        Paging paging,
        AssetType? assetType,
        string sort = null,
        CancellationToken cancellationToken = default)
    {
        var assetFilter = QueryBuilder.GetFilter(
            (nameof(Asset.Type), typeof(AssetType), assetType, FilterOptions.IsEqualTo));

        sort ??= $"{nameof(Asset.Name)} asc";

        return await unitOfWork.Repository<Asset>().GetProjectedPaginatedAsync(
            paging,
            asset => new AssetDto(asset),
            filter: assetFilter,
            orderBy: sort,
            cancellationToken: cancellationToken);
    }

    private static BigInteger GetDeploymentId()
    {
        // Set the buffer to 31 bytes (< 2^256) to ensure the value is always in range.
        var bytes = new byte[31];

        // Generate a random number.
        new Random().NextBytes(bytes);

        // Set the most significant bit to 0 to ensure the number is positive.
        bytes[^1] &= 0x7F;

        // Return the result as a BigInteger.
        return new BigInteger(bytes);
    }

    private async Task<bool> IsAssetRegisteredOnChainAsync(
        AssetType type,
        string address,
        BigInteger quantum)
    {
        var assetType = AssetEncoder
            .GetAssetType(type, quantum: quantum.ToBouncyCastle(), address: address);

        return await starkExContractService.IsAssetRegisteredAsync(assetType);
    }
}