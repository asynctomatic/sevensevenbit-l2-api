namespace SevenSevenBit.Operator.IntegrationTests.Configuration;

using System.Text.Json;
using System.Text.Json.Serialization;
using MassTransit;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using NodaTime;
using NodaTime.Serialization.SystemTextJson;
using SevenSevenBit.Operator.Infrastructure.MessageBus.Consumers;
using SevenSevenBit.Operator.IntegrationTests.Configuration.Auth;
using SevenSevenBit.Operator.SharedKernel.Json.Converters;

public class ApplicationFactory<TProgram> : WebApplicationFactory<TProgram>
    where TProgram : class
{
    private const string DefaultTenantId = "QyGf33tuI4GMJquSOpOZauWsJ6xxB7sL";

    public ApplicationFactory()
    {
        JsonSerializerOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            Converters = { new BigIntegerStringConverter(), new MemberValueConverter(), new JsonStringEnumConverter() },
        };
        JsonSerializerOptions.ConfigureForNodaTime(DateTimeZoneProviders.Tzdb);
    }

    public JsonSerializerOptions JsonSerializerOptions { get; set; }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureTestServices(services =>
        {
            services.Configure<ApiHandlerMockOptions>(options => options.DefaultClientId = DefaultTenantId);

            services.AddAuthentication(AuthHandlerMock.AuthScheme)
                .AddScheme<ApiHandlerMockOptions, AuthHandlerMock>(AuthHandlerMock.AuthScheme, _ => { });

            services.AddMassTransitTestHarness(x =>
            {
                x.AddConsumer<TransactionStreamResultConsumer>();
                x.SetTestTimeouts(
                    new TimeSpan(0, 1, 0),
                    new TimeSpan(0, 1, 0));
            });
        });
    }
}