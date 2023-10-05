namespace SevenSevenBit.Operator.TestHelpers.Data;

using System.Numerics;
using SevenSevenBit.Operator.Domain.Entities;
using SevenSevenBit.Operator.Domain.Enums;
using SevenSevenBit.Operator.Domain.ValueObjects;

public static class Vaults
{
    public static readonly Vault AliceEth = new()
    {
        Id = Guid.NewGuid(),
        User = Users.Alice,
        Asset = Assets.Eth,
        DataAvailabilityMode = DataAvailabilityModes.Validium,
        VaultChainId = BigInteger.Zero,
        QuantizedAvailableBalance = BigInteger.Zero,
        QuantizedAccountingBalance = BigInteger.Zero,
    };

    public static readonly Vault AliceErc20 = new()
    {
        Id = Guid.NewGuid(),
        User = Users.Alice,
        Asset = Assets.Erc20,
        DataAvailabilityMode = DataAvailabilityModes.Validium,
        VaultChainId = BigInteger.One,
        QuantizedAvailableBalance = BigInteger.Zero,
        QuantizedAccountingBalance = BigInteger.Zero,
    };

    public static readonly Vault AliceErc721 = new()
    {
        Id = Guid.NewGuid(),
        User = Users.Alice,
        Asset = Assets.Erc721,
        TokenId = new TokenId("0x01"),
        DataAvailabilityMode = DataAvailabilityModes.Validium,
        VaultChainId = new BigInteger(2),
        QuantizedAvailableBalance = BigInteger.Zero,
        QuantizedAccountingBalance = BigInteger.Zero,
    };

    public static IEnumerable<Vault> GetVaults()
    {
        yield return AliceEth;
        yield return AliceErc20;
        yield return AliceErc721;
    }
}