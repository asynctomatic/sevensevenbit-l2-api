namespace SevenSevenBit.Operator.Infrastructure.SQL.DbSeed;

using SevenSevenBit.Operator.Domain.Entities.MintingBlobs;

public static class Erc721MintingBlobs
{
    public static readonly Erc721MintingBlob MErc721Bayc = new()
    {
        Id = Guid.NewGuid(),
        Asset = Assets.MErc721Bayc1,
        MintingBlobHex = "0x123",
    };
}