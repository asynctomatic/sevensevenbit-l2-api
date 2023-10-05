namespace SevenSevenBit.Operator.Infrastructure.SQL.ValueConverters;

using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SevenSevenBit.Operator.Domain.ValueObjects;
using SevenSevenBit.Operator.SharedKernel.Extensions;

public class StarkExAssetTypeConverter : ValueConverter<StarkExAssetType, string>
{
    public StarkExAssetTypeConverter()
        : base(
            v => v.Value != null ? v.Value.ToNormalized() : null,
            v => new StarkExAssetType(v))
    {
    }
}