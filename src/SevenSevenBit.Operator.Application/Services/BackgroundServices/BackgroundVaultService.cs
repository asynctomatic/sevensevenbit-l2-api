namespace SevenSevenBit.Operator.Application.Services.BackgroundServices;

using System.Numerics;
using SevenSevenBit.Operator.Application.Common.Interfaces;
using SevenSevenBit.Operator.Application.Interfaces.Services.BackgroundServices;
using SevenSevenBit.Operator.Domain.Entities;
using SevenSevenBit.Operator.Domain.Entities.Marketplace;
using SevenSevenBit.Operator.Domain.Enums;
using SevenSevenBit.Operator.Domain.ValueObjects;
using StarkEx.Client.SDK.Models.Spot.TransactionModels;
using StarkEx.Crypto.SDK.Enums;

// Vault service implementation to be used on background services since it doesn't depend on the clientContext
public class BackgroundVaultService : IBackgroundVaultService
{
    private readonly IUnitOfWork unitOfWork;
    private readonly IMessageBusService messageBusService;

    public BackgroundVaultService(
        IUnitOfWork unitOfWork,
        IMessageBusService messageBusService)
    {
        this.unitOfWork = unitOfWork;
        this.messageBusService = messageBusService;
    }

    public async Task<Vault> GetVaultAsync(
        BigInteger vaultId,
        CancellationToken cancellationToken)
    {
        return await unitOfWork.Repository<Vault>().GetSingleAsync(
            filter: vault => vault.VaultChainId.Equals(vaultId),
            cancellationToken: cancellationToken);
    }

    public async Task DepositAsync(
        Vault vault,
        BigInteger quantizedAmount,
        CancellationToken cancellationToken)
    {
        var depositBody = new DepositModel
        {
            VaultId = vault.VaultChainId,
            Amount = quantizedAmount,
            StarkKey = vault.User.StarkKey,
            TokenId = vault.AssetStarkExId(),
        };

        var newTransaction = new Transaction
        {
            RawTransaction = depositBody,
            Operation = StarkExOperation.Deposit,
            Status = TransactionStatus.Streamed,
        };

        // Update vault accounting in DB
        var vaultUpdate = new VaultUpdate
        {
            Vault = vault,
            QuantizedAmount = quantizedAmount,
            Transaction = newTransaction,
        };

        vault.QuantizedAccountingBalance += quantizedAmount;

        await unitOfWork.Repository<Transaction>().InsertAsync(newTransaction, cancellationToken);
        await unitOfWork.Repository<VaultUpdate>().InsertAsync(vaultUpdate, cancellationToken);

        switch (vault.Asset.Type)
        {
            case AssetType.Erc721:
                // TODO await UpdateOrCreateTokenIdProductOnDepositAsync<Erc721Product>(vault, quantizedAmount, cancellationToken);
                break;
            case AssetType.Erc1155:
                // TODO await UpdateOrCreateTokenIdProductOnDepositAsync<Erc1155Product>(vault, quantizedAmount, cancellationToken);
                break;
        }

        /*var message = new StreamTransaction
        {
            TransactionStreamId = newTransaction.Id,
            StarkExInstanceId = vault.StarkExInstanceId,
            StarkExInstanceBaseAddress = vault.StarkExInstance.Host,
            StarkExInstanceApiVersion = vault.StarkExInstance.ApiVersion,
            Transaction = depositBody.ToJson(),
        };

        await messageBusService.Publish(message, cancellationToken);*/
        await unitOfWork.SaveAsync(cancellationToken);
    }

    public async Task FullWithdrawAsync(
        Vault vault,
        CancellationToken cancellationToken)
    {
        var fullWithdrawalBody = new FullWithdrawalModel
        {
            VaultId = vault.VaultChainId,
            StarkKey = vault.User.StarkKey,
        };

        // Add transaction record to DB
        var newTransaction = new Transaction
        {
            RawTransaction = fullWithdrawalBody,
            Operation = StarkExOperation.FullWithdrawal,
            Status = TransactionStatus.Streamed,
        };

        // Update vault accounting in DB
        var vaultUpdate = new VaultUpdate
        {
            Vault = vault,
            QuantizedAmount = vault.QuantizedAvailableBalance * BigInteger.MinusOne,
            Transaction = newTransaction,
        };

        vault.QuantizedAvailableBalance = BigInteger.Zero;
        vault.QuantizedAccountingBalance = BigInteger.Zero;

        await unitOfWork.Repository<Transaction>().InsertAsync(newTransaction, cancellationToken);
        await unitOfWork.Repository<VaultUpdate>().InsertAsync(vaultUpdate, cancellationToken);

        switch (vault.Asset.Type)
        {
            case AssetType.Erc721:
                // TODO await UpdateOrCreateTokenIdProductOnFullWithdrawalAsync<Erc721Product>(vault, BigInteger.Abs(vaultUpdate.QuantizedAmount), cancellationToken);
                break;
            case AssetType.Erc1155:
                // TODO: await UpdateOrCreateTokenIdProductOnFullWithdrawalAsync<Erc1155Product>(vault, BigInteger.Abs(vaultUpdate.QuantizedAmount), cancellationToken);
                break;
            case AssetType.MintableErc721:
                // TODO These minting blob updates should also be done with separate updates entities (e.g. vaultUpdates or productUpdates)
                vault.BaseMintingBlob!.QuantizedQuantity -= vaultUpdate.QuantizedAmount;
                // TODO: await UpdateOrCreateMintableProductOnFullWithdrawalAsync<MintableErc721Product>(vault, BigInteger.Abs(vaultUpdate.QuantizedAmount), cancellationToken);
                break;
            case AssetType.MintableErc1155:
                // TODO These minting blob updates should also be done with separate updates entities (e.g. vaultUpdates or productUpdates)
                vault.BaseMintingBlob!.QuantizedQuantity -= vaultUpdate.QuantizedAmount;
                // TODO: await UpdateOrCreateMintableProductOnFullWithdrawalAsync<MintableErc721Product>(vault, BigInteger.Abs(vaultUpdate.QuantizedAmount), cancellationToken);
                break;
        }

        /*var message = new StreamTransaction
        {
            TransactionStreamId = newTransaction.Id,
            StarkExInstanceId = vault.StarkExInstanceId,
            StarkExInstanceBaseAddress = vault.StarkExInstance.Host,
            StarkExInstanceApiVersion = vault.StarkExInstance.ApiVersion,
            Transaction = fullWithdrawalBody.ToJson(),
        };

        await messageBusService.Publish(message, cancellationToken);*/
        await unitOfWork.SaveAsync(cancellationToken);
    }

    public async Task FalseFullWithdrawAsync(
        BigInteger vaultChainId,
        StarkKey requesterStarkKey,
        CancellationToken cancellationToken)
    {
        var falseFullWithdrawalBody = new FalseFullWithdrawalModel
        {
            VaultId = vaultChainId,
            RequesterStarkKey = requesterStarkKey,
        };

        // Add transaction record to DB
        var newTransaction = new Transaction
        {
            RawTransaction = falseFullWithdrawalBody,
            Status = TransactionStatus.Streamed,
            Operation = StarkExOperation.FalseFullWithdrawal,
        };

        await unitOfWork.Repository<Transaction>().InsertAsync(newTransaction, cancellationToken);

        /*var message = new StreamTransaction
        {
            TransactionStreamId = newTransaction.Id,
            StarkExInstanceId = starkExInstance.Id,
            StarkExInstanceBaseAddress = starkExInstance.Host,
            StarkExInstanceApiVersion = starkExInstance.ApiVersion,
            Transaction = falseFullWithdrawalBody.ToJson(),
        };

        await messageBusService.Publish(message, cancellationToken);*/
        await unitOfWork.SaveAsync(cancellationToken);
    }
}