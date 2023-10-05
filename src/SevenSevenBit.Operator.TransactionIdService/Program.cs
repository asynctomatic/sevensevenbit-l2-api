using Serilog;
using SevenSevenBit.Operator.Application.DependencyInjection;
using SevenSevenBit.Operator.Infrastructure.MessageBus.DependencyInjection;
using SevenSevenBit.Operator.Infrastructure.NoSQL.DependencyInjection;
using SevenSevenBit.Operator.Infrastructure.StarkExApi.DependencyInjection;

var builder = Host.CreateDefaultBuilder(args);

builder.ConfigureLogging(logging
    => logging.ClearProviders().AddSerilog());

builder.UseSerilog((context, cfg)
    => cfg.ReadFrom.Configuration(context.Configuration));

builder.ConfigureServices((context, services) =>
{
    services
        .AddNoSqlTxIdInfrastructureServices()
        .AddStarkExInfrastructureServices()
        .AddApplicationTransactionIdServices()
        .AddMessageBusTransactionIdInfrastructureServices(context.HostingEnvironment.IsProduction());
});

var host = builder.Build();

try
{
    Log.Information("Starting the service {ApplicationName}", AppDomain.CurrentDomain.FriendlyName);
    await host.RunAsync();
    Log.Information("Shutting down the service");
}
catch (Exception ex)
{
    Log.Fatal(ex, "An unhandled exception occurred");
    await host.StopAsync();
}
finally
{
    Log.Information("Disposing the service");
    host.Dispose();
}