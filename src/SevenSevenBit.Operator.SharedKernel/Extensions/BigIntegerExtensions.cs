namespace SevenSevenBit.Operator.SharedKernel.Extensions;

using System.Globalization;
using System.Numerics;
using Nethereum.Hex.HexConvertors.Extensions;
using BC = Org.BouncyCastle.Math;

public static class BigIntegerExtensions
{
    public static BC.BigInteger ToBouncyCastle(this BigInteger bigInteger)
    {
        return new BC.BigInteger(bigInteger.ToString());
    }

    public static BigInteger ToQuantized(this BigInteger unquantizedAmount, BigInteger quantum)
    {
        return unquantizedAmount / quantum;
    }

    public static BigInteger ToUnquantized(this BigInteger quantizedAmount, BigInteger quantum)
    {
        return quantizedAmount * quantum;
    }

    public static bool IsQuantizableBy(this BigInteger unquantizedAmount, BigInteger quantum)
    {
        return unquantizedAmount % quantum == 0;
    }

    public static BigInteger ParseHexToPositiveBigInteger(string hexString)
    {
        return BigInteger.Parse($"0{hexString.RemoveHexPrefix()}", NumberStyles.AllowHexSpecifier);
    }
}