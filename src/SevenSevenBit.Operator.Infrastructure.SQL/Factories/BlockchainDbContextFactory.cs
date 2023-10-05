namespace SevenSevenBit.Operator.Infrastructure.SQL.Factories;

using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.Options;
using Nethereum.BlockchainStore.EFCore;
using SevenSevenBit.Operator.Infrastructure.SQL.Data.BlockchainData;
using SevenSevenBit.Operator.Infrastructure.SQL.Options;

[ExcludeFromCodeCoverage]
public class BlockchainDbContextFactory : IBlockchainDbContextFactory
{
    private readonly IOptions<PostgresOptions> options;

    public BlockchainDbContextFactory(IOptions<PostgresOptions> options)
    {
        this.options = options;
    }

    public BlockchainDbContextBase CreateContext()
    {
        return (BlockchainDbContextBase)Activator.CreateInstance(typeof(BlockchainDbContext), options);
    }

    public BlockchainDbContext CreateDbContext(string[] args)
    {
        return (BlockchainDbContext)Activator.CreateInstance(typeof(BlockchainDbContext), options);
    }
}