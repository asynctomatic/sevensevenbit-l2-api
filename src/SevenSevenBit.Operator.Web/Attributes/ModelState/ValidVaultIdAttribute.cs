namespace SevenSevenBit.Operator.Web.Attributes.ModelState;

using System.ComponentModel.DataAnnotations;
using System.Numerics;
using SevenSevenBit.Operator.Domain.Enums;

public class ValidVaultIdAttribute : ValidationAttribute
{
    protected override ValidationResult IsValid(
        object value,
        ValidationContext validationContext)
    {
        if (value is null)
        {
            return ValidationResult.Success;
        }

        var vaultId = (BigInteger)value;

        if (vaultId < BigInteger.Zero ||
            (vaultId > BigInteger.Parse("2147483648") && vaultId < BigInteger.Parse("9223372036854775808")) ||
             vaultId > BigInteger.Parse("9223372039002259456"))
        {
            return new ValidationResult(ErrorCodes.VaultOutOfBounds.ToString());
        }

        return ValidationResult.Success;
    }
}