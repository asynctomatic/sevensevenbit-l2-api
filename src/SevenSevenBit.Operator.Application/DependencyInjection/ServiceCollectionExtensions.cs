namespace SevenSevenBit.Operator.Application.DependencyInjection;

using Microsoft.Extensions.DependencyInjection;
using SevenSevenBit.Operator.Application.Blockchain;
using SevenSevenBit.Operator.Application.Interfaces.Services;
using SevenSevenBit.Operator.Application.Interfaces.Services.BackgroundServices;
using SevenSevenBit.Operator.Application.Interfaces.Services.Signatures;
using SevenSevenBit.Operator.Application.Interfaces.Services.StarkExServices;
using SevenSevenBit.Operator.Application.Options;
using SevenSevenBit.Operator.Application.Services;
using SevenSevenBit.Operator.Application.Services.BackgroundServices;
using SevenSevenBit.Operator.Application.Services.Signatures;
using SevenSevenBit.Operator.Application.Services.StarkExServices;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplicationWebServices(this IServiceCollection serviceCollection)
    {
        // Application services
        serviceCollection.AddSingleton<IStarkExEncodingService, StarkExEncodingService>();
        serviceCollection.AddSingleton<IStarkExSignatureService, StarkExSignatureService>();
        serviceCollection.AddSingleton<IEthereumService, EthereumService>();
        serviceCollection.AddSingleton<INonceService, NonceService>();
        serviceCollection.AddScoped<IAssetService, AssetService>();
        serviceCollection.AddScoped<IStarkExContractService, StarkExContractService>();
        serviceCollection.AddScoped<IFeeService, FeeService>();
        serviceCollection.AddScoped<IStarkExService, StarkExService>();
        serviceCollection.AddScoped<IUsersService, UserService>();
        serviceCollection.AddScoped<ITimestampService, TimestampService>();
        serviceCollection.AddScoped<ITransferService, TransferService>();
        serviceCollection.AddScoped<IWithdrawService, WithdrawService>();
        serviceCollection.AddScoped<IVaultService, VaultService>();
        serviceCollection.AddScoped<ITransactionService, TransactionService>();
        serviceCollection.AddScoped<IMintService, MintService>();
        serviceCollection.AddScoped<IMarketplaceService, MarketplaceService>();

        // Add options
        serviceCollection
            .AddOptions<FeatureTogglesOptions>()
            .BindConfiguration(FeatureTogglesOptions.FeatureToggles)
            .ValidateDataAnnotations()
            .ValidateOnStart();

        // Add mediatr.
        serviceCollection.AddMediatR(cfg =>
            cfg.RegisterServicesFromAssembly(typeof(ServiceCollectionExtensions).Assembly));

        return serviceCollection;
    }

    public static IServiceCollection AddApplicationTransactionIdServices(this IServiceCollection serviceCollection)
    {
        // Application services
        serviceCollection.AddSingleton<ITransactionIdService, TransactionIdService>();

        serviceCollection.AddHttpClient();

        // Add options
        serviceCollection
            .AddOptions<FeatureTogglesOptions>()
            .BindConfiguration(FeatureTogglesOptions.FeatureToggles)
            .ValidateDataAnnotations()
            .ValidateOnStart();

        return serviceCollection;
    }

    public static IServiceCollection AddApplicationTransactionStreamServices(this IServiceCollection serviceCollection)
    {
        // Add options
        serviceCollection
            .AddOptions<FeatureTogglesOptions>()
            .BindConfiguration(FeatureTogglesOptions.FeatureToggles)
            .ValidateDataAnnotations()
            .ValidateOnStart();

        return serviceCollection;
    }

    public static IServiceCollection AddApplicationWorkerServices(this IServiceCollection serviceCollection)
    {
        // Scoped Services
        serviceCollection.AddScoped<IBackgroundVaultService, BackgroundVaultService>();
        serviceCollection.AddScoped<IStarkExEventHandlerService, StarkExEventHandlerService>();

        // Add options
        serviceCollection
            .AddOptions<FeatureTogglesOptions>()
            .BindConfiguration(FeatureTogglesOptions.FeatureToggles)
            .ValidateDataAnnotations()
            .ValidateOnStart();

        return serviceCollection;
    }
}