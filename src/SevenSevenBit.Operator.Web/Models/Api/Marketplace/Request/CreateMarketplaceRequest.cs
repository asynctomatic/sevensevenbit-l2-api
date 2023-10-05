namespace SevenSevenBit.Operator.Web.Models.Api.Marketplace.Request;

using System.ComponentModel.DataAnnotations;
using SevenSevenBit.Operator.Web.Attributes.ModelState;
using Swashbuckle.AspNetCore.Annotations;

public class CreateMarketplaceRequest
{
    [Required(AllowEmptyStrings = false, ErrorMessage = "BaseAssetIdRequired")]
    [ValidGuid]
    [SwaggerSchema(
        Title = "Base Asset ID",
        Description = "The unique identifier of the marketplace base asset.",
        Format = "uuid")]
    public Guid BaseAssetId { get; set; }

    [Required(AllowEmptyStrings = false, ErrorMessage = "QuoteAssetIdRequired")]
    [ValidGuid]
    [SwaggerSchema(
        Title = "Quote Asset ID",
        Description = "The unique identifier of the marketplace quote asset.",
        Format = "uuid")]
    public Guid QuoteAssetId { get; set; }

    [ValidHexString]
    [SwaggerSchema(
        Title = "Base Asset Token ID",
        Description = "The token ID of the marketplace base asset (required for ERC721 and ERC1155 assets).",
        Format = "hex")]
    public string BaseAssetTokenId { get; set; }

    [ValidHexString]
    [SwaggerSchema(
        Title = "Quote Asset Token ID",
        Description = "The token ID of the marketplace base asset (required for ERC721 and ERC1155 assets).",
        Format = "hex")]
    public string QuoteAssetTokenId { get; set; }
}