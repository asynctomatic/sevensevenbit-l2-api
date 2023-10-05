namespace SevenSevenBit.Operator.Web.Attributes.ModelState;

using System.ComponentModel.DataAnnotations;
using Microsoft.Extensions.Options;
using SevenSevenBit.Operator.Domain.Enums;
using SevenSevenBit.Operator.Infrastructure.StarkExApi.Options;

public class ValidStarkExVersionAttribute : ValidationAttribute
{
    protected override ValidationResult IsValid(
        object value,
        ValidationContext validationContext)
    {
        // Let required validation attribute handle null values
        if (value is null)
        {
            return ValidationResult.Success;
        }

        var starkExVersion = (string)value;
        var starkExApiOptions = validationContext.GetRequiredService<IOptions<StarkExApiOptions>>();

        return !starkExApiOptions.Value.SupportedVersions.Contains(starkExVersion) ?
            new ValidationResult(ErrorCodes.StarkExVersionNotSupported.ToString()) :
            ValidationResult.Success;
    }
}