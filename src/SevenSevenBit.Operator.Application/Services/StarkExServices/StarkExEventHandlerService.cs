namespace SevenSevenBit.Operator.Application.Services.StarkExServices;

using Microsoft.Extensions.Logging;
using Nethereum.Contracts;
using SevenSevenBit.Operator.Application.Blockchain.Events;
using SevenSevenBit.Operator.Application.Interfaces.Services.BackgroundServices;
using SevenSevenBit.Operator.Application.Interfaces.Services.StarkExServices;
using SevenSevenBit.Operator.Domain.ValueObjects;

public class StarkExEventHandlerService : IStarkExEventHandlerService
{
    private readonly ILogger<IStarkExEventHandlerService> logger;
    private readonly IBackgroundVaultService vaultService;

    public StarkExEventHandlerService(
        ILogger<StarkExEventHandlerService> logger,
        IBackgroundVaultService vaultService)
    {
        this.logger = logger;
        this.vaultService = vaultService;
    }

    public async Task HandleLogDepositEventAsync(
        EventLog<LogDepositEvent> log,
        CancellationToken cancellationToken)
    {
        var vaultId = log.Event.VaultId;
        var vault = await vaultService.GetVaultAsync(vaultId, cancellationToken);

        // Ignore deposits from unallocated vaults
        if (vault == null)
        {
            logger.LogError("Vault {VaultId} not found", vaultId);
            return;
        }

        // Check if vault belongs to user
        var recipientStarkExKey = new StarkKey(log.Event.StarkKey);
        if (vault.User.StarkKey != recipientStarkExKey)
        {
            logger.LogError(
                "Vault {VaultId} owner {Owner} does not match depositor {recipientStarkExKey}",
                vault.VaultChainId,
                vault.User.StarkKey,
                recipientStarkExKey);
            return;
        }

        // Check if vault and holds assets of the correct type
        var assetStarkExType = new StarkExAssetType(log.Event.AssetType);
        if (vault.Asset.StarkExType != assetStarkExType)
        {
            logger.LogError(
                "Vault {VaultId} asset {VaultAsset} does not match deposited asset",
                vault.VaultChainId,
                vault.AssetStarkExId());
            return;
        }

        await vaultService.DepositAsync(
            vault,
            log.Event.QuantizedAmount,
            cancellationToken);
    }

    public async Task HandleLogDepositWithTokenIdEventAsync(
        EventLog<LogDepositWithTokenIdEvent> log,
        CancellationToken cancellationToken)
    {
        var vaultId = log.Event.VaultId;
        var vault = await vaultService.GetVaultAsync(vaultId, cancellationToken);

        // Ignore deposits from unallocated vaults
        if (vault == null)
        {
            logger.LogError("Vault {VaultId} not found", vaultId);
            return;
        }

        // Check if vault belongs to user
        var recipientStarkExKey = new StarkKey(log.Event.StarkKey);
        if (vault.User.StarkKey != recipientStarkExKey)
        {
            logger.LogError(
                "Vault {VaultId} owner {Owner} does not match depositor",
                vault.VaultChainId,
                vault.User.StarkKey);
            return;
        }

        // Check if vault and holds assets of the correct type
        var assetStarkExType = new StarkExAssetType(log.Event.AssetType);
        var assetStarkExId = new StarkExAssetId(log.Event.AssetId);
        if (vault.Asset.StarkExType != assetStarkExType || vault.AssetStarkExId() != assetStarkExId)
        {
            logger.LogError(
                "Vault {VaultId} asset {VaultAsset} does not match deposited asset",
                vault.VaultChainId,
                vault.AssetStarkExId());
            return;
        }

        await vaultService.DepositAsync(
            vault,
            log.Event.QuantizedAmount,
            cancellationToken);
    }

    public void HandleLogDepositCancelEvent(EventLog<LogDepositCancelEvent> log)
    {
        logger.LogWarning(
            "LogDepositCancelEvent {@Log} emitted",
            log);
    }

    public void HandleLogDepositCancelReclaimedEvent(EventLog<LogDepositCancelReclaimedEvent> log)
    {
        logger.LogWarning(
            "LogDepositCancelReclaimedEvent {@Log} emitted",
            log);
    }

    public void HandleLogDepositWithTokenIdCancelReclaimedEvent(EventLog<LogDepositWithTokenIdCancelReclaimedEvent> log)
    {
        logger.LogWarning(
            "LogDepositWithTokenIdCancelReclaimedEvent {@Log} emitted",
            log);
    }

    public async Task HandleLogFullWithdrawalRequestEventAsync(
        EventLog<LogFullWithdrawalRequestEvent> log,
        CancellationToken cancellationToken)
    {
        var vaultId = log.Event.VaultId;
        var starkKey = new StarkKey(log.Event.StarkKey);

        var vault = await vaultService.GetVaultAsync(vaultId, cancellationToken);

        if (vault?.User == null)
        {
            logger.LogError("Vault {VaultId} not found", vaultId);

            await vaultService.FalseFullWithdrawAsync(vaultId, starkKey, cancellationToken);
            return;
        }

        // Check if vault belongs to user
        if (vault.User.StarkKey != starkKey)
        {
            logger.LogError(
                "Vault {VaultId} owner {Owner} does not match withdrawer",
                vault.VaultChainId,
                vault.User.StarkKey);

            await vaultService.FalseFullWithdrawAsync(vaultId, starkKey, cancellationToken);
            return;
        }

        await vaultService.FullWithdrawAsync(vault, cancellationToken);
    }
}