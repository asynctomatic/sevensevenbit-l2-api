namespace SevenSevenBit.Operator.SharedKernel.Extensions;

using System.Text.Json;

public static class JsonExtensions
{
    public static string ToJson(this object value)
    {
        return JsonSerializer.Serialize(value);
    }
}