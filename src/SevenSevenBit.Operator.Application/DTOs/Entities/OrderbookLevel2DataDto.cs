namespace SevenSevenBit.Operator.Application.DTOs.Entities;

using System.Diagnostics.CodeAnalysis;
using Swashbuckle.AspNetCore.Annotations;

[ExcludeFromCodeCoverage]
public record OrderbookLevel2DataDto
{
    [SwaggerSchema(
        Title = "Orderbook ID",
        Description = "The ID of the orderbook.",
        Format = "uuid")]
    public Guid OrderbookId { get; set; }

    [SwaggerSchema(
        Title = "Bids",
        Description = "The orderbook bids.",
        Format = "array")]
    public List<OrderbookLevel2EntryDto> Bids { get; set; }

    [SwaggerSchema(
        Title = "Asks",
        Description = "The orderbook asks.",
        Format = "array")]
    public List<OrderbookLevel2EntryDto> Asks { get; set; }
}