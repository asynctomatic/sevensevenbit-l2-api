namespace SevenSevenBit.Operator.Web.Attributes.ModelState;

using System.ComponentModel.DataAnnotations;
using SevenSevenBit.Operator.Application.Interfaces.Services;
using SevenSevenBit.Operator.Domain.Enums;

public class ValidExpirationTimestampAttribute : ValidationAttribute
{
    protected override ValidationResult IsValid(
        object value,
        ValidationContext validationContext)
    {
        if (value is null)
        {
            return ValidationResult.Success;
        }

        var timestamp = (long)value;

        var timestampService = validationContext.GetRequiredService<ITimestampService>();

        var limitTimestamp = timestampService.GetLimitExpirationTimestamp();

        return timestamp < limitTimestamp ?
            new ValidationResult(ErrorCodes.TimestampOutsideValidRange.ToString()) :
            ValidationResult.Success;
    }
}