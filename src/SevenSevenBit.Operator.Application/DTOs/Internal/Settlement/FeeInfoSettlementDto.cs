namespace SevenSevenBit.Operator.Application.DTOs.Internal.Settlement;

using System.Numerics;
using SevenSevenBit.Operator.Domain.Entities;

public record FeeInfoSettlementDto
{
    public Vault FeeDestinationVault { get; init; }

    public BigInteger FeeTaken { get; init; }
}