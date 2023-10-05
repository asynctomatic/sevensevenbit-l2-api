namespace SevenSevenBit.Operator.Application.UseCases.Deposit.GetSignableDeposit.Functions;

using System.Numerics;
using Nethereum.ABI.FunctionEncoding.Attributes;
using Nethereum.Contracts;

[Function("depositEth")]
public class DepositEthFunction : FunctionMessage
{
    [Parameter("uint256", "starkKey")]
    public BigInteger StarkKey { get; set; }

    [Parameter("uint256", "assetType", 2)]
    public BigInteger AssetType { get; set; }

    [Parameter("uint256", "vaultId", 3)]
    public BigInteger VaultId { get; set; }
}