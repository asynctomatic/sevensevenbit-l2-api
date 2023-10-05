namespace SevenSevenBit.Operator.Application.Contracts.Events;

/// <summary>
/// A message contract representing a transaction ID freeing event.
/// </summary>
public record TransactionIdFreed
{
    /// <summary>
    /// Gets the correlation ID of the event.
    /// </summary>
    public Guid TransactionStreamId { get; init; }
}