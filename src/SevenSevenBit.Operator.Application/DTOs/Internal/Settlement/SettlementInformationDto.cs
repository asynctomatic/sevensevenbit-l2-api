namespace SevenSevenBit.Operator.Application.DTOs.Internal.Settlement;

using System.Numerics;

public record SettlementInformationDto
{
    public BigInteger OrderASold { get; init; }

    public FeeInfoSettlementDto OrderAInfo { get; init; }

    public BigInteger OrderBSold { get; init; }

    public FeeInfoSettlementDto OrderBInfo { get; init; }
}