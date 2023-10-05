namespace SevenSevenBit.Operator.TestHelpers.Blockchain.Functions;

using System.Numerics;
using Nethereum.ABI.FunctionEncoding.Attributes;
using Nethereum.Contracts;

[Function("safeMint")]
public class SafeMintErc721Function : FunctionMessage
{
    [Parameter("address", "to")]
    public string To { get; set; }

    [Parameter("uint256", "tokenId", 2)]
    public BigInteger TokenId { get; set; }
}