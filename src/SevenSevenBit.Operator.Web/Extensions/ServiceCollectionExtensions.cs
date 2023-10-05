namespace SevenSevenBit.Operator.Web.Extensions;

using System.Diagnostics.CodeAnalysis;

[ExcludeFromCodeCoverage]
public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddWebServices(
        this IServiceCollection serviceCollection)
    {
        // Microsoft Services
        serviceCollection.AddHttpContextAccessor();
        serviceCollection.AddHttpClient();

        return serviceCollection;
    }
}