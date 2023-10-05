namespace SevenSevenBit.Operator.Infrastructure.SQL.Data.BlockchainData;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Nethereum.BlockchainStore.EFCore;
using SevenSevenBit.Operator.Infrastructure.SQL.Options;

public class BlockchainDbContext : BlockchainDbContextBase
{
    private readonly IOptions<PostgresOptions> options;

    public BlockchainDbContext(IOptions<PostgresOptions> options)
    {
        this.options = options;
        ColumnTypeForUnlimitedText = "text";
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseNpgsql(
            options.Value.BlockchainDb.DefaultConnectionString,
            o =>
            {
                o.UseNodaTime();
                o.CommandTimeout(options.Value.BlockchainDb.CommandTimeoutInSeconds);
            });
}