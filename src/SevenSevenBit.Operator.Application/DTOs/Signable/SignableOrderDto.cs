namespace SevenSevenBit.Operator.Application.DTOs.Signable;

using System.Diagnostics.CodeAnalysis;
using Swashbuckle.AspNetCore.Annotations;

[ExcludeFromCodeCoverage]
public record SignableOrderDto : SignablePayloadDto
{
    [SwaggerSchema(
        Title = "Order ID",
        Description = "The ID of the order.",
        Format = "uuid")]
    public Guid OrderId { get; }
}