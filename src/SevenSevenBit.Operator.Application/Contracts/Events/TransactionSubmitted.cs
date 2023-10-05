namespace SevenSevenBit.Operator.Application.Contracts.Events;

/// <summary>
/// A message contract representing a transaction submission event.
/// </summary>
public record TransactionSubmitted
{
    /// <summary>
    /// Gets the correlation ID of the event.
    /// </summary>
    public Guid TransactionStreamId { get; init; }

    /// <summary>
    /// Gets the sequence ID of the transaction.
    /// </summary>
    public long TransactionSequenceId { get; init; }
}