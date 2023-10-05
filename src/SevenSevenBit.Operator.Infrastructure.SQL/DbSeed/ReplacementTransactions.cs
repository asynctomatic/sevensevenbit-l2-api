namespace SevenSevenBit.Operator.Infrastructure.SQL.DbSeed;

using SevenSevenBit.Operator.Domain.Entities;
using StarkEx.Client.SDK.Enums.Spot;
using StarkEx.Client.SDK.Models.Spot.TransactionModels;

public static class ReplacementTransactions
{
    // Name should be Replacement{Operation}{User}{Asset}
    public static readonly ReplacementTransaction ReplacedDepositBadjoras123Erc20Usdc10000 = new()
    {
        Transaction = Transactions.DepositBadjoras123Erc20Usdc10000,
        ReplacementCounter = 1,
        RawReplacementTransactions = Enumerable.Empty<TransactionModel>(),
        ErrorMessage = "Insufficient funds",
        ErrorCode = SpotApiCodes.InsufficientOnChainBalance,
    };

    public static readonly ReplacementTransaction ReplacedDepositBadjoras123Erc721Bayc1AtMaxReplacements = new()
    {
        Transaction = Transactions.DepositBadjoras123Erc721Bayc1,
        ReplacementCounter = 3, // TODO: StarkExInstances.BaseStarkExInstance.MaxNrOfTransactionReplacements,
        RawReplacementTransactions = Enumerable.Empty<TransactionModel>(),
        ErrorMessage = "Insufficient funds",
        ErrorCode = SpotApiCodes.InsufficientOnChainBalance,
        ReplacementVaultUpdates = new List<ReplacementVaultUpdate>(),
    };

    public static IEnumerable<ReplacementTransaction> GetReplacementTransactions()
    {
        yield return ReplacedDepositBadjoras123Erc20Usdc10000;
        yield return ReplacedDepositBadjoras123Erc721Bayc1AtMaxReplacements;
    }
}