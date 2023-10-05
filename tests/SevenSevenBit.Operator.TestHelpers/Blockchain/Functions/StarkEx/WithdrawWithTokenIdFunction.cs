namespace SevenSevenBit.Operator.TestHelpers.Blockchain.Functions.StarkEx;

using System.Numerics;
using Nethereum.ABI.FunctionEncoding.Attributes;
using Nethereum.Contracts;

[Function("withdrawWithTokenId")]
public class WithdrawWithTokenIdFunction : FunctionMessage
{
    [Parameter("uint256", "ownerKey")]
    public BigInteger OwnerKey { get; set; }

    [Parameter("uint256", "assetType", 2)]
    public BigInteger AssetType { get; set; }

    [Parameter("uint256", "tokenId", 3)]
    public BigInteger TokenId { get; set; }
}