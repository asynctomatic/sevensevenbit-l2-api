namespace SevenSevenBit.Operator.IntegrationTests.UseCases.Asset.GetAssetById;

using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using SevenSevenBit.Operator.Application.DTOs.Entities;
using SevenSevenBit.Operator.Domain.Entities;
using SevenSevenBit.Operator.Infrastructure.SQL.UnitOfWork;
using SevenSevenBit.Operator.IntegrationTests.Fixture;
using SevenSevenBit.Operator.TestHelpers.Data;
using Xunit;

[Collection("Api Integration Tests")]
public class GetAssetByIdTests : IAsyncLifetime
{
    private readonly OperatorApiFactory api;
    private readonly DatabaseFixture database;

    public GetAssetByIdTests(OperatorApiFactory api, DatabaseFixture database)
    {
        this.api = api;
        this.database = database;
    }

    public Task InitializeAsync() => Task.CompletedTask;

    public Task DisposeAsync() => database.ResetAsync();

    [Fact]
    public async Task GetAssetById_AssetExists_AssetIsReturned()
    {
        // Arrange
        var unitOfWork = new UnitOfWork(database.Context);
        await unitOfWork.Repository<Asset>().InsertAsync(Assets.Eth, CancellationToken.None);
        await unitOfWork.SaveAsync(CancellationToken.None);

        // Act
        var response = await api.HttpClient.GetAsync(new Uri($"assets/{Assets.Eth.Id}", UriKind.Relative));
        var asset = await response.Content.ReadFromJsonAsync<AssetDto>(api.JsonSerializerOptions);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task GetAssetById_AssetDoesNotExist_NotFoundIsReturned()
    {
        // Arrange
        var assetId = Guid.NewGuid();

        // Act
        var response = await api.HttpClient.GetAsync(new Uri($"assets/{assetId}", UriKind.Relative));

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
}