namespace SevenSevenBit.Operator.Web.Attributes.ModelState;

using System.ComponentModel.DataAnnotations;
using System.Globalization;
using SevenSevenBit.Operator.Domain.Enums;

public class ValidGuidAttribute : ValidationAttribute
{
    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        var input = Convert.ToString(value, CultureInfo.CurrentCulture);

        // Let the Required attribute take care of this validation
        if (string.IsNullOrWhiteSpace(input))
        {
            return ValidationResult.Success;
        }

        return !Guid.TryParse(input, out _) ?
            new ValidationResult(ErrorCodes.InvalidGuid.ToString()) :
            ValidationResult.Success;
    }
}