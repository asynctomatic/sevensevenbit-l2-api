namespace SevenSevenBit.Operator.Application.DTOs.Details;

using SevenSevenBit.Operator.Application.DTOs.Entities;
using Swashbuckle.AspNetCore.Annotations;

public record WithdrawDetailsDto
{
    [SwaggerSchema(
        Title = "Vault from which the assets were withdrawn",
        Description = "The vault from which the assets were withdrawn.")]
    public VaultDto Vault { get; init; }

    [SwaggerSchema(
        Title = "Smart Contract Address",
        Description = "The withdraw function to use on-chain.",
        Format = "string")]
    public string WithdrawFunction { get; init; }

    [SwaggerSchema(
        Title = "User's STARK Key",
        Description = "The user's public STARK key",
        Format = "hex")]
    public string StarkKey { get; init; }

    [SwaggerSchema(
        Title = "Asset type",
        Description = "The asset type identifier.",
        Format = "hex")]
    public string AssetType { get; init; }

    [SwaggerSchema(
        Title = "Token Id",
        Description = "The token Id for ERC-721 and ERC-1155 assets.",
        Format = "hex")]
    public string TokenId { get; init; }

    [SwaggerSchema(
        Title = "Minting blob",
        Description = "The minting blob for Mintable ERC-20, ERC-721 and ERC-1155 assets.",
        Format = "hex")]
    public string MintingBlob { get; init; }
}