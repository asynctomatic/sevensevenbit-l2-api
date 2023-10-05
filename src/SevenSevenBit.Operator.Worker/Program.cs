using Microsoft.EntityFrameworkCore;
using Serilog;
using SevenSevenBit.Operator.Application.DependencyInjection;
using SevenSevenBit.Operator.Infrastructure.Blockchain.DependencyInjection;
using SevenSevenBit.Operator.Infrastructure.MessageBus.DependencyInjection;
using SevenSevenBit.Operator.Infrastructure.SQL.Data.OperatorData;
using SevenSevenBit.Operator.Infrastructure.SQL.DbSeed;
using SevenSevenBit.Operator.Infrastructure.SQL.DependencyInjection;
using SevenSevenBit.Operator.Infrastructure.SQL.UnitOfWork;
using SevenSevenBit.Operator.Worker.Extensions;

var builder = Host.CreateDefaultBuilder(args);

builder.ConfigureLogging(logging
    => logging.ClearProviders().AddSerilog());

builder.UseSerilog((context, cfg)
    => cfg.ReadFrom.Configuration(context.Configuration));

builder.ConfigureServices((context, services) =>
{
    services
        .AddWorkerServices()
        .AddApplicationWorkerServices()
        .AddInfrastructureWorkerServices()
        .AddBlockchainInfrastructureHostedServices()
        .AddMessageBusWorkerInfrastructureServices<OperatorDbContext>(context.HostingEnvironment.IsProduction());
});

var host = builder.Build();

var seedTestData = Environment.GetEnvironmentVariable("SEED_TEST_DATA");

if (seedTestData is not null && seedTestData.Equals("true"))
{
    using var sp = host.Services.CreateScope();
    var operatorDbContext = sp.ServiceProvider.GetRequiredService<OperatorDbContext>();
    operatorDbContext.Database.Migrate();
    var unitOfWork = new UnitOfWork(operatorDbContext);
    unitOfWork.AddBaseTestData(operatorDbContext).Wait();
}

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