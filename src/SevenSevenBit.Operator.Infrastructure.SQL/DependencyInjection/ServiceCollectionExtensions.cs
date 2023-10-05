namespace SevenSevenBit.Operator.Infrastructure.SQL.DependencyInjection;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Nethereum.BlockchainProcessing.ProgressRepositories;
using Nethereum.BlockchainStore.EFCore;
using SevenSevenBit.Operator.Application.Common.Interfaces;
using SevenSevenBit.Operator.Infrastructure.SQL.Data.OperatorData;
using SevenSevenBit.Operator.Infrastructure.SQL.Factories;
using SevenSevenBit.Operator.Infrastructure.SQL.Options;
using SevenSevenBit.Operator.Infrastructure.SQL.UnitOfWork;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddSqlInfrastructureServices(this IServiceCollection serviceCollection)
    {
        // Add DbContexts
        serviceCollection.AddDbContext<OperatorDbContext>((provider, builder) =>
        {
            var postgresOptions = provider.GetRequiredService<IOptions<PostgresOptions>>().Value;

            builder.UseNpgsql(
                    connectionString: postgresOptions.OperatorDb.DefaultConnectionString,
                    npgsqlOptionsAction: options =>
                    {
                        options.UseNodaTime();
                        options.CommandTimeout(postgresOptions.OperatorDb.CommandTimeoutInSeconds);
                    });
        });

        // Add UnitOfWork
        serviceCollection.AddScoped<IUnitOfWork, UnitOfWork>();

        // Add options
        serviceCollection
            .AddOptions<PostgresOptions>()
            .BindConfiguration(PostgresOptions.Postgres)
            .ValidateDataAnnotations()
            .ValidateOnStart();

        return serviceCollection;
    }

    public static IServiceCollection AddInfrastructureWorkerServices(this IServiceCollection serviceCollection)
    {
        // Add DbContexts
        serviceCollection.AddDbContext<OperatorDbContext>((provider, builder) =>
        {
            var postgresOptions = provider.GetRequiredService<IOptions<PostgresOptions>>().Value;

            builder.UseNpgsql(
                    connectionString: postgresOptions.OperatorDb.DefaultConnectionString,
                    npgsqlOptionsAction: options =>
                    {
                        options.UseNodaTime();
                        options.CommandTimeout(postgresOptions.OperatorDb.CommandTimeoutInSeconds);
                    });
        });

        serviceCollection.AddSingleton<IBlockchainDbContextFactory, BlockchainDbContextFactory>();
        serviceCollection.AddSingleton<IBlockProgressRepositoryFactory, BlockProgressRepositoryFactory>();

        serviceCollection.AddScoped<IUnitOfWork, UnitOfWork>();

        // Add options
        serviceCollection
            .AddOptions<PostgresOptions>()
            .BindConfiguration(PostgresOptions.Postgres)
            .ValidateDataAnnotations()
            .ValidateOnStart();

        return serviceCollection;
    }
}