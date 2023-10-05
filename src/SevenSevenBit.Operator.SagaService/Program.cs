using Serilog;
using SevenSevenBit.Operator.Infrastructure.MessageBus.DependencyInjection;
using SevenSevenBit.Operator.Infrastructure.NoSQL.DependencyInjection;
using SevenSevenBit.Operator.Infrastructure.NoSQL.Options;

var builder = Host.CreateDefaultBuilder(args);

builder.ConfigureLogging(logging
    => logging.ClearProviders().AddSerilog());

builder.UseSerilog((context, cfg)
    => cfg.ReadFrom.Configuration(context.Configuration));

builder.ConfigureServices((context, services) =>
{
    // Load message bus settings from config files.
    var isProduction = context.HostingEnvironment.IsProduction();
    var redisOptions = context.Configuration.GetSection(RedisOptions.Redis).Get<RedisOptions>();

    services
        .AddNoSqlSagaInfrastructureServices()
        .AddMessageBusSagaInfrastructureServices(isProduction, redisOptions.ConnectionString, redisOptions.DatabaseId);
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