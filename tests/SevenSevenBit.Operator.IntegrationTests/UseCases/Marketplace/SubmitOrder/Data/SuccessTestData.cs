namespace SevenSevenBit.Operator.IntegrationTests.UseCases.Marketplace.SubmitOrder.Data;

using System.Collections;
using Domain.Enums;
using NodaTime;
using SevenSevenBit.Operator.Domain.Entities.Marketplace;
using SevenSevenBit.Operator.TestHelpers.Data;

public class SuccessTestData : IEnumerable<object[]>
{
    public IEnumerator<object[]> GetEnumerator()
    {
        // Place bid on empty order book
        yield return new object[]
        {
            "Place bid on empty order book",
            new Marketplace
            {
                Id = Guid.NewGuid(),
                BaseAssetId = Assets.Eth.Id,
                QuoteAssetId = Assets.Erc20.Id,
                Orders = new List<MarketplaceOrder>(),
            },
        };

        // Place bid on non-empty order book (with no matches)
        yield return new object[]
        {
            "Place bid on non-empty order book (with no matches)",
            new Marketplace
            {
                Id = Guid.NewGuid(),
                BaseAssetId = Guid.NewGuid(),
                QuoteAssetId = Guid.NewGuid(),
                Orders = new List<MarketplaceOrder>
                {
                    new MarketplaceOrder
                    {
                        Id = Guid.NewGuid(),
                        Type = OrderType.Limit,
                        Side = OrderSide.Ask,
                        Status = OrderStatus.Placed,
                        CreatedAt = new LocalDateTime(2022, 08, 30, 15, 0, 0, 0),
                    },
                },
            },
        };

        yield return new object[]
        {
        };
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}