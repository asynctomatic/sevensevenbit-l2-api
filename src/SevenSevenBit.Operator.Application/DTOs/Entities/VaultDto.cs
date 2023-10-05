namespace SevenSevenBit.Operator.Application.DTOs.Entities;

using System.Numerics;
using System.Text.Json.Serialization;
using SevenSevenBit.Operator.Domain.Entities;
using SevenSevenBit.Operator.Domain.Enums;
using SevenSevenBit.Operator.SharedKernel.Extensions;
using Swashbuckle.AspNetCore.Annotations;

public record VaultDto
{
    [JsonConstructor]
    public VaultDto(
        Guid vaultId,
        BigInteger vaultChainId,
        string assetSymbol,
        string tokenId,
        string mintingBlob,
        string assetStarkExId,
        string userStarkKey,
        BigInteger availableBalance,
        BigInteger accountingBalance,
        DataAvailabilityModes dataAvailabilityMode)
    {
        VaultId = vaultId;
        VaultChainId = vaultChainId;
        AssetSymbol = assetSymbol;
        TokenId = tokenId;
        MintingBlob = mintingBlob;
        AssetStarkExId = assetStarkExId;
        UserStarkKey = userStarkKey;
        AvailableBalance = availableBalance;
        AccountingBalance = accountingBalance;
        DataAvailabilityMode = dataAvailabilityMode;
    }

    public VaultDto(Vault vault, bool isMintableOperation = false)
    {
        VaultId = vault.Id;
        VaultChainId = vault.VaultChainId;
        AssetSymbol = vault.Asset.Symbol;
        TokenId = vault.TokenId;
        MintingBlob = vault.BaseMintingBlob?.MintingBlobHex;
        AssetStarkExId = vault.AssetStarkExId(isMintableOperation);
        UserStarkKey = vault.User.StarkKey;
        AvailableBalance = vault.QuantizedAvailableBalance.ToUnquantized(vault.Asset.Quantum);
        AccountingBalance = vault.QuantizedAccountingBalance.ToUnquantized(vault.Asset.Quantum);
        DataAvailabilityMode = vault.DataAvailabilityMode;
    }

    [SwaggerSchema(
        Title = "Vault ID",
        Description = "The ID of the vault.",
        Format = "uuid")]
    public Guid VaultId { get; }

    [SwaggerSchema(
        Title = "Vault Chain ID",
        Description = "The StarkEx ID of the vault.")]
    public BigInteger VaultChainId { get; }

    [SwaggerSchema(
        Title = "Asset Symbol",
        Description = "The symbol of the asset associated with the vault.",
        Format = "string")]
    public string AssetSymbol { get; }

    [SwaggerSchema(
        Title = "Token ID",
        Description = "The token id of the asset associated with the vault, if the asset is an ERC721 or ERC1155.",
        Format = "hex")]
    public string TokenId { get; }

    [SwaggerSchema(
        Title = "Minting Blob",
        Description = "The minting blob of the asset associated with the vault, if the asset is a Mintable ERC20, ERC721 or ERC1155.",
        Format = "hex")]
    public string MintingBlob { get; }

    [SwaggerSchema(
        Title = "Asset StarkEx ID",
        Description = "The StarkEx ID of the asset associated with the vault.",
        Format = "hex")]
    public string AssetStarkExId { get; }

    [SwaggerSchema(
        Title = "STARK Key",
        Description = "The STARK key of the user associated with the vault.",
        Format = "hex")]
    public string UserStarkKey { get; }

    [SwaggerSchema(
        Title = "Available Balance",
        Description = "The available balance of the vault.")]
    public BigInteger AvailableBalance { get; }

    [SwaggerSchema(
        Title = "Accounting Balance",
        Description = "The accounting balance of the vault.")]
    public BigInteger AccountingBalance { get; }

    [SwaggerSchema(
        Title = "Data Availability Mode",
        Description = "The data availability mode of the vault.")]
    public DataAvailabilityModes DataAvailabilityMode { get; }

    public static implicit operator VaultDto(Vault vault) => new(vault);
}