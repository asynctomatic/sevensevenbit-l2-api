namespace SevenSevenBit.Operator.IntegrationTests.UseCases.Mint.SubmitBatchMint;

using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using SevenSevenBit.Operator.Application.DTOs;
using SevenSevenBit.Operator.Domain.Entities;
using SevenSevenBit.Operator.Domain.Enums;
using SevenSevenBit.Operator.Infrastructure.SQL.UnitOfWork;
using SevenSevenBit.Operator.IntegrationTests.Fixture;
using SevenSevenBit.Operator.IntegrationTests.UseCases.Mint.SubmitBatchMint.Data;
using SevenSevenBit.Operator.TestHelpers.Data;
using SevenSevenBit.Operator.Web.Models.Api.Mint;
using Xunit;

[Trait("Type", "Mint")]
[Trait("UseCase", "SubmitBatchMint")]
[Collection("Api Integration Tests")]
public class SubmitBatchMintTests : IAsyncLifetime
{
    private readonly OperatorApiFactory api;
    private readonly DatabaseFixture database;

    public SubmitBatchMintTests(OperatorApiFactory api, DatabaseFixture database)
    {
        this.api = api;
        this.database = database;
    }

    public async Task InitializeAsync()
    {
        // Seed the database with users and assets.
        var unitOfWork = new UnitOfWork(database.Context);  // TODO: Move to db fixture class.
        await unitOfWork.Repository<User>().InsertAsync(Users.GetUsers(), CancellationToken.None);
        await unitOfWork.Repository<Asset>().InsertAsync(Assets.GetAssets(), CancellationToken.None);
        await unitOfWork.SaveAsync(CancellationToken.None);
    }

    public Task DisposeAsync() => database.ResetAsync();

    [Theory]
    [ClassData(typeof(SuccessTestData))]
    public async Task SubmitBatchMint_HappyPath_BatchMintIsAccepted(BatchMintRequestModel mintRequest)
    {
        // Arrange
        var client = api.HttpClient;

        // Act
        // Send mint POST request to /api/mint endpoint.
        var response = await client.PostAsJsonAsync(
            new Uri("mint", UriKind.Relative),
            mintRequest,
            api.JsonSerializerOptions);

        // Deserialize response body into response object.
        var responseBody = await response.Content.ReadFromJsonAsync<TransactionIdDto>();

        // Check that the response is 202 Accepted and that the response body is not null.
        response.StatusCode.Should().Be(HttpStatusCode.Accepted);
        responseBody.Should().NotBeNull();
        responseBody.TransactionId.Should().NotBeEmpty();

        // Check that the transaction is in the database.
        var transaction = await database.UnitOfWork.Repository<Transaction>()
            .GetByIdAsync(responseBody.TransactionId, CancellationToken.None);
        transaction.Should().NotBeNull();
        transaction.Operation.Should().Be(StarkExOperation.MultiTransaction);
        transaction.Status.Should().Be(TransactionStatus.Streamed);
    }

    // TODO: Implement.
}