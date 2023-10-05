namespace SevenSevenBit.Operator.Application.Interfaces.Services;

using SevenSevenBit.Operator.Domain.Entities;
using StarkEx.Client.SDK.Enums.Spot;
using StarkEx.Client.SDK.Models.Spot.TransactionModels;

public interface IStarkExService
{
    Task<IEnumerable<TransactionModel>> GetAlternativeTransactionsAsync(
        Transaction transaction,
        SpotApiCodes reasonCode,
        string errorMsg,
        CancellationToken cancellationToken);
}