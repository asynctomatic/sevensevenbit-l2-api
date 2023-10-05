namespace SevenSevenBit.Operator.Web.Attributes.ModelState;

using System.ComponentModel.DataAnnotations;
using System.Numerics;
using SevenSevenBit.Operator.Domain.Enums;

public class ValidAmountAttribute : ValidationAttribute
{
    protected override ValidationResult IsValid(
        object value,
        ValidationContext validationContext)
    {
        if (value is null)
        {
            return ValidationResult.Success;
        }

        var amount = (BigInteger)value;

        // Quantized values can't be bigger than 2**64 - https://docs.starkware.co/starkex/starkex-specific-concepts.html#quantization_and_resolution
        if (amount <= BigInteger.Zero || amount >= BigInteger.Parse("18446744073709551616"))
        {
            return new ValidationResult(ErrorCodes.OperationAmountInvalid.ToString());
        }

        return ValidationResult.Success;
    }
}