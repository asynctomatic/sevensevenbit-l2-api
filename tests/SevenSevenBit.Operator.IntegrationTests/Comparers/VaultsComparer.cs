namespace SevenSevenBit.Operator.IntegrationTests.Comparers;

using SevenSevenBit.Operator.Application.DTOs.Entities;

public class VaultsComparer : IEqualityComparer<VaultDto>
{
    public bool Equals(VaultDto x, VaultDto y)
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

        return x.VaultId.Equals(y.VaultId);
    }

    public int GetHashCode(VaultDto obj)
    {
        return HashCode.Combine(
            obj.VaultId,
            obj.VaultChainId,
            obj.AssetStarkExId,
            obj.UserStarkKey,
            obj.AvailableBalance,
            obj.AccountingBalance,
            obj.TokenId);
    }
}