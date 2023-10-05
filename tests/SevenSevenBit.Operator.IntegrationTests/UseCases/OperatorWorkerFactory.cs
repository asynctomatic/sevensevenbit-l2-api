namespace SevenSevenBit.Operator.IntegrationTests.UseCases;

using MassTransit;
using Microsoft.Extensions.Hosting;
using SevenSevenBit.Operator.Application.DependencyInjection;
using SevenSevenBit.Operator.Infrastructure.Blockchain.DependencyInjection;
using SevenSevenBit.Operator.Infrastructure.MessageBus.DependencyInjection;
using SevenSevenBit.Operator.Infrastructure.SQL.Data.OperatorData;
using SevenSevenBit.Operator.Infrastructure.SQL.DependencyInjection;
using SevenSevenBit.Operator.TestHelpers.Extensions;
using SevenSevenBit.Operator.Worker.Extensions;
using Xunit;

public class OperatorWorkerFactory : IAsyncLifetime
{
    public IHost Worker { get; private set; }

    public async Task InitializeAsync()
    {
        Worker = Host.CreateDefaultBuilder(default)
            .ConfigureAppConfiguration((hostingContext, config) =>
            {
                // TODO: Should we ensure this is never run in production (ASPNETCORE_ENVIRONMENT=Production or None)?
                // TODO: If ASPNETCORE_ENVIRONMENT is not set his runs the apppssettings..json file, which is not what we want.
                // TODO: Error="The configuration file 'appsettings..json' was not found and is not optional."
                // TODO: config.AddAppSettings();
            })
            .ConfigureServices((_, services) =>
            {
                // TODO: The AddWorkerServices can be removed (all worker services have been deprecated).
                // TODO: All Infrastructure dependency injection should happen in a single .AddInfrastructureServices() call.
                // TODO: The AddMessageBusWorkerInfrastructureServices does not follow the same naming pattern of AddApplicationWorkerServices.
                services
                    .AddWorkerServices()
                    .AddApplicationWorkerServices()
                    .AddInfrastructureWorkerServices()
                    .AddBlockchainInfrastructureHostedServices()
                    // TODO: Do we need this with the MassTransit test harness?
                    .AddMessageBusWorkerInfrastructureServices<OperatorDbContext>(false)
                    .AddMassTransitTestHarness();
            })
            .Build();
    }

    public async Task DisposeAsync()
    {
        await Worker.StopAsync();
    }
}