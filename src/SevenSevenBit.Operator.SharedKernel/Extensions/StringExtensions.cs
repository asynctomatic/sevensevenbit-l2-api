namespace SevenSevenBit.Operator.SharedKernel.Extensions;

using System.Runtime.Serialization;
using System.Text.RegularExpressions;
using Nethereum.Hex.HexConvertors.Extensions;

public static class StringExtensions
{
    public static string ToNormalized(this string hexString)
    {
        return hexString.RemoveHexPrefix().ToLower();
    }

    public static string ToDenormalized(this string hexString)
    {
        return hexString.EnsureHexPrefix().ToLower();
    }

    public static string FromTitleCaseToCamelCase(this string fieldName)
        => char.ToLowerInvariant(fieldName[0]) + fieldName[1..];

    public static T ToEnum<T>(this string str)
    {
        var enumType = typeof(T);
        foreach (var name in Enum.GetNames(enumType))
        {
            var enumMemberAttribute = (((EnumMemberAttribute[])enumType.GetField(name)?.GetCustomAttributes(typeof(EnumMemberAttribute), true)) ??
                                       Array.Empty<EnumMemberAttribute>()).Single();

            if (enumMemberAttribute.Value == str)
            {
                return (T)Enum.Parse(enumType, name);
            }
        }

        throw new ArgumentException("Invalid enum member value.");
    }

    public static bool IsValidHexString(this string hexString)
    {
        return Regex.IsMatch(hexString.RemoveHexPrefix(), @"\A\b[0-9a-fA-F]+\b\Z");
    }

    public static string ConvertToHex(this string str)
    {
        return str.ToHexUTF8();
    }
}