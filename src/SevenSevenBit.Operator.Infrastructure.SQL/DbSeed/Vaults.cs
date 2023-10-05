namespace SevenSevenBit.Operator.Infrastructure.SQL.DbSeed;

using System.Numerics;
using SevenSevenBit.Operator.Domain.Entities;
using SevenSevenBit.Operator.Domain.Enums;

public static class Vaults
{
    // Name should be {VaultOwner}{AssetVarName}{DAMode}
    public static readonly Vault Badjoras123Erc20Usdc10000Validium = new()
    {
        Id = new Guid("6554d7c3-364e-4536-9cd7-bb0ce8f7c60c"),
        User = Users.Badjoras123,
        Asset = Assets.Erc20Usdc10000,
        DataAvailabilityMode = DataAvailabilityModes.Validium,
        VaultChainId = BigInteger.Zero,
        QuantizedAvailableBalance = new BigInteger(31415926535),
        QuantizedAccountingBalance = new BigInteger(31415926535),
    };

    public static readonly Vault Badjoras1234Erc20Usdc10000Validium = new()
    {
        Id = new Guid("82e36762-d5d9-4f14-b83c-8cb0a7056254"),
        User = Users.Badjoras1234,
        Asset = Assets.Erc20Usdc10000,
        DataAvailabilityMode = DataAvailabilityModes.Validium,
        VaultChainId = BigInteger.One,
        QuantizedAvailableBalance = BigInteger.Zero,
        QuantizedAccountingBalance = new BigInteger(1000),
    };

    public static readonly Vault Badjoras123Erc721Bayc1Validium = new()
    {
        Id = Guid.NewGuid(),
        User = Users.Badjoras123,
        Asset = Assets.Erc721Bayc1,
        TokenId = "123abcdef",
        DataAvailabilityMode = DataAvailabilityModes.Validium,
        VaultChainId = new BigInteger(2),
        QuantizedAvailableBalance = new BigInteger(1),
        QuantizedAccountingBalance = new BigInteger(1),
    };

    public static readonly Vault Badjoras1234Erc721Bayc1Validium = new()
    {
        Id = Guid.NewGuid(),
        User = Users.Badjoras1234,
        Asset = Assets.Erc721Bayc1,
        TokenId = "abcd123",
        DataAvailabilityMode = DataAvailabilityModes.Validium,
        VaultChainId = new BigInteger(4),
        QuantizedAvailableBalance = new BigInteger(1),
        QuantizedAccountingBalance = new BigInteger(1),
    };

    public static readonly Vault Badjoras123MErc721Bayc1Validium = new()
    {
        Id = Guid.NewGuid(),
        User = Users.Badjoras123,
        Asset = Assets.MErc721Bayc1,
        BaseMintingBlob = Erc721MintingBlobs.MErc721Bayc,
        DataAvailabilityMode = DataAvailabilityModes.Validium,
        VaultChainId = new BigInteger(3),
        QuantizedAvailableBalance = new BigInteger(1),
        QuantizedAccountingBalance = new BigInteger(1),
    };

    public static readonly Vault Badjoras123Erc20Usdc10000ZkRollup = new()
    {
        Id = Guid.NewGuid(),
        User = Users.Badjoras123,
        Asset = Assets.Erc20Usdc10000,
        DataAvailabilityMode = DataAvailabilityModes.ZkRollup,
        VaultChainId = new BigInteger(9223372036854775808),
        QuantizedAvailableBalance = BigInteger.Zero,
        QuantizedAccountingBalance = BigInteger.Zero,
    };

    public static readonly Vault Badjoras1234Erc20Usdc10000ZkRollup = new()
    {
        Id = new Guid("129DEC85-78EE-4C53-8D90-9E014239FAC6"),
        User = Users.Badjoras1234,
        Asset = Assets.Erc20Usdc10000,
        DataAvailabilityMode = DataAvailabilityModes.ZkRollup,
        VaultChainId = new BigInteger(12358585),
        QuantizedAvailableBalance = 1000,
        QuantizedAccountingBalance = new BigInteger(1000),
    };

    public static readonly Vault Badjoras1234Erc20Dai1000ZkRollup = new()
    {
        Id = new Guid("80E3F347-194B-41C6-912D-1DE11C811DA0"),
        User = Users.Badjoras1234,
        Asset = Assets.Erc20Dai1000,
        DataAvailabilityMode = DataAvailabilityModes.ZkRollup,
        VaultChainId = new BigInteger(123585),
        QuantizedAvailableBalance = 1000,
        QuantizedAccountingBalance = new BigInteger(1000),
    };

    public static readonly Vault Badjoras123Erc20Dai1000ZkRollup = new()
    {
        Id = new Guid("9DA0819B-7655-4D9C-BC54-2D859FF2C61B"),
        User = Users.Badjoras123,
        Asset = Assets.Erc20Dai1000,
        DataAvailabilityMode = DataAvailabilityModes.ZkRollup,
        VaultChainId = new BigInteger(92233720),
        QuantizedAvailableBalance = BigInteger.Zero,
        QuantizedAccountingBalance = BigInteger.Zero,
    };

    public static readonly Vault BitMexArthurUnconfirmedEth1ZkRollup = new()
    {
        Id = Guid.NewGuid(),
        User = Users.BitMexArthur,
        Asset = Assets.UnconfirmedEthEth1,
        DataAvailabilityMode = DataAvailabilityModes.ZkRollup,
        VaultChainId = new BigInteger(9223372036854775809),
        QuantizedAvailableBalance = new BigInteger(9999999999),
        QuantizedAccountingBalance = new BigInteger(9999999999),
    };

    public static readonly Vault BitMexArthurErc20Usdc10000Validium = new()
    {
        Id = Guid.NewGuid(),
        User = Users.BitMexArthur,
        Asset = Assets.Erc20Usdc10000,
        DataAvailabilityMode = DataAvailabilityModes.Validium,
        VaultChainId = new BigInteger(21474836),
        QuantizedAvailableBalance = new BigInteger(9999999999),
        QuantizedAccountingBalance = new BigInteger(9999999999),
    };

    public static IEnumerable<Vault> GetVaults()
    {
        yield return Badjoras123Erc20Usdc10000Validium;
        yield return Badjoras1234Erc20Usdc10000Validium;
        yield return Badjoras123Erc721Bayc1Validium;
        yield return Badjoras1234Erc721Bayc1Validium;
        yield return Badjoras123MErc721Bayc1Validium;
        yield return Badjoras123Erc20Usdc10000ZkRollup;
        yield return BitMexArthurUnconfirmedEth1ZkRollup;
        yield return Badjoras1234Erc20Usdc10000ZkRollup;
        yield return Badjoras1234Erc20Dai1000ZkRollup;
        yield return Badjoras123Erc20Dai1000ZkRollup;
        yield return BitMexArthurErc20Usdc10000Validium;
    }
}