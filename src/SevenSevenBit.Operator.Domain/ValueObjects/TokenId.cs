namespace SevenSevenBit.Operator.Domain.ValueObjects;

using Nethereum.Hex.HexConvertors.Extensions;
using SevenSevenBit.Operator.SharedKernel.Extensions;

public record TokenId
{
    public TokenId(string value)
    {
        // TODO After refactoring the db schema for specific vaults per assetType, we should validate against null values
        if (value is not null && !value.IsValidHexString())
        {
            throw new ArgumentException("TokenId must be a valid hex string.");
        }

        Value = value?.ToDenormalized();
    }

    public string Value { get; }

    public static implicit operator string(TokenId tokenId) => tokenId?.Value.EnsureHexPrefix();

    public static implicit operator TokenId(string tokenId) => new(tokenId);

    public static bool operator ==(TokenId tokenId, string other)
    {
        return other == tokenId;
    }

    public static bool operator !=(TokenId tokenId, string other)
    {
        return other != tokenId;
    }

    public override string ToString()
    {
        return this;
    }
}