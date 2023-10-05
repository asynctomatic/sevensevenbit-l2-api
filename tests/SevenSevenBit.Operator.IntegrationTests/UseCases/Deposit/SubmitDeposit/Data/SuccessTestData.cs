namespace SevenSevenBit.Operator.IntegrationTests.UseCases.Deposit.SubmitDeposit.Data;

using System.Collections;
using System.Globalization;
using System.Numerics;
using SevenSevenBit.Operator.TestHelpers.Blockchain.Functions.StarkEx;
using SevenSevenBit.Operator.TestHelpers.Data;

public class SuccessTestData : IEnumerable<object[]>
{
    public IEnumerator<object[]> GetEnumerator()
    {
        yield return new object[]
        {
            new DepositEthFunction
            {
                StarkKey = BigInteger.Parse(Users.Alice.StarkKey.Value[2..], NumberStyles.AllowHexSpecifier),
                AssetType = BigInteger.Parse(Assets.Eth.StarkExType.Value[2..], NumberStyles.AllowHexSpecifier),
                VaultId = Vaults.AliceEth.VaultChainId.Value,
                AmountToSend = BigInteger.Parse("10000000000000000"),
            },
        };
        yield return new object[]
        {
            new DepositErc20Function
            {
                StarkKey = BigInteger.Parse(Users.Alice.StarkKey.Value[2..], NumberStyles.HexNumber),
                AssetType = BigInteger.Parse(Assets.Erc20.StarkExType.Value[2..], NumberStyles.AllowHexSpecifier),
                VaultId = Vaults.AliceErc20.VaultChainId.Value,
                QuantizedAmount = BigInteger.Parse("1000000000000000000"),
            },
        };
        yield return new object[]
        {
            new DepositNftFunction
            {
                StarkKey = BigInteger.Parse(Users.Alice.StarkKey.Value[2..], NumberStyles.AllowHexSpecifier),
                AssetType = BigInteger.Parse(Assets.Erc721.StarkExType.Value[2..], NumberStyles.AllowHexSpecifier),
                VaultId = Vaults.AliceErc721.VaultChainId.Value,
                TokenId = BigInteger.One,
            },
        };

        // TODO: Implement the DepositErc1155Function after contract update to StarkEx V5.
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}