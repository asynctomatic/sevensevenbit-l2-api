namespace SevenSevenBit.Operator.Application.DTOs.Details;

using System.Diagnostics.CodeAnalysis;
using Swashbuckle.AspNetCore.Annotations;

[ExcludeFromCodeCoverage]
public class TransferSignableDto
{
    [SwaggerSchema(
        Title = "Signable Payload",
        Description = "The signable payload for the transfer.",
        Format = "hex")]
    public string Signable { get; set; }

    [SwaggerSchema(
        Title = "Expiration Timestamp",
        Description = "The timestamp at which this transfer becomes invalid, in seconds since the Unix epoch.",
        Format = "int64")]
    public long ExpirationTimestamp { get; set; }

    [SwaggerSchema(
        Title = "Nonce",
        Description = "The unique nonce for the transfer.",
        Format = "int32")]
    public int Nonce { get; set; }
}