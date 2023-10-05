namespace SevenSevenBit.Operator.Application.Services;

using System.Numerics;
using SevenSevenBit.Operator.Application.Common.Interfaces;
using SevenSevenBit.Operator.Application.Interfaces.Services;
using SevenSevenBit.Operator.Domain.Entities;
using SevenSevenBit.Operator.Domain.Enums;
using StarkEx.Client.SDK.Models.Spot.TransactionModels;
using StarkEx.Crypto.SDK.Enums;

public class WithdrawService : IWithdrawService
{
    private readonly IMessageBusService messageBusService;
    private readonly IUnitOfWork unitOfWork;

    public WithdrawService(
        IMessageBusService messageBusService,
        IUnitOfWork unitOfWork)
    {
        this.messageBusService = messageBusService;
        this.unitOfWork = unitOfWork;
    }

    public async Task WithdrawAsync(
        Vault vault,
        BigInteger quantizedAmount,
        CancellationToken cancellationToken)
    {
        var withdrawBody = new WithdrawalModel
        {
            VaultId = vault.VaultChainId,
            Amount = quantizedAmount,
            StarkKey = vault.User.StarkKey,
            TokenId = vault.AssetStarkExId(),
        };

        var newTransaction = new Transaction
        {
            RawTransaction = withdrawBody,
            Status = TransactionStatus.Streamed,
            Operation = StarkExOperation.Withdrawal,
        };

        await unitOfWork.Repository<Transaction>().InsertAsync(newTransaction, cancellationToken);

        vault.QuantizedAvailableBalance -= BigInteger.Abs(quantizedAmount);
        vault.QuantizedAccountingBalance -= BigInteger.Abs(quantizedAmount);

        // update vault accounting in DB
        var vaultUpdate = new VaultUpdate
        {
            Vault = vault,
            QuantizedAmount = -BigInteger.Abs(quantizedAmount),
            Transaction = newTransaction,
        };

        await unitOfWork.Repository<VaultUpdate>().InsertAsync(vaultUpdate, cancellationToken);

        switch (vault.Asset.Type)
        {
            case AssetType.Erc721:
                // TODO: await UpdateOrCreateTokenIdProductOnWithdrawalAsync<Erc721Product>(vault, quantizedAmount, cancellationToken);
                break;
            case AssetType.Erc1155:
                // TODO: await UpdateOrCreateTokenIdProductOnWithdrawalAsync<Erc1155Product>(vault, quantizedAmount, cancellationToken);
                break;
            case AssetType.MintableErc721:
                vault.BaseMintingBlob!.QuantizedQuantity -= quantizedAmount;
                // TODO: await UpdateOrCreateMintableProductOnWithdrawalAsync<MintableErc721Product>(vault, quantizedAmount, cancellationToken);
                break;
            case AssetType.MintableErc1155:
                vault.BaseMintingBlob!.QuantizedQuantity -= quantizedAmount;
                // TODO: await UpdateOrCreateMintableProductOnWithdrawalAsync<MintableErc1155Product>(vault, quantizedAmount, cancellationToken);
                break;
        }

        /* TODO
        var message = new StreamTransaction
        {
            TransactionStreamId = newTransaction.Id,
            StarkExInstanceId = tenantContext.StarkExInstanceDetails.StarkExInstanceId,
            StarkExInstanceBaseAddress = tenantContext.StarkExInstanceDetails.StarkExInstanceHost,
            StarkExInstanceApiVersion = tenantContext.StarkExInstanceDetails.StarkExInstanceApiVersion,
            Transaction = withdrawBody.ToJson(),
        };

        await messageBusService.Publish(message, cancellationToken);*/
        await unitOfWork.SaveAsync(cancellationToken);
    }
}