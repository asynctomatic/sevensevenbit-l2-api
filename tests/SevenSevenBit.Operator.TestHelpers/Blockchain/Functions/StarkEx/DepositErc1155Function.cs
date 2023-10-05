namespace SevenSevenBit.Operator.TestHelpers.Blockchain.Functions.StarkEx;

using System.Numerics;
using Nethereum.ABI.FunctionEncoding.Attributes;
using Nethereum.Contracts;

[Function("depositERC1155")]
public class DepositErc1155Function : FunctionMessage
{
    [Parameter("uint256", "starkKey")]
    public BigInteger StarkKey { get; set; }

    [Parameter("uint256", "assetType", 2)]
    public BigInteger AssetType { get; set; }

    [Parameter("uint256", "tokenId", 3)]
    public BigInteger TokenId { get; set; }

    [Parameter("uint256", "vaultId", 4)]
    public BigInteger VaultId { get; set; }

    [Parameter("uint256", "quantizedAmount", 5)]
    public BigInteger QuantizedAmount { get; set; }
}