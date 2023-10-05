namespace SevenSevenBit.Operator.Web.Attributes.ModelState;

using System.ComponentModel.DataAnnotations;
using Nethereum.Hex.HexConvertors.Extensions;
using SevenSevenBit.Operator.Domain.Enums;

public class ValidEthereumAddressAttribute : ValidationAttribute
{
    protected override ValidationResult IsValid(
        object value,
        ValidationContext validationContext)
    {
        if (value is null)
        {
            return ValidationResult.Success;
        }

        var address = (string)value;

        return IsValidEthereumAddressHexFormat(address)
            ? ValidationResult.Success
            : new(ErrorCodes.AddressInvalid.ToString());
    }

    private bool IsValidEthereumAddressHexFormat(string address)
    {
        if (string.IsNullOrEmpty(address))
        {
            return false;
        }

        return IsValidAddressLength(address) && address.IsHex();
    }

    private bool IsValidAddressLength(string address)
    {
        if (string.IsNullOrEmpty(address))
        {
            return false;
        }

        address = address.RemoveHexPrefix();
        return address.Length == 40;
    }
}