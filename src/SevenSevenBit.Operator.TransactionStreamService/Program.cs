using Serilog;
using SevenSevenBit.Operator.Application.DependencyInjection;
using SevenSevenBit.Operator.Infrastructure.MessageBus.DependencyInjection;
using SevenSevenBit.Operator.Infrastructure.StarkExApi.DependencyInjection;
using SevenSevenBit.Operator.TransactionStreamService.Extensions;

var builder = Host.CreateDefaultBuilder(args);

builder.ConfigureLogging(logging
    => logging.ClearProviders().AddSerilog());

builder.UseSerilog((context, cfg)
    => cfg.ReadFrom.Configuration(context.Configuration));

builder.ConfigureServices((context, services) =>
{
    services
        .AddStarkExInfrastructureServices()
        .AddTransactionStreamServices(context.Configuration)
        .AddMessageBusTransactionStreamInfrastructureServices(context.HostingEnvironment.IsProduction())
        .AddApplicationTransactionStreamServices();
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