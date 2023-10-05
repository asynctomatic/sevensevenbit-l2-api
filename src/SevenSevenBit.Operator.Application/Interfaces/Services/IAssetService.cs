namespace SevenSevenBit.Operator.Application.Interfaces.Services;

using System.Numerics;
using SevenSevenBit.Operator.Application.DTOs.Entities;
using SevenSevenBit.Operator.Application.DTOs.Pagination;
using SevenSevenBit.Operator.Domain.Entities;
using SevenSevenBit.Operator.Domain.ValueObjects;
using StarkEx.Crypto.SDK.Enums;

public interface IAssetService
{
    Task<bool> IsAssetRegisteredAsync(
        AssetType type,
        BlockchainAddress address,
        BigInteger quantum);

    Task<Asset> RegisterAssetAsync(
        AssetType type,
        BlockchainAddress address,
        string name,
        string symbol,
        BigInteger quantum,
        CancellationToken cancellationToken);

    Task<Asset> GetAssetAsync(
        Guid assetId,
        CancellationToken cancellationToken);

    Task<IEnumerable<Asset>> GetAssetsAsync(CancellationToken cancellationToken);

    Task<IEnumerable<Asset>> GetAssetsAsync(
        IEnumerable<Guid> assetIds,
        CancellationToken cancellationToken);

    Task<PaginatedResponseDto<AssetDto>> GetAssetsAsync(
        Paging paging,
        AssetType? assetType,
        string sort = null,
        CancellationToken cancellationToken = default);
}