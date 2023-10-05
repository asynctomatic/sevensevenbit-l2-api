namespace SevenSevenBit.Operator.Application.UseCases.Deposit.GetSignableDeposit.Functions;

using System.Numerics;
using Nethereum.ABI.FunctionEncoding.Attributes;
using Nethereum.Contracts;

[Function("depositNft")]
public class DepositErc721Function : FunctionMessage
{
    [Parameter("uint256", "starkKey")]
    public BigInteger StarkKey { get; set; }

    [Parameter("uint256", "assetType", 2)]
    public BigInteger AssetType { get; set; }

    [Parameter("uint256", "vaultId", 3)]
    public BigInteger VaultId { get; set; }

    [Parameter("uint256", "tokenId", 4)]
    public BigInteger TokenId { get; set; }
}