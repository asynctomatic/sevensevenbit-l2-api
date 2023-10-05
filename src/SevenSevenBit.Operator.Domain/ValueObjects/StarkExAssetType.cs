namespace SevenSevenBit.Operator.Domain.ValueObjects;

using System.Numerics;
using Nethereum.Hex.HexConvertors.Extensions;
using Nethereum.Hex.HexTypes;
using SevenSevenBit.Operator.SharedKernel.Extensions;

public record StarkExAssetType
{
    public StarkExAssetType(string value)
    {
        if (value is null)
        {
            throw new ArgumentNullException(nameof(value), "StarkExAssetType cannot be null.");
        }

        if (!value.IsValidHexString())
        {
            throw new ArgumentException("StarkExAssetType must be a valid hex string.");
        }

        Value = value.ToDenormalized();
    }

    public StarkExAssetType(BigInteger value)
    {
        Value = value.ToHexBigInteger().HexValue.ToDenormalized();
    }

    public string Value { get; }

    public static implicit operator string(StarkExAssetType starkExAssetType) => starkExAssetType.Value.EnsureHexPrefix();

    public static implicit operator StarkExAssetType(string starkExAssetId) => new(starkExAssetId);

    public static bool operator ==(StarkExAssetType starkExAssetType, string other)
    {
        return other == starkExAssetType;
    }

    public static bool operator !=(StarkExAssetType starkExAssetType, string other)
    {
        return other != starkExAssetType;
    }

    public override string ToString()
    {
        return this;
    }
}