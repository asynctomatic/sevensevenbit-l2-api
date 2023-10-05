namespace SevenSevenBit.Operator.Web.Models.Api.Mint;

using System.ComponentModel.DataAnnotations;
using System.Numerics;
using SevenSevenBit.Operator.Web.Attributes.ModelState;
using Swashbuckle.AspNetCore.Annotations;

/// <summary>
/// Request model to mint user assets.
/// </summary>
public record MintRequestModel
{
    [ValidGuid]
    [Required(AllowEmptyStrings = false, ErrorMessage = "UserIdRequired")]
    [SwaggerSchema(
        Title = "User ID",
        Description = "The ID of the user for which the assets should be minted.",
        Format = "uuid")]
    public Guid? UserId { get; set; }

    [ValidGuid]
    [Required(ErrorMessage = "AssetIdRequired")]
    [SwaggerSchema(
        Title = "Asset ID",
        Description = "The ID of the of the asset being minted.",
        Format = "uuid")]
    public Guid? AssetId { get; set; }

    [ValidHexString]
    [SwaggerSchema(
        Title = "Token ID",
        Description = "The token id of the asset, if the asset that is being minted is an ERC721 or ERC1155.",
        Format = "hex")]
    public string TokenId { get; set; }

    // This amount is Unquantized
    [ValidAmount]
    [Required(ErrorMessage = "OperationAmountRequired")]
    [SwaggerSchema(
        Title = "Amount",
        Description = "The amount of the asset to be minted.",
        Format = "string")]
    public BigInteger Amount { get; set; }
}