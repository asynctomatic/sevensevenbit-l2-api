namespace SevenSevenBit.Operator.Application.Contracts.Events;

/// <summary>
/// A message contract representing the result of a transaction stream saga.
/// </summary>
public record TransactionStreamResult
{
    /// <summary>
    /// Gets the correlation ID of the transaction stream.
    /// </summary>
    public Guid TransactionStreamId { get; init; }

    /// <summary>
    /// Gets the sequence ID of the transaction.
    /// </summary>
    public long TransactionSequenceId { get; init; }

    /// <summary>
    /// Gets a value indicating whether the transaction stream was successful.
    /// </summary>
    public bool Success { get; init; }
}