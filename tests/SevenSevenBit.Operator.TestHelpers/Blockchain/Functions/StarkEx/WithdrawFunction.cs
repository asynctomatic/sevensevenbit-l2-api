namespace SevenSevenBit.Operator.TestHelpers.Blockchain.Functions.StarkEx;

using System.Numerics;
using Nethereum.ABI.FunctionEncoding.Attributes;
using Nethereum.Contracts;

[Function("withdraw")]
public class WithdrawFunction : FunctionMessage
{
    [Parameter("uint256", "ownerKey")]
    public BigInteger OwnerKey { get; set; }

    [Parameter("uint256", "assetType", 2)]
    public BigInteger AssetType { get; set; }
}