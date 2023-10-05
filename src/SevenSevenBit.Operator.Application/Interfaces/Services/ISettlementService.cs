namespace SevenSevenBit.Operator.Application.Interfaces.Services;

using SevenSevenBit.Operator.Application.DTOs.Internal.Settlement;
using SevenSevenBit.Operator.Domain.Entities;

public interface ISettlementService
{
    Task<IEnumerable<Vault>> SubmitSettlementAsync(
        SettlementDataDto settlementDto,
        CancellationToken cancellationToken);
}