namespace SevenSevenBit.Operator.Infrastructure.SQL.ValueConverters;

using System.Numerics;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SevenSevenBit.Operator.Domain.ValueObjects;

public class VaultChainIdConverter : ValueConverter<VaultChainId, BigInteger>
{
    public VaultChainIdConverter()
        : base(
            v => v,
            v => new VaultChainId(v))
    {
    }
}