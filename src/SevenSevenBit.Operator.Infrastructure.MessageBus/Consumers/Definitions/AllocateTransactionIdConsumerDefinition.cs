namespace SevenSevenBit.Operator.Infrastructure.MessageBus.Consumers.Definitions;

using System.Diagnostics.CodeAnalysis;
using MassTransit;
using Microsoft.Extensions.Options;
using SevenSevenBit.Operator.Infrastructure.MessageBus.Options;

/// <summary>
/// Configure the behaviour of a consumer of type <see cref="AllocateTransactionIdConsumer"/>.
/// More information on consumers can be found <see href="https://masstransit-project.com/usage/containers/definitions.html">here</see>.
/// </summary>
[ExcludeFromCodeCoverage]
public class AllocateTransactionIdConsumerDefinition
    : ConsumerDefinition<AllocateTransactionIdConsumer>
{
    private readonly MessageBusOptions messageBusOptions;

    /// <summary>
    /// Initializes a new instance of the <see cref="AllocateTransactionIdConsumerDefinition"/> class.
    /// </summary>
    /// <param name="messageBusOptions">The message bus options to be used for configuration.</param>
    public AllocateTransactionIdConsumerDefinition(IOptions<MessageBusOptions> messageBusOptions)
    {
        this.messageBusOptions = messageBusOptions.Value;

        // Override the default endpoint name.
        // https://masstransit-project.com/usage/configuration.html#endpoint-definition
        EndpointName = messageBusOptions.Value.Endpoints!.AllocateTransactionId!;
    }

    /// <inheritdoc />
    protected override void ConfigureConsumer(
        IReceiveEndpointConfigurator endpointConfigurator,
        IConsumerConfigurator<AllocateTransactionIdConsumer> consumerConfigurator)
    {
        // Disable the creation of a topic/exchange for the consumer message type.
        // https://masstransit-project.com/advanced/topology/message.html#configureconsumetopology-attribute
        // TODO endpointConfigurator.ConfigureConsumeTopology = false;

        // Enable message retries.
        // https://masstransit-project.com/usage/exceptions.html#retry
        endpointConfigurator.UseMessageRetry(r =>
        {
            // Configure retry policy.
            // https://masstransit-project.com/usage/exceptions.html#retry-configuration
            r.SetRetryPolicy(p => p.Interval(
                messageBusOptions.Retry!.Limit, TimeSpan.FromMilliseconds(messageBusOptions.Retry!.IntervalMs)));
        });
    }
}