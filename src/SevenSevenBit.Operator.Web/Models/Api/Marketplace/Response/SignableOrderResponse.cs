namespace SevenSevenBit.Operator.Web.Models.Api.Marketplace.Response;

using System.Diagnostics.CodeAnalysis;
using System.Numerics;
using System.Text.Json.Serialization;
using SevenSevenBit.Operator.Domain.ValueObjects.Signable;
using StarkEx.Client.SDK.JSON.Converter;
using Swashbuckle.AspNetCore.Annotations;

[ExcludeFromCodeCoverage]
public class SignableOrderResponse
{
    public SignableOrderResponse()
    {
    }

    public SignableOrderResponse(SignableOrder signableOrder)
    {
        Metadata = new SignableOrderMetadata
        {
            BaseAssetAmountQuantized = signableOrder.BaseAssetAmountQuantized,
            QuoteAssetAmountQuantized = signableOrder.QuoteAssetAmountQuantized,
            ExpirationTimestamp = signableOrder.ExpirationTimestamp,
            Nonce = signableOrder.Nonce,
        };
        Signable = signableOrder.Signable;
    }

    [SwaggerSchema(
        Title = "Metadata",
        Description = "Metadata to reconstruct the order.",
        Format = "json")]
    public SignableOrderMetadata Metadata { get; init; }

    [SwaggerSchema(
        Title = "Signable Payload",
        Description = "The signable payload for the order.",
        Format = "hex")]
    public string Signable { get; set; }
}

[ExcludeFromCodeCoverage]
public class SignableOrderMetadata
{
    [SwaggerSchema(
        Title = "Base Asset Amount Quantized",
        Description = "The quantized amount of the base asset.",
        Format = "string")]
    [JsonConverter(typeof(BigIntegerConverter))]
    public BigInteger BaseAssetAmountQuantized { get; set; }

    [SwaggerSchema(
        Title = "Quote Asset Amount Quantized",
        Description = "The quantized amount of the quote asset.",
        Format = "string")]
    [JsonConverter(typeof(BigIntegerConverter))]
    public BigInteger QuoteAssetAmountQuantized { get; set; }

    [SwaggerSchema(
        Title = "Nonce",
        Description = "The unique nonce for the order.",
        Format = "int32")]
    public int Nonce { get; set; }

    [SwaggerSchema(
        Title = "Expiration Timestamp",
        Description = "The timestamp at which this transfer becomes invalid, in seconds since the Unix epoch.",
        Format = "int64")]
    public long ExpirationTimestamp { get; set; }
}