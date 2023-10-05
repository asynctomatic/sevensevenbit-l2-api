namespace SevenSevenBit.Operator.Application.Comparers;

using StarkEx.Client.SDK.Models.Spot.TransactionModels;

public class MultiTransactionModelComparer : IEqualityComparer<MultiTransactionModel>
{
    public bool Equals(MultiTransactionModel x, MultiTransactionModel y)
    {
        if (ReferenceEquals(x, y))
        {
            return true;
        }

        if (ReferenceEquals(x, null))
        {
            return false;
        }

        if (ReferenceEquals(y, null))
        {
            return false;
        }

        if (x.GetType() != y.GetType())
        {
            return false;
        }

        var transactionComparer = new TransactionModelComparer();
        return x.Transactions.SequenceEqual(y.Transactions, transactionComparer);
    }

    public int GetHashCode(MultiTransactionModel obj)
    {
        return HashCode.Combine(obj.Transactions, obj.Type);
    }
}