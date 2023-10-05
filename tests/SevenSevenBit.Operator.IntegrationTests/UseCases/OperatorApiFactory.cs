namespace SevenSevenBit.Operator.IntegrationTests.UseCases;

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
using Xunit;

public class OperatorApiFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
    // private DbConnection dbConnection;
    // private Respawner dbRespawner;

    public JsonSerializerOptions JsonSerializerOptions { get; set; }

    public HttpClient HttpClient { get; private set; }

    public OperatorApiFactory()
    {
        // TODO: can we use the same options as in Startup?
        // TODO: does this need to be here?
        // TODO: can we move the HttpClient creation here? It is sync (or move all to InitializeAsync)
        JsonSerializerOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            Converters = { new BigIntegerStringConverter(), new MemberValueConverter(), new JsonStringEnumConverter() },
        };
        JsonSerializerOptions.ConfigureForNodaTime(DateTimeZoneProviders.Tzdb);
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureTestServices(services =>
        {
            // TODO: do we need this?
            // TODO: services.Configure<ApiHandlerMockOptions>(options => options.DefaultClientId = DefaultTenantId);

            // Mock auth handler.
            services.AddAuthentication(AuthHandlerMock.AuthScheme)
                .AddScheme<ApiHandlerMockOptions, AuthHandlerMock>(AuthHandlerMock.AuthScheme, _ => { });

            // Add MassTransit test harness.
            // TODO: What are these timeouts?
            services.AddMassTransitTestHarness(x =>
            {
                x.AddConsumer<TransactionStreamResultConsumer>();
                x.SetTestTimeouts(
                    new TimeSpan(0, 1, 0),
                    new TimeSpan(0, 1, 0));
            });
        });
    }

    public async Task InitializeAsync()
    {
        // TODO: hardcode connection string
        //var dbOptions = new DbContextOptionsBuilder<OperatorDbContext>()
        //    .UseNpgsql(
        //        connectionString: "Host=localhost:5432;Database=sevensevenbit-db;Username=postgres;Password=somepassword",
        //        npgsqlOptionsAction: options =>
        //        {
        //            options.UseNodaTime();
        //        })
        //    .Options;

        //dbConnection = new OperatorDbContext(dbOptions).Database.GetDbConnection();
        //await dbConnection.OpenAsync();

        //dbRespawner = await Respawner.CreateAsync(dbConnection, new RespawnerOptions
        //{
        //    DbAdapter = DbAdapter.Postgres,
        //    SchemasToInclude = new[] { "public" },
        //});

        HttpClient = CreateClient();
        HttpClient.BaseAddress = new Uri($"https://localhost/api/v1/");
        // TODO: check other client config from WebApplicationFactoryExtensions
    }

    public async Task DisposeAsync()
    {
        // await dbConnection.CloseAsync();
    }

    // TODO: Can we refactor all database setup/teardown to a dedicated fixture?
    // public async Task ResetDatabaseAsync()
    // {
    //     await dbRespawner.ResetAsync(dbConnection);
    // }
}