namespace SevenSevenBit.Operator.Application.Contracts.Commands;

/// <summary>
/// A message contract representing a command to submit a transaction.
/// </summary>
public record SubmitTransaction
{
    /// <summary>
    /// Gets or sets the correlation ID of the command.
    /// </summary>
    public Guid TransactionStreamId { get; set; }

    /// <summary>
    /// Gets or sets the Gateway API base address of the StarkEx instance.
    /// </summary>
    public string StarkExBaseAddress { get; set; }

    /// <summary>
    /// Gets or sets the Gateway API version of the StarkEx instance.
    /// </summary>
    public string StarkExVersion { get; set; }

    /// <summary>
    /// Gets or sets the sequence ID of the transaction.
    /// </summary>
    public long TransactionSequenceId { get; set; }

    /// <summary>
    /// Gets or sets the transaction to submit.
    /// </summary>
    public string Transaction { get; set; }
}