namespace SevenSevenBit.Operator.TestHelpers.Data;

using NodaTime;
using SevenSevenBit.Operator.Domain.Entities;

public static class Users
{
    // stark private key - 0x639b2a37cd7aa37d722d3ddfaa04b6adf2a0422437d3a693980cf4ecc47e68a
    public static readonly User Alice = new()
    {
        Id = new Guid("5a35dbab-02ba-4ed4-b799-59daa263488b"),
        StarkKey = "74b754d29ba56e3fd5e9bc15fba539c9f887e1656defa78ba1035da751517e4",
        CreationDate = new LocalDateTime(2022, 08, 29, 15, 56, 30, 995),
    };

    // stark private key - 0x29a74d55d5d000fb3ffb86c715baae2819c840eb2105281bc194f33447997c7
    public static readonly User Bob = new()
    {
        Id = new Guid("5a35dbab-02ba-4ed4-b799-59daa263488c"),
        StarkKey = "2b1fb604c9641aff4e7dd5ac13d1cb9ec4fb49276b1b2b2a0add7c37ca5d906",
        CreationDate = new LocalDateTime(2022, 08, 30, 15, 56, 30, 995),
    };

    public static readonly User Carol = new()
    {
        Id = Guid.NewGuid(),
        StarkKey = "3d8a9687c613b2be32b55c5c0460e012b592e2fbbb4fc281fb87b0d8c441b3e",
        CreationDate = new LocalDateTime(2022, 11, 16, 10, 52, 40, 566),
    };

    public static IEnumerable<User> GetUsers()
    {
        yield return Alice;
        yield return Bob;
        yield return Carol;
    }
}