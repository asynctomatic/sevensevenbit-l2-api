namespace SevenSevenBit.Operator.Application.DTOs.Entities;

using System.Diagnostics.CodeAnalysis;
using System.Numerics;
using System.Text.Json.Serialization;
using SevenSevenBit.Operator.Domain.Entities;
using Swashbuckle.AspNetCore.Annotations;

[ExcludeFromCodeCoverage]
public record UserDto
{
    [JsonConstructor]
    public UserDto(Guid userId, string starkKey, BigInteger balance)
    {
        UserId = userId;
        StarkKey = starkKey;
        Balance = balance;
    }

    public UserDto(User user)
    {
        UserId = user.Id;
        StarkKey = user.StarkKey;
        Balance = BigInteger.Zero;
    }

    [SwaggerSchema(
        Title = "User ID",
        Description = "The ID of the user.",
        Format = "uuid")]
    public Guid UserId { get; }

    [SwaggerSchema(
        Title = "STARK Key",
        Description = "The STARK key of the user.",
        Format = "hex")]
    public string StarkKey { get; }

    [SwaggerSchema(
        Title = "Balance",
        Description = "The $RAG balance of the user.",
        Format = "hex")]
    public BigInteger Balance { get; }

    public static implicit operator UserDto(User user) => new(user);
}