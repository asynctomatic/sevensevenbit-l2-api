namespace SevenSevenBit.Operator.Web.Models.Api;

using System.ComponentModel.DataAnnotations;
using Org.BouncyCastle.Math;
using SevenSevenBit.Operator.Domain.Enums;
using SevenSevenBit.Operator.Web.Attributes.ModelState;
using Swashbuckle.AspNetCore.Annotations;

/// <summary>
/// Request model to fetch details for a signable order.
/// </summary>
public class OrderDetailsModel
{
    [ValidGuid]
    [Required(AllowEmptyStrings = false, ErrorMessage = "OrderbookIdRequired")]
    [SwaggerSchema(
        Title = "Orderbook ID",
        Description = "The ID of the orderbook.",
        Format = "uuid")]
    public Guid? OrderbookId { get; set; }

    [ValidGuid]
    [Required(AllowEmptyStrings = false, ErrorMessage = "UserIdRequired")]
    [SwaggerSchema(
        Title = "User ID",
        Description = "The unique identifier of the user submitting the order.",
        Format = "uuid")]
    public Guid? UserId { get; set; }

    [Required(ErrorMessage = "OrderSideRequired")]
    [EnumDataType(typeof(OrderSide))]
    [SwaggerSchema(
        Title = "Side",
        Description = "The order side.",
        Format = "string")]
    public OrderSide Side { get; set; }

    // TODO validate range.
    [Required(ErrorMessage = "OrderPriceRequired")]
    [SwaggerSchema(
        Title = "Price",
        Description = "The order price.")]
    public double Price { get; set; }

    // TODO validate range.
    [Required(ErrorMessage = "OrderAmountRequired")]
    [SwaggerSchema(
        Title = "Amount",
        Description = "The order amount.",
        Format = "string")]
    public BigInteger Amount { get; set; }

    [Required(ErrorMessage = "DataAvailabilityRequired")]
    [EnumDataType(typeof(DataAvailabilityModes))]
    [SwaggerSchema(
        Title = "Sell Data Availability Mode",
        Description = "The data availability used for the asset being sold.",
        Format = "string")]
    public DataAvailabilityModes? SellDataAvailabilityMode { get; set; }

    [Required(ErrorMessage = "DataAvailabilityRequired")]
    [EnumDataType(typeof(DataAvailabilityModes))]
    [SwaggerSchema(
        Title = "Buy Data Availability Mode",
        Description = "The data availability used for the asset being bought.",
        Format = "string")]
    public DataAvailabilityModes? BuyDataAvailabilityMode { get; set; }
}