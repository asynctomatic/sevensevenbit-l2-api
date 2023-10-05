namespace SevenSevenBit.Operator.Infrastructure.SQL.DbSeed;

using SevenSevenBit.Operator.Domain.Entities;
using SevenSevenBit.Operator.Domain.Enums;

public static class FeeConfigs
{
    // Name should be {TenantName}{Action}Config
    public static readonly FeeConfig TestingTenantTransferConfig = new()
    {
        Id = Guid.NewGuid(),
        Action = FeeAction.Transfer,
        Amount = 100,
    };

    public static IEnumerable<FeeConfig> GetFeeConfigs()
    {
        yield return TestingTenantTransferConfig;
    }
}