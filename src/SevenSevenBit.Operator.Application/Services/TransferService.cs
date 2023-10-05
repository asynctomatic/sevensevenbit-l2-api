namespace SevenSevenBit.Operator.Application.Services;

using System.Numerics;
using SevenSevenBit.Operator.Application.Common.Interfaces;
using SevenSevenBit.Operator.Application.Interfaces.Services;
using SevenSevenBit.Operator.Domain.Entities;
using SevenSevenBit.Operator.Domain.Enums;
using SevenSevenBit.Operator.Domain.ValueObjects;
using SevenSevenBit.Operator.SharedKernel.Extensions;
using StarkEx.Client.SDK.Models.Spot.TransactionModels;

public class TransferService : ITransferService
{
    private readonly IMessageBusService messageBusService;
    private readonly IUnitOfWork unitOfWork;

    public TransferService(
        IMessageBusService messageBusService,
        IUnitOfWork unitOfWork)
    {
        this.messageBusService = messageBusService;
        this.unitOfWork = unitOfWork;
    }

    public async Task<Guid> TransferAsync(
        Vault senderVault,
        Vault receiverVault,
        BigInteger amountQuantized,
        Vault feeSenderVault,
        Vault feeReceiverVault,
        BigInteger feeAmountQuantized,
        long expirationTimestamp,
        StarkSignature starkSignature,
        int nonce,
        CancellationToken cancellationToken)
    {
        var transferBody = new TransferModel
        {
            SenderPublicKey = senderVault.User.StarkKey,
            ReceiverPublicKey = receiverVault.User.StarkKey,
            SenderVaultId = senderVault.VaultChainId,
            ReceiverVaultId = receiverVault.VaultChainId,
            Token = senderVault.AssetStarkExId(),
            Amount = amountQuantized,
            FeeInfo = new FeeInfoModel
            {
                FeeLimit = feeAmountQuantized,
                SourceVaultId = feeSenderVault.VaultChainId,
                TokenId = feeSenderVault.AssetStarkExId(),
            },
            FeeInfoExchange = new FeeInfoExchangeModel
            {
                FeeTaken = feeAmountQuantized,
                DestinationVaultId = feeReceiverVault.VaultChainId,
                DestinationStarkKey = feeReceiverVault.User.StarkKey,
            },
            ExpirationTimestamp = expirationTimestamp / 3600,
            Nonce = nonce,
            Signature = starkSignature,
        };

        var newTransaction = new Transaction
        {
            RawTransaction = transferBody,
            Operation = StarkExOperation.Transfer,
            Status = TransactionStatus.Streamed,
        };

        await unitOfWork.Repository<Transaction>().InsertAsync(newTransaction, cancellationToken);

        // update from, to and fee vaults accounting in DB
        await unitOfWork.Repository<VaultUpdate>().InsertAsync(
            new VaultUpdate
            {
                Vault = senderVault,
                QuantizedAmount = amountQuantized * BigInteger.MinusOne,
                Transaction = newTransaction,
            },
            cancellationToken);

        await unitOfWork.Repository<VaultUpdate>().InsertAsync(
            new VaultUpdate
            {
                Vault = receiverVault,
                QuantizedAmount = amountQuantized,
                Transaction = newTransaction,
            },
            cancellationToken);

        if (feeAmountQuantized > BigInteger.Zero)
        {
            await unitOfWork.Repository<VaultUpdate>().InsertAsync(
                new VaultUpdate
                {
                    Vault = feeSenderVault,
                    QuantizedAmount = feeAmountQuantized * BigInteger.MinusOne,
                    Transaction = newTransaction,
                },
                cancellationToken);

            await unitOfWork.Repository<VaultUpdate>().InsertAsync(
                new VaultUpdate
                {
                    Vault = feeReceiverVault,
                    QuantizedAmount = feeAmountQuantized,
                    Transaction = newTransaction,
                },
                cancellationToken);
        }

        // Available balance deductions
        senderVault.QuantizedAvailableBalance -= amountQuantized;
        feeSenderVault.QuantizedAvailableBalance -= feeAmountQuantized;

        // Accounting balance updates
        senderVault.QuantizedAccountingBalance -= amountQuantized;
        feeSenderVault.QuantizedAccountingBalance -= feeAmountQuantized;
        receiverVault.QuantizedAccountingBalance += amountQuantized;
        feeReceiverVault.QuantizedAccountingBalance += feeAmountQuantized;

        // Exchange mintingBlobs if applicable
        if (senderVault.Asset.Type.IsMintable())
        {
            receiverVault.BaseMintingBlob = senderVault.BaseMintingBlob;
        }

        /* TODO
        var message = new StreamTransaction
        {
            TransactionStreamId = newTransaction.Id,
            StarkExInstanceId = tenantContext.StarkExInstanceDetails.StarkExInstanceId,
            StarkExInstanceBaseAddress = tenantContext.StarkExInstanceDetails.StarkExInstanceHost,
            StarkExInstanceApiVersion = tenantContext.StarkExInstanceDetails.StarkExInstanceApiVersion,
            Transaction = transferBody.ToJson(),
        };

        await messageBusService.Publish(message, cancellationToken);*/
        await unitOfWork.SaveAsync(cancellationToken);

        return newTransaction.Id;
    }
}