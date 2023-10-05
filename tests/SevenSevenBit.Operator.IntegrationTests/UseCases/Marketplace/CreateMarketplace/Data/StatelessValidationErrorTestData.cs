namespace SevenSevenBit.Operator.IntegrationTests.UseCases.Marketplace.CreateMarketplace.Data;

using System.Collections;
using SevenSevenBit.Operator.Domain.Enums;
using SevenSevenBit.Operator.TestHelpers.Data;
using SevenSevenBit.Operator.Web.Models.Api.Marketplace.Request;

public class StatelessValidationErrorTestData : IEnumerable<object[]>
{
    public IEnumerator<object[]> GetEnumerator()
    {
        // ERC-721 base asset without token ID.
        yield return new object[]
        {
            new CreateMarketplaceRequest
            {
                BaseAssetId = Assets.Erc721.Id,
                QuoteAssetId = Assets.Eth.Id,
            },
            ((int)ErrorCodes.TokenIdRequired).ToString(),
        };

        // ERC-1155 base asset without token ID.
        yield return new object[]
        {
            new CreateMarketplaceRequest
            {
                BaseAssetId = Assets.Erc1155.Id,
                QuoteAssetId = Assets.Eth.Id,
            },
            ((int)ErrorCodes.TokenIdRequired).ToString(),
        };

        // Mintable ERC-721 base asset without token ID.
        yield return new object[]
        {
            new CreateMarketplaceRequest
            {
                BaseAssetId = Assets.MintableErc721.Id,
                QuoteAssetId = Assets.Eth.Id,
            },
            ((int)ErrorCodes.TokenIdRequired).ToString(),
        };

        // Mintable ERC-1155 base asset without token ID.
        yield return new object[]
        {
            new CreateMarketplaceRequest
            {
                BaseAssetId = Assets.MintableErc1155.Id,
                QuoteAssetId = Assets.Eth.Id,
            },
            ((int)ErrorCodes.TokenIdRequired).ToString(),
        };

        // ERC-721 quote asset without token ID.
        yield return new object[]
        {
            new CreateMarketplaceRequest
            {
                BaseAssetId = Assets.Eth.Id,
                QuoteAssetId = Assets.Erc721.Id,
            },
            ((int)ErrorCodes.TokenIdRequired).ToString(),
        };

        // ERC-1155 quote asset without token ID.
        yield return new object[]
        {
            new CreateMarketplaceRequest
            {
                BaseAssetId = Assets.Eth.Id,
                QuoteAssetId = Assets.Erc1155.Id,
            },
            ((int)ErrorCodes.TokenIdRequired).ToString(),
        };

        // Mintable ERC-721 quote asset without token ID.
        yield return new object[]
        {
            new CreateMarketplaceRequest
            {
                BaseAssetId = Assets.Eth.Id,
                QuoteAssetId = Assets.MintableErc721.Id,
            },
            ((int)ErrorCodes.TokenIdRequired).ToString(),
        };

        // Mintable ERC-1155 quote asset without token ID.
        yield return new object[]
        {
            new CreateMarketplaceRequest
            {
                BaseAssetId = Assets.Eth.Id,
                QuoteAssetId = Assets.MintableErc1155.Id,
            },
            ((int)ErrorCodes.TokenIdRequired).ToString(),
        };

        // Same base and quote asset.
        yield return new object[]
        {
            new CreateMarketplaceRequest
            {
                BaseAssetId = Assets.Eth.Id,
                QuoteAssetId = Assets.Eth.Id,
            },
            ((int)ErrorCodes.SameBaseAndQuoteAssets).ToString(),
        };
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}