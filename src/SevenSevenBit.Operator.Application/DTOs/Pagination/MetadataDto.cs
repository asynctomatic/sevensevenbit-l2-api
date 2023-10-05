namespace SevenSevenBit.Operator.Application.DTOs.Pagination;

using System.Text.Json.Serialization;
using Swashbuckle.AspNetCore.Annotations;

public record MetadataDto
{
    [JsonConstructor]
    public MetadataDto(bool hasNext, int totalCount)
    {
        HasNext = hasNext;
        TotalCount = totalCount;
    }

    [SwaggerSchema(
        Title = "HasNext",
        Description = "Indicates whether there is a next page available.")]
    public bool HasNext { get; }

    [SwaggerSchema(
        Title = "TotalCount",
        Description = "The total count of results available.")]
    public int TotalCount { get; }
}