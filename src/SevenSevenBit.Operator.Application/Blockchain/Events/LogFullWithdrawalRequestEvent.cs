namespace SevenSevenBit.Operator.Application.Blockchain.Events;

using System.Diagnostics.CodeAnalysis;
using System.Numerics;
using Nethereum.ABI.FunctionEncoding.Attributes;

[ExcludeFromCodeCoverage]
[Event("LogFullWithdrawalRequest")]
public class LogFullWithdrawalRequestEvent
{
    [Parameter("uint256", "ownerKey", 1, false)]
    public BigInteger StarkKey { get; set; }

    [Parameter("uint256", "vaultId", 2, false)]
    public BigInteger VaultId { get; set; }
}