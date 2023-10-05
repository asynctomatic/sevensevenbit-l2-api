namespace SevenSevenBit.Operator.Domain.Enums;

using System.Runtime.Serialization;

public enum OfferStatus
{
    [EnumMember(Value = "open")]
    Open,
    [EnumMember(Value = "cancelled")]
    Cancelled,
    [EnumMember(Value = "closed")]
    Closed,
    [EnumMember(Value = "expired")]
    Expired,
}