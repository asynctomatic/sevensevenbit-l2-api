namespace SevenSevenBit.Operator.Domain.ValueObjects;

using System.Numerics;
using SevenSevenBit.Operator.SharedKernel.Extensions;

public record Quantum
{
    private readonly BigInteger quantumUpperBound = BigInteger.Parse("340282366920938463463374607431768211456");

    public Quantum(BigInteger value)
    {
        if (value < BigInteger.One || value >= quantumUpperBound)
        {
            throw new ArgumentOutOfRangeException(nameof(value));
        }

        Value = value;
    }

    public BigInteger Value { get; }

    public static implicit operator Org.BouncyCastle.Math.BigInteger(Quantum quantum) => quantum?.Value.ToBouncyCastle();

    public static implicit operator BigInteger(Quantum quantum) => quantum.Value;

    public static implicit operator Quantum(BigInteger quantum) => new(quantum);

    public static implicit operator Quantum(int quantum) => new(value: quantum);

    public static BigInteger operator *(Quantum quantum, int factor)
    {
        return quantum.Value * factor;
    }

    public static BigInteger operator *(Quantum quantum, long factor)
    {
        return quantum.Value * factor;
    }

    public static BigInteger operator -(Quantum quantum, int term)
    {
        return quantum.Value - term;
    }
}