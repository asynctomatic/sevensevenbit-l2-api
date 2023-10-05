namespace SevenSevenBit.Operator.Application.DTOs.Entities;

using System.Diagnostics.CodeAnalysis;
using Swashbuckle.AspNetCore.Annotations;

[ExcludeFromCodeCoverage]
public record OrderbookAssetDto
{
    [SwaggerSchema(
        Title = "Asset ID",
        Description = "The ID of the asset.",
        Format = "uuid")]
    public Guid AssetId { get; set; }

    [SwaggerSchema(
        Title = "Precision",
        Description = "The orderbook precision of the asset.")]
    public uint Precision { get; set; }
}