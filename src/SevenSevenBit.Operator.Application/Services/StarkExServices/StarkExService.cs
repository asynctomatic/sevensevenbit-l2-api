namespace SevenSevenBit.Operator.Application.Services.StarkExServices;

using Microsoft.Extensions.Logging;
using NodaTime;
using NodaTime.Extensions;
using SevenSevenBit.Operator.Application.Common.Interfaces;
using SevenSevenBit.Operator.Application.Interfaces.Services;
using SevenSevenBit.Operator.Domain.Entities;
using SevenSevenBit.Operator.Domain.Enums;
using StarkEx.Client.SDK.Enums.Spot;
using StarkEx.Client.SDK.Models.Spot.TransactionModels;

public class StarkExService : IStarkExService
{
    private readonly ILogger<StarkExService> logger;
    private readonly IUnitOfWork unitOfWork;

    public StarkExService(
        ILogger<StarkExService> logger,
        IUnitOfWork unitOfWork)
    {
        this.logger = logger;
        this.unitOfWork = unitOfWork;
    }

    public async Task<IEnumerable<TransactionModel>> GetAlternativeTransactionsAsync(
        Transaction transaction,
        SpotApiCodes reasonCode,
        string errorMsg,
        CancellationToken cancellationToken)
    {
        // Handle REPLACE_BEFORE error - https://docs.starkware.co/starkex/con_about_handling_invalid_transactions.html#built_in_safety_StarkEx
        if (reasonCode.Equals(SpotApiCodes.ReplacedBefore))
        {
            var replacementTransactions = transaction.ReplacementTransactions;

            if (replacementTransactions is null || !replacementTransactions.Any())
            {
                logger.LogCritical(
                    "Reverted transaction number {TxId} was never replaced before",
                    transaction.StarkExTransactionId);

                // Return null to force 400 response
                return null;
            }

            // Since the LocalDateTime has a precision of 10^-6 seconds and the fact that this is an iterative process,
            // sorting by the RevertedDate should be safe
            var latestReplacementTransaction = replacementTransactions
                .OrderBy(x => x.RevertedDate)
                .Last();

            // TODO if (latestReplacementTransaction.ReplacementCounter >= tenantContext.StarkExInstanceDetails.MaxNrOfTransactionReplacements)
            if (latestReplacementTransaction.ReplacementCounter >= 3)
            {
                logger.LogCritical(
                    "Reverted transaction number {TxId} has reached the maximum number of replacements. Returning empty list",
                    transaction.StarkExTransactionId);

                // Revert state from maxed out replacement transaction
                foreach (var replacementVaultUpdate in latestReplacementTransaction.ReplacementVaultUpdates)
                {
                    var vault = replacementVaultUpdate.Vault;

                    vault.QuantizedAvailableBalance -= replacementVaultUpdate.QuantizedAmount;
                }

                // Add latest replacement transaction
                var newReplacementTransaction = new ReplacementTransaction
                {
                    TransactionId = transaction.Id,
                    ErrorCode = reasonCode,
                    ErrorMessage = errorMsg,
                    RawReplacementTransactions = Enumerable.Empty<TransactionModel>(),
                };

                await unitOfWork.Repository<ReplacementTransaction>().InsertAsync(newReplacementTransaction, cancellationToken);
                await unitOfWork.SaveAsync(cancellationToken);

                return newReplacementTransaction.RawReplacementTransactions;
            }

            latestReplacementTransaction.ReplacementCounter++;
            await unitOfWork.SaveAsync(cancellationToken);

            logger.LogCritical(
                "Replacing transaction {TxId} for the {ReplacementCounter} time",
                transaction.StarkExTransactionId,
                latestReplacementTransaction.ReplacementCounter);

            return latestReplacementTransaction.RawReplacementTransactions;
        }

        // Revert transaction
        transaction.Status = TransactionStatus.Reverted;

        // TODO We should have a job to delete unused Vaults or mechanism to free these vaults
        foreach (var vaultUpdate in transaction.VaultUpdates)
        {
            var vault = vaultUpdate.Vault;

            vault.QuantizedAvailableBalance -= vaultUpdate.QuantizedAmount;
        }

        // TODO: Replace this with new order match mechanism
        // foreach (var productUpdate in transaction.ProductUpdates)
        // {
        //     var product = productUpdate.Product;
        //
        //     product.QuantizedAvailableQuantity -= productUpdate.QuantizedAmount;
        // }

        // Insert data for manual tx replacement
        var replacementTransaction = new ReplacementTransaction
        {
            TransactionId = transaction.Id,
            RevertedDate = SystemClock.Instance.InTzdbSystemDefaultZone().GetCurrentLocalDateTime(),
            ErrorCode = reasonCode,
            ErrorMessage = errorMsg,
            RawReplacementTransactions = Enumerable.Empty<TransactionModel>(),
        };

        await unitOfWork.Repository<ReplacementTransaction>().InsertAsync(replacementTransaction, cancellationToken);
        await unitOfWork.SaveAsync(cancellationToken);

        logger.LogWarning(
            "Transaction number {TxId} was successfully reverted",
            transaction.StarkExTransactionId);

        return replacementTransaction.RawReplacementTransactions;
    }
}