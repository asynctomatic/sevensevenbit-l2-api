namespace SevenSevenBit.Operator.Infrastructure.SQL.ValueConverters;

using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SevenSevenBit.Operator.Domain.ValueObjects;
using SevenSevenBit.Operator.SharedKernel.Extensions;

public class StarkKeyConverter : ValueConverter<StarkKey, string>
{
    public StarkKeyConverter()
        : base(
            v => v.Value.ToNormalized(),
            v => new StarkKey(v))
    {
    }
}