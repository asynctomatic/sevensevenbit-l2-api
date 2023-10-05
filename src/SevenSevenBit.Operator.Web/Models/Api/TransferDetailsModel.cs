namespace SevenSevenBit.Operator.Web.Models.Api;

using System.ComponentModel.DataAnnotations;
using System.Numerics;
using Microsoft.AspNetCore.Mvc;
using SevenSevenBit.Operator.Domain.Enums;
using SevenSevenBit.Operator.Web.Attributes.ModelState;
using Swashbuckle.AspNetCore.Annotations;

/// <summary>
/// Request model to fetch details for a signable transfer.
/// </summary>
public class TransferDetailsModel
{
    [ValidGuid]
    [Required(AllowEmptyStrings = false, ErrorMessage = "UserIdRequired")]
    [FromQuery(Name = "from_user_id")]
    [SwaggerSchema(
        Title = "Sender User ID",
        Description = "The unique identifier of the user sending the transfer.",
        Format = "uuid")]
    public Guid? FromUserId { get; set; }

    [ValidGuid]
    [Required(AllowEmptyStrings = false, ErrorMessage = "UserIdRequired")]
    [FromQuery(Name = "to_user_id")]
    [SwaggerSchema(
        Title = "Receiver User ID",
        Description = "The unique identifier of the user receiving the transfer.",
        Format = "uuid")]
    public Guid? ToUserId { get; set; }

    [ValidGuid]
    [Required(AllowEmptyStrings = false, ErrorMessage = "AssetIdRequired")]
    [FromQuery(Name = "asset_id")]
    [SwaggerSchema(
        Title = "Asset ID",
        Description = "The unique identifier of the asset being transferred.",
        Format = "uuid")]
    public Guid? AssetId { get; set; }

    [ValidHexString]
    [FromQuery(Name = "token_id")]
    [SwaggerSchema(
        Title = "Token ID",
        Description = "The hexadecimal string representation of the token ID, if applicable (ie. ERC-721/ERC-1155).",
        Format = "hex")]
    public string TokenId { get; set; }

    // This amount is unquantized
    [Required(ErrorMessage = "OperationAmountRequired")]
    [ValidAmount]
    [FromQuery(Name = "amount")]
    [SwaggerSchema(
        Title = "Amount",
        Description = "The amount of the asset to be transferred.")]
    public BigInteger Amount { get; set; }
}