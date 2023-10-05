namespace SevenSevenBit.Operator.Application.Comparers;

using StarkEx.Client.SDK.Models.Spot.TransactionModels;

public class SettlementModelComparer : IEqualityComparer<SettlementModel>
{
    public bool Equals(SettlementModel x, SettlementModel y)
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

        return Equals(x.PartyA, y.PartyA) &&
               Equals(x.PartyB, y.PartyB) &&
               Equals(x.SettlementInfo, y.SettlementInfo);
    }

    public int GetHashCode(SettlementModel obj)
    {
        return HashCode.Combine(obj.PartyA, obj.PartyB, obj.SettlementInfo, obj.Type);
    }
}