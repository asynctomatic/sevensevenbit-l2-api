namespace SevenSevenBit.Operator.Application.Blockchain.Events;

using System.Diagnostics.CodeAnalysis;
using System.Numerics;
using Nethereum.ABI.FunctionEncoding.Attributes;

[ExcludeFromCodeCoverage]
[Event("LogDepositCancel")]
public class LogDepositCancelEvent
{
    [Parameter("uint256", "starkKey", 1, false)]
    public BigInteger StarkKey { get; set; }

    [Parameter("uint256", "vaultId", 2, false)]
    public BigInteger VaultId { get; set; }

    [Parameter("uint256", "assetId", 3, false)]
    public BigInteger AssetId { get; set; }
}