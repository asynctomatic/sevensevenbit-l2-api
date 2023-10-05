namespace SevenSevenBit.Operator.Infrastructure.SQL.ValueConverters;

using System.Numerics;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SevenSevenBit.Operator.Domain.ValueObjects;

public class QuantumConverter : ValueConverter<Quantum, BigInteger>
{
    public QuantumConverter()
        : base(
            v => v,
            v => new Quantum(v))
    {
    }
}