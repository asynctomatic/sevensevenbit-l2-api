namespace SevenSevenBit.Operator.Application.Contracts.Commands;

/// <summary>
/// A message contract representing a command to free a transaction ID.
/// </summary>
public record FreeTransactionId
{
    /// <summary>
    /// Gets the correlation id of the command.
    /// </summary>
    public Guid TransactionStreamId { get; init; }

    /// <summary>
    /// Gets the ID of the StarkEx instance.
    /// </summary>
    public Guid StarkExInstanceId { get; init; }
}