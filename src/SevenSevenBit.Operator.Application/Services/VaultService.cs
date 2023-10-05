namespace SevenSevenBit.Operator.Application.Services;

using System.Numerics;
using Microsoft.Extensions.Logging;
using SevenSevenBit.Operator.Application.Common.Interfaces;
using SevenSevenBit.Operator.Application.DTOs.Entities;
using SevenSevenBit.Operator.Application.DTOs.Pagination;
using SevenSevenBit.Operator.Application.Helpers;
using SevenSevenBit.Operator.Application.Interfaces.Services;
using SevenSevenBit.Operator.Domain.Entities;
using SevenSevenBit.Operator.Domain.Enums;
using SevenSevenBit.Operator.Domain.ValueObjects;
using SevenSevenBit.Operator.SharedKernel.Extensions;

public class VaultService : IVaultService
{
    private readonly ILogger<IVaultService> logger;
    private readonly IUnitOfWork unitOfWork;

    public VaultService(
        ILogger<VaultService> logger,
        IUnitOfWork unitOfWork)
    {
        this.logger = logger;
        this.unitOfWork = unitOfWork;
    }

    public async Task<Vault> GetVaultAsync(
        Guid vaultId,
        CancellationToken cancellationToken)
    {
        return await unitOfWork.Repository<Vault>().GetByIdAsync(
            vaultId,
            cancellationToken);
    }

    public Vault GetVault(
        User user,
        Asset asset,
        string tokenId = null,
        string mintingBlob = null)
    {
        // Try to return the vault with the biggest balance. If there is a draw, return the vault with the smallest vaultId
        var query = user.Vaults.Where(v => v.AssetId.Equals(asset.Id));

        if (tokenId != null)
        {
            query = query.Where(v => v.TokenId is not null && v.TokenId == tokenId);
        }

        if (mintingBlob != null)
        {
            query = query.Where(v => v.BaseMintingBlob is not null && v.BaseMintingBlob.MintingBlobHex == mintingBlob);
        }

        return query.OrderByDescending(x => x.QuantizedAvailableBalance)
            .ThenBy(x => x.VaultChainId)
            .FirstOrDefault();
    }

    public async Task<Vault> AllocateVaultAsync(
        User user,
        Asset asset,
        CancellationToken cancellationToken,
        TokenId tokenId = null)
    {
        var vault = new Vault
        {
            QuantizedAvailableBalance = BigInteger.Zero,
            User = user,
            Asset = asset,
            TokenId = tokenId,
        };

        await unitOfWork.Repository<Vault>().InsertAsync(vault, cancellationToken);
        await unitOfWork.SaveAsync(cancellationToken);

        logger.LogInformation(
            "Allocated vault with id {VaultId}: " +
            "(starkKey={StarkKey}, starkExType={StarkExType}, starkExId={StarkExId})",
            vault.VaultChainId,
            vault.User.StarkKey,
            vault.Asset.StarkExType,
            vault.AssetStarkExId());

        return vault;
    }

    public async Task<PaginatedResponseDto<VaultDto>> GetVaultsAsync(
        Paging paging,
        Guid? assetId,
        string sort = null,
        CancellationToken cancellationToken = default)
    {
        var filter = QueryBuilder.GetFilter(
            (nameof(Vault.AssetId), typeof(Guid), assetId, FilterOptions.IsEqualTo));

        sort ??= $"{nameof(Vault.VaultChainId)} desc";

        return await unitOfWork.Repository<Vault>().GetProjectedPaginatedAsync(
            paging: paging,
            vault => new VaultDto(vault),
            filter: filter,
            orderBy: sort,
            cancellationToken: cancellationToken);
    }

    public async Task<bool> DoesVaultExistsAsync(
        Guid vaultId,
        CancellationToken cancellationToken)
    {
        return await unitOfWork.Repository<Vault>().AnyAsync(
            filter: vault => vault.Id.Equals(vaultId),
            cancellationToken);
    }

    public Vault GetVaultWithTokenId(User user, Asset asset, string tokenId)
    {
        var query = user.Vaults
            .Where(v => v.AssetId.Equals(asset.Id) && v.TokenId is not null && v.TokenId == tokenId);

        if (asset.Type.IsNft())
        {
            return query.SingleOrDefault();
        }

        // Try to return the vault with the biggest balance in case it is a ERC1155. If there is a draw, return the vault with the smallest vaultId
        return query.OrderByDescending(x => x.QuantizedAvailableBalance)
            .ThenBy(x => x.VaultChainId)
            .FirstOrDefault();
    }

    public Vault GetVaultWithMintingBlob(User user, Asset asset, string mintingBlob)
    {
        var query = user.Vaults
            .Where(v => v.AssetId.Equals(asset.Id) && v.BaseMintingBlob is not null && v.BaseMintingBlob.MintingBlobHex.Equals(mintingBlob));

        if (asset.Type.IsNft())
        {
            return query.SingleOrDefault();
        }

        // Try to return the vault with the biggest balance in case it is a ERC1155. If there is a draw, return the vault with the smallest vaultId
        return query.OrderByDescending(x => x.QuantizedAvailableBalance)
            .ThenBy(x => x.VaultChainId)
            .FirstOrDefault();
    }
}