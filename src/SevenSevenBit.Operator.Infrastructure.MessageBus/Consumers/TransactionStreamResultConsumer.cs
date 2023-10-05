namespace SevenSevenBit.Operator.Infrastructure.MessageBus.Consumers;

using MassTransit;
using Microsoft.Extensions.Logging;
using SevenSevenBit.Operator.Application.Common.Interfaces;
using SevenSevenBit.Operator.Application.Contracts.Events;
using SevenSevenBit.Operator.Domain.Entities;
using SevenSevenBit.Operator.Domain.Enums;

public class TransactionStreamResultConsumer : IConsumer<TransactionStreamResult>
{
    private readonly ILogger<TransactionStreamResultConsumer> logger;
    private readonly IUnitOfWork unitOfWork;

    public TransactionStreamResultConsumer(
        ILogger<TransactionStreamResultConsumer> logger,
        IUnitOfWork unitOfWork)
    {
        this.logger = logger;
        this.unitOfWork = unitOfWork;
    }

    public async Task Consume(ConsumeContext<TransactionStreamResult> context)
    {
        var dbTransaction = await unitOfWork.Repository<Transaction>().GetByIdAsync(
            context.Message.TransactionStreamId,
            CancellationToken.None);

        if (dbTransaction == null)
        {
            logger.LogError(
                "Transaction with ID {TransactionId} not found",
                context.Message.TransactionStreamId);
            return;
        }

        // Prevent processing of already processed transactions
        if (dbTransaction.Status != TransactionStatus.Streamed)
        {
            logger.LogWarning(
                "Transaction {TransactionId} was received with the status {TxStatus} instead of expected status Streamed",
                dbTransaction.Id,
                dbTransaction.Status);

            return;
        }

        // Transaction failed to be submitted to StarkEx
        if (!context.Message.Success)
        {
            logger.LogInformation(
                "Transaction {TransactionId} failed to be submitted to StarkEx. Reverting vault updates",
                dbTransaction.Id);

            dbTransaction.Status = TransactionStatus.Failed;
            foreach (var vaultUpdate in dbTransaction.VaultUpdates)
            {
                // Restore initial accounting balance
                vaultUpdate.Vault.QuantizedAccountingBalance -= vaultUpdate.QuantizedAmount;

                // Restore captives values
                if (vaultUpdate.QuantizedAmount < 0)
                {
                    vaultUpdate.Vault.QuantizedAvailableBalance -= vaultUpdate.QuantizedAmount;
                }
            }

            await unitOfWork.SaveAsync(CancellationToken.None);

            return;
        }

        // Transaction was successfully submitted to StarkEx
        logger.LogInformation(
            "Transaction {TransactionId} was successfully submitted to StarkEx. Crediting pending values",
            dbTransaction.Id);

        dbTransaction.Status = TransactionStatus.Pending;
        dbTransaction.StarkExTransactionId = context.Message.TransactionSequenceId;
        foreach (var vaultUpdate in dbTransaction.VaultUpdates.Where(vaultUpdate => vaultUpdate.QuantizedAmount > 0))
        {
            vaultUpdate.Vault.QuantizedAvailableBalance += vaultUpdate.QuantizedAmount;
        }

        await unitOfWork.SaveAsync(CancellationToken.None);
    }
}