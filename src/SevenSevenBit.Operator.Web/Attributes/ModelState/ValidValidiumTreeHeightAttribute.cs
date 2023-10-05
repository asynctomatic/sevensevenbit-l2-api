namespace SevenSevenBit.Operator.Web.Attributes.ModelState;

using System.ComponentModel.DataAnnotations;
using Microsoft.Extensions.Options;
using SevenSevenBit.Operator.Domain.Enums;
using SevenSevenBit.Operator.Infrastructure.StarkExApi.Options;

public class ValidValidiumTreeHeightAttribute : ValidationAttribute
{
    protected override ValidationResult IsValid(
        object value,
        ValidationContext validationContext)
    {
        if (value is null)
        {
            return ValidationResult.Success;
        }

        var treeHeight = (int)value;

        var starkExOptions = validationContext.GetRequiredService<IOptions<StarkExApiOptions>>();

        if (treeHeight < starkExOptions.Value.MinValidiumTreeHeight || treeHeight > starkExOptions.Value.MaxValidiumTreeHeight)
        {
            return new ValidationResult(ErrorCodes.ValidiumTreeHeightOutOfRange.ToString());
        }

        return ValidationResult.Success;
    }
}