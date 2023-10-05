namespace SevenSevenBit.Operator.Infrastructure.Identity.DependencyInjection;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SevenSevenBit.Operator.Infrastructure.Identity.Auth;
using SevenSevenBit.Operator.Infrastructure.Identity.Extensions;
using SevenSevenBit.Operator.Infrastructure.Identity.Options;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddIdentityInfrastructureServices(
        this IServiceCollection serviceCollection,
        IConfiguration configuration)
    {
        // Add options
        serviceCollection
            .AddOptions<AuthenticationOptions>()
            .BindConfiguration(AuthenticationOptions.Authentication)
            .ValidateDataAnnotations()
            .ValidateOnStart();

        // Auth setup
        var authSettings = configuration.GetSection(AuthenticationOptions.Authentication).Get<AuthenticationOptions>();

        serviceCollection.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.Audience = authSettings.Audience;
                options.Authority = authSettings.Authority;
            });

        serviceCollection.AddAuthorization(options =>
        {
            // API POLICIES
            options.AddPolicy(
                Policies.ReadUsers,
                policy => policy.RequireScope(Policies.ReadUsers.GetScopes()));

            options.AddPolicy(
                Policies.WriteUsers,
                policy => policy.RequireScope(Policies.WriteUsers.GetScopes()));

            options.AddPolicy(
                Policies.ReadAssets,
                policy => policy.RequireScope(Policies.ReadAssets.GetScopes()));

            options.AddPolicy(
                Policies.WriteAssets,
                policy => policy.RequireScope(Policies.WriteAssets.GetScopes()));

            options.AddPolicy(
                Policies.ReadVaults,
                policy => policy.RequireScope(Policies.ReadVaults.GetScopes()));

            options.AddPolicy(
                Policies.WriteVaults,
                policy => policy.RequireScope(Policies.WriteVaults.GetScopes()));

            options.AddPolicy(
                Policies.ReadWriteVaults,
                policy => policy.RequireScope(Policies.ReadWriteVaults.GetScopes()));

            options.AddPolicy(
                Policies.MintAssets,
                policy => policy.RequireScope(Policies.MintAssets.GetScopes()));

            options.AddPolicy(
                Policies.WriteTransfers,
                policy => policy.RequireScope(Policies.WriteTransfers.GetScopes()));

            options.AddPolicy(
                Policies.WriteSettlements,
                policy => policy.RequireScope(Policies.WriteSettlements.GetScopes()));

            options.AddPolicy(
                Policies.ReadTransactions,
                policy => policy.RequireScope(Policies.ReadTransactions.GetScopes()));

            options.AddPolicy(
                Policies.WriteMarketplaces,
                policy => policy.RequireScope(Policies.WriteMarketplaces.GetScopes()));

            options.AddPolicy(
                Policies.ReadMarketplaces,
                policy => policy.RequireScope(Policies.ReadMarketplaces.GetScopes()));

            options.AddPolicy(
                Policies.WriteOrders,
                policy => policy.RequireScope(Policies.WriteOrders.GetScopes()));

            options.AddPolicy(
                Policies.ReadOrders,
                policy => policy.RequireScope(Policies.ReadOrders.GetScopes()));

            // StarkEx POLICIES
            options.AddPolicy(
                Policies.AlternativeTxs,
                policy => policy.RequireScope(Policies.AlternativeTxs.GetScopes()));
        });

        return serviceCollection;
    }
}