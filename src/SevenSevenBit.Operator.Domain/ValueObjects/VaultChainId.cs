namespace SevenSevenBit.Operator.Domain.ValueObjects;

using System.Numerics;
using SevenSevenBit.Operator.Domain.Constants;
using SevenSevenBit.Operator.SharedKernel.Extensions;

public record VaultChainId
{
    public VaultChainId(BigInteger value)
    {
        if (value < StarkExConstants.ValidiumVaultsLowerBound || value > StarkExConstants.ValidiumVaultsUpperBound)
        {
            throw new ArgumentOutOfRangeException(nameof(value));
        }

        Value = value;
    }

    public BigInteger Value { get; }

    public static implicit operator BigInteger(VaultChainId vaultChainId) => vaultChainId.Value;

    public static implicit operator VaultChainId(BigInteger vaultChainId) => new(vaultChainId);

    public static implicit operator Org.BouncyCastle.Math.BigInteger(VaultChainId vaultChainId) => vaultChainId.Value.ToBouncyCastle();

    public static bool operator ==(VaultChainId vaultChainId, BigInteger other)
    {
        return other.Equals(vaultChainId);
    }

    public static bool operator !=(VaultChainId vaultChainId, BigInteger other)
    {
        return !other.Equals(vaultChainId);
    }

    public override string ToString()
    {
        return Value.ToString();
    }
}