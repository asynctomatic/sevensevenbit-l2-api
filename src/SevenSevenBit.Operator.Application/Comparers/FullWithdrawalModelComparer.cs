namespace SevenSevenBit.Operator.Application.Comparers;

using StarkEx.Client.SDK.Models.Spot.TransactionModels;

public class FullWithdrawalModelComparer : IEqualityComparer<FullWithdrawalModel>
{
    public bool Equals(FullWithdrawalModel x, FullWithdrawalModel y)
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

        return string.Equals(x.StarkKey, y.StarkKey) &&
               x.VaultId.Equals(y.VaultId);
    }

    public int GetHashCode(FullWithdrawalModel obj)
    {
        return HashCode.Combine(obj.StarkKey, obj.Type, obj.VaultId);
    }
}