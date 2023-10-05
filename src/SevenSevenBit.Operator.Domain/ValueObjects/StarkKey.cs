namespace SevenSevenBit.Operator.Domain.ValueObjects;

using System.Numerics;
using Nethereum.Hex.HexConvertors.Extensions;
using Nethereum.Hex.HexTypes;
using SevenSevenBit.Operator.SharedKernel.Extensions;

public record StarkKey
{
    public static readonly StarkKey EmptyKey = new("0x0");

    public StarkKey(string value)
    {
        if (value is null)
        {
            throw new ArgumentNullException(nameof(value), "StarkKey cannot be null.");
        }

        if (!value.IsValidHexString())
        {
            throw new ArgumentException("StarkKey must be a valid hex string.");
        }

        Value = value.ToDenormalized();
    }

    public StarkKey(BigInteger value)
    {
        Value = value.ToHexBigInteger().HexValue.ToDenormalized();
    }

    public string Value { get; }

    public static implicit operator string(StarkKey starkKey) => starkKey.Value.EnsureHexPrefix();

    public static implicit operator StarkKey(string starkKey) => new(starkKey);

    public static bool operator ==(StarkKey starkKey, string other)
    {
        return other == starkKey;
    }

    public static bool operator !=(StarkKey starkKey, string other)
    {
        return other != starkKey;
    }

    public override string ToString()
    {
        return this;
    }
}