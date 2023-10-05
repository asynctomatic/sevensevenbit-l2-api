namespace SevenSevenBit.Operator.Application.DTOs.Entities.Order;

using System.Diagnostics.CodeAnalysis;
using System.Numerics;
using Swashbuckle.AspNetCore.Annotations;

[ExcludeFromCodeCoverage]
public record OrderFillDto
{
    [SwaggerSchema(
        Title = "Price",
        Description = "The fill price.")]
    public double Price { get; set; }

    [SwaggerSchema(
        Title = "Amount",
        Description = "The fill amount.",
        Format = "string")]
    public BigInteger Amount { get; set; }
}