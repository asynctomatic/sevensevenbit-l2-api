namespace SevenSevenBit.Operator.Domain.ValueObjects;

using Nethereum.Hex.HexConvertors.Extensions;
using SevenSevenBit.Operator.SharedKernel.Extensions;

public record MintingBlob
{
    public MintingBlob(string value)
    {
        // TODO After refactoring the db schema for specific vaults per assetType, we should validate against null values
        if (value is not null && !value.IsValidHexString())
        {
            throw new ArgumentException("MintingBlob must be a valid hex string.");
        }

        Value = value?.ToDenormalized();
    }

    public string Value { get; }

    public static implicit operator string(MintingBlob mintingBlob) => mintingBlob?.Value.EnsureHexPrefix();

    public static implicit operator MintingBlob(string mintingBlob) => new(mintingBlob);

    public static bool operator ==(MintingBlob mintingBlob, string other)
    {
        return other == mintingBlob;
    }

    public static bool operator !=(MintingBlob mintingBlob, string other)
    {
        return other != mintingBlob;
    }

    public override string ToString()
    {
        return this;
    }
}