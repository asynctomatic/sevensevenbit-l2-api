namespace SevenSevenBit.Operator.Application.DTOs.Internal.Settlement;

using System.Numerics;
using SevenSevenBit.Operator.Domain.Entities;

public record SettlementOrderDataDto
{
    public Vault SellVault { get; init; }

    public Vault BuyVault { get; init; }

    public Vault FeeVault { get; init; }

    public BigInteger SellQuantizedAmount { get; init; }

    public BigInteger BuyQuantizedAmount { get; init; }

    public BigInteger? FeeQuantizedAmount { get; init; }

    public long ExpirationTimestamp { get; init; }

    public string SignatureR { get; init; }

    public string SignatureS { get; init; }

    public int Nonce { get; init; }
}