using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using NodaTime;
using NodaTime.Serialization.SystemTextJson;
using Prometheus;
using Serilog;
using SevenSevenBit.Operator.Application.DependencyInjection;
using SevenSevenBit.Operator.Infrastructure.Blockchain.DependencyInjection;
using SevenSevenBit.Operator.Infrastructure.Identity.DependencyInjection;
using SevenSevenBit.Operator.Infrastructure.MessageBus.DependencyInjection;
using SevenSevenBit.Operator.Infrastructure.SQL.Data.OperatorData;
using SevenSevenBit.Operator.Infrastructure.SQL.DependencyInjection;
using SevenSevenBit.Operator.Infrastructure.StarkExApi.DependencyInjection;
using SevenSevenBit.Operator.SharedKernel.Json.Converters;
using SevenSevenBit.Operator.Web.Extensions;
using SevenSevenBit.Operator.Web.Factories;

var builder = WebApplication.CreateBuilder(args);

builder.Host.ConfigureLogging(logging
    => logging.ClearProviders().AddSerilog());

builder.Host.UseSerilog((context, cfg)
    => cfg.ReadFrom.Configuration(context.Configuration));

builder.Host.UseDefaultServiceProvider((context, provider) =>
{
    provider.ValidateScopes =
        provider.ValidateOnBuild =
            context.HostingEnvironment.IsDevelopment();
});

builder.Host.ConfigureServices((context, services) =>
{
    // MVC
    services
        .AddControllers()
        .AddJsonOptions(options =>
        {
            options.JsonSerializerOptions.Converters.Add(new BigIntegerStringConverter());
            options.JsonSerializerOptions.Converters.Add(new MemberValueConverter());
            options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
            options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
            options.JsonSerializerOptions.ConfigureForNodaTime(DateTimeZoneProviders.Tzdb);
            options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
        })
        .ConfigureApiBehaviorOptions(options =>
        {
            options.SuppressMapClientErrors = true;
            options.InvalidModelStateResponseFactory = InvalidModelStateResponseFactory.SerializeInvalidModelStateResponse;
        });

    // Healthcheck
    services.AddHealthChecks();

    // Services
    services
        .AddWebServices()
        .AddApplicationWebServices()
        .AddSqlInfrastructureServices()
        .AddBlockchainInfrastructureServices()
        .AddIdentityInfrastructureServices(context.Configuration)
        .AddStarkExInfrastructureServices()
        .AddMessageBusWebInfrastructureServices<OperatorDbContext>(context.HostingEnvironment.IsProduction())
        .AddApiVersioning(opt =>
        {
            opt.DefaultApiVersion = new(1, 0);
            opt.AssumeDefaultVersionWhenUnspecified = false;
            opt.ReportApiVersions = true;
            opt.ApiVersionReader = new UrlSegmentApiVersionReader();
        }).AddVersionedApiExplorer(setup =>
        {
            setup.GroupNameFormat = "'v'VVV";
            setup.SubstituteApiVersionInUrl = true;
        })
        .AddEndpointsApiExplorer()
        .AddSwagger(context.Configuration);
});

var app = builder.Build();

app.UseSerilogRequestLogging();

app.UseExceptionHandler(appBuilder =>
{
    appBuilder.Run(async context =>
    {
        context.Response.StatusCode = StatusCodes.Status500InternalServerError;

        ProblemDetails problemDetails = new()
        {
            Title = "Internal Server Error",
            Status = (int)HttpStatusCode.InternalServerError,
            Type = "InternalServerError",
            Detail = "An internal server error occurred",
            Instance = context.Request.Path,
        };

        await context.Response.WriteAsJsonAsync(problemDetails);
    });
});

if (app.Environment.IsDevelopment())
{
    app.UseDocumentationPage();
}

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.UseHealthChecks("/healthz");

app.MapMetrics();
app.MapControllers();

try
{
    Log.Information("Starting the service {ApplicationName}", app.Environment.ApplicationName);
    await app.RunAsync();
    Log.Information("Shutting down the service");
}
catch (Exception ex)
{
    Log.Fatal(ex, "An unhandled exception occurred");
    await app.StopAsync();
}
finally
{
    Log.Information("Disposing the service");
    await app.DisposeAsync();
}

[ExcludeFromCodeCoverage]
public partial class Program
{
}