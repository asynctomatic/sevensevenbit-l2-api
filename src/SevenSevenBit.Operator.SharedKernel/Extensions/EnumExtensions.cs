namespace SevenSevenBit.Operator.SharedKernel.Extensions;

using System.Runtime.Serialization;

public static class EnumExtensions
{
    public static string ToEnumString<T>(this T type)
    {
        var enumType = typeof(T);
        var name = Enum.GetName(enumType, type);

        if (name == null)
        {
            return null;
        }

        var enumMemberAttribute = (((EnumMemberAttribute[])enumType.GetField(name)?.GetCustomAttributes(typeof(EnumMemberAttribute), true)) ??
                                   Array.Empty<EnumMemberAttribute>()).Single();

        return enumMemberAttribute.Value;
    }
}