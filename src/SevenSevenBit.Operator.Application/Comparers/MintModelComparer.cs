namespace SevenSevenBit.Operator.Application.Comparers;

using StarkEx.Client.SDK.Models.Spot.TransactionModels;

public class MintModelComparer : IEqualityComparer<MintModel>
{
    public bool Equals(MintModel x, MintModel y)
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

        return x.Amount.Equals(y.Amount) &&
               string.Equals(x.StarkKey, y.StarkKey) &&
               string.Equals(x.TokenId, y.TokenId) &&
               x.VaultId.Equals(y.VaultId);
    }

    public int GetHashCode(MintModel obj)
    {
        return HashCode.Combine(obj.Amount, obj.StarkKey, obj.TokenId, obj.Type, obj.VaultId);
    }
}