namespace SevenSevenBit.Operator.Web.Attributes.ModelState;

using System.ComponentModel.DataAnnotations;
using SevenSevenBit.Operator.Domain.Enums;
using SevenSevenBit.Operator.Web.Models.Api;

public class ValidReceiverVaultIdAttribute : ValidationAttribute
{
    protected override ValidationResult IsValid(
        object value,
        ValidationContext validationContext)
    {
        if (value is null)
        {
            return ValidationResult.Success;
        }

        var receiverVaultId = (Guid)value;
        var model = validationContext.ObjectInstance as TransferModel;

        return model!.FromUserId.Equals(receiverVaultId) ?
            new ValidationResult(ErrorCodes.TransferIntoSameVault.ToString()) :
            ValidationResult.Success;
    }
}