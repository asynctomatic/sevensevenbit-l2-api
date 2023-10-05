namespace SevenSevenBit.Operator.Infrastructure.MessageBus.Options;

using System.Diagnostics.CodeAnalysis;

/// <summary>
/// Represents configuration options for a state machine saga.
/// </summary>
[ExcludeFromCodeCoverage]
public class SagaOptions
{
    /// <summary>
    /// Gets or sets the endpoint name of the saga.
    /// </summary>
    public string EndpointName { get; set; }

    /// <summary>
    /// Gets or sets the concurrency limit for the saga.
    /// </summary>
    public int ConcurrencyLimit { get; set; }
}