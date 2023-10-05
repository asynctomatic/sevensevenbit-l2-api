namespace SevenSevenBit.Operator.Infrastructure.MessageBus.Consumers.Definitions;

using System.Diagnostics.CodeAnalysis;
using MassTransit;
using Microsoft.Extensions.Options;
using SevenSevenBit.Operator.Infrastructure.MessageBus.Options;
using SevenSevenBit.Operator.Infrastructure.MessageBus.States;

[ExcludeFromCodeCoverage]
public class TransactionStreamStateDefinition : SagaDefinition<TransactionStreamState>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="TransactionStreamStateDefinition"/> class.
    /// </summary>
    /// <param name="messageBusOptions">The message bus options to be used for configuration.</param>
    public TransactionStreamStateDefinition(IOptions<MessageBusOptions> messageBusOptions)
    {
        var options = messageBusOptions.Value;

        // Override the default endpoint name.
        // https://masstransit-project.com/usage/configuration.html#endpoint-definition
        Endpoint(e =>
        {
            e.Name = options.Saga!.EndpointName!;
            e.PrefetchCount = options.Saga!.ConcurrencyLimit;
        });
    }

    /// <inheritdoc />
    protected override void ConfigureSaga(
        IReceiveEndpointConfigurator endpointConfigurator,
        ISagaConfigurator<TransactionStreamState> sagaConfigurator)
    {
        // An outbox holds published and sent messages in memory until the message is processed successfully.
        // This helps avoid duplicate messages in the event of a concurrency failure.
        // https://masstransit-project.com/articles/outbox.html#in-memory-outbox-take-out-the-trash
        // https://masstransit-project.com/usage/sagas/guidance.html#concurrency
        endpointConfigurator.UseInMemoryOutbox();

        // Configure partitions (skipped for now).
        // http://masstransit-project.com/usage/sagas/guidance.html#saga-guidance

        // Enable message retries (skipped for now).
        // https://masstransit-project.com/usage/exceptions.html#retry
    }
}