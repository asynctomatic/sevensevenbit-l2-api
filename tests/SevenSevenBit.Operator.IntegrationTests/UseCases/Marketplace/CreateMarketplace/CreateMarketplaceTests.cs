namespace SevenSevenBit.Operator.IntegrationTests.UseCases.Marketplace.CreateMarketplace;

using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using SevenSevenBit.Operator.Domain.Entities;
using SevenSevenBit.Operator.Infrastructure.SQL.UnitOfWork;
using SevenSevenBit.Operator.IntegrationTests.Fixture;
using SevenSevenBit.Operator.IntegrationTests.UseCases.Marketplace.CreateMarketplace.Data;
using SevenSevenBit.Operator.TestHelpers.Data;
using SevenSevenBit.Operator.Web.Models.Api.Marketplace.Request;
using SevenSevenBit.Operator.Web.Models.Api.Marketplace.Response;
using Xunit;

[Trait("Type", "Marketplace")]
[Trait("UseCase", "CreateMarketplace")]
[Collection("Api Integration Tests")]
public class CreateMarketplaceTests : IAsyncLifetime
{
    private readonly OperatorApiFactory api;
    private readonly DatabaseFixture database;

    public CreateMarketplaceTests(OperatorApiFactory api, DatabaseFixture database)
    {
        this.api = api;
        this.database = database;
    }

    public async Task InitializeAsync()
    {
        var unitOfWork = new UnitOfWork(database.Context);  // TODO: Move to db fixture class.
        await unitOfWork.Repository<Asset>().InsertAsync(Assets.GetAssets(), CancellationToken.None);
        await unitOfWork.SaveAsync(CancellationToken.None);
    }

    public Task DisposeAsync() => database.ResetAsync();

    [Theory]
    [ClassData(typeof(StatelessValidationErrorTestData))]
    public async Task SubmitSingleMint_StatelessValidationError(
        CreateMarketplaceRequest request, string errorCode)
    {
        // Arrange
        var client = api.HttpClient;

        // Send mint POST request to /api/marketplaces endpoint.
        var response = await client.PostAsJsonAsync(
            new Uri("marketplaces", UriKind.Relative),
            request,
            api.JsonSerializerOptions);

        // Deserialize response body into ProblemDetails object.
        var problemDetails = await response.Content.ReadFromJsonAsync<ProblemDetails>();

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        problemDetails.Type.Should().Be(errorCode);
    }

    [Theory]
    [ClassData(typeof(SuccessTestData))]
    public async Task CreateMarketplace_HappyPath_MarketplaceCreated(CreateMarketplaceRequest request)
    {
        // Arrange
        var client = api.HttpClient;

        // Act
        // Send user registration POST request to /api/marketplaces endpoint.
        var response = await client.PostAsJsonAsync(
            new Uri("marketplaces", UriKind.Relative),
            request,
            api.JsonSerializerOptions);

        // Deserialize response body into response object.
        // TODO: Validate response body.
        var responseBody = await response.Content.ReadFromJsonAsync<MarketplaceResponse>();

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Created);
        response.Headers.Location.Should().NotBeNull();
    }
}