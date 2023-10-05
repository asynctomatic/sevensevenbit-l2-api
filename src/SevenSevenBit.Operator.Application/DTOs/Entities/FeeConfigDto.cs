namespace SevenSevenBit.Operator.Application.DTOs.Entities;

using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;
using SevenSevenBit.Operator.Domain.Entities;
using SevenSevenBit.Operator.Domain.Enums;
using Swashbuckle.AspNetCore.Annotations;

[ExcludeFromCodeCoverage]
public record FeeConfigDto
{
    [JsonConstructor]
    public FeeConfigDto(Guid feeId, FeeAction action, int basisPoints)
    {
        FeeId = feeId;
        Action = action;
        BasisPoints = basisPoints;
    }

    public FeeConfigDto(FeeConfig feeConfig)
    {
        FeeId = feeConfig.Id;
        Action = feeConfig.Action;
        BasisPoints = feeConfig.Amount;
    }

    [SwaggerSchema(
        Title = "Fee ID",
        Description = "The unique identifier of the fee configuration.",
        Format = "uuid")]
    public Guid FeeId { get; }

    [SwaggerSchema(
        Title = "Operation",
        Description = "The system operation associated with the fee configuration.",
        Format = "string")]
    public FeeAction Action { get; }

    [SwaggerSchema(
        Title = "Basis Points",
        Description = "The basis points (1/100 of a percent) of the fee to take on the operation.",
        Format = "int32")]
    public int BasisPoints { get; }

    public static implicit operator FeeConfigDto(FeeConfig feeConfig) => new(feeConfig);
}