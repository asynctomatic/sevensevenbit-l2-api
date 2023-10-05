namespace SevenSevenBit.Operator.Web.Models.Api;

using System.ComponentModel.DataAnnotations;
using System.Numerics;
using SevenSevenBit.Operator.Web.Attributes.ModelState;
using Swashbuckle.AspNetCore.Annotations;

public class SignableDepositModel
{
    [ValidGuid]
    [Required(AllowEmptyStrings = false, ErrorMessage = "UserIdRequired")]
    [SwaggerSchema(
        Title = "User ID",
        Description = "The ID of the user for which the vault should be allocated.",
        Format = "uuid")]
    public Guid? UserId { get; set; }

    [ValidGuid]
    [Required(AllowEmptyStrings = false, ErrorMessage = "AssetIdRequired")]
    [SwaggerSchema(
        Title = "Asset ID",
        Description = "The ID of the vault's asset.",
        Format = "uuid")]
    public Guid? AssetId { get; set; }

    [ValidHexString]
    [SwaggerSchema(
        Title = "Token ID",
        Description = "The hexadecimal string representation of the vault's asset token ID, if applicable (ie. ERC-721/ERC-1155).",
        Format = "hex")]
    public string TokenId { get; set; }

    // This amount is not quantized.
    [ValidAmount]
    [Required(ErrorMessage = "OperationAmountRequired")]
    [SwaggerSchema(
        Title = "Amount",
        Description = "The amount of the asset to be deposited.",
        Format = "string")]
    public BigInteger Amount { get; set; }
}