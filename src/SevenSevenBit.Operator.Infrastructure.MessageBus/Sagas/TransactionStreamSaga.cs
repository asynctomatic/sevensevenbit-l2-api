namespace SevenSevenBit.Operator.Infrastructure.MessageBus.Sagas;

using MassTransit;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NewRelic.Api.Agent;
using SevenSevenBit.Operator.Application.Contracts.Commands;
using SevenSevenBit.Operator.Application.Contracts.Events;
using SevenSevenBit.Operator.Infrastructure.MessageBus.Options;
using SevenSevenBit.Operator.Infrastructure.MessageBus.States;
using SevenSevenBit.Operator.SharedKernel.Telemetry.Extensions;

/// <summary>
/// Defines the states, events, and behavior of a finite state machine for handling transaction streaming.
/// This class is derived from <see cref="MassTransitStateMachine{T}"/> and is used to apply event-triggered behavior to instances of the state machine.
/// More information can be found <see href="https://masstransit-project.com/usage/sagas/automatonymous.html#state-machine">here</see>.
/// </summary>
public class TransactionStreamSaga : MassTransitStateMachine<TransactionStreamState>
{
    private readonly ILogger<TransactionStreamSaga> logger;
    private readonly MessageBusOptions messageBusOptions;

    /// <summary>
    /// Initializes a new instance of the <see cref="TransactionStreamSaga"/> class.
    /// </summary>
    /// <param name="logger">The logger to use for logging events and messages.</param>
    /// <param name="messageBusOptions">The message bus options to be used for configuration.</param>
    public TransactionStreamSaga(
        ILogger<TransactionStreamSaga> logger,
        IOptions<MessageBusOptions> messageBusOptions)
    {
        // Set the logger and options.
        this.logger = logger;
        this.messageBusOptions = messageBusOptions.Value;

        // Manually configure the state property of the saga instance (required for string types).
        // https://masstransit-project.com/usage/sagas/automatonymous.html#instance
        InstanceState(s => s.CurrentState);

        // Configure the correlation IDs for all events to correlate them to an instance.
        // https://masstransit-project.com/usage/sagas/automatonymous.html#event
        Event(() => StreamTransactionEvent, e =>
            e.CorrelateById(s => s.Message.TransactionStreamId));
        Event(() => TransactionIdAllocatedEvent, e =>
            e.CorrelateById(s => s.Message.TransactionStreamId));
        Event(() => AllocateTransactionIdFaultedEvent, e =>
            e.CorrelateById(s => s.Message.Message.TransactionStreamId));
        Event(() => TransactionIdFreedEvent, e =>
            e.CorrelateById(s => s.Message.TransactionStreamId));
        Event(() => FreeTransactionIdFaultedEvent, e =>
            e.CorrelateById(s => s.Message.Message.TransactionStreamId));
        Event(() => TransactionSubmittedEvent, e => e
            .CorrelateById(s => s.Message.TransactionStreamId));
        Event(() => SubmitTransactionFaultedEvent, e => e
            .CorrelateById(s => s.Message.Message.TransactionStreamId));

        // Configure event handlers and state transition behaviours.
        // https://masstransit-project.com/usage/sagas/automatonymous.html#behavior
        // TODO should free a TX ID when an unexpected TransactionIdAllocated event is received?
        Initially(SetStreamTransactionHandler());
        During(AllocatingId, SetTransactionIdAllocatedHandler(), SetAllocateTransactionIdFaultedHandler());
        During(SubmittingTx, SetTransactionSubmittedHandler(), SetSubmitTransactionFaultedHandler());
        During(FreeingId, SetTransactionIdFreedHandler(), SetFreeTransactionIdFaultedHandler());

        // Remove the saga instance from the repository once finalized.
        // https://masstransit-project.com/usage/sagas/automatonymous.html#completed-instance
        SetCompletedWhenFinalized();
    }

    /// <summary>
    /// Gets the state of the saga instance when a transaction ID is being allocated.
    /// </summary>
    public State AllocatingId { get; private set; }

    /// <summary>
    /// Gets the state of the saga instance when a transaction is being submitted.
    /// </summary>
    public State SubmittingTx { get; private set; }

    /// <summary>
    /// Gets the state of the saga instance when a transaction ID is being freed.
    /// </summary>
    public State FreeingId { get; private set; }

    /// <summary>
    /// Gets or sets an event that triggers the state machine to start streaming a transaction.
    /// </summary>
    public Event<StreamTransaction> StreamTransactionEvent { get; set; }

    /// <summary>
    /// Gets or sets an event that indicates that a transaction ID has been allocated.
    /// </summary>
    public Event<TransactionIdAllocated> TransactionIdAllocatedEvent { get; set; }

    /// <summary>
    /// Gets or sets an event that indicates that there was a fault when trying to allocate a transaction ID.
    /// </summary>
    public Event<Fault<AllocateTransactionId>> AllocateTransactionIdFaultedEvent { get; set; }

    /// <summary>
    /// Gets or sets an event that indicates that a transaction has been submitted.
    /// </summary>
    public Event<TransactionSubmitted> TransactionSubmittedEvent { get; set; }

    /// <summary>
    /// Gets or sets an event that indicates that there was a fault when trying to submit a transaction.
    /// </summary>
    public Event<Fault<SubmitTransaction>> SubmitTransactionFaultedEvent { get; set; }

    /// <summary>
    /// Gets or sets an event that indicates that a transaction ID has been freed.
    /// </summary>
    public Event<TransactionIdFreed> TransactionIdFreedEvent { get; set; }

    /// <summary>
    /// Gets or sets an event that indicates that there was a fault when trying to free a transaction ID.
    /// </summary>
    public Event<Fault<FreeTransactionId>> FreeTransactionIdFaultedEvent { get; set; }

    [Transaction(Web = false)]
    private static void NewRelicTransaction<T>(
        BehaviorContext<TransactionStreamState, T> behaviorContext)
        where T : class
    {
        TelemetryAgent.AddCustomAttribute("CorrelationId", behaviorContext.Saga.CorrelationId.ToString());
        TelemetryAgent.AddCustomAttribute("CurrentState", behaviorContext.Saga.CurrentState);
        TelemetryAgent.AddCustomAttribute("Version", behaviorContext.Saga.Version.ToString());
        TelemetryAgent.AddCustomAttribute("StarkExInstanceId", behaviorContext.Saga.StarkExInstanceId.ToString());
        TelemetryAgent.AddCustomAttribute("StarkExInstanceBaseAddress", behaviorContext.Saga.StarkExInstanceBaseAddress);
        TelemetryAgent.AddCustomAttribute("StarkExInstanceVersion", behaviorContext.Saga.StarkExInstanceVersion);
        TelemetryAgent.AddCustomAttribute("Transaction", behaviorContext.Saga.Transaction);
    }

    private EventActivityBinder<TransactionStreamState, StreamTransaction> SetStreamTransactionHandler()
    {
        return When(StreamTransactionEvent)
            .Then(ctx =>
            {
                logger.LogInformation("Handle StreamTransactionEvent: {Id}", ctx.Message.TransactionStreamId);

                ctx.Saga.StarkExInstanceId = ctx.Message.StarkExInstanceId;
                ctx.Saga.StarkExInstanceBaseAddress = ctx.Message.StarkExInstanceBaseAddress;
                ctx.Saga.StarkExInstanceVersion = ctx.Message.StarkExInstanceApiVersion;
                ctx.Saga.Transaction = ctx.Message.Transaction;
            })
            .SendAsync(
                new Uri(messageBusOptions.Endpoints!.AllocateTransactionId!),
                ctx => ctx.Init<AllocateTransactionId>(new AllocateTransactionId
                {
                    TransactionStreamId = ctx.Message.TransactionStreamId,
                    StarkExInstanceId = ctx.Message.StarkExInstanceId,
                }))
            .Then(NewRelicTransaction)
            .TransitionTo(AllocatingId);
    }

    private EventActivityBinder<TransactionStreamState, TransactionIdAllocated> SetTransactionIdAllocatedHandler()
    {
        return When(TransactionIdAllocatedEvent)
            .Then(ctx =>
            {
                logger.LogInformation("Handle TransactionIdAllocatedEvent: {Id}", ctx.Message.TransactionStreamId);
            })
            .SendAsync(
                new Uri(messageBusOptions.Endpoints!.SubmitTransaction!),
                ctx => ctx.Init<SubmitTransaction>(new SubmitTransaction
                {
                    TransactionStreamId = ctx.Message.TransactionStreamId,
                    StarkExBaseAddress = ctx.Saga.StarkExInstanceBaseAddress,
                    StarkExVersion = ctx.Saga.StarkExInstanceVersion,
                    TransactionSequenceId = ctx.Message.TransactionId,
                    Transaction = ctx.Saga.Transaction,
                }))
            .Then(NewRelicTransaction)
            .TransitionTo(SubmittingTx);
    }

    private EventActivityBinder<TransactionStreamState, Fault<AllocateTransactionId>> SetAllocateTransactionIdFaultedHandler()
    {
        return When(AllocateTransactionIdFaultedEvent)
            .Then(ctx =>
            {
                // This is logged as warning because it triggers the saga compensation path.
                logger.LogWarning("Handle AllocateTransactionIdFaultedEvent: {Id}", ctx.Message.Message.TransactionStreamId);
            })
            .SendAsync(
                new Uri(messageBusOptions.Endpoints!.FreeTransactionId!),
                ctx => ctx.Init<FreeTransactionId>(new FreeTransactionId
                {
                    TransactionStreamId = ctx.Message.Message.TransactionStreamId,
                    StarkExInstanceId = ctx.Message.Message.StarkExInstanceId,
                }))
            .Then(NewRelicTransaction)
            .TransitionTo(FreeingId);
    }

    private EventActivityBinder<TransactionStreamState, TransactionSubmitted> SetTransactionSubmittedHandler()
    {
        return When(TransactionSubmittedEvent)
            .Then(ctx =>
            {
                logger.LogInformation("Handle TransactionSubmittedEvent: {Id}", ctx.Message.TransactionStreamId);
            })
            .PublishAsync(context => context.Init<TransactionStreamResult>(new TransactionStreamResult
            {
                TransactionStreamId = context.Message.TransactionStreamId,
                TransactionSequenceId = context.Message.TransactionSequenceId,
                Success = true,
            }))
            .Then(NewRelicTransaction)
            .Finalize();
    }

    private EventActivityBinder<TransactionStreamState, Fault<SubmitTransaction>> SetSubmitTransactionFaultedHandler()
    {
        return When(SubmitTransactionFaultedEvent)
            .Then(ctx =>
            {
                // This is logged as warning because it triggers the saga compensation path.
                logger.LogWarning("Handle SubmitTransactionFaultedEvent: {Id}", ctx.Message.Message.TransactionStreamId);
            })
            .SendAsync(
                new Uri(messageBusOptions.Endpoints!.FreeTransactionId!),
                ctx => ctx.Init<FreeTransactionId>(new FreeTransactionId
                {
                    TransactionStreamId = ctx.Message.Message.TransactionStreamId,
                    StarkExInstanceId = ctx.Saga.StarkExInstanceId,
                }))
            .Then(NewRelicTransaction)
            .TransitionTo(FreeingId);
    }

    private EventActivityBinder<TransactionStreamState, TransactionIdFreed> SetTransactionIdFreedHandler()
    {
        return When(TransactionIdFreedEvent)
            .Then(ctx =>
            {
                logger.LogInformation("Handle TransactionIdFreedEvent: {Id}", ctx.Message.TransactionStreamId);
            })
            .PublishAsync(context => context.Init<TransactionStreamResult>(new TransactionStreamResult
            {
                TransactionStreamId = context.Message.TransactionStreamId,
                Success = false,
            }))
            .Then(NewRelicTransaction)
            .Finalize();
    }

    private EventActivityBinder<TransactionStreamState, Fault<FreeTransactionId>> SetFreeTransactionIdFaultedHandler()
    {
        return When(FreeTransactionIdFaultedEvent)
            .Then(ctx =>
            {
                // This is logged as critical because a compensating transaction failed.
                // Must be handled manually, by repairing the tx stream gap, to ensure system liveliness.
                logger.LogCritical("Handle FreeTransactionIdFaultedEvent: {Id}", ctx.Message.Message.TransactionStreamId);
            })
            .PublishAsync(context => context.Init<TransactionStreamResult>(new TransactionStreamResult
            {
                TransactionStreamId = context.Message.Message.TransactionStreamId,
                Success = false,
            }))
            .Then(NewRelicTransaction)
            .Finalize();
    }
}