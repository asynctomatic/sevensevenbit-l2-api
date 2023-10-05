namespace SevenSevenBit.Operator.Application.DTOs.Details;

using Org.BouncyCastle.Math;
using Swashbuckle.AspNetCore.Annotations;

public record FeeDto
{
    [SwaggerSchema(
        Title = "Vault ID",
        Description = "The vault ID of the fee sender.")]
    public Guid VaultId { get; set; }

    [SwaggerSchema(
        Title = "Vault Chain ID",
        Description = "The vault chain ID of the fee sender.",
        Format = "string")]
    public BigInteger VaultChainId { get; set; }

    [SwaggerSchema(
        Title = "StarkEx Asset ID",
        Description = "The StarkEx ID of the fee asset to be collected.",
        Format = "string")]
    public string AssetId { get; set; }

    [SwaggerSchema(
        Title = "Quantized Amount",
        Description = "The amount of the fee asset to be collected, in quantized form.",
        Format = "string")]
    public BigInteger QuantizedAmount { get; set; }
}