namespace SevenSevenBit.Operator.Web.Models.Api;

using System.ComponentModel.DataAnnotations;
using System.Numerics;
using SevenSevenBit.Operator.Web.Attributes.ModelState;
using StarkEx.Crypto.SDK.Enums;
using Swashbuckle.AspNetCore.Annotations;

public class DeployAssetModel
{
    [Required(AllowEmptyStrings = false, ErrorMessage = "AssetTypeRequired")]
    [EnumDataType(typeof(AssetType))]
    public AssetType? Type { get; set; }

    [Required(AllowEmptyStrings = false, ErrorMessage = "AssetNameRequired")]
    [SwaggerSchema(
        Title = "Name",
        Description = "The token's name (eg. USD Coin).",
        Format = "string")]
    public string Name { get; set; }

    [Required(AllowEmptyStrings = false, ErrorMessage = "AssetSymbolRequired")]
    [SwaggerSchema(
        Title = "Name",
        Description = "The token's symbol (eg. USDC).",
        Format = "string")]
    public string Symbol { get; set; }

    [SwaggerSchema(
        Title = "Uri",
        Description = "The token's metadata uri (for ERC-721 and ERC-1155 tokens).",
        Format = "string")]
    public string Uri { get; set; }

    [ValidQuantum]
    [SwaggerSchema(
        Title = "Quantum",
        Description = "The token's StarkEx asset quantum (for ERC-20 and ERC-1155 tokens).",
        Format = "string")]
    public BigInteger Quantum { get; set; } = BigInteger.One;
}