namespace SevenSevenBit.Operator.Application.DTOs.Details;

using System.Text.Json.Serialization;
using Swashbuckle.AspNetCore.Annotations;

public record RegisterTenantDto
{
    [JsonConstructor]
    public RegisterTenantDto(Guid tenantId, string name, string starkExAddress, int chainId, string verifyingContractAddress, int version, string clientId, string clientSecret)
    {
        TenantId = tenantId;
        Name = name;
        StarkExAddress = starkExAddress;
        ChainId = chainId;
        VerifyingContractAddress = verifyingContractAddress;
        Version = version;
        ClientId = clientId;
        ClientSecret = clientSecret;
    }

    [SwaggerSchema(
        Title = "Tenant Id",
        Description = "The id of the new Tenant.",
        Format = "uuid")]
    public Guid TenantId { get; }

    [SwaggerSchema(
        Title = "Name",
        Description = "The name of the new Tenant.",
        Format = "string")]
    public string Name { get; }

    [SwaggerSchema(
        Title = "StarkEx Address",
        Description = "The ethereum address of the StarkEx instance.",
        Format = "hex")]
    public string StarkExAddress { get; }

    [SwaggerSchema(
        Title = "Chain Id",
        Description = "The chain id of the Tenant application.",
        Format = "int32")]
    public int ChainId { get; }

    [SwaggerSchema(
        Title = "Verifying Contract Address",
        Description = "The verifying contract address of the Tenant for the EIP-712.",
        Format = "hex")]
    public string VerifyingContractAddress { get; }

    [SwaggerSchema(
        Title = "Version",
        Description = "The version of the Tenant for the EIP-712.",
        Format = "int32")]
    public int Version { get; }

    [SwaggerSchema(
        Title = "ClientId",
        Description = "The client id used to obtain the access token.",
        Format = "string")]
    public string ClientId { get; }

    [SwaggerSchema(
        Title = "Client Secret",
        Description = "The client secret used to obtain the access token.",
        Format = "string")]
    public string ClientSecret { get; }
}