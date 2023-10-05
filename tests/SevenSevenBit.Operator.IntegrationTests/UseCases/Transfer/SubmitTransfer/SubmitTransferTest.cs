namespace SevenSevenBit.Operator.IntegrationTests.UseCases.Transfer.SubmitTransfer;

using System.Net;
using System.Net.Http.Json;
using System.Numerics;
using FluentAssertions;
using SevenSevenBit.Operator.Application.DTOs.Details;
using SevenSevenBit.Operator.Domain.Entities;
using SevenSevenBit.Operator.Domain.Enums;
using SevenSevenBit.Operator.Infrastructure.SQL.UnitOfWork;
using SevenSevenBit.Operator.IntegrationTests.Fixture;
using SevenSevenBit.Operator.IntegrationTests.UseCases.Transfer.SubmitTransfer.Data;
using SevenSevenBit.Operator.TestHelpers.Data;
using SevenSevenBit.Operator.Web.Models.Api;
using Xunit;

[Collection("Api Integration Tests")]
public class SubmitTransferTest : IAsyncLifetime
{
    private readonly OperatorApiFactory api;
    private readonly DatabaseFixture database;

    public SubmitTransferTest(OperatorApiFactory api, DatabaseFixture database)
    {
        this.api = api;
        this.database = database;
    }

    public async Task InitializeAsync()
    {
        // TODO: Move to db fixture class.
        var unitOfWork = new UnitOfWork(database.Context);

        // Seed the database with users.
        await unitOfWork.Repository<User>().InsertAsync(Users.GetUsers(), CancellationToken.None);

        // Seed the database with assets.
        await unitOfWork.Repository<Asset>().InsertAsync(Assets.GetAssets(), CancellationToken.None);

        await unitOfWork.SaveAsync(CancellationToken.None);
    }

    public Task DisposeAsync() => database.ResetAsync();

    [Theory]
    [ClassData(typeof(SuccessTestData))]
    public async Task SubmitTransfer_HappyPath_TransferIsAccepted(
        User sender, User recipient, Asset asset, string balance, string amount)
    {
        // Arrange
        var senderVault = new Vault
        {
            Id = Guid.NewGuid(),
            User = sender,
            Asset = asset,
            DataAvailabilityMode = DataAvailabilityModes.Validium,
            VaultChainId = new BigInteger(2), // TODO get random validium vault.
            QuantizedAvailableBalance = BigInteger.Parse(balance),
            QuantizedAccountingBalance = BigInteger.Parse(balance),
        };

        // Act
        var singableTransferResponse = await api.HttpClient.PostAsJsonAsync(
            new Uri("transfers/signable", UriKind.Relative),
            new TransferDetailsModel
            {
                FromUserId = sender.Id,
                ToUserId = recipient.Id,
                Amount = BigInteger.Parse(amount),
            },
            api.JsonSerializerOptions);

        var singableTransfer = await singableTransferResponse.Content.ReadFromJsonAsync<TransferSignableDto>();

        // Arrange
        // TODO: Compute signature

        // TODO fix this
        // Act
        var transferResponse = await api.HttpClient.PostAsJsonAsync(
            new Uri("transfers", UriKind.Relative),
            new TransferModel
            {
                FromUserId = senderVault.Id,
                // ReceiverVaultId = singableTransfer.ReceiverVaultId,
                // QuantizedAmount = singableTransfer.QuantizedAmount,
                // ExpirationTimestamp = singableTransfer.ExpirationTimestamp,
                // Nonce = singableTransfer.Nonce,
                Signature = new SignatureModel // TODO: Lazy compute signature.
                {
                    R = "b531b384cad3a256de13b2cc22a4541dddd33e806321abec2ae6ba8efd5182",
                    S = "7b99ab6bd4056472a62bc49fd74def65e0ef35a4ba66193bcd12755fc17205a",
                },
            },
            api.JsonSerializerOptions);

        // Assert
        transferResponse.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    // [Theory]
    // [ClassData(typeof(SuccessTestData))]
    public async Task SubmitTransfer_HappyPath_TransferIsAccepted_Old(
        User sender, User recipient, Asset asset, string balance, string amount)
    {
        // Arrange
        var senderVault = new Vault
        {
            Id = Guid.NewGuid(),
            User = sender,
            Asset = asset,
            DataAvailabilityMode = DataAvailabilityModes.Validium,
            VaultChainId = new BigInteger(2), // TODO get random validium vault.
            QuantizedAvailableBalance = BigInteger.Parse(balance),
            QuantizedAccountingBalance = BigInteger.Parse(balance),
        };
        var recipientVault = new Vault
        {
            Id = Guid.NewGuid(),
            User = recipient,
            Asset = asset,
            DataAvailabilityMode = DataAvailabilityModes.Validium,
            VaultChainId = new BigInteger(3), // TODO get random validium vault.
            QuantizedAvailableBalance = BigInteger.Zero,
            QuantizedAccountingBalance = BigInteger.Zero,
        };

        var unitOfWork = new UnitOfWork(database.Context);
        await unitOfWork.Repository<Vault>().InsertAsync(
            new[] { senderVault, recipientVault }, CancellationToken.None);
        await unitOfWork.SaveAsync(CancellationToken.None);

        var transferModel = new TransferModel
        {
            FromUserId = senderVault.Id,
            ToUserId = recipientVault.Id, // TODO get random vault? Do I need to allocate it first?
            Amount = BigInteger.Parse(amount) * asset.Quantum,
            ExpirationTimestamp = 9999999999,   // TODO: what is this value?
            Nonce = 1,
            Signature = new SignatureModel // TODO: Lazy compute signature.
            {
                R = "b531b384cad3a256de13b2cc22a4541dddd33e806321abec2ae6ba8efd5182",
                S = "7b99ab6bd4056472a62bc49fd74def65e0ef35a4ba66193bcd12755fc17205a",
            },
        };

        // Act
        var response = await api.HttpClient.PostAsJsonAsync(
            new Uri("transfers", UriKind.Relative),
            transferModel,
            api.JsonSerializerOptions);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }
}