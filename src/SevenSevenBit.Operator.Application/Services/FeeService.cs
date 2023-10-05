namespace SevenSevenBit.Operator.Application.Services;

using System.Numerics;
using SevenSevenBit.Operator.Application.Common.Interfaces;
using SevenSevenBit.Operator.Application.DTOs.Internal;
using SevenSevenBit.Operator.Application.Interfaces.Services;
using SevenSevenBit.Operator.Domain.Constants;
using SevenSevenBit.Operator.Domain.Entities;
using SevenSevenBit.Operator.Domain.Enums;

public class FeeService : IFeeService
{
    private readonly IUnitOfWork unitOfWork;

    public FeeService(IUnitOfWork unitOfWork)
    {
        this.unitOfWork = unitOfWork;
    }

    public async Task<bool> IsFeeConfiguredAsync(
        FeeAction action,
        CancellationToken cancellationToken)
    {
        return await unitOfWork.Repository<FeeConfig>().AnyAsync(
            filter: config => config.Action.Equals(action),
            cancellationToken: cancellationToken);
    }

    public async Task<FeeConfig> ConfigureFeeAsync(
        FeeAction action,
        int amount,
        CancellationToken cancellationToken)
    {
        var newFeeConfig = new FeeConfig
        {
            Action = action,
            Amount = amount,
        };

        await unitOfWork.Repository<FeeConfig>().InsertAsync(newFeeConfig, cancellationToken);
        await unitOfWork.SaveAsync(cancellationToken);

        return newFeeConfig;
    }

    public async Task<FeeConfig> GetFeeConfigAsync(
        Guid feeId,
        CancellationToken cancellationToken)
    {
        return await unitOfWork.Repository<FeeConfig>().GetByIdAsync(feeId, cancellationToken);
    }

    public async Task<Fee> GetFeeAsync(
        FeeAction action,
        Asset asset,
        BigInteger quantizedAmount,
        CancellationToken cancellationToken)
    {
        var feeConfig = await unitOfWork.Repository<FeeConfig>().GetSingleAsync(
            config => config.Action.Equals(action),
            cancellationToken: cancellationToken);

        // If there is no FeeConfig associated, assume 0 fees
        feeConfig ??= new FeeConfig
        {
            Action = action,
            Amount = 0,
        };

        var feeDenominator = new BigInteger(FeeConstants.MaxBasisPoints);
        var fee = new Fee
        {
            Asset = asset,
            Amount = (quantizedAmount * new BigInteger(feeConfig.Amount)) / feeDenominator,
        };

        // TODO disable fee for NFT transfers - To resolve in Fee PR
        return fee;
    }
}