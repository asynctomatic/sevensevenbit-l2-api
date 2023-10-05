namespace SevenSevenBit.Operator.Infrastructure.MessageBus.Consumers;

using System.Text.Json;
using MassTransit;
using Microsoft.Extensions.Logging;
using SevenSevenBit.Operator.Application.Contracts.Commands;
using SevenSevenBit.Operator.Application.Contracts.Events;
using SevenSevenBit.Operator.Application.Interfaces.Services;
using SevenSevenBit.Operator.SharedKernel.Telemetry.Attributes;
using StarkEx.Client.SDK.Models.Spot.TransactionModels;

/// <summary>
/// A consumer that consumes messages of type <see cref="SubmitTransaction"/> and is responsible for sending
/// transactions to the StarkEx API.
/// More information on consumers can be found <see href="https://masstransit-project.com/usage/consumers.html#consumers">here</see>.
/// </summary>
public class SubmitTransactionConsumer : IConsumer<SubmitTransaction>
{
    private readonly ILogger<SubmitTransactionConsumer> logger;
    private readonly IStarkExApiGateway starkExApiGateway;

    /// <summary>
    /// Initializes a new instance of the <see cref="SubmitTransactionConsumer"/> class.
    /// </summary>
    /// <param name="logger">An instance of the <see cref="ILogger{SubmitTransactionConsumer}"/> interface for logging.</param>
    /// <param name="starkExApiGateway">An instance of the <see cref="IStarkExApiGateway"/> interface for sending transaction to StarkEx.</param>
    public SubmitTransactionConsumer(
        ILogger<SubmitTransactionConsumer> logger,
        IStarkExApiGateway starkExApiGateway)
    {
        this.logger = logger;
        this.starkExApiGateway = starkExApiGateway;
    }

    /// <summary>
    /// Consumes a message of type <see cref="SubmitTransaction"/> by submitting a transaction to the StarkEx API and
    /// responding with a message of type <see cref="TransactionSubmitted"/>.
    /// </summary>
    /// <param name="context">An instance of the <see cref="ConsumeContext{AllocateTransactionId}"/> representing the
    /// context of the consumed message.</param>
    /// <exception cref="ArgumentNullException">Thrown when the unable to decode the transaction.</exception>
    /// <returns>A task that represents the asynchronous operation.</returns>
    [Transaction(Web = false)]
    public async Task Consume(ConsumeContext<SubmitTransaction> context)
    {
        logger.LogInformation("Consumed command: {Id}", context.Message.TransactionStreamId);

        var transaction = JsonSerializer.Deserialize<TransactionModel>(context.Message.Transaction!);
        if (transaction == null)
        {
            throw new ArgumentNullException(nameof(transaction), "Unable to deserialize transaction.");
        }

        await starkExApiGateway.SendTransactionAsync(
            new Uri(context.Message.StarkExBaseAddress),
            context.Message.StarkExVersion,
            transaction,
            context.Message.TransactionSequenceId,
            context.CancellationToken);

        await context.RespondAsync(new TransactionSubmitted
        {
            TransactionStreamId = context.Message.TransactionStreamId,
            TransactionSequenceId = context.Message.TransactionSequenceId,
        });
    }
}