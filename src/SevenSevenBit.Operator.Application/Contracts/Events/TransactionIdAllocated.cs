namespace SevenSevenBit.Operator.Application.Contracts.Events;

/// <summary>
/// A message contract representing a transaction ID allocation event.
/// </summary>
public record TransactionIdAllocated
{
    /// <summary>
    /// Gets the correlation ID of the event.
    /// </summary>
    public Guid TransactionStreamId { get; init; }

    /// <summary>
    /// Gets the allocated transaction ID.
    /// </summary>
    public long TransactionId { get; init; }
}