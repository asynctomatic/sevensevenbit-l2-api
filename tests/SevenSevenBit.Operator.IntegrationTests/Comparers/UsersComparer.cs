namespace SevenSevenBit.Operator.IntegrationTests.Comparers;

using SevenSevenBit.Operator.Application.DTOs.Entities;

public class UsersComparer : IEqualityComparer<UserDto>
{
    public bool Equals(UserDto x, UserDto y)
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

        return x.UserId.Equals(y.UserId);
    }

    public int GetHashCode(UserDto obj)
    {
        return HashCode.Combine(
            obj.UserId,
            obj.StarkKey);
    }
}