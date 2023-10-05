namespace SevenSevenBit.Operator.TestHelpers.Blockchain.Functions.StarkEx;

using System.Numerics;
using Nethereum.ABI.FunctionEncoding.Attributes;
using Nethereum.Contracts;

[Function("fullWithdrawalRequest")]
public class FullWithdrawalRequestFunction : FunctionMessage
{
    [Parameter("uint256", "ownerKey")]
    public BigInteger StarkKey { get; set; }

    [Parameter("uint256", "vaultId", 2)]
    public BigInteger VaultId { get; set; }
}