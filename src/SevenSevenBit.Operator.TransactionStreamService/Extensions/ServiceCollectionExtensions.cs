namespace SevenSevenBit.Operator.TransactionStreamService.Extensions;

using System.Diagnostics.CodeAnalysis;
using SevenSevenBit.Operator.Application.Options;

[ExcludeFromCodeCoverage]
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Adds core services to the service collection.
    /// </summary>
    /// <param name="serviceCollection">The instance of the <see cref="IServiceCollection"/> to which the services are being added.</param>
    /// <param name="configuration">Configuration of the TransactionStream runtime.</param>
    /// <returns>The service collection to allow chained DI setups.</returns>
    public static IServiceCollection AddTransactionStreamServices(this IServiceCollection serviceCollection, IConfiguration configuration)
    {
        var featureTogglesOptions = configuration.GetSection(FeatureTogglesOptions.FeatureToggles).Get<FeatureTogglesOptions>();

        /* TODO
         serviceCollection.AddHttpClient(HttpConstants.HttpStarkExClientName)
            .ConfigurePrimaryHttpMessageHandler(sp =>
            {
                var handler = new HttpClientHandler();

                if (!featureTogglesOptions.UseMtls)
                {
                    return handler;
                }

                var certificateClient = sp.GetService<CertificateClient>();

                var allCertificatesProperties = certificateClient.GetPropertiesOfCertificates();

                // This is only done once, on the application startup.
                foreach (var certificateProperties in allCertificatesProperties.
                             Where(certProps => certProps.Tags.TryGetValue(Tags.Domain, out var domain) && domain.Contains("starkex")))
                {
                    handler.ClientCertificates.Add(certificateClient.DownloadCertificate(certificateProperties.Name));
                }

                return handler;
            });*/

        return serviceCollection;
    }
}