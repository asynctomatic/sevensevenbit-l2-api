namespace SevenSevenBit.Operator.IntegrationTests.UseCases.Deposit.GetSignableDeposit;

using System.Net;
using System.Net.Http.Json;
using System.Numerics;
using Data;
using FluentAssertions;
using SevenSevenBit.Operator.Application.DTOs.Details;
using SevenSevenBit.Operator.Domain.Entities;
using SevenSevenBit.Operator.IntegrationTests.Fixture;
using SevenSevenBit.Operator.TestHelpers.Data;
using SevenSevenBit.Operator.Web.Models.Api;
using Xunit;

[Collection("Api Integration Tests")]
public class GetSignableDepositTests : IAsyncLifetime
{
    private readonly OperatorApiFactory api;
    private readonly DatabaseFixture database;

    public GetSignableDepositTests(OperatorApiFactory api, DatabaseFixture database)
    {
        this.api = api;
        this.database = database;
    }

    public async Task InitializeAsync()
    {
        // Seed the database with users.
        await database.UnitOfWork.Repository<User>().InsertAsync(Users.GetUsers(), CancellationToken.None);

        // Seed the database with assets.
        await database.UnitOfWork.Repository<Asset>().InsertAsync(Assets.GetAssets(), CancellationToken.None);

        await database.UnitOfWork.SaveAsync(CancellationToken.None);
    }

    public Task DisposeAsync() => database.ResetAsync();

    [Theory]
    [ClassData(typeof(SuccessTestData))]
    public async Task GetSignableDeposit_HappyPath_SignableDepositIsReturned(
        SignableDepositModel signableDepositRequest, DepositDetailsDto expectedResponseBody)
    {
        // Arrange
        var client = api.HttpClient;

        // Act
        // Send POST request to /api/deposits/signable endpoint.
        var response = await client.PostAsJsonAsync(
            new Uri("deposits/signable", UriKind.Relative),
            signableDepositRequest,
            api.JsonSerializerOptions);

        // Deserialize response body into response object.
        var responseBody = await response.Content.ReadFromJsonAsync<DepositDetailsDto>(api.JsonSerializerOptions);

        // Assert
        // Check that the response is 200 Ok and that the response body matches the expected.
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        responseBody.Should().BeEquivalentTo(expectedResponseBody);
    }
}