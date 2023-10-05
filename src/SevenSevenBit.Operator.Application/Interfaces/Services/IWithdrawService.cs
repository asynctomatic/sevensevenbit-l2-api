namespace SevenSevenBit.Operator.Application.Interfaces.Services;

using System.Numerics;
using SevenSevenBit.Operator.Domain.Entities;

public interface IWithdrawService
{
    Task WithdrawAsync(
        Vault vault,
        BigInteger quantizedAmount,
        CancellationToken cancellationToken);
}