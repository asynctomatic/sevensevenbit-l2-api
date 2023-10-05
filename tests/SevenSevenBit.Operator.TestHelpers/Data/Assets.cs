namespace SevenSevenBit.Operator.TestHelpers.Data;

using System.Numerics;
using SevenSevenBit.Operator.Domain.Entities;
using SevenSevenBit.Operator.Domain.ValueObjects;
using SevenSevenBit.Operator.SharedKernel.Extensions;
using StarkEx.Crypto.SDK.Encoding;
using StarkEx.Crypto.SDK.Enums;

public static class Assets
{
    public static readonly Asset Eth = new()
    {
        Id = Guid.NewGuid(),
        StarkExType = AssetEncoder
            .GetAssetType(AssetType.Eth, quantum: new BigInteger(10000).ToBouncyCastle()),
        Type = AssetType.Eth,
        Name = "Ether",
        Symbol = "Eth",
        Quantum = new BigInteger(10000),
        Enabled = true,
    };

    public static readonly Asset Erc20 = new()
    {
        Id = Guid.NewGuid(),
        Address = new BlockchainAddress("0x9465D06d7F219092b10431feF0FeA2197305826C"),
        StarkExType = AssetEncoder
            .GetAssetType(
                AssetType.Erc20,
                quantum: new BigInteger(1000000000).ToBouncyCastle(),
                address: "0x9465D06d7F219092b10431feF0FeA2197305826C"),
        Type = AssetType.Erc20,
        Name = "Default ERC20",
        Symbol = "DERC20",
        Quantum = new BigInteger(1000000000),
        Enabled = true,
    };

    public static readonly Asset Erc721 = new()
    {
        Id = Guid.NewGuid(),
        Address = new BlockchainAddress("0x837f1f14D0808611493f5FB80e846CA1C3Ba5FB6"),
        StarkExType = AssetEncoder
            .GetAssetType(
                AssetType.Erc721,
                quantum: BigInteger.One.ToBouncyCastle(),
                address: "0x837f1f14D0808611493f5FB80e846CA1C3Ba5FB6"),
        Type = AssetType.Erc721,
        Name = "Default ERC721",
        Symbol = "DERC721",
        Quantum = new Quantum(BigInteger.One),
        Enabled = true,
    };

    public static readonly Asset Erc1155 = new()
    {
        Id = Guid.NewGuid(),
        Address = new BlockchainAddress("0xfF31BF92a5071a9AAc14CFC2fa0c9b92Ce30cfb6"),
        StarkExType = AssetEncoder
            .GetAssetType(
                AssetType.Erc1155,
                quantum: BigInteger.Parse("10").ToBouncyCastle(),
                address: "0xfF31BF92a5071a9AAc14CFC2fa0c9b92Ce30cfb6"),
        Type = AssetType.Erc1155,
        Name = "Default ERC1155",
        Symbol = "DERC1155",
        Quantum = new Quantum(BigInteger.Parse("10")),
        Enabled = true,
    };

    public static readonly Asset MiladyErc721 = new()
    {
        Id = Guid.NewGuid(),
        Address = new BlockchainAddress("0x5Af0D9827E0c53E4799BB226655A1de152A425a5"),
        StarkExType = AssetEncoder.GetAssetType(
            AssetType.Erc721,
            quantum: BigInteger.One.ToBouncyCastle(),
            address: "0x5Af0D9827E0c53E4799BB226655A1de152A425a5"),
        Type = AssetType.Erc721,
        Name = "Milady Maker",
        Symbol = "MIL",
        Quantum = new Quantum(BigInteger.One),
        Enabled = true,
    };

    public static readonly Asset MintableErc20 = new()
    {
        Id = Guid.NewGuid(),
        Address = new BlockchainAddress("0x16641Df912A17442A6b03F39FdeBFABe161eE5af"),
        StarkExType = AssetEncoder.GetAssetType(
            AssetType.MintableErc20,
            quantum: new BigInteger(10000).ToBouncyCastle(),
            address: "0x16641Df912A17442A6b03F39FdeBFABe161eE5af"),
        Type = AssetType.MintableErc20,
        Name = "Default Mintable ERC20",
        Symbol = "DMERC20",
        Quantum = new Quantum(10000),
        Enabled = true,
    };

    public static readonly Asset MintableErc721 = new()
    {
        Id = Guid.NewGuid(),
        Address = new BlockchainAddress("0x77113D54A897c1FD370Dee6a07998eB795A1e084"),
        StarkExType = AssetEncoder.GetAssetType(
            AssetType.MintableErc721,
            quantum: BigInteger.One.ToBouncyCastle(),
            address: "0x77113D54A897c1FD370Dee6a07998eB795A1e084"),
        Type = AssetType.MintableErc721,
        Name = "Default Mintable ERC721",
        Symbol = "DMERC721",
        Quantum = new Quantum(BigInteger.One),
        Enabled = true,
    };

    public static readonly Asset MintableErc1155 = new()
    {
        Id = Guid.NewGuid(),
        Address = new BlockchainAddress("0xf342586d9CE98Cf0aB6BB0217b7361640f9Ab523"),
        StarkExType = AssetEncoder.GetAssetType(
            AssetType.MintableErc1155,
            quantum: BigInteger.One.ToBouncyCastle(),
            address: "0xf342586d9CE98Cf0aB6BB0217b7361640f9Ab523"),
        Type = AssetType.MintableErc1155,
        Name = "Default Mintable ERC1155",
        Symbol = "DMERC1155",
        Quantum = new Quantum(BigInteger.One),
        Enabled = true,
    };

    public static IEnumerable<Asset> GetAssets()
    {
        yield return Eth;
        yield return Erc20;
        yield return Erc721;
        yield return Erc1155;
        yield return MintableErc20;
        yield return MintableErc721;
        yield return MintableErc1155;
        // TODO yield return MiladyErc721;
    }
}