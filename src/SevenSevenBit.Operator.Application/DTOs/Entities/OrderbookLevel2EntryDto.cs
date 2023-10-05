namespace SevenSevenBit.Operator.Application.DTOs.Entities;

using System.Diagnostics.CodeAnalysis;
using System.Numerics;
using Swashbuckle.AspNetCore.Annotations;

[ExcludeFromCodeCoverage]
public record OrderbookLevel2EntryDto
{
    [SwaggerSchema(
        Title = "Price",
        Description = "The orderbook level price.")]
    public double Price { get; set; }

    [SwaggerSchema(
        Title = "Amount",
        Description = "The orderbook level amount.",
        Format = "string")]
    public BigInteger Amount { get; set; }
}