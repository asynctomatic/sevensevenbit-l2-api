namespace SevenSevenBit.Operator.Application.DTOs.Internal.Settlement;

public record SettlementDataDto
{
    public SettlementOrderDataDto OrderA { get; init; }

    public SettlementOrderDataDto OrderB { get; init; }

    public SettlementInformationDto SettlementInfo { get; init; }
}