namespace SevenSevenBit.Operator.Infrastructure.SQL.DbSeed;

using System.Numerics;
using SevenSevenBit.Operator.Domain.Entities;
using SevenSevenBit.Operator.SharedKernel.Extensions;
using StarkEx.Crypto.SDK.Encoding;
using StarkEx.Crypto.SDK.Enums;

public static class Assets
{
    // Name should be {AssetType}{Symbol}{Quantum}
    public static readonly Asset Erc20Usdc10000 = new()
    {
        Id = Guid.NewGuid(),
        StarkExType = "352386d5b7c781d47ecd404765307d74edc4d43b0490b8e03c71ac7a7429653",
        Type = AssetType.Erc20,
        Address = "dac17f958d2ee523a2206206994597c13d831ec7",
        Name = "USD Coin",
        Symbol = "USDC",
        Quantum = new BigInteger(10000),
    };

    public static readonly Asset Erc721Bayc1 = new()
    {
        Id = Guid.NewGuid(),
        StarkExType = "29ecf31d0540ee130fd43fe8c647d0f86613a730410e8e9e4b12ecf3148ac2b",
        Type = AssetType.Erc721,
        Address = "bc4ca0eda7647a8ab7c2061c2e118a18a936f13d",
        Name = "Bored Ape Yatch Club",
        Symbol = "BAYC",
        Quantum = BigInteger.One,
    };

    public static readonly Asset MErc20Usdc10000 = new()
    {
        Id = Guid.NewGuid(),
        StarkExType = "2a3deb614248ad8e8a961b004d56aeb983b5e641c98adaa57b6182f09c9e843",
        Type = AssetType.MintableErc20,
        Address = "dac17f958d2ee523a2206206994597c13d831ec7",
        Name = "USD Coin",
        Symbol = "USDC",
        Quantum = new BigInteger(10000),
    };

    public static readonly Asset MErc721Bayc1 = new()
    {
        Id = Guid.NewGuid(),
        StarkExType = "11bb2243721c8e1d28a901862329da9e432d1caab39e7e274234ba48de73ebb",
        Type = AssetType.MintableErc721,
        Address = "b18ed4768f87b0ffab83408014f1caf066b91381",
        Name = "Bored Ape Yatch Club",
        Symbol = "BAYC",
        Quantum = BigInteger.One,
    };

    public static readonly Asset Erc20Dai1000 = new()
    {
        Id = Guid.NewGuid(),
        StarkExType = "3e25d8e4bf0a22fc86219f2c77ef144ed30bf2f54e6ae2c7a2a64282ec25ceb",
        Type = AssetType.Erc20,
        Address = "6b175474e89094c44da98b954eedeac495271d0f",
        Name = "Dai Stablecoin",
        Symbol = "DAI",
        Quantum = new BigInteger(1000),
    };

    public static readonly Asset UnconfirmedMErc721Bayc1 = new()
    {
        Id = Guid.NewGuid(),
        StarkExType = "11bb2243721c8e1d28a901862329da9e432d1caab39e7e274234ba48de74ebb",
        Type = AssetType.MintableErc721,
        Address = "b18ed4768f87b0ffab83408014f1caf066b91382",
        Name = "Bored Ape Yatch Club",
        Symbol = "BAYC",
        Quantum = BigInteger.One,
    };

    public static readonly Asset Erc20Usdc100000 = new()
    {
        Id = Guid.NewGuid(),
        StarkExType = "352386d5b7c781d47ecd404765307d74edc4d43b0490b8e03c71ac7a7429753",
        Type = AssetType.Erc20,
        Address = "dac17f958d2ee523a2206206994597c13d831ec7",
        Name = "USD Coin",
        Symbol = "USDC",
        Quantum = new BigInteger(100000),
    };

    public static readonly Asset UnconfirmedEthEth1 = new()
    {
        Id = Guid.NewGuid(),
        StarkExType = "1142460171646987f20c714eda4b92812b22b811f56f27130937c267e29bd9e",
        Type = AssetType.Eth,
        Name = "Ether",
        Symbol = "Eth",
        Quantum = BigInteger.One,
    };

    public static readonly Asset ConfirmedEthEth10000 = new()
    {
        Id = Guid.NewGuid(),
        StarkExType = AssetEncoder
            .GetAssetType(AssetType.Eth, quantum: new BigInteger(10000).ToBouncyCastle()),
        Type = AssetType.Eth,
        Name = "Ether",
        Symbol = "Eth",
        Quantum = new BigInteger(10000),
    };

    // TODO Can I reuse the mintable assets as normal assets?
    // Three Sigma Test Assets
    public static readonly Asset Erc20TstE2010000 = new()
    {
        Id = Guid.NewGuid(),
        StarkExType = AssetEncoder
            .GetAssetType(AssetType.Erc20, quantum: new BigInteger(10000).ToBouncyCastle(), address: "4ed7c70F96B99c776995fB64377f0d4aB3B0e1C1"),
        Type = AssetType.Erc20,
        Address = "4ed7c70F96B99c776995fB64377f0d4aB3B0e1C1",
        Name = "Three Sigma ERC20 Token",
        Symbol = "TSTE20",
        Quantum = new BigInteger(10000),
    };

    public static readonly Asset Erc721TstE7211 = new()
    {
        Id = Guid.NewGuid(),
        StarkExType = AssetEncoder
            .GetAssetType(AssetType.Erc721, quantum: BigInteger.One.ToBouncyCastle(), address: "322813Fd9A801c5507c9de605d63CEA4f2CE6c44"),
        Type = AssetType.Erc721,
        Address = "322813Fd9A801c5507c9de605d63CEA4f2CE6c44",
        Name = "Three Sigma ERC721 Token",
        Symbol = "TSTE721",
        Quantum = BigInteger.One,
    };

    public static readonly Asset Erc1155TstE11551 = new()
    {
        Id = Guid.NewGuid(),
        StarkExType = AssetEncoder
            .GetAssetType(AssetType.Erc1155, quantum: BigInteger.One.ToBouncyCastle(), address: "a85233C63b9Ee964Add6F2cffe00Fd84eb32338f"),
        Type = AssetType.Erc1155,
        Address = "a85233C63b9Ee964Add6F2cffe00Fd84eb32338f",
        Name = "Three Sigma ERC1155 Token",
        Symbol = "TSTE1155",
        Quantum = BigInteger.One,
    };

    public static readonly Asset MErc20TstMe20100000 = new()
    {
        Id = Guid.NewGuid(),
        StarkExType = AssetEncoder
            .GetAssetType(AssetType.MintableErc20, quantum: new BigInteger(10000).ToBouncyCastle(), address: "58bF480191d4F717a4C4482Ad9036F7d69f783e8"),
        Type = AssetType.MintableErc20,
        Address = "58bF480191d4F717a4C4482Ad9036F7d69f783e8",
        Name = "Three Sigma MERC20 Token",
        Symbol = "TSTME20",
        Quantum = new BigInteger(10000),
    };

    public static readonly Asset MErc721TstMe7211 = new()
    {
        Id = Guid.NewGuid(),
        StarkExType = AssetEncoder
            .GetAssetType(AssetType.MintableErc721, quantum: BigInteger.One.ToBouncyCastle(), address: "7c35fc9ea830bd05f8883dc568ce4e14be390f0f"),
        Type = AssetType.MintableErc721,
        Address = "7c35fc9ea830bd05f8883dc568ce4e14be390f0f",
        Name = "Three Sigma MERC721 Token",
        Symbol = "TSTME721",
        Quantum = BigInteger.One,
    };

    public static IEnumerable<Asset> GetAssets()
    {
        yield return Erc20Usdc10000;
        yield return Erc721Bayc1;
        yield return MErc20Usdc10000;
        yield return MErc721Bayc1;
        yield return Erc20Dai1000;
        yield return UnconfirmedMErc721Bayc1;
        yield return Erc20Usdc100000;
        yield return UnconfirmedEthEth1;
        yield return ConfirmedEthEth10000;

        // Goerli Test Assets
        yield return Erc20TstE2010000;
        yield return Erc721TstE7211;
        yield return Erc1155TstE11551;
        yield return MErc20TstMe20100000;
    }
}