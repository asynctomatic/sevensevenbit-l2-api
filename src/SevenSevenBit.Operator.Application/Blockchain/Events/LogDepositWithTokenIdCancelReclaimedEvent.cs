namespace SevenSevenBit.Operator.Application.Blockchain.Events;

using System.Diagnostics.CodeAnalysis;
using System.Numerics;
using Nethereum.ABI.FunctionEncoding.Attributes;

[ExcludeFromCodeCoverage]
[Event("LogDepositWithTokenIdCancelReclaimed")]
public class LogDepositWithTokenIdCancelReclaimedEvent
{
    [Parameter("uint256", "starkKey", 1, false)]
    public BigInteger StarkKey { get; set; }

    [Parameter("uint256", "vaultId", 2, false)]
    public BigInteger VaultId { get; set; }

    [Parameter("uint256", "assetType", 3, false)]
    public BigInteger AssetType { get; set; }

    [Parameter("uint256", "tokenId", 4, false)]
    public BigInteger TokenId { get; set; }

    [Parameter("uint256", "assetId", 5, false)]
    public BigInteger AssetId { get; set; }

    [Parameter("uint256", "nonQuantizedAmount", 6, false)]
    public BigInteger NonQuantizedAmount { get; set; }

    [Parameter("uint256", "quantizedAmount", 7, false)]
    public BigInteger QuantizedAmount { get; set; }
}