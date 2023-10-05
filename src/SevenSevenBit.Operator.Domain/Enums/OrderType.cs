namespace SevenSevenBit.Operator.Domain.Enums;

using System.Runtime.Serialization;

public enum OrderType
{
    // A limit order, which is either filled immediately, or added to the order book.
    [EnumMember(Value = "limit")]
    Limit,

    // A market order, which is either filled immediately (even partially), or canceled.
    [EnumMember(Value = "market")]
    Market,
}