namespace SevenSevenBit.Operator.IntegrationTests.UseCases.Marketplace.CreateMarketplace.Data;

using System.Collections;
using SevenSevenBit.Operator.TestHelpers.Data;
using SevenSevenBit.Operator.Web.Models.Api.Marketplace.Request;

public class SuccessTestData : IEnumerable<object[]>
{
    public IEnumerator<object[]> GetEnumerator()
    {
        yield return new object[]
        {
            new CreateMarketplaceRequest
            {
                BaseAssetId = Assets.Erc20.Id,
                QuoteAssetId = Assets.Eth.Id,
            },
        };

        yield return new object[]
        {
            new CreateMarketplaceRequest
            {
                BaseAssetId = Assets.Erc20.Id,
                QuoteAssetId = Assets.Erc721.Id,
            },
        };

        yield return new object[]
        {
            new CreateMarketplaceRequest
            {
                BaseAssetId = Assets.Erc20.Id,
                QuoteAssetId = Assets.MintableErc20.Id,
            },
        };
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}