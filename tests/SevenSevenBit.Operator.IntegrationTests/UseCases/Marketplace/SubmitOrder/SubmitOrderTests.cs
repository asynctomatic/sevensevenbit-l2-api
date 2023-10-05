namespace SevenSevenBit.Operator.IntegrationTests.UseCases.Marketplace.SubmitOrder;

using System.Net;
using System.Net.Http.Json;
using System.Numerics;
using FluentAssertions;
using NodaTime;
using SevenSevenBit.Operator.Domain.Entities;
using SevenSevenBit.Operator.Domain.Entities.Marketplace;
using SevenSevenBit.Operator.Domain.Enums;
using SevenSevenBit.Operator.Infrastructure.SQL.UnitOfWork;
using SevenSevenBit.Operator.IntegrationTests.Fixture;
using SevenSevenBit.Operator.TestHelpers.Data;
using SevenSevenBit.Operator.Web.Models.Api;
using SevenSevenBit.Operator.Web.Models.Api.Marketplace.Response;
using SevenSevenBit.Operator.Web.Models.Api.Order;
using StarkEx.Client.SDK.Models.Spot.TransactionModels;
using StarkEx.Crypto.SDK.Signing;
using Xunit;

[Trait("Type", "Marketplace")]
[Trait("UseCase", "SubmitOrder")]
[Collection("Api Integration Tests")]
public class SubmitOrderTests : IAsyncLifetime
{
    private readonly OperatorApiFactory api;
    private readonly DatabaseFixture database;

    public SubmitOrderTests(OperatorApiFactory api, DatabaseFixture database)
    {
        this.api = api;
        this.database = database;
    }

    public async Task InitializeAsync()
    {
        // Seed the database with test data.
        var unitOfWork = new UnitOfWork(database.Context);
        await unitOfWork.Repository<User>().InsertAsync(Users.GetUsers(), CancellationToken.None);
        await unitOfWork.Repository<Asset>().InsertAsync(Assets.GetAssets(), CancellationToken.None);
        await unitOfWork.Repository<Marketplace>().InsertAsync(Marketplaces.GetAll(), CancellationToken.None);
        await unitOfWork.SaveAsync(CancellationToken.None);
    }

    public Task DisposeAsync() => database.ResetAsync();

    // Place bid on empty order book.
    // Place ask on empty order book.
    [Theory]
    [InlineData(OrderSide.Bid)]
    [InlineData(OrderSide.Ask)]
    public async Task SubmitOrder_EmptyOrderBook_OrderSubmitted(OrderSide side)
    {
        // Arrange
        var user = Users.Alice;
        var privateKey = "0x639b2a37cd7aa37d722d3ddfaa04b6adf2a0422437d3a693980cf4ecc47e68a";
        var marketplace = Marketplaces.Erc20EthMarketplace;

        var baseAssetAmount = BigInteger.Parse("10") * BigInteger.Pow(10, 18);
        var quoteAssetAmount = BigInteger.Parse("1567") * BigInteger.Pow(10, 18);

        var unitOfWork = new UnitOfWork(database.Context);
        await unitOfWork.Repository<Vault>().InsertAsync(
            new Vault
            {
                UserId = user.Id,
                AssetId = side == OrderSide.Bid ? marketplace.QuoteAssetId : marketplace.BaseAssetId,
                QuantizedAvailableBalance = side == OrderSide.Bid ? quoteAssetAmount : baseAssetAmount,
                QuantizedAccountingBalance = side == OrderSide.Bid ? quoteAssetAmount : baseAssetAmount,
                DataAvailabilityMode = DataAvailabilityModes.Validium,
            },
            CancellationToken.None);
        await unitOfWork.SaveAsync(CancellationToken.None);

        // Act
        // Request signable order from /api/marketplaces/{marketplaceId}/orders/signable endpoint.
        var signableOrderRequest = new SignableOrderRequest
        {
            UserId = Users.Alice.Id,
            Side = side,
            BaseAssetAmount = baseAssetAmount,
            QuoteAssetAmount = quoteAssetAmount,
        };
        var signableOrderResponse = await api.HttpClient.PostAsJsonAsync(
            new Uri($"marketplaces/{marketplace.Id}/orders/signable", UriKind.Relative),
            signableOrderRequest,
            api.JsonSerializerOptions);

        // Assert
        signableOrderResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        var signableOrder = await signableOrderResponse.Content.ReadFromJsonAsync<SignableOrderResponse>();
        signableOrder.Metadata.BaseAssetAmountQuantized.Should()
            .Be(baseAssetAmount / marketplace.BaseAsset.Quantum.Value);
        signableOrder.Metadata.QuoteAssetAmountQuantized.Should()
            .Be(quoteAssetAmount / marketplace.QuoteAsset.Quantum.Value);

        // Arrange
        var signer = new StarkExSigner(new StarkCurve());
        var orderSignature = signer.SignMessage(signableOrder.Signable, privateKey);

        // Act
        // Send order submission POST request to /api/marketplaces/{marketplaceId}/orders endpoint.
        var submitOrderResponse = await api.HttpClient.PostAsJsonAsync(
            new Uri($"marketplaces/{marketplace.Id}/orders", UriKind.Relative),
            new SubmitOrderRequest
            {
                UserId = Users.Alice.Id,
                Side = side,
                BaseAssetAmountQuantized = signableOrder.Metadata.BaseAssetAmountQuantized,
                QuoteAssetAmountQuantized = signableOrder.Metadata.QuoteAssetAmountQuantized,
                Nonce = signableOrder.Metadata.Nonce,
                ExpirationTimestamp = signableOrder.Metadata.ExpirationTimestamp,
                Signature = new SignatureModel
                {
                    R = orderSignature.R,
                    S = orderSignature.S,
                },
            },
            api.JsonSerializerOptions);

        // Assert
        submitOrderResponse.StatusCode.Should().Be(HttpStatusCode.Created);
        var order = await submitOrderResponse.Content.ReadFromJsonAsync<OrderResponse>();
        var expectedOrder = new OrderResponse
        {
            MarketplaceId = marketplace.Id,
            UserId = user.Id,
            Type = OrderType.Limit,
            Side = side,
            Status = OrderStatus.Placed,
            Price = (quoteAssetAmount / marketplace.QuoteAsset.Quantum) / (baseAssetAmount / marketplace.BaseAsset.Quantum),
            Size = quoteAssetAmount,
            AvailableSize = quoteAssetAmount,
            Matches = new List<OrderMatchResponse>(),
        };
        order.Should().BeEquivalentTo(
            expectedOrder, options => options.Excluding(o => o.Id));
    }

    // Place bid on non-empty order book (with no matches).
    // Place ask on non-empty order book (with no matches).
    // Place bid on non-empty order book (with single match).
    // Place ask on non-empty order book (with single match).
    // Place bid on non-empty order book (with multiple matches, fully filled).
    // Place ask on non-empty order book (with multiple matches, fully filled).
    // Place bid on non-empty order book (with multiple matches, some partial, fully filled).
    // Place ask on non-empty order book (with multiple matches, some partial, fully filled).
    // Place bid on non-empty order book (with multiple matches and some partial, not fully filled).
    // Place ask on non-empty order book (with multiple matches and some partial, not fully filled).

    [Theory]
    [InlineData(OrderSide.Bid)]
    [InlineData(OrderSide.Ask)]
    public async Task SubmitOrder_WithSingleMatch_OrderFilled(OrderSide side)
    {
        // Arrange
        var baseAssetAmount = BigInteger.Parse("1") * BigInteger.Pow(10, 18);
        var quoteAssetAmount = BigInteger.Parse("157097") * BigInteger.Pow(10, 16);

        var marketplace = Marketplaces.Erc20EthMarketplace;
        marketplace.Orders.Add(new MarketplaceOrder
        {
            Id = Guid.NewGuid(),
            UserId = Users.Bob.Id,
            Type = OrderType.Limit,
            Side = side == OrderSide.Bid ? OrderSide.Ask : OrderSide.Bid,
            Status = OrderStatus.Placed,
            CreatedAt = new LocalDateTime(2022, 08, 30, 15, 0, 0, 0),
            OrderModel = new OrderRequestModel
            {
                BuyAmount = side == OrderSide.Bid ? quoteAssetAmount : baseAssetAmount,
                SellAmount = side == OrderSide.Ask ? quoteAssetAmount : baseAssetAmount,
                ExpirationTimestamp = long.MaxValue,
            },
        });

        // Act
        // Assert
    }

    [Theory]
    [InlineData(OrderSide.Bid)]
    [InlineData(OrderSide.Ask)]
    public async Task SubmitOrder_WithSingleMatch_OrderPartiallyFilled(OrderSide side)
    {
        // Arrange
        // Act
        // Assert
    }

    [Theory]
    [InlineData(OrderSide.Bid)]
    [InlineData(OrderSide.Ask)]
    public async Task SubmitOrder_WithMultipleMatches_OrderFilled(OrderSide side)
    {
        // Arrange
        // Act
        // Assert
    }

    [Theory]
    [InlineData(OrderSide.Bid)]
    [InlineData(OrderSide.Ask)]
    public async Task SubmitOrder_WithMultipleMatches_OrderPartiallyFilled(OrderSide side)
    {
        // Arrange
        // Act
        // Assert
    }
}