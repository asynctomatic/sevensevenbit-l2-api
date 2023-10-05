namespace SevenSevenBit.Operator.Infrastructure.Blockchain.DependencyInjection;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Nethereum.Web3;
using SevenSevenBit.Operator.Infrastructure.Blockchain.Options;
using SevenSevenBit.Operator.Infrastructure.Blockchain.Services;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddBlockchainInfrastructureServices(
        this IServiceCollection serviceCollection)
    {
        // Add services
        serviceCollection.AddSingleton<IWeb3>(sp =>
        {
            var blockchainOptions = sp.GetRequiredService<IOptions<BlockchainOptions>>();

            return new Web3(blockchainOptions.Value.RpcEndpoint);
        });

        // Add options
        serviceCollection
            .AddOptions<BlockchainOptions>()
            .BindConfiguration(BlockchainOptions.Blockchain)
            .ValidateDataAnnotations()
            .ValidateOnStart();

        return serviceCollection;
    }

    public static IServiceCollection AddBlockchainInfrastructureHostedServices(
        this IServiceCollection serviceCollection)
    {
        // Add services
        serviceCollection.AddHostedService<StarkExContractEventProcessingService>();
        serviceCollection.AddSingleton<IWeb3>(sp =>
        {
            var blockchainOptions = sp.GetRequiredService<IOptions<BlockchainOptions>>();

            return new Web3(blockchainOptions.Value.RpcEndpoint);
        });

        // Add options
        serviceCollection
            .AddOptions<BlockchainOptions>()
            .BindConfiguration(BlockchainOptions.Blockchain)
            .ValidateDataAnnotations()
            .ValidateOnStart();

        return serviceCollection;
    }
}