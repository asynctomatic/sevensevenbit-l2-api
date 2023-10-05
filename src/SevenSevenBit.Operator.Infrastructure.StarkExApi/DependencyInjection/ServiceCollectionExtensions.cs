namespace SevenSevenBit.Operator.Infrastructure.StarkExApi.DependencyInjection;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SevenSevenBit.Operator.Application.Common.Interfaces;
using SevenSevenBit.Operator.Application.Interfaces.Services;
using SevenSevenBit.Operator.Infrastructure.StarkExApi.Factories;
using SevenSevenBit.Operator.Infrastructure.StarkExApi.Options;
using SevenSevenBit.Operator.Infrastructure.StarkExApi.Services;
using StarkEx.Client.SDK.Extensions;
using StarkEx.Client.SDK.Interfaces.Spot;
using StarkEx.Client.SDK.Settings;
using StarkEx.Crypto.SDK.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddStarkExInfrastructureServices(this IServiceCollection serviceCollection)
    {
        // External SDK
        serviceCollection.AddStarkEx();
        serviceCollection.AddStarkExCryptoUtils();

        // Services
        serviceCollection.AddSingleton<IStarkExApiGateway, StarkExApiGateway>();

        // Factories
        serviceCollection.AddSingleton<IFactory<StarkExApiSettings, ISpotGatewayClient>, SpotGatewayClientFactory>();

        // Options
        serviceCollection
            .AddOptions<StarkExApiOptions>()
            .BindConfiguration(StarkExApiOptions.StarkExApi)
            .ValidateDataAnnotations()
            .ValidateOnStart();

        serviceCollection.AddSingleton(sp =>
        {
            var starkExApiSettings = new StarkExApiSettings();
            var configuration = sp.GetRequiredService<IConfiguration>();
            configuration.GetSection(StarkExApiOptions.StarkExApi).Bind(starkExApiSettings);

            return starkExApiSettings;
        });

        return serviceCollection;
    }
}