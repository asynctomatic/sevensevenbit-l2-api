namespace SevenSevenBit.Operator.IntegrationTests.Configuration;

using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Nethereum.Web3;
using SevenSevenBit.Operator.Application.DependencyInjection;
using SevenSevenBit.Operator.Infrastructure.Blockchain.DependencyInjection;
using SevenSevenBit.Operator.Infrastructure.Blockchain.Options;
using SevenSevenBit.Operator.Infrastructure.MessageBus.DependencyInjection;
using SevenSevenBit.Operator.Infrastructure.SQL.Data.OperatorData;
using SevenSevenBit.Operator.Infrastructure.SQL.DependencyInjection;
using SevenSevenBit.Operator.TestHelpers.Extensions;
using SevenSevenBit.Operator.Worker.Extensions;
using Xunit;

public class WorkerFactory : IDisposable, IAsyncLifetime
{
    private bool disposed;

    public IServiceProvider ServiceProvider { get; set; }

    public IWeb3 Web3 { get; private set; }

    private IHost WorkerHost { get; set; }

    public async Task InitializeAsync()
    {
        WorkerHost = Host.CreateDefaultBuilder(default)
            .ConfigureAppConfiguration((_, config) =>
            {
                config.AddAppSettings();
            })
            .ConfigureServices((_, services) =>
            {
                services
                    .AddWorkerServices()
                    .AddApplicationWorkerServices()
                    .AddInfrastructureWorkerServices()
                    .AddBlockchainInfrastructureHostedServices()
                    .AddMessageBusWorkerInfrastructureServices<OperatorDbContext>(false)
                    .AddMassTransitTestHarness(configurator =>
                    {
                        configurator.SetTestTimeouts(
                            new TimeSpan(0, 0, 2),
                            new TimeSpan(0, 0, 2));
                    });
            })
            .Build();
        ServiceProvider = WorkerHost.Services;

        // Init Web3 RPC connection
        var options = ServiceProvider.GetRequiredService<IOptions<BlockchainOptions>>();
        Web3 = new Web3(options.Value.RpcEndpoint);

        // Start worker
        await WorkerHost.StartAsync();
    }

    public async Task DisposeAsync()
    {
        await WorkerHost.StopAsync();
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!disposed && disposing)
        {
            WorkerHost?.Dispose();
        }

        disposed = true;
    }
}