namespace SevenSevenBit.Operator.Application.Comparers;

using StarkEx.Client.SDK.Models.Spot.TransactionModels;

public class FalseFullWithdrawalModelComparer : IEqualityComparer<FalseFullWithdrawalModel>
{
    public bool Equals(FalseFullWithdrawalModel x, FalseFullWithdrawalModel y)
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

        return string.Equals(x.RequesterStarkKey, y.RequesterStarkKey) &&
               x.VaultId.Equals(y.VaultId);
    }

    public int GetHashCode(FalseFullWithdrawalModel obj)
    {
        return HashCode.Combine(obj.RequesterStarkKey, obj.Type, obj.VaultId);
    }
}