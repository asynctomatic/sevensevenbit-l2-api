namespace SevenSevenBit.Operator.Application.DTOs.Signable;

using System.Diagnostics.CodeAnalysis;
using Swashbuckle.AspNetCore.Annotations;

[ExcludeFromCodeCoverage]
public record SignablePayloadDto
{
    [SwaggerSchema(
        Title = "Signable payload",
        Description = "The signable payload for the operation.",
        Format = "hex")]
    public string Signable { get; init; }
}