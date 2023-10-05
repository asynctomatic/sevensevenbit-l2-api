namespace SevenSevenBit.Operator.Web.Models.Api.Marketplace;

using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

public class GetBuyOrdersQueryModel : BasePaginatedRequestQueryModel
{
    [Required]
    [FromQuery(Name = "offer_id")]
    [SwaggerSchema(
        Title = "Offer ID",
        Description = "The unique identifier of the offer.",
        Format = "guid")]
    public Guid? OfferId { get; set; }
}