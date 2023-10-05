namespace SevenSevenBit.Operator.Web.Models.Api.Marketplace;

using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using SevenSevenBit.Operator.Domain.Enums;
using Swashbuckle.AspNetCore.Annotations;

public class GetSellOffersQueryModel : BasePaginatedRequestQueryModel
{
    [EnumDataType(typeof(OfferStatus))]
    [FromQuery(Name = "offer_status")]
    [SwaggerSchema(
        Title = "Offer Status",
        Description = "The status of the offer.",
        Format = "string")]
    public OfferStatus? OfferStatus { get; set; }

    [Required]
    [FromQuery(Name = "product_id")]
    [SwaggerSchema(
        Title = "Product ID",
        Description = "The unique identifier of the product.",
        Format = "guid")]
    public Guid? ProductId { get; set; }

    [FromQuery(Name = "user_id")]
    [SwaggerSchema(
        Title = "User ID",
        Description = "The unique identifier of the user.",
        Format = "guid")]
    public Guid? UserId { get; set; }
}