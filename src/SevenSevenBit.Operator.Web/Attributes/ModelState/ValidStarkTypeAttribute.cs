namespace SevenSevenBit.Operator.Web.Attributes.ModelState;

using System.ComponentModel.DataAnnotations;
using Nethereum.Hex.HexConvertors.Extensions;
using SevenSevenBit.Operator.Domain.Enums;

public class ValidStarkTypeAttribute : ValidationAttribute
{
    protected override ValidationResult IsValid(
        object value,
        ValidationContext validationContext)
    {
        var starkType = (string)value;

        if (starkType is null)
        {
            return ValidationResult.Success;
        }

        starkType = starkType.RemoveHexPrefix();

        return starkType.Length != 63 ?
            new ValidationResult(ErrorCodes.StarkTypeInvalid.ToString()) :
            ValidationResult.Success;
    }
}