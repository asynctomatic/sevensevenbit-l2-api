namespace SevenSevenBit.Operator.Application.DTOs.Entities;

using System.Text.Json.Serialization;
using NodaTime;
using SevenSevenBit.Operator.Domain.Entities;
using SevenSevenBit.Operator.Domain.Enums;
using StarkEx.Client.SDK.Models.Spot.TransactionModels;
using Swashbuckle.AspNetCore.Annotations;

public record TransactionDto
{
    [JsonConstructor]
    public TransactionDto(
        Guid transactionId,
        StarkExOperation operation,
        TransactionStatus status,
        TransactionModel starkExTransaction)
    {
        TransactionId = transactionId;
        Operation = operation;
        Status = status;
        StarkExTransaction = starkExTransaction;
    }

    public TransactionDto(Transaction transaction)
    {
        TransactionId = transaction.Id;
        Operation = transaction.Operation;
        Status = transaction.Status;
        StarkExTransaction = transaction.RawTransaction;
    }

    [SwaggerSchema(
        Title = "Transaction ID",
        Description = "The ID of the transaction.",
        Format = "uuid")]
    public Guid TransactionId { get; }

    [SwaggerSchema(
        Title = "Operation",
        Description = "The operation performed by the transaction.",
        Format = "string")]
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public StarkExOperation Operation { get; }

    [SwaggerSchema(
        Title = "Status",
        Description = "The status of the transaction.",
        Format = "string")]
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public TransactionStatus Status { get; }

    [SwaggerSchema(
        Title = "StarkEx Transaction",
        Description = "The StarkEx transaction model.",
        Format = "json")]
    public TransactionModel StarkExTransaction { get; }

    public static implicit operator TransactionDto(Transaction transaction) => new(transaction);
}