namespace SevenSevenBit.Operator.Infrastructure.MessageBus.Options;

using System.Diagnostics.CodeAnalysis;

/// <summary>
/// Represents configuration options for connecting to a message bus.
/// </summary>
[ExcludeFromCodeCoverage]
public class MessageBusOptions
{
    /// <summary>
    /// The configuration key for accessing the message bus options in a configuration file.
    /// </summary>
    public const string MessageBus = "Services:MessageBus";

    /// <summary>
    /// Gets or sets the Azure Service Bus options, to be used in the prod environment.
    /// </summary>
    public AzureServiceBusOptions AzureServiceBus { get; set; }

    /// <summary>
    /// Gets or sets the RabbitMq options, should only be used in dev environments.
    /// </summary>
    public RabbitMqOptions RabbitMq { get; set; }

    /// <summary>
    /// Gets or sets the saga options.
    /// </summary>
    public SagaOptions Saga { get; set; }

    /// <summary>
    /// Gets or sets the message bus endpoint mappings.
    /// </summary>
    public EndpointOptions Endpoints { get; set; }

    /// <summary>
    /// Gets or sets the consumer options for the message bus.
    /// </summary>
    public ConsumersOptions Consumers { get; set; }

    /// <summary>
    /// Gets or sets the retry options with the message bus.
    /// </summary>
    public RetryOptions Retry { get; set; }
}