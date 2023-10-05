namespace SevenSevenBit.Operator.Application.Interfaces.Services;

using SevenSevenBit.Operator.Application.DTOs.Entities;
using SevenSevenBit.Operator.Application.DTOs.Pagination;
using SevenSevenBit.Operator.Domain.Entities;
using SevenSevenBit.Operator.Domain.ValueObjects;

public interface IVaultService
{
    Task<Vault> GetVaultAsync(
        Guid vaultId,
        CancellationToken cancellationToken);

    Vault GetVault(
        User user,
        Asset asset,
        string tokenId = null,
        string mintingBlob = null);

    Task<Vault> AllocateVaultAsync(
        User user,
        Asset asset,
        CancellationToken cancellationToken,
        TokenId tokenId = null);

    Task<PaginatedResponseDto<VaultDto>> GetVaultsAsync(
        Paging paging,
        Guid? assetId,
        string sort = null,
        CancellationToken cancellationToken = default);

    Task<bool> DoesVaultExistsAsync(
        Guid vaultId,
        CancellationToken cancellationToken);

    Vault GetVaultWithTokenId(
        User user,
        Asset asset,
        string tokenId);

    Vault GetVaultWithMintingBlob(
        User user,
        Asset asset,
        string mintingBlob);
}