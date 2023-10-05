namespace SevenSevenBit.Operator.Infrastructure.MessageBus.Consumers;

using MassTransit;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SevenSevenBit.Operator.Application.Contracts.Commands;
using SevenSevenBit.Operator.Application.Contracts.Events;
using SevenSevenBit.Operator.Application.Interfaces.Services;
using SevenSevenBit.Operator.Application.Options;
using SevenSevenBit.Operator.SharedKernel.Telemetry.Attributes;

/// <summary>
/// A consumer that consumes messages of type <see cref="FreeTransactionId"/> and is responsible for freeing a previously allocated transaction ID.
/// More information on consumers can be found <see href="https://masstransit-project.com/usage/consumers.html#consumers">here</see>.
/// </summary>
public class FreeTransactionIdConsumer : IConsumer<FreeTransactionId>
{
    private readonly ILogger<FreeTransactionIdConsumer> logger;
    private readonly ITransactionIdService transactionIdService;
    private readonly FeatureTogglesOptions featureToggleOptions;

    /// <summary>
    /// Initializes a new instance of the <see cref="FreeTransactionIdConsumer"/> class.
    /// </summary>
    /// <param name="featureToggleOptions">Feature toggle settings.</param>
    /// <param name="logger">An instance of the <see cref="ILogger{FreeTransactionId}"/> interface for logging.</param>
    /// <param name="transactionIdService">An instance of the <see cref="ITransactionIdService"/> interface for freeing transaction IDs.</param>
    public FreeTransactionIdConsumer(
        IOptions<FeatureTogglesOptions> featureToggleOptions,
        ILogger<FreeTransactionIdConsumer> logger,
        ITransactionIdService transactionIdService)
    {
        this.featureToggleOptions = featureToggleOptions.Value;
        this.logger = logger;
        this.transactionIdService = transactionIdService;
    }

    /// <summary>
    /// Consumes a message of type <see cref="FreeTransactionId"/> by freeing the previously allocated transaction ID and responding with a message of type <see cref="TransactionIdFreed"/>.
    /// </summary>
    /// <param name="context">An instance of the <see cref="ConsumeContext{FreeTransactionId}"/> representing the context of the consumed message.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    [Transaction(Web = false)]
    public async Task Consume(ConsumeContext<FreeTransactionId> context)
    {
        logger.LogInformation("Consumed command: {Id}", context.Message.TransactionStreamId);

        if (!featureToggleOptions.GenerateTransactionIdFromStarkExApi)
        {
            await transactionIdService.FreeTransactionIdAsync(
                context.Message.TransactionStreamId, context.Message.StarkExInstanceId);
        }

        await context.RespondAsync(new TransactionIdFreed
        {
            TransactionStreamId = context.Message.TransactionStreamId,
        });
    }
}