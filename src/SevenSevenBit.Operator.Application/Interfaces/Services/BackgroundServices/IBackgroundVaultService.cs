namespace SevenSevenBit.Operator.Application.Interfaces.Services.BackgroundServices;

using System.Numerics;
using SevenSevenBit.Operator.Domain.Entities;
using SevenSevenBit.Operator.Domain.ValueObjects;

public interface IBackgroundVaultService
{
    Task<Vault> GetVaultAsync(
        BigInteger vaultId,
        CancellationToken cancellationToken);

    Task DepositAsync(
        Vault vault,
        BigInteger quantizedAmount,
        CancellationToken cancellationToken);

    Task FullWithdrawAsync(
        Vault vault,
        CancellationToken cancellationToken);

    Task FalseFullWithdrawAsync(
        BigInteger vaultChainId,
        StarkKey requesterStarkKey,
        CancellationToken cancellationToken);
}