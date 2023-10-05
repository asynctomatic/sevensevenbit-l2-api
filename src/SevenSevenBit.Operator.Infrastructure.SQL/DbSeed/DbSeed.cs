namespace SevenSevenBit.Operator.Infrastructure.SQL.DbSeed;

using Microsoft.EntityFrameworkCore;
using SevenSevenBit.Operator.Application.Common.Interfaces;
using SevenSevenBit.Operator.Domain.Entities;
using SevenSevenBit.Operator.Infrastructure.SQL.Data.BlockchainData;
using SevenSevenBit.Operator.Infrastructure.SQL.Data.OperatorData;

public static class DbSeed
{
    public static async Task AddBaseTestData(this IUnitOfWork unitOfWork, OperatorDbContext operatorDbContext)
    {
        // Add Fee Configs
        await unitOfWork.Repository<FeeConfig>().InsertAsync(FeeConfigs.GetFeeConfigs(), CancellationToken.None);

        // Add Users
        await unitOfWork.Repository<User>().InsertAsync(Users.GetUsers(), CancellationToken.None);

        // Add Assets
        await unitOfWork.Repository<Asset>().InsertAsync(Assets.GetAssets(), CancellationToken.None);

        // Add Transactions
        await unitOfWork.Repository<Transaction>().InsertAsync(Transactions.GetTransactions(), CancellationToken.None);

        // Add Replacement Transactions
        await unitOfWork.Repository<ReplacementTransaction>().InsertAsync(ReplacementTransactions.GetReplacementTransactions(), CancellationToken.None);

        // Add Vaults
        // Detach vault entries to avoid batch inserts in tests that run after the 1st one.
        // On repeated tests, the static dbseed entities already have relations which are inserted on the 1st saveAsync
        foreach (var entityEntry in operatorDbContext.ChangeTracker.Entries<Vault>())
        {
            entityEntry.State = EntityState.Detached;
        }

        foreach (var vault in Vaults.GetVaults())
        {
            await unitOfWork.Repository<Vault>().InsertAsync(vault, CancellationToken.None);
            await unitOfWork.SaveAsync(CancellationToken.None);
        }

        if (operatorDbContext.ChangeTracker.Entries<VaultUpdate>().Any())
        {
            // On repeated tests, the updates (which are related with the vaults) are already in the DB
            return;
        }

        // Add Vault Updates
        await unitOfWork.Repository<VaultUpdate>().InsertAsync(VaultUpdates.GetVaultUpdates(), CancellationToken.None);

        // Add Replacement Vault Updates
        await unitOfWork.Repository<ReplacementVaultUpdate>().InsertAsync(ReplacementVaultUpdates.GetReplacementVaultUpdates(), CancellationToken.None);

        await unitOfWork.SaveAsync(CancellationToken.None);
    }

    public static async Task ClearTestData(this IUnitOfWork unitOfWork)
    {
        // TODO: Clear all tables.
    }

    public static async Task ClearTestData(this BlockchainDbContext blockchainDbContext)
    {
        // Clear BlockProgress Table
        blockchainDbContext.BlockProgress.RemoveRange(blockchainDbContext.BlockProgress);
        await blockchainDbContext.SaveChangesAsync();
    }
}