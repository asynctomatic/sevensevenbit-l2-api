namespace SevenSevenBit.Operator.Domain.Enums;

using System.Runtime.Serialization;

public enum OrderStatus
{
    // Indicating that the corresponding order was not filled. It is only sent
    // in response to market orders.
    [EnumMember(Value = "unfilled")]
    Unfilled,

    // Indicating that the corresponding order was placed on the order book. It
    // is only send in response to limit orders.
    [EnumMember(Value = "placed")]
    Placed,

    // Indicating that the corresponding order was removed from the order book.
    // It is only sent in response to cancel orders.
    [EnumMember(Value = "cancelled")]
    Cancelled,

    // Indicating that the corresponding order was only partially filled. It is
    // sent in response to market or limit orders.
    [EnumMember(Value = "partially_filled")]
    PartiallyFilled,

    // Indicating that the corresponding order was filled completely. It is
    // sent in response to market or limit orders.
    [EnumMember(Value = "filled")]
    Filled,

    // Indicating that the corresponding order was expired. It is sent in
    // response to limit orders.
    [EnumMember(Value = "expired")]
    Expired,
}