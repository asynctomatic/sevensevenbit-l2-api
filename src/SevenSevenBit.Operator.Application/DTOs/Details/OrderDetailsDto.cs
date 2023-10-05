namespace SevenSevenBit.Operator.Application.DTOs.Details;

using System.Numerics;
using Swashbuckle.AspNetCore.Annotations;

public record OrderDetailsDto
{
    [SwaggerSchema(
        Title = "User STARK Key",
        Description = "The STARK key of the user.",
        Format = "hex")]
    public string StarkKey { get; set; }

    [SwaggerSchema(
        Title = "Sell Quantized Amount",
        Description = "The amount to be sold, in quantized form.")]
    public BigInteger SellQuantizedAmount { get; set; }

    [SwaggerSchema(
        Title = "Buy Quantized Amount",
        Description = "The amount to be bough, in quantized form.")]
    public BigInteger BuyQuantizedAmount { get; set; }

    [SwaggerSchema(
        Title = "Sell Vault Chain ID",
        Description = "The vault chain ID for the asset being sold.",
        Format = "string")]
    public BigInteger SellVaultChainId { get; set; }

    [SwaggerSchema(
        Title = "Sell Vault Chain ID",
        Description = "The vault chain ID for the asset being bought.",
        Format = "string")]
    public BigInteger BuyVaultChainId { get; set; }

    [SwaggerSchema(
        Title = "Fee",
        Description = "The fee for the order.")]
    public FeeDto Fee { get; set; }

    [SwaggerSchema(
        Title = "Expiration Timestamp",
        Description = "The timestamp at which this order becomes invalid, in seconds since the Unix epoch.",
        Format = "int64")]
    public long ExpirationTimestamp { get; set; }

    [SwaggerSchema(
        Title = "Nonce",
        Description = "The unique nonce for the order.",
        Format = "int32")]
    public long Nonce { get; set; }

    [SwaggerSchema(
        Title = "Signable Payload",
        Description = "The signable payload for the order.",
        Format = "hex")]
    public string SignablePayload { get; set; }
}