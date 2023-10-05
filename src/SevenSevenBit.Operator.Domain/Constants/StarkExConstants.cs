namespace SevenSevenBit.Operator.Domain.Constants;

using System.Numerics;

public static class StarkExConstants
{
    public static readonly BigInteger ValidiumVaultsLowerBound = BigInteger.Zero;

    public static readonly BigInteger ValidiumVaultsUpperBound = BigInteger.Parse("2147483648");

    public static readonly BigInteger ZkRollupVaultsLowerBound = BigInteger.Parse("9223372036854775808");

    public static readonly BigInteger ZkRollupVaultsUpperBound = BigInteger.Parse("9223372039002259456");
}