namespace SevenSevenBit.Operator.Web.Attributes.ModelState;

using System.ComponentModel.DataAnnotations;
using System.Numerics;
using SevenSevenBit.Operator.Domain.Enums;
using SevenSevenBit.Operator.Web.Models.Admin;
using SevenSevenBit.Operator.Web.Models.Api;
using StarkEx.Crypto.SDK.Enums;

public class ValidQuantumAttribute : ValidationAttribute
{
    // Represents StarkEx contracts UpperBound 2**128
    private readonly BigInteger quantumUpperBound = BigInteger.Parse("340282366920938463463374607431768211456");

    protected override ValidationResult IsValid(
        object value,
        ValidationContext validationContext)
    {
        if (value is null)
        {
            return ValidationResult.Success;
        }

        var quantum = (BigInteger)value;

        var registerAssetModel = validationContext.ObjectInstance as RegisterAssetModel;
        var deployAssetModel = validationContext.ObjectInstance as DeployAssetModel;
        if (registerAssetModel?.Type is null && deployAssetModel?.Type is null)
        {
            return ValidationResult.Success;
        }

        var type = registerAssetModel?.Type ?? deployAssetModel.Type.Value;

        var isNft = IsNft(type);

        return (isNft && quantum == BigInteger.One) || (!isNft && quantum >= BigInteger.One && quantum < quantumUpperBound)
            ? ValidationResult.Success
            : new ValidationResult(ErrorCodes.AssetQuantumInvalid.ToString());
    }

    private static bool IsNft(AssetType assetType)
    {
        return assetType.Equals(AssetType.Erc721) ||
               assetType.Equals(AssetType.MintableErc721);
    }
}