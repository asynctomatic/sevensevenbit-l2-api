namespace SevenSevenBit.Operator.Web.Models.Admin;

using System.ComponentModel.DataAnnotations;
using System.Numerics;
using SevenSevenBit.Operator.Domain.Enums;
using SevenSevenBit.Operator.Web.Attributes.ModelState;
using StarkEx.Crypto.SDK.Enums;

public record RegisterAssetModel
{
    [Required(AllowEmptyStrings = false, ErrorMessage = "AssetTypeRequired")]
    [EnumDataType(typeof(AssetType))]
    public AssetType? Type { get; set; }

    [ConditionalRequiredIfNot(nameof(Type), AssetType.Eth, ErrorCodes.AddressRequired)]
    [ValidHexString]
    [ValidEthereumAddress]
    public string Address { get; set; }

    [Required(AllowEmptyStrings = false, ErrorMessage = "AssetNameRequired")]
    public string Name { get; set; }

    [Required(AllowEmptyStrings = false, ErrorMessage = "AssetSymbolRequired")]
    public string Symbol { get; set; }

    [Required(AllowEmptyStrings = false, ErrorMessage = "AssetQuantumRequired")]
    [ValidQuantum]
    public BigInteger Quantum { get; set; }
}