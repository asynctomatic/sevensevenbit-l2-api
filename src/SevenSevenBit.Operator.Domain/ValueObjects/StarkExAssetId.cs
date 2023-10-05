namespace SevenSevenBit.Operator.Domain.ValueObjects;

using System.Numerics;
using Nethereum.Hex.HexConvertors.Extensions;
using Nethereum.Hex.HexTypes;
using SevenSevenBit.Operator.SharedKernel.Extensions;

public record StarkExAssetId
{
    public static readonly StarkExAssetId EmptyAssetId = new("0x0");

    public StarkExAssetId(string value)
    {
        if (value is null)
        {
            throw new ArgumentNullException(nameof(value), "StarkExAssetId cannot be null.");
        }

        if (!value.IsValidHexString())
        {
            throw new ArgumentException("StarkExAssetId must be a valid hex string.");
        }

        Value = value.ToDenormalized();
    }

    public StarkExAssetId(BigInteger value)
    {
        Value = value.ToHexBigInteger().HexValue.ToDenormalized();
    }

    public string Value { get; }

    public static implicit operator string(StarkExAssetId starkExAssetId) => starkExAssetId?.Value.EnsureHexPrefix();

    public static implicit operator StarkExAssetId(string starkExAssetId) => new(starkExAssetId);

    public static bool operator ==(StarkExAssetId starkExAssetId, string other)
    {
        return other == starkExAssetId;
    }

    public static bool operator !=(StarkExAssetId starkExAssetId, string other)
    {
        return other != starkExAssetId;
    }

    public override string ToString()
    {
        return this;
    }
}