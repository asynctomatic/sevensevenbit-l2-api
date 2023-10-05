namespace SevenSevenBit.Operator.IntegrationTests.Comparers;

using SevenSevenBit.Operator.Application.DTOs.Entities;

public class AssetsComparer : IEqualityComparer<AssetDto>
{
    public bool Equals(AssetDto x, AssetDto y)
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

        return x.AssetId.Equals(y.AssetId);
    }

    public int GetHashCode(AssetDto obj)
    {
        return HashCode.Combine(
            obj.AssetId,
            obj.StarkExType,
            (int)obj.Type,
            obj.Address,
            obj.Name,
            obj.Symbol,
            obj.Quantum);
    }
}