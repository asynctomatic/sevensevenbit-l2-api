namespace SevenSevenBit.Operator.IntegrationTests.UseCases.Mint.SubmitSingleMint;

using System.Net;
using System.Net.Http.Json;
using System.Numerics;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using SevenSevenBit.Operator.Application.DTOs;
using SevenSevenBit.Operator.Domain.Entities;
using SevenSevenBit.Operator.Domain.Enums;
using SevenSevenBit.Operator.Infrastructure.SQL.UnitOfWork;
using SevenSevenBit.Operator.IntegrationTests.Fixture;
using SevenSevenBit.Operator.IntegrationTests.UseCases.Mint.SubmitSingleMint.Data;
using SevenSevenBit.Operator.TestHelpers.Data;
using SevenSevenBit.Operator.Web.Models.Api.Mint;
using Xunit;

[Trait("Type", "Mint")]
[Trait("UseCase", "SubmitSingleMint")]
[Collection("Api Integration Tests")]
public class SubmitSingleMintTests : IAsyncLifetime
{
    private readonly OperatorApiFactory api;
    private readonly DatabaseFixture database;

    public SubmitSingleMintTests(OperatorApiFactory api, DatabaseFixture database)
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
    [ClassData(typeof(StatefulValidationErrorTestData))]
    public async Task SubmitSingleMint_StatefulValidationError(
        BatchMintRequestModel mintRequest, HttpStatusCode statusCode, string errorCode)
    {
        // Arrange
        var client = api.HttpClient;

        // Act
        // Send mint POST request to /api/mint endpoint.
        var response = await client.PostAsJsonAsync(
            new Uri("mint", UriKind.Relative),
            mintRequest,
            api.JsonSerializerOptions);

        // Deserialize response body into ProblemDetails object.
        var problemDetails = await response.Content.ReadFromJsonAsync<ProblemDetails>();

        // Assert
        response.StatusCode.Should().Be(statusCode);
        problemDetails.Type.Should().Be(errorCode);
    }

    [Theory]
    [ClassData(typeof(SuccessTestData))]
    public async Task SubmitSingleMint_HappyPath_SingleMintIsAccepted(
        BatchMintRequestModel mintRequest)
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

        // Assert
        // Check that the response is 202 Accepted and that the response body is not null.
        response.StatusCode.Should().Be(HttpStatusCode.Accepted);
        responseBody.Should().NotBeNull();
        responseBody.TransactionId.Should().NotBeEmpty();

        // Check that the transaction is in the database.
        var transaction = await database.UnitOfWork.Repository<Transaction>()
            .GetByIdAsync(responseBody.TransactionId, CancellationToken.None);
        transaction.Should().NotBeNull();
        transaction.Operation.Should().Be(StarkExOperation.Mint);
        transaction.Status.Should().Be(TransactionStatus.Streamed);

        // TODO: Check that the vault update is in the database.
        // var recipient = await database.UnitOfWork.Repository<User>()
        //     .GetByIdAsync(mintRequest.Mints.First().UserId, CancellationToken.None);
        // var recipientVault = recipient.Vaults.Find(vault => vault.VaultUpdates.Exists(
        //     update => update.TransactionId.Equals(responseBody.TransactionId)));
        // recipientVault.Should().NotBeNull();
        // recipientVault.QuantizedAvailableBalance.Should().Be(BigInteger.Zero);
        // recipientVault.QuantizedAccountingBalance.Should().Be(
        //     mintRequest.Mints.First().Amount / recipientVault.Asset.Quantum.Value);
    }
}