namespace SevenSevenBit.Operator.Infrastructure.NoSQL.DependencyInjection;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using SevenSevenBit.Operator.Application.Common.Interfaces;
using SevenSevenBit.Operator.Infrastructure.NoSQL.Options;
using SevenSevenBit.Operator.Infrastructure.NoSQL.Services;
using StackExchange.Redis;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddNoSqlTxIdInfrastructureServices(this IServiceCollection serviceCollection)
    {
        // Add services
        serviceCollection.AddSingleton<INoSqlService, NoSqlService>();

        // Add Redis connection to the service container.
        serviceCollection.AddSingleton<IConnectionMultiplexer>(sp =>
        {
            var redisOptions = sp.GetRequiredService<IOptions<RedisOptions>>();

            // Establish connection to Redis instance.
            // https://stackexchange.github.io/StackExchange.Redis/Configuration
            return ConnectionMultiplexer.Connect(redisOptions.Value.ConnectionString);
        });

        // Add options
        serviceCollection
            .AddOptions<RedisOptions>()
            .BindConfiguration(RedisOptions.Redis)
            .ValidateDataAnnotations()
            .ValidateOnStart();

        return serviceCollection;
    }

    public static IServiceCollection AddNoSqlSagaInfrastructureServices(this IServiceCollection serviceCollection)
    {
        // Add options
        serviceCollection
            .AddOptions<RedisOptions>()
            .BindConfiguration(RedisOptions.Redis)
            .ValidateDataAnnotations()
            .ValidateOnStart();

        return serviceCollection;
    }
}