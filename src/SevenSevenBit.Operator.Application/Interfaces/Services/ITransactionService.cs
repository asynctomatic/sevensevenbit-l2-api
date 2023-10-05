namespace SevenSevenBit.Operator.Application.Interfaces.Services;

using SevenSevenBit.Operator.Application.DTOs.Entities;
using SevenSevenBit.Operator.Application.DTOs.Pagination;
using SevenSevenBit.Operator.Domain.Entities;
using SevenSevenBit.Operator.Domain.Enums;

public interface ITransactionService
{
    Task<Transaction> GetTransactionByStarkExIdAsync(long starkExId, CancellationToken cancellationToken);

    Task<Transaction> GetTransactionAsync(Guid transactionId, CancellationToken cancellationToken);

    Task<PaginatedResponseDto<TransactionDto>> GetTransactionsAsync(
        Paging paging,
        long? starkExTransactionId,
        FilterOptions? starkExTransactionComparison,
        TransactionStatus? transactionStatus,
        FilterOptions? transactionStatusComparison,
        StarkExOperation? transactionType,
        FilterOptions? transactionTypeComparison,
        string sort = null,
        CancellationToken cancellationToken = default);
}