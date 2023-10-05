namespace SevenSevenBit.Operator.TestHelpers.Data;

using SevenSevenBit.Operator.Domain.Entities.Marketplace;

public static class Marketplaces
{
    public static readonly Marketplace Erc20EthMarketplace = new()
    {
        Id = Guid.NewGuid(),
        BaseAssetId = Assets.Erc20.Id,
        QuoteAssetId = Assets.Eth.Id,
    };

    public static readonly Marketplace Erc20Erc721Marketplace = new()
    {
        Id = Guid.NewGuid(),
        BaseAssetId = Assets.Erc20.Id,
        QuoteAssetId = Assets.Erc721.Id,
    };

    public static IEnumerable<Marketplace> GetAll()
    {
        yield return Erc20EthMarketplace;
        yield return Erc20Erc721Marketplace;
    }
}