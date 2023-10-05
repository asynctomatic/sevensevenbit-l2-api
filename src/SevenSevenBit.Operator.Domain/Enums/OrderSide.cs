namespace SevenSevenBit.Operator.Domain.Enums;

using System.Runtime.Serialization;

public enum OrderSide
{
    [EnumMember(Value = "bid")]
    Bid,

    [EnumMember(Value = "ask")]
    Ask,
}