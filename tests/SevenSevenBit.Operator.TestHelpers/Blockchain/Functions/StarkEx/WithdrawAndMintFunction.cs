namespace SevenSevenBit.Operator.TestHelpers.Blockchain.Functions.StarkEx;

using System.Numerics;
using Nethereum.ABI.FunctionEncoding.Attributes;
using Nethereum.Contracts;

[Function("withdrawAndMint")]
public class WithdrawAndMintFunction : FunctionMessage
{
    [Parameter("uint256", "ownerKey")]
    public BigInteger OwnerKey { get; set; }

    [Parameter("uint256", "assetType", 2)]
    public BigInteger AssetType { get; set; }

    [Parameter("bytes", "mintingBlob", 3)]
    public byte[] MintingBlob { get; set; }
}