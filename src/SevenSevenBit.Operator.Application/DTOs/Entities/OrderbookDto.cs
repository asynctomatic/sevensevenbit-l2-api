namespace SevenSevenBit.Operator.Application.DTOs.Entities;

using System.Diagnostics.CodeAnalysis;
using Swashbuckle.AspNetCore.Annotations;

[ExcludeFromCodeCoverage]
public record OrderbookDto
{
    [SwaggerSchema(
        Title = "Orderbook ID",
        Description = "The ID of the orderbook.",
        Format = "uuid")]
    public Guid OrderbookId { get; set; }

    [SwaggerSchema(
        Title = "Symbol",
        Description = "The orderbook symbol.")]
    public string Symbol { get; set; }

    // TODO SwaggerSchema
    public OrderbookAssetDto BaseAsset { get; set; }

    // TODO SwaggerSchema
    public OrderbookAssetDto QuoteAsset { get; set; }
}