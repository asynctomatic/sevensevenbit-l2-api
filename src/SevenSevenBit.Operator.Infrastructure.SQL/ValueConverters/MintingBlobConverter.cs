namespace SevenSevenBit.Operator.Infrastructure.SQL.ValueConverters;

using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SevenSevenBit.Operator.Domain.ValueObjects;
using SevenSevenBit.Operator.SharedKernel.Extensions;

public class MintingBlobConverter : ValueConverter<MintingBlob, string>
{
    public MintingBlobConverter()
        : base(
            v => v.Value != null ? v.Value.ToNormalized() : null,
            v => new MintingBlob(v))
    {
    }
}