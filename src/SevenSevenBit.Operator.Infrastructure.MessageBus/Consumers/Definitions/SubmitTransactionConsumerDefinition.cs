namespace SevenSevenBit.Operator.Infrastructure.MessageBus.Consumers.Definitions;

using System.Diagnostics.CodeAnalysis;
using System.Security.Authentication;
using MassTransit;
using Microsoft.Extensions.Options;
using SevenSevenBit.Operator.Infrastructure.MessageBus.Options;
using StarkEx.Client.SDK.Enums.Spot;
using StarkEx.Client.SDK.Exceptions;

/// <summary>
/// Configure the behaviour of a consumer of type <see cref="SubmitTransactionConsumer"/>.
/// More information on consumers can be found <see href="https://masstransit-project.com/usage/containers/definitions.html">here</see>.
/// </summary>
[ExcludeFromCodeCoverage]
public class SubmitTransactionConsumerDefinition : ConsumerDefinition<SubmitTransactionConsumer>
{
    private readonly MessageBusOptions messageBusOptions;

    /// <summary>
    /// Initializes a new instance of the <see cref="SubmitTransactionConsumerDefinition"/> class.
    /// </summary>
    /// <param name="messageBusOptions">The message bus options to be used for configuration.</param>
    public SubmitTransactionConsumerDefinition(IOptions<MessageBusOptions> messageBusOptions)
    {
        this.messageBusOptions = messageBusOptions.Value;

        // Override the default endpoint name.
        // https://masstransit-project.com/usage/configuration.html#endpoint-definition
        EndpointName = this.messageBusOptions.Endpoints!.SubmitTransaction!;
    }

    /// <inheritdoc />
    protected override void ConfigureConsumer(
        IReceiveEndpointConfigurator endpointConfigurator,
        IConsumerConfigurator<SubmitTransactionConsumer> consumerConfigurator)
    {
        // Disable the creation of a topic/exchange for the consumer message type.
        // https://masstransit-project.com/advanced/topology/message.html#configureconsumetopology-attribute
        // TODO endpointConfigurator.ConfigureConsumeTopology = false;

        // Enable message retries.
        // https://masstransit-project.com/usage/exceptions.html#retry
        endpointConfigurator.UseMessageRetry(r =>
        {
            // Configure exponential backoff policy.
            // https://masstransit-project.com/usage/exceptions.html#retry-configuration
            r.SetRetryPolicy(p => p.Exponential(
                retryLimit: messageBusOptions.Retry!.Limit,
                minInterval: TimeSpan.FromMilliseconds(messageBusOptions.Retry!.MinIntervalMs),
                maxInterval: TimeSpan.FromMilliseconds(messageBusOptions.Retry!.MaxIntervalMs),
                intervalDelta: TimeSpan.FromMilliseconds(messageBusOptions.Retry!.IntervalDeltaMs)));

            // Only retry on specific exceptions.
            // https://masstransit-project.com/usage/exceptions.html#retry-configuration
            r.Handle<StarkExErrorException>(e => e.Code.Equals(SpotApiCodes.ConnectionError));

            // An authentication wrapped in an HttpRequestException is thrown when the mTLS fails.
            r.Handle<HttpRequestException>(e => e.InnerException is not AuthenticationException);
            r.Handle<TaskCanceledException>();

            // Explicitly ignore certain exceptions.
            // https://masstransit-project.com/usage/exceptions.html#retry-configuration
            r.Ignore<ArgumentNullException>();
        });
    }
}