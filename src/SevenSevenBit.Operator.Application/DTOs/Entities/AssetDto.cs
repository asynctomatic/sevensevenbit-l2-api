namespace SevenSevenBit.Operator.Application.DTOs.Entities;

using System.Numerics;
using System.Text.Json.Serialization;
using SevenSevenBit.Operator.Domain.Entities;
using StarkEx.Crypto.SDK.Enums;
using Swashbuckle.AspNetCore.Annotations;

public record AssetDto
{
    [JsonConstructor]
    public AssetDto(
        Guid assetId,
        string starkExType,
        AssetType type,
        string address,
        string name,
        string symbol,
        BigInteger quantum)
    {
        AssetId = assetId;
        StarkExType = starkExType;
        Type = type;
        Address = address;
        Name = name;
        Symbol = symbol;
        Quantum = quantum;
    }

    public AssetDto(Asset asset)
    {
        AssetId = asset.Id;
        StarkExType = asset.StarkExType;
        Type = asset.Type;
        Address = asset.Address;
        Name = asset.Name;
        Symbol = asset.Symbol;
        Quantum = asset.Quantum;
    }

    [SwaggerSchema(
        Title = "Asset ID",
        Description = "The ID of the asset.",
        Format = "uuid")]
    public Guid AssetId { get; set; }

    [SwaggerSchema(
        Title = "StarkEx Asset type",
        Description = "The StarkEx asset type.",
        Format = "hex")]
    public string StarkExType { get; set; }

    [SwaggerSchema(
        Title = "Type",
        Description = "The asset type.",
        Format = "enum")]
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public AssetType Type { get; set; }

    [SwaggerSchema(
        Title = "Asset Address",
        Description = "The ethereum address of the asset.",
        Format = "hex")]
    public string Address { get; set; }

    [SwaggerSchema(
        Title = "Asset Name",
        Description = "The name of the asset.",
        Format = "string")]
    public string Name { get; set; }

    [SwaggerSchema(
        Title = "Asset Symbol",
        Description = "The symbol of the asset.",
        Format = "string")]
    public string Symbol { get; set; }

    [SwaggerSchema(
        Title = "Asset Quantum",
        Description = "The quantum of the asset.",
        Format = "int64")]
    public BigInteger Quantum { get; set; }

    public static implicit operator AssetDto(Asset asset) => new(asset);
}