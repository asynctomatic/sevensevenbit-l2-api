namespace SevenSevenBit.Operator.Application.DTOs.Entities.Order;

using System.Diagnostics.CodeAnalysis;
using System.Numerics;
using Swashbuckle.AspNetCore.Annotations;

[ExcludeFromCodeCoverage]
public record OrderDto
{
    [SwaggerSchema(
        Title = "Order ID",
        Description = "The ID of the order.",
        Format = "uuid")]
    public Guid OrderId { get; set; }

    [SwaggerSchema(
        Title = "Orderbook ID",
        Description = "The ID of the orderbook.",
        Format = "uuid")]
    public Guid OrderbookId { get; set; }

    [SwaggerSchema(
        Title = "Price",
        Description = "The order price.")]
    public double Price { get; set; }

    [SwaggerSchema(
        Title = "Original Amount",
        Description = "The original order amount.",
        Format = "string")]
    public BigInteger OriginalAmount { get; set; }

    [SwaggerSchema(
        Title = "Executed Amount",
        Description = "The executed order amount.",
        Format = "string")]
    public BigInteger ExecutedAmount { get; set; }

    [SwaggerSchema(
        Title = "Fills",
        Description = "The order fills.",
        Format = "array")]
    public IList<OrderFillDto> Fills { get; set; }
}