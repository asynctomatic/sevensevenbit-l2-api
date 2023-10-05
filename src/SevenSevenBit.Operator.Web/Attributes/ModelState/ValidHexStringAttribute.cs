namespace SevenSevenBit.Operator.Web.Attributes.ModelState;

using System.ComponentModel.DataAnnotations;
using SevenSevenBit.Operator.Domain.Enums;
using SevenSevenBit.Operator.SharedKernel.Extensions;

public class ValidHexStringAttribute : ValidationAttribute
{
    protected override ValidationResult IsValid(
        object value,
        ValidationContext validationContext)
    {
        if (value is null)
        {
            return ValidationResult.Success;
        }

        var hexString = (string)value;

        return hexString.IsValidHexString() ?
            ValidationResult.Success :
            new ValidationResult(ErrorCodes.InvalidHexString.ToString());
    }
}