namespace SevenSevenBit.Operator.Infrastructure.MessageBus.Consumers;

using MassTransit;
using Microsoft.Extensions.Logging;
using SevenSevenBit.Operator.Application.Contracts.Commands;
using SevenSevenBit.Operator.Application.Contracts.Events;
using SevenSevenBit.Operator.Application.Interfaces.Services;
using SevenSevenBit.Operator.SharedKernel.Telemetry.Attributes;

/// <summary>
/// A consumer that consumes messages of type <see cref="AllocateTransactionId"/> and is responsible for allocating a new transaction ID in response.
/// More information on consumers can be found <see href="https://masstransit-project.com/usage/consumers.html#consumers">here</see>.
/// </summary>
public class AllocateTransactionIdConsumer : IConsumer<AllocateTransactionId>
{
    private readonly ILogger<AllocateTransactionIdConsumer> logger;
    private readonly ITransactionIdService transactionIdService;

    /// <summary>
    /// Initializes a new instance of the <see cref="AllocateTransactionIdConsumer"/> class.
    /// </summary>
    /// <param name="logger">An instance of the <see cref="ILogger{AllocateTransactionIdConsumer}"/> interface for logging.</param>
    /// <param name="transactionIdService">An instance of the <see cref="ITransactionIdService"/> interface for allocating transaction IDs.</param>
    public AllocateTransactionIdConsumer(
        ILogger<AllocateTransactionIdConsumer> logger,
        ITransactionIdService transactionIdService)
    {
        this.logger = logger;
        this.transactionIdService = transactionIdService;
    }

    /// <summary>
    /// Consumes a message of type <see cref="AllocateTransactionId"/> by allocating a new transaction ID and responding with a message of type <see cref="TransactionIdAllocated"/>.
    /// </summary>
    /// <param name="context">An instance of the <see cref="ConsumeContext{AllocateTransactionId}"/> representing the context of the consumed message.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    [Transaction(Web = false)]
    public async Task Consume(ConsumeContext<AllocateTransactionId> context)
    {
        logger.LogInformation("Consumed command: {Id}", context.Message.TransactionStreamId);

        var transactionId = await transactionIdService.AllocateTransactionIdAsync(
            context.Message.TransactionStreamId,
            context.Message.StarkExInstanceId,
            context.CancellationToken);

        await context.RespondAsync(new TransactionIdAllocated
        {
            TransactionStreamId = context.Message.TransactionStreamId,
            TransactionId = transactionId,
        });
    }
}