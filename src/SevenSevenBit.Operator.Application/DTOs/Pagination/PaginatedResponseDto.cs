namespace SevenSevenBit.Operator.Application.DTOs.Pagination;

using System.Text.Json.Serialization;
using Swashbuckle.AspNetCore.Annotations;

public record PaginatedResponseDto<T>
    where T : class
{
    [JsonConstructor]
    public PaginatedResponseDto(IList<T> data, MetadataDto metadata)
    {
        Data = data;
        Metadata = metadata;
    }

    [SwaggerSchema(
        Title = "Data",
        Description= "The data of the response.")]
    public IList<T> Data { get; }

    [SwaggerSchema(
        Title = "Metadata",
        Description = "The metadata for the response.")]
    public MetadataDto Metadata { get; }
}