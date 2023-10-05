namespace SevenSevenBit.Operator.Web.Models.Api;

using System.ComponentModel.DataAnnotations;
using SevenSevenBit.Operator.Web.Attributes.ModelState;
using Swashbuckle.AspNetCore.Annotations;

/// <summary>
/// Request model to create an marketplace.
/// </summary>
public class CreateMarketplaceModel
{
    [ValidGuid]
    [Required(AllowEmptyStrings = false, ErrorMessage = "TradableAssetIdRequired")]
    [SwaggerSchema(
        Title = "Tradable Asset A ID",
        Description = "The ID of the marketplace tradable asset A.",
        Format = "uuid")]
    public Guid? TradableAssetA { get; set; }

    [ValidGuid]
    [Required(AllowEmptyStrings = false, ErrorMessage = "TradableAssetIdRequired")]
    [SwaggerSchema(
        Title = "Tradable Asset B ID",
        Description = "The ID of the marketplace tradable asset B.",
        Format = "uuid")]
    public Guid? TradableAssetB { get; set; }
}