namespace SevenSevenBit.Operator.Application.Interfaces.Services;

using System.Numerics;
using SevenSevenBit.Operator.Application.DTOs.Internal;
using SevenSevenBit.Operator.Domain.Entities;
using SevenSevenBit.Operator.Domain.Enums;

public interface IFeeService
{
    Task<bool> IsFeeConfiguredAsync(
        FeeAction action,
        CancellationToken cancellationToken);

    Task<FeeConfig> ConfigureFeeAsync(
        FeeAction action,
        int amount,
        CancellationToken cancellationToken);

    Task<FeeConfig> GetFeeConfigAsync(
        Guid feeId,
        CancellationToken cancellationToken);

    Task<Fee> GetFeeAsync(
        FeeAction action,
        Asset asset,
        BigInteger quantizedAmount,
        CancellationToken cancellationToken);
}