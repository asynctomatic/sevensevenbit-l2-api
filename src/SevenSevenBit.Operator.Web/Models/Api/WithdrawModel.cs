namespace SevenSevenBit.Operator.Web.Models.Api;

using System.ComponentModel.DataAnnotations;
using System.Numerics;
using SevenSevenBit.Operator.Web.Attributes.ModelState;
using Swashbuckle.AspNetCore.Annotations;

public record WithdrawModel
{
    [ValidGuid]
    [Required(AllowEmptyStrings = false, ErrorMessage = "VaultIdRequired")]
    [SwaggerSchema(
        Title = "Vault ID",
        Description = "The unique identifier of the vault to withdraw from.",
        Format = "uuid")]
    public Guid? VaultId { get; set; }

    // Unquantized Amount
    [Required(AllowEmptyStrings = false, ErrorMessage = "OperationAmountRequired")]
    [ValidAmount]
    [SwaggerSchema(
        Title = "Amount",
        Description = "The amount of the asset to be withdrawn, in unquantized form.")]
    public BigInteger Amount { get; set; }
}