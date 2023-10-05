namespace SevenSevenBit.Operator.Application.Blockchain.Events;

using System.Diagnostics.CodeAnalysis;
using System.Numerics;
using Nethereum.ABI.FunctionEncoding.Attributes;

[ExcludeFromCodeCoverage]
[Event("LogDepositWithTokenId")]
public class LogDepositWithTokenIdEvent
{
    [Parameter("address", "depositorEthKey", 1, false)]
    public string DepositorEthKey { get; set; }

    [Parameter("uint256", "starkKey", 2, false)]
    public BigInteger StarkKey { get; set; }

    [Parameter("uint256", "vaultId", 3, false)]
    public BigInteger VaultId { get; set; }

    [Parameter("uint256", "assetType", 4, false)]
    public BigInteger AssetType { get; set; }

    [Parameter("uint256", "tokenId", 5, false)]
    public BigInteger TokenId { get; set; }

    [Parameter("uint256", "assetId", 6, false)]
    public BigInteger AssetId { get; set; }

    [Parameter("uint256", "nonQuantizedAmount", 7, false)]
    public BigInteger NonQuantizedAmount { get; set; }

    [Parameter("uint256", "quantizedAmount", 8, false)]
    public BigInteger QuantizedAmount { get; set; }
}