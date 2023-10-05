namespace SevenSevenBit.Operator.Infrastructure.MessageBus.States;

using System.Diagnostics.CodeAnalysis;
using MassTransit;

/// <summary>
/// Represents an instance of a state machine for handling transaction streaming.
/// A new instance is created for every consumed initial event where an existing instance with the same CorrelationId was not found.
/// This class implements the <see cref="ISagaVersion"/> interface, which is required for storing the saga in Redis using optimistic concurrency.
/// More information can be found <see href="https://masstransit-project.com/usage/sagas/automatonymous.html#instance">here</see>.
/// </summary>
[ExcludeFromCodeCoverage]
public class TransactionStreamState : SagaStateMachineInstance, ISagaVersion
{
    /// <summary>
    /// Gets or sets the unique identifier used to correlate this instance with other events.
    /// </summary>
    public Guid CorrelationId { get; set; }

    /// <summary>
    /// Gets or sets the current state of this instance.
    /// </summary>
    public string CurrentState { get; set; }

    /// <summary>
    /// Gets or sets the version of the Saga.
    /// </summary>
    public int Version { get; set; }

    /// <summary>
    /// Gets or sets the instance identifier of the StarkEx system.
    /// </summary>
    public Guid StarkExInstanceId { get; set; }

    /// <summary>
    /// Gets or sets the Gateway API endpoint of the StarkEx instance.
    /// </summary>
    public string StarkExInstanceBaseAddress { get; set; }

    /// <summary>
    /// Gets or sets the version of the StarkEx instance.
    /// </summary>
    public string StarkExInstanceVersion { get; set; }

    /// <summary>
    /// Gets or sets the transaction data for this instance.
    /// </summary>
    public string Transaction { get; set; }
}