namespace SevenSevenBit.Operator.Application.Blockchain.Events;

using System.Diagnostics.CodeAnalysis;
using System.Numerics;
using Nethereum.ABI.FunctionEncoding.Attributes;

[ExcludeFromCodeCoverage]
[Event("LogL1LimitOrderRegistered")]
public class LogL1LimitOrderRegisteredEvent
{
    [Parameter("address", "userAddress", 1, false)]
    public string UserAddress { get; set; }

    [Parameter("address", "exchangeAddress", 2, false)]
    public string ExchangeAddress { get; set; }

    [Parameter("uint256", "tokenIdSell", 3, false)]
    public BigInteger TokenIdSell { get; set; }

    [Parameter("uint256", "tokenIdBuy", 4, false)]
    public BigInteger TokenIdBuy { get; set; }

    [Parameter("uint256", "tokenIdFee", 5, false)]
    public BigInteger TokenIdFee { get; set; }

    [Parameter("uint256", "amountSell", 6, false)]
    public BigInteger AmountSell { get; set; }

    [Parameter("uint256", "amountBuy", 7, false)]
    public BigInteger AmountBuy { get; set; }

    [Parameter("uint256", "amountFee", 8, false)]
    public BigInteger AmountFee { get; set; }

    [Parameter("uint256", "vaultIdSell", 9, false)]
    public BigInteger VaultIdSell { get; set; }

    [Parameter("uint256", "vaultIdBuy", 10, false)]
    public BigInteger VaultIdBuy { get; set; }

    [Parameter("uint256", "vaultIdFee", 11, false)]
    public BigInteger VaultIdFee { get; set; }

    [Parameter("uint256", "nonce", 12, false)]
    public BigInteger Nonce { get; set; }

    [Parameter("uint256", "expirationTimestamp", 13, false)]
    public BigInteger ExpirationTimestamp { get; set; }
}