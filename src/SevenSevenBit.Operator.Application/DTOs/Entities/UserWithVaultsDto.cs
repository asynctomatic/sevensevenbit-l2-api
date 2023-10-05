namespace SevenSevenBit.Operator.Application.DTOs.Entities;

using Swashbuckle.AspNetCore.Annotations;

public record UserWithVaultsDto
{
    [SwaggerSchema(
        Title = "User",
        Description = "The user object.")]
    public UserDto User { get; init; }

    [SwaggerSchema(
        Title = "Vaults",
        Description = "The user vaults grouped by the asset id.")]
    public IDictionary<Guid, IEnumerable<VaultDto>> VaultsPerAsset { get; init; }
}