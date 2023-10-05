namespace SevenSevenBit.Operator.IntegrationTests.Comparers;

using SevenSevenBit.Operator.Application.DTOs.Entities;

public class TransactionsComparer : IEqualityComparer<TransactionDto>
{
    public bool Equals(TransactionDto x, TransactionDto y)
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

        return x.TransactionId.Equals(y.TransactionId);
    }

    public int GetHashCode(TransactionDto obj)
    {
        return HashCode.Combine(
            obj.TransactionId,
            (int)obj.Operation,
            (int)obj.Status,
            obj.StarkExTransaction);
    }
}