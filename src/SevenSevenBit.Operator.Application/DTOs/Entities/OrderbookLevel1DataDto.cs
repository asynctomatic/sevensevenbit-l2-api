namespace SevenSevenBit.Operator.Application.DTOs.Entities;

using System.Diagnostics.CodeAnalysis;
using System.Numerics;
using Swashbuckle.AspNetCore.Annotations;

[ExcludeFromCodeCoverage]
public record OrderbookLevel1DataDto
{
    [SwaggerSchema(
        Title = "Orderbook ID",
        Description = "The ID of the orderbook.",
        Format = "uuid")]
    public Guid OrderbookId { get; set; }

    [SwaggerSchema(
        Title = "Bid Price",
        Description = "The highest posted price where someone is willing to buy an asset.")]
    public double BidPrice { get; set; }

    [SwaggerSchema(
        Title = "Bid Size",
        Description = "The number of asset shares that users are trying to buy at the bid price.",
        Format = "string")]
    public BigInteger BidSize { get; set; }

    [SwaggerSchema(
        Title = "Ask Price",
        Description = "The lowest posted price where someone is willing to sell an asset.")]
    public double AskPrice { get; set; }

    [SwaggerSchema(
        Title = "Ask Size",
        Description = "The number of asset shares that users are trying to sell at the bid ask.",
        Format = "string")]
    public BigInteger AskSize { get; set; }

    [SwaggerSchema(
        Title = "Last Price",
        Description = "The price at which the last transaction occurred.")]
    public double LastPrice { get; set; }

    [SwaggerSchema(
        Title = "Last Size",
        Description = "The number of asset shares involved in the last transaction.",
        Format = "string")]
    public BigInteger LastSize { get; set; }
}