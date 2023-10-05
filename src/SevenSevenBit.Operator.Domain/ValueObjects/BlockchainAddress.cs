namespace SevenSevenBit.Operator.Domain.ValueObjects;

using Nethereum.Hex.HexConvertors.Extensions;
using Nethereum.Util;

public record BlockchainAddress
{
    public BlockchainAddress(string value)
    {
        // TODO After refactoring the db schema for specific vaults per assetType, we should validate against null values
        if (value is not null && !value.EnsureHexPrefix().IsValidEthereumAddressHexFormat())
        {
            throw new ArgumentException("BlockchainAddress must be a valid hex string.");
        }

        Value = value?.ConvertToEthereumChecksumAddress();
    }

    public string Value { get; }

    public static implicit operator string(BlockchainAddress address) => address?.Value?.ConvertToEthereumChecksumAddress();

    public static implicit operator BlockchainAddress(string address) => new(address);

    public static bool operator ==(BlockchainAddress blockchainAddress, string other)
    {
        return other == blockchainAddress;
    }

    public static bool operator !=(BlockchainAddress blockchainAddress, string other)
    {
        return other != blockchainAddress;
    }

    public override string ToString()
    {
        return this;
    }
}