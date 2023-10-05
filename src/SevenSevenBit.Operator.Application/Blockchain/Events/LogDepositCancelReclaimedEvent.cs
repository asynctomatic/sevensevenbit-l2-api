namespace SevenSevenBit.Operator.Application.Blockchain.Events;

using System.Diagnostics.CodeAnalysis;
using System.Numerics;
using Nethereum.ABI.FunctionEncoding.Attributes;

[ExcludeFromCodeCoverage]
[Event("LogDepositCancelReclaimed")]
public class LogDepositCancelReclaimedEvent
{
    [Parameter("uint256", "starkKey", 1, false)]
    public BigInteger StarkKey { get; set; }

    [Parameter("uint256", "vaultId", 2, false)]
    public BigInteger VaultId { get; set; }

    [Parameter("uint256", "assetType", 3, false)]
    public BigInteger AssetType { get; set; }

    [Parameter("uint256", "nonQuantizedAmount", 4, false)]
    public BigInteger NonQuantizedAmount { get; set; }

    [Parameter("uint256", "quantizedAmount", 5, false)]
    public BigInteger QuantizedAmount { get; set; }
}