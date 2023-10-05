namespace SevenSevenBit.Operator.Application.Comparers;

using StarkEx.Client.SDK.Models.Spot.TransactionModels;

public class TransactionModelComparer : IEqualityComparer<TransactionModel>
{
    public bool Equals(TransactionModel x, TransactionModel y)
    {
        if (!x!.Type.Equals(y!.Type))
        {
            return false;
        }

        switch (x.Type)
        {
            case "DepositRequest":
            {
                var comparer = new DepositModelComparer();
                return comparer.Equals(x as DepositModel, y as DepositModel);
            }

            case "FalseFullWithdrawalRequest":
            {
                var comparer = new FalseFullWithdrawalModelComparer();
                return comparer.Equals(x as FalseFullWithdrawalModel, y as FalseFullWithdrawalModel);
            }

            case "FullWithdrawalRequest":
            {
                var comparer = new FullWithdrawalModelComparer();
                return comparer.Equals(x as FullWithdrawalModel, y as FullWithdrawalModel);
            }

            case "MintRequest":
            {
                var comparer = new MintModelComparer();
                return comparer.Equals(x as MintModel, y as MintModel);
            }

            case "MultiTransactionRequest":
            {
                var comparer = new MultiTransactionModelComparer();
                return comparer.Equals(x as MultiTransactionModel, y as MultiTransactionModel);
            }

            case "SettlementRequest":
            {
                var comparer = new SettlementModelComparer();
                return comparer.Equals(x as SettlementModel, y as SettlementModel);
            }

            case "TransferRequest":
            {
                var comparer = new TransferModelComparer();
                return comparer.Equals(x as TransferModel, y as TransferModel);
            }

            case "WithdrawalRequest":
            {
                var comparer = new WithdrawalModelComparer();
                return comparer.Equals(x as WithdrawalModel, y as WithdrawalModel);
            }

            default:
                return false;
        }
    }

    public int GetHashCode(TransactionModel obj)
    {
        switch (obj.Type)
        {
            case "DepositRequest":
            {
                var comparer = new DepositModelComparer();

                return obj is not DepositModel depositModel ? 0 : comparer.GetHashCode(depositModel);
            }

            case "FalseFullWithdrawalRequest":
            {
                var comparer = new FalseFullWithdrawalModelComparer();

                return obj is not FalseFullWithdrawalModel falseFullWithdrawalModel ? 0 : comparer.GetHashCode(falseFullWithdrawalModel);
            }

            case "FullWithdrawalRequest":
            {
                var comparer = new FullWithdrawalModelComparer();

                return obj is not FullWithdrawalModel fullWithdrawalModel ? 0 : comparer.GetHashCode(fullWithdrawalModel);
            }

            case "MintRequest":
            {
                var comparer = new MintModelComparer();

                return obj is not MintModel mintModel ? 0 : comparer.GetHashCode(mintModel);
            }

            case "MultiTransactionRequest":
            {
                var comparer = new MultiTransactionModelComparer();

                return obj is not MultiTransactionModel multiTransactionModel ? 0 : comparer.GetHashCode(multiTransactionModel);
            }

            case "SettlementRequest":
            {
                var comparer = new SettlementModelComparer();

                return obj is not SettlementModel settlementModel ? 0 : comparer.GetHashCode(settlementModel);
            }

            case "TransferRequest":
            {
                var comparer = new TransferModelComparer();

                return obj is not TransferModel transferModel ? 0 : comparer.GetHashCode(transferModel);
            }

            case "WithdrawalRequest":
            {
                var comparer = new WithdrawalModelComparer();

                return obj is not WithdrawalModel withdrawalModel ? 0 : comparer.GetHashCode(withdrawalModel);
            }

            default:
                return 0;
        }
    }
}