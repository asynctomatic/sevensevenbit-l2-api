namespace SevenSevenBit.Operator.Infrastructure.MessageBus.Options;

public class ConsumerOptions
{
    /// <summary>
    /// Gets or sets the retry options at the consumer level.
    /// </summary>
    public RetryOptions Retry { get; set; }
}