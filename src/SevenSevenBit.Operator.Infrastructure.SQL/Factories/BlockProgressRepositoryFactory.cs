namespace SevenSevenBit.Operator.Infrastructure.SQL.Factories;

using System.Diagnostics.CodeAnalysis;
using Nethereum.BlockchainProcessing.ProgressRepositories;
using Nethereum.BlockchainStore.EFCore;
using Nethereum.BlockchainStore.EFCore.Repositories;

[ExcludeFromCodeCoverage]
public class BlockProgressRepositoryFactory : IBlockProgressRepositoryFactory
{
    private readonly IBlockchainDbContextFactory blockchainDbContextFactory;

    public BlockProgressRepositoryFactory(IBlockchainDbContextFactory blockchainDbContextFactory)
    {
        this.blockchainDbContextFactory = blockchainDbContextFactory;
    }

    public IBlockProgressRepository CreateBlockProgressRepository()
    {
        return new BlockProgressRepository(blockchainDbContextFactory);
    }
}