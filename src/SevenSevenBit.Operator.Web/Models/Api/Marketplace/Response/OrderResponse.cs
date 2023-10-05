namespace SevenSevenBit.Operator.Web.Models.Api.Marketplace.Response;

using System.Diagnostics.CodeAnalysis;
using System.Numerics;
using System.Text.Json.Serialization;
using SevenSevenBit.Operator.Domain.Entities.Marketplace;
using SevenSevenBit.Operator.Domain.Enums;
using StarkEx.Client.SDK.JSON.Converter;
using Swashbuckle.AspNetCore.Annotations;

[ExcludeFromCodeCoverage]
public class OrderResponse
{
    public OrderResponse()
    {
    }

    public OrderResponse(MarketplaceOrder order)
    {
        Id = order.Id;
        MarketplaceId = order.MarketplaceId;
        UserId = order.UserId;
        Type = order.Type;
        Side = order.Side;
        Status = order.Status;
        Price = order.Price;
        Size = order.Size * order.Marketplace.QuoteAsset.Quantum;
        AvailableSize = order.AvailableSize * order.Marketplace.QuoteAsset.Quantum;
        Matches = order.Matches?.Select(x => new OrderMatchResponse(x));
    }

    [SwaggerSchema(
        Title = "Order ID",
        Description = "The unique ID of the order.",
        Format = "uuid")]
    public Guid Id { get; set; }

    [SwaggerSchema(
        Title = "Marketplace ID",
        Description = "The ID of the marketplace.",
        Format = "uuid")]
    public Guid MarketplaceId { get; set; }

    [SwaggerSchema(
        Title = "User ID",
        Description = "The ID of the user.",
        Format = "uuid")]
    public Guid UserId { get; set; }

    [SwaggerSchema(
        Title = "Type",
        Description = "The order type.",
        Format = "enum")]
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public OrderType Type { get; set; }

    [SwaggerSchema(
        Title = "Side",
        Description = "The order side.",
        Format = "enum")]
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public OrderSide Side { get; set; }

    [SwaggerSchema(
        Title = "Status",
        Description = "The order status.",
        Format = "enum")]
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public OrderStatus Status { get; set; }

    [SwaggerSchema(
        Title = "Price",
        Description = "The order spot price.",
        Format = "string")]
    [JsonConverter(typeof(BigIntegerConverter))]
    public BigInteger Price { get; set; }

    [SwaggerSchema(
        Title = "Size",
        Description = "The original order size.",
        Format = "string")]
    [JsonConverter(typeof(BigIntegerConverter))]
    public BigInteger Size { get; set; }

    [SwaggerSchema(
        Title = "Available Size",
        Description = "The available order size (unfilled).",
        Format = "string")]
    [JsonConverter(typeof(BigIntegerConverter))]
    public BigInteger AvailableSize { get; set; }

    public IEnumerable<OrderMatchResponse> Matches { get; set; }
}

[ExcludeFromCodeCoverage]
public class OrderMatchResponse
{
    public OrderMatchResponse()
    {
    }

    public OrderMatchResponse(OrderMatch match)
    {
        TransactionId = match.TransactionId;
        Quantity = match.Quantity;
    }

    public Guid TransactionId { get; set; }

    [JsonConverter(typeof(BigIntegerConverter))]
    public BigInteger Quantity { get; set; }
}