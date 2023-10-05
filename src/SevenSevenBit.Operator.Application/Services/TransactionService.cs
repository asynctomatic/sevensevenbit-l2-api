namespace SevenSevenBit.Operator.Application.Services;

using SevenSevenBit.Operator.Application.Common.Interfaces;
using SevenSevenBit.Operator.Application.DTOs.Entities;
using SevenSevenBit.Operator.Application.DTOs.Pagination;
using SevenSevenBit.Operator.Application.Helpers;
using SevenSevenBit.Operator.Application.Interfaces.Services;
using SevenSevenBit.Operator.Domain.Entities;
using SevenSevenBit.Operator.Domain.Enums;

public class TransactionService : ITransactionService
{
    private readonly IUnitOfWork unitOfWork;

    public TransactionService(IUnitOfWork unitOfWork)
    {
        this.unitOfWork = unitOfWork;
    }

    public async Task<Transaction> GetTransactionByStarkExIdAsync(
        long starkExId,
        CancellationToken cancellationToken)
    {
        return await unitOfWork.Repository<Transaction>().GetSingleAsync(
            filter: x => x.StarkExTransactionId.Equals(starkExId),
            cancellationToken: cancellationToken);
    }

    public async Task<Transaction> GetTransactionAsync(Guid transactionId, CancellationToken cancellationToken)
    {
        return await unitOfWork.Repository<Transaction>().GetByIdAsync(transactionId, cancellationToken);
    }

    public async Task<PaginatedResponseDto<TransactionDto>> GetTransactionsAsync(
        Paging paging,
        long? starkExTransactionId,
        FilterOptions? starkExTransactionComparison,
        TransactionStatus? transactionStatus,
        FilterOptions? transactionStatusComparison,
        StarkExOperation? transactionType,
        FilterOptions? transactionTypeComparison,
        string sort = null,
        CancellationToken cancellationToken = default)
    {
        var filter = QueryBuilder.GetFilter(
            (nameof(Transaction.StarkExTransactionId), typeof(long), starkExTransactionId, starkExTransactionComparison),
            (nameof(Transaction.Status), typeof(string), transactionStatus, transactionStatusComparison),
            (nameof(Transaction.Operation), typeof(string), transactionType, transactionTypeComparison));

        sort ??= $"{nameof(Transaction.CreationDate)} desc";

        return await unitOfWork.Repository<Transaction>().GetProjectedPaginatedAsync(
            paging,
            transaction => new TransactionDto(transaction),
            filter: filter,
            orderBy: sort,
            cancellationToken: cancellationToken);
    }
}