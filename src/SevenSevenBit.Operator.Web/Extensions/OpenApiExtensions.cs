namespace SevenSevenBit.Operator.Web.Extensions;

using System.Diagnostics.CodeAnalysis;
using System.Numerics;
using System.Reflection;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Interfaces;
using Microsoft.OpenApi.Models;
using NodaTime;
using SevenSevenBit.Operator.Domain.Constants.Auth;
using SevenSevenBit.Operator.Infrastructure.Identity.Options;
using SevenSevenBit.Operator.Web.Docs.Filters;
using SevenSevenBit.Operator.Web.Options.Documentation;

[ExcludeFromCodeCoverage]
public static class OpenApiExtensions
{
    public static IServiceCollection AddSwagger(this IServiceCollection serviceCollection, IConfiguration configuration)
    {
        var authSettings = configuration.GetSection(AuthenticationOptions.Authentication).Get<AuthenticationOptions>();
        var docSettings = configuration.GetSection(DocumentationOptions.Documentation).Get<DocumentationOptions>();
        var serviceProvider = serviceCollection.BuildServiceProvider();
        var apiVersionDescriptionProvider = serviceProvider.GetRequiredService<IApiVersionDescriptionProvider>();

        serviceCollection.AddSwaggerGen(options =>
        {
            var apiInfo = new OpenApiInfo
            {
                Title = docSettings.Title,
                Description = docSettings.Description,
                TermsOfService = new Uri(docSettings.License),
                License = new OpenApiLicense
                {
                    Name = "License",
                    Url = new Uri(docSettings.TermsOfService),
                },
                Extensions = new Dictionary<string, IOpenApiExtension>
                {
                    {
                        "x-logo", new OpenApiObject
                        {
                            { "url", new OpenApiString(docSettings.Logo) },
                            { "altText", new OpenApiString(docSettings.LogoTooltip) },
                        }
                    },
                },
            };

            foreach (var description in apiVersionDescriptionProvider.ApiVersionDescriptions)
            {
                options.SwaggerDoc($"api-{description.GroupName}", CreateVersionInfo(description, apiInfo));
                options.SwaggerDoc($"admin-{description.GroupName}", CreateVersionInfo(description, apiInfo));
                options.SwaggerDoc($"starkex-{description.GroupName}", CreateVersionInfo(description, apiInfo));
            }

            // Enable xml annotations.
            var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
            options.IncludeXmlComments(xmlPath);

            // Enable Swashbuckle annotations.
            options.EnableAnnotations();

            // Define the auth scheme that's in use (ie. OAuth2.0).
            options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
            {
                Type = SecuritySchemeType.OAuth2,
                Flows = new OpenApiOAuthFlows
                {
                    ClientCredentials = new OpenApiOAuthFlow
                    {
                        TokenUrl = new Uri(authSettings.Authority + "oauth/token", UriKind.Absolute),
                        Scopes = new Dictionary<string, string>
                        {
                            { Scopes.WriteUsers, "Access user write operations." },
                            { Scopes.ReadUsers, "Access user read operations." },
                            { Scopes.WriteAssets, "Access asset write operations." },
                            { Scopes.ReadAssets, "Access asset read operations." },
                            { Scopes.MintAssets, "Access mint operations." },
                            { Scopes.ReadVaults, "Access vault read operations." },
                            { Scopes.WriteTransfers, "Access transfer write operations." },
                            { Scopes.WriteSettlements, "Access settlement write operations." },
                            { Scopes.ReadTransactions, "Access tarnsaction read operations." },
                        },
                    },
                },
            });

            // Add servers.
            options.AddServer(new OpenApiServer
            {
                Url = "https://testnet-api.77-bit.com",
                Description = "Testnet server (Goerli).",
            });
            options.AddServer(new OpenApiServer
            {
                Url = "https://api.77-bit.com",
                Description = "Mainnet server (Ethereum).",
            });

            // Configure custom filters.
            options.DocumentFilter<TagDescriptionsDocumentFilter>();
            options.OperationFilter<SecurityRequirementsOperationFilter>();

            // Override schema types.
            options.MapType<BigInteger>(() => new OpenApiSchema { Type = "string" });
            options.MapType<LocalDateTime>(() => new OpenApiSchema { Type = "string" });
        });

        return serviceCollection;
    }

    public static void UseDocumentationPage(this WebApplication app)
    {
        var configuration = app.Services.GetRequiredService<IConfiguration>();
        var docSettings = configuration.GetSection(DocumentationOptions.Documentation).Get<DocumentationOptions>();
        var apiVersionDescriptionProvider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();

        // Expose OpenAPI json file endpoint.
        app.UseSwagger();

        // Expose Swagger UI.
        app.UseSwaggerUI(c =>
        {
            foreach (var description in apiVersionDescriptionProvider.ApiVersionDescriptions.Reverse())
            {
                c.SwaggerEndpoint(
                    $"/swagger/api-{description.GroupName}/swagger.json",
                    $"API Docs {description.GroupName.ToUpperInvariant()}");
                c.SwaggerEndpoint(
                    $"/swagger/admin-{description.GroupName}/swagger.json",
                    $"Admin Docs {description.GroupName.ToUpperInvariant()}");
                c.SwaggerEndpoint(
                    $"/swagger/starkex-{description.GroupName}/swagger.json",
                    $"StarkEx Docs {description.GroupName.ToUpperInvariant()}");
            }
        });

        var lastVersion = apiVersionDescriptionProvider.ApiVersionDescriptions[^1];

        // Create API documentation UI using ReDoc.
        app.UseReDoc(c =>
        {
            c.DocumentTitle = docSettings.Title;
            c.SpecUrl = $"/swagger/api-{lastVersion.GroupName}/swagger.json";

            // Only expand responses for 200:Ok and 201:Created status codes.
            c.ExpandResponses("200,201");
        });
    }

    private static OpenApiInfo CreateVersionInfo(
        ApiVersionDescription desc,
        OpenApiInfo info)
    {
        info.Version = desc.ApiVersion.ToString();

        if (desc.IsDeprecated)
        {
            info.Description += " This API version has been deprecated. Please use one of the new APIs available from the explorer.";
        }

        return info;
    }
}