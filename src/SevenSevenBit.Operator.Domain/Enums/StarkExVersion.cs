namespace SevenSevenBit.Operator.Domain.Enums;

using System.Runtime.Serialization;

public enum StarkExVersion
{
    [EnumMember(Value = "v2")]
    Version2 = 0,
    [EnumMember(Value = "v4.5")]
    Version4dot5 = 1,
    [EnumMember(Value = "v5")]
    Version5 = 2,
}