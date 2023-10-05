namespace SevenSevenBit.Operator.Application.Services;

using Microsoft.Extensions.Logging;
using SevenSevenBit.Operator.Application.Common.Interfaces;
using SevenSevenBit.Operator.Application.DTOs.Internal;
using SevenSevenBit.Operator.Application.Interfaces.Services;
using SevenSevenBit.Operator.Domain.Common;
using SevenSevenBit.Operator.Domain.Entities;
using SevenSevenBit.Operator.Domain.Entities.MintingBlobs;
using SevenSevenBit.Operator.Domain.Enums;
using StarkEx.Client.SDK.Models.Spot.TransactionModels;
using StarkEx.Crypto.SDK.Enums;

public class MintService : IMintService
{
    private readonly ILogger<MintService> logger;
    private readonly IVaultService vaultService;
    private readonly IMessageBusService messageBusService;
    private readonly IUnitOfWork unitOfWork;

    public MintService(
        ILogger<MintService> logger,
        IVaultService vaultService,
        IMessageBusService messageBusService,
        IUnitOfWork unitOfWork)
    {
        this.logger = logger;
        this.vaultService = vaultService;
        this.messageBusService = messageBusService;
        this.unitOfWork = unitOfWork;
    }

    public async Task<Guid> MintAssetsAsync(
        IEnumerable<MintAssetDataDto> mints,
        CancellationToken cancellationToken)
    {
        var mintModels = new List<TransactionModel>();
        var vaults = new List<Vault>();
        var mintingBlobs = new List<(MintAssetDataDto MintDto, BaseMintingBlob MintingBlob)>();

        foreach (var mint in mints)
        {
            var mintingBlob = mint.Asset.Type switch
            {
                AssetType.MintableErc20 => await GetUpdateOrCreateMintingBlobAsync<Erc20MintingBlob>(mint, cancellationToken),
                AssetType.MintableErc721 => await GetUpdateOrCreateMintingBlobAsync<Erc721MintingBlob>(mint, cancellationToken),
                AssetType.MintableErc1155 => await GetUpdateOrCreateMintingBlobAsync<Erc1155MintingBlob>(mint, cancellationToken),
                _ => throw new ArgumentException("Asset type is not mintable.", nameof(mint)),
            };
            mintingBlobs.Add((mint, mintingBlob));

            var vault = await GetVaultForMintAsync(mint, mintingBlob, cancellationToken);
            vault.QuantizedAccountingBalance += mint.QuantizedAmount;

            vaults.Add(vault);
            mintModels.Add(
                new MintModel
                {
                    Amount = mint.QuantizedAmount,
                    StarkKey = mint.User.StarkKey,
                    TokenId = vault.AssetStarkExId(true),
                    VaultId = vault.VaultChainId,
                });
        }

        var multiTransactionBody = new MultiTransactionModel
        {
            Transactions = mintModels,
        };

        var newTransaction = new Transaction
        {
            RawTransaction = multiTransactionBody,
            Status = TransactionStatus.Streamed,
            Operation = StarkExOperation.MultiTransaction,
        };

        await unitOfWork.Repository<Transaction>().InsertAsync(newTransaction, cancellationToken);

        var vaultUpdates = vaults
            .Select(x => new VaultUpdate
            {
                Vault = x,

                // We can use the existing accounting balance here because new vaults are always allocated for mints
                // but we have to change this if StarkEx disables unique_minting.
                QuantizedAmount = x.QuantizedAccountingBalance,
                Transaction = newTransaction,
            })
            .ToList();

        await unitOfWork.Repository<VaultUpdate>().InsertAsync(vaultUpdates, cancellationToken);

        foreach (var tuple in mintingBlobs)
        {
            switch (tuple.MintingBlob)
            {
                // Update mintable products
                case Erc721MintingBlob:
                    // TODO: await UpdateOrCreateMintableProductAsync<MintableErc721Product>(tuple.MintDto, tuple.MintingBlob, newTransaction, cancellationToken);
                    break;
                case Erc1155MintingBlob:
                    // TODO: await UpdateOrCreateMintableProductAsync<MintableErc1155Product>(tuple.MintDto, tuple.MintingBlob, newTransaction, cancellationToken);
                    break;
            }
        }

        /* TODO
        var message = new StreamTransaction
        {
            TransactionStreamId = newTransaction.Id,
            StarkExInstanceId = tenantContext.StarkExInstanceDetails.StarkExInstanceId,
            StarkExInstanceBaseAddress = tenantContext.StarkExInstanceDetails.StarkExInstanceHost,
            StarkExInstanceApiVersion = tenantContext.StarkExInstanceDetails.StarkExInstanceApiVersion,
            Transaction = multiTransactionBody.ToJson(),
        };

        await messageBusService.Publish(message, cancellationToken);*/
        await unitOfWork.SaveAsync(cancellationToken);

        return newTransaction.Id;
    }

    public async Task<Guid> MintAssetAsync(
        MintAssetDataDto mint,
        CancellationToken cancellationToken)
    {
        var mintingBlob = mint.Asset.Type switch
        {
            AssetType.MintableErc20 => await GetUpdateOrCreateMintingBlobAsync<Erc20MintingBlob>(mint, cancellationToken),
            AssetType.MintableErc721 => await GetUpdateOrCreateMintingBlobAsync<Erc721MintingBlob>(mint, cancellationToken),
            AssetType.MintableErc1155 => await GetUpdateOrCreateMintingBlobAsync<Erc1155MintingBlob>(mint, cancellationToken),
            _ => throw new ArgumentException("Asset type is not mintable.", nameof(mint)),
        };

        var vault = await GetVaultForMintAsync(mint, mintingBlob, cancellationToken);
        vault.QuantizedAccountingBalance += mint.QuantizedAmount;

        var mintTransactionBody = new MintModel
        {
            VaultId = vault.VaultChainId,
            Amount = mint.QuantizedAmount,
            StarkKey = vault.User.StarkKey,
            TokenId = vault.AssetStarkExId(true),
        };

        var newTransaction = new Transaction
        {
            RawTransaction = mintTransactionBody,
            Status = TransactionStatus.Streamed,
            Operation = StarkExOperation.Mint,
        };

        await unitOfWork.Repository<Transaction>().InsertAsync(newTransaction, cancellationToken);

        var vaultUpdate = new VaultUpdate
        {
            Vault = vault,
            QuantizedAmount = mint.QuantizedAmount,
            Transaction = newTransaction,
        };

        await unitOfWork.Repository<VaultUpdate>().InsertAsync(vaultUpdate, cancellationToken);

        switch (mintingBlob)
        {
            // Update mintable products
            case Erc721MintingBlob:
                // TODO: await UpdateOrCreateMintableProductAsync<MintableErc721Product>(mint, mintingBlob, newTransaction, cancellationToken);
                break;
            case Erc1155MintingBlob:
                // TODO: await UpdateOrCreateMintableProductAsync<MintableErc1155Product>(mint, mintingBlob, newTransaction, cancellationToken);
                break;
        }

        /* TODO
        var message = new StreamTransaction
        {
            TransactionStreamId = newTransaction.Id,
            StarkExInstanceId = tenantContext.StarkExInstanceDetails.StarkExInstanceId,
            StarkExInstanceBaseAddress = tenantContext.StarkExInstanceDetails.StarkExInstanceHost,
            StarkExInstanceApiVersion = tenantContext.StarkExInstanceDetails.StarkExInstanceApiVersion,
            Transaction = mintTransactionBody.ToJson(),
        };

        await messageBusService.Publish(message, cancellationToken);*/
        await unitOfWork.SaveAsync(cancellationToken);

        return newTransaction.Id;
    }

    private async Task<Vault> GetVaultForMintAsync(
        MintAssetDataDto mint,
        BaseMintingBlob mintingBlob,
        CancellationToken cancellationToken)
    {
        // Return existing vault if it exists and if unique_minting constraint is disabled.
        // If there is no existing vault, we need to create a new one.
        var vault = vaultService.GetVault(mint.User, mint.Asset, mintingBlob: mint.MintingBlob);

        // TODO
        // If unique_minting constraint is enabled, mints should only be done to unallocated vaults.
        // https://docs.starkware.co/starkex/spot/proc_minting_an_nft.html
        // if (!tenantContext.StarkExInstanceDetails.StarkExInstanceHasUniqueMintingEnabled && existingVault is not null)
        if (vault is not null)
        {
            return vault;
        }

        vault = new Vault
        {
            DataAvailabilityMode = mint.DataAvailabilityMode,
            User = mint.User,
            Asset = mint.Asset,
            BaseMintingBlob = mintingBlob,
        };

        await unitOfWork.Repository<Vault>().InsertAsync(vault, cancellationToken);

        // TODO: We need to save here to get the vault chain id from the DB.
        await unitOfWork.SaveAsync(cancellationToken);

        logger.LogInformation(
            "Allocated vault with id {VaultId} in {DataAvailabilityMode} mode: " +
            "(starkKey={StarkKey}, starkExType={StarkExType}, starkExId={StarkExId})",
            vault.VaultChainId,
            vault.DataAvailabilityMode,
            vault.User.StarkKey,
            vault.Asset.StarkExType,
            vault.AssetStarkExId(true));

        return vault;
    }

    private async Task<BaseMintingBlob> GetUpdateOrCreateMintingBlobAsync<T>(MintAssetDataDto mint, CancellationToken cancellationToken)
        where T : BaseMintingBlob, new()
    {
        var mintingBlob = mint.Asset.MintingBlobs.SingleOrDefault(x => x.MintingBlobHex.Value.Equals(mint.MintingBlob));

        if (mintingBlob is null)
        {
            mintingBlob = new T
            {
                Asset = mint.Asset,
                MintingBlobHex = mint.MintingBlob,
                QuantizedQuantity = 0,
            };

            await unitOfWork.Repository<BaseMintingBlob>().InsertAsync(mintingBlob, cancellationToken);
        }

        mintingBlob.QuantizedQuantity += mint.QuantizedAmount;

        return mintingBlob;
    }
}