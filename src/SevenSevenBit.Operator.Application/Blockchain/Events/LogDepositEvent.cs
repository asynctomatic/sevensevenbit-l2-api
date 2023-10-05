namespace SevenSevenBit.Operator.Application.Blockchain.Events;

using System.Diagnostics.CodeAnalysis;
using System.Numerics;
using Nethereum.ABI.FunctionEncoding.Attributes;

[ExcludeFromCodeCoverage]
[Event("LogDeposit")]
public class LogDepositEvent
{
    [Parameter("address", "depositorEthKey", 1, false)]
    public string DepositorEthKey { get; set; }

    [Parameter("uint256", "starkKey", 2, false)]
    public BigInteger StarkKey { get; set; }

    [Parameter("uint256", "vaultId", 3, false)]
    public BigInteger VaultId { get; set; }

    [Parameter("uint256", "assetType", 4, false)]
    public BigInteger AssetType { get; set; }

    [Parameter("uint256", "nonQuantizedAmount", 5, false)]
    public BigInteger NonQuantizedAmount { get; set; }

    [Parameter("uint256", "quantizedAmount", 6, false)]
    public BigInteger QuantizedAmount { get; set; }
}