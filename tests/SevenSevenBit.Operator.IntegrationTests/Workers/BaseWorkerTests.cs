namespace SevenSevenBit.Operator.IntegrationTests.Workers;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using SevenSevenBit.Operator.Infrastructure.SQL.Data.BlockchainData;
using SevenSevenBit.Operator.Infrastructure.SQL.Data.OperatorData;
using SevenSevenBit.Operator.Infrastructure.SQL.DbSeed;
using SevenSevenBit.Operator.Infrastructure.SQL.Options;
using SevenSevenBit.Operator.Infrastructure.SQL.UnitOfWork;
using SevenSevenBit.Operator.IntegrationTests.Configuration;
using SevenSevenBit.Operator.TestHelpers.Blockchain.Functions.Anvil;
using Xunit;

public class BaseWorkerTests : IAsyncLifetime
{
#pragma warning disable SA1401
    private protected readonly IOptions<PostgresOptions> PostgresSettings;
    private protected readonly WorkerFactory Factory;
    private string snapshot;
#pragma warning restore SA1401

    protected BaseWorkerTests(WorkerFactory factory)
    {
        PostgresSettings = factory.ServiceProvider.GetRequiredService<IOptions<PostgresOptions>>();
        Factory = factory;
    }

    public async Task InitializeAsync()
    {
        // Setup OperatorDb
        await using var scope = Factory.ServiceProvider.CreateAsyncScope();
        var operatorDbContext = scope.ServiceProvider.GetRequiredService<OperatorDbContext>();
        var unitOfWork = new UnitOfWork(operatorDbContext);
        await unitOfWork.ClearTestData();
        await unitOfWork.AddBaseTestData(operatorDbContext);

        // Setup BlockchainDb
        await using var blockchainDbContext = new BlockchainDbContext(PostgresSettings);
        await blockchainDbContext.ClearTestData();

        // Snapshot the state of the blockchain.
        snapshot = await new EvmSnapshot(Factory.Web3.Client).SendRequestAsync();
    }

    public async Task DisposeAsync()
    {
        // Restore the blockchain snapshot.
        await new EvmRevert(Factory.Web3.Client).SendRequestAsync(snapshot);
    }
}