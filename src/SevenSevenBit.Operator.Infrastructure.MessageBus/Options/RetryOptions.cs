namespace SevenSevenBit.Operator.Infrastructure.MessageBus.Options;

using System.Diagnostics.CodeAnalysis;

/// <summary>
/// Represents configuration options for the retry policy of message bus communication.
/// </summary>
[ExcludeFromCodeCoverage]
public class RetryOptions
{
    /// <summary>
    /// Gets or sets the retry limit when trying to connect to the message bus.
    /// </summary>
    public int Limit { get; set; } = int.MaxValue;

    /// <summary>
    /// Gets or sets the interval in milliseconds between retries.
    /// </summary>
    public int IntervalMs { get; set; }

    /// <summary>
    /// Gets or sets the minimum interval time in milliseconds between exp backoff retries.
    /// </summary>
    public int MinIntervalMs { get; set; }

    /// <summary>
    /// Gets or sets the maximum interval time in milliseconds between exp backoff retries.
    /// </summary>
    public int MaxIntervalMs { get; set; } = int.MaxValue;

    /// <summary>
    /// Gets or sets the delta interval time in milliseconds between exp backoff retries.
    /// </summary>
    public int IntervalDeltaMs { get; set; } = 1000;
}