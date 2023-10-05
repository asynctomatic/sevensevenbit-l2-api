namespace SevenSevenBit.Operator.Application.DTOs;

using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;
using Swashbuckle.AspNetCore.Annotations;

[ExcludeFromCodeCoverage]
public record TransactionIdDto
{
    [JsonConstructor]
    public TransactionIdDto(Guid transactionId)
    {
        TransactionId = transactionId;
    }

    [SwaggerSchema(
        Title = "Transaction ID",
        Description = "The unique identifier of the transaction.",
        Format = "uuid")]
    public Guid TransactionId { get; set; }
}