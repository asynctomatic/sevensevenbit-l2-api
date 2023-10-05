namespace SevenSevenBit.Operator.IntegrationTests.Fixture;

using System.Data.Common;
using Microsoft.EntityFrameworkCore;
using Respawn;
using SevenSevenBit.Operator.Application.Common.Interfaces;
using SevenSevenBit.Operator.Infrastructure.SQL.Data.OperatorData;
using SevenSevenBit.Operator.Infrastructure.SQL.UnitOfWork;
using Testcontainers.PostgreSql;
using Xunit;

public class DatabaseFixture : IAsyncLifetime
{
    private readonly PostgreSqlContainer database;

    private DbConnection connection;
    private Respawner respawner;

    public DatabaseFixture()
    {
        database = new PostgreSqlBuilder()
                .WithImage("postgres:14.6")
                .WithPortBinding(5432)
                .WithDatabase("sevensevenbit-db")
                .WithUsername("postgres")
                .WithPassword("somepassword")
                .WithCleanUp(true)
                .Build();
    }

    public OperatorDbContext Context { get; private set; }

    public IUnitOfWork UnitOfWork { get; private set; }

    public async Task InitializeAsync()
    {
        // Start the container.
        await database.StartAsync();

        // TODO: hard-coded connection string
        var options = new DbContextOptionsBuilder<OperatorDbContext>()
            .UseNpgsql(
                connectionString: database.GetConnectionString(),
                npgsqlOptionsAction: options =>
                {
                    options.UseNodaTime();
                })
            .Options;

        Context = new OperatorDbContext(options);
        UnitOfWork = new UnitOfWork(Context);

        // Initialize database and apply migrations.
        await Context.Database.EnsureDeletedAsync();
        await Context.Database.MigrateAsync();

        // TODO: seed DB data.

        // Open database connection.
        connection = Context.Database.GetDbConnection();
        await connection.OpenAsync();

        // Initialize database respawner.
        respawner = await Respawner.CreateAsync(connection, new RespawnerOptions
        {
            DbAdapter = DbAdapter.Postgres,
            SchemasToInclude = new[] { "public" },
        });
    }

    public async Task DisposeAsync()
    {
        // Close database connection.
        await connection.CloseAsync();

        // Delete database and dispose context.
        await Context.Database.EnsureDeletedAsync();
        await Context.DisposeAsync();

        // Stop the container.
        await database.StopAsync();
    }

    public async Task ResetAsync()
    {
        await respawner.ResetAsync(connection);
    }
}