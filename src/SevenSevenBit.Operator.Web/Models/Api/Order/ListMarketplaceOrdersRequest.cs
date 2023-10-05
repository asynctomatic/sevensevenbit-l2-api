namespace SevenSevenBit.Operator.Web.Models.Api.Order;

using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using SevenSevenBit.Operator.Domain.Enums;
using Swashbuckle.AspNetCore.Annotations;

public class ListMarketplaceOrdersRequest : BasePaginatedRequestQueryModel
{
    [Required]
    [FromQuery(Name = "side")]
    [SwaggerSchema(
        Title = "Order Side",
        Description = "The order side.",
        Format = "string")]
    public OrderSide Side { get; set; }

    [FromQuery(Name = "include_inactive")]
    [SwaggerSchema(
        Title = "Include Inactive Orders",
        Description = "Whether to include inactive orders in the results.",
        Format = "bool")]
    public bool IncludeInactive { get; set; } = false;
}