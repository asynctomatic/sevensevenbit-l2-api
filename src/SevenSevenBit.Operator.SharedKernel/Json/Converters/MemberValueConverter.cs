namespace SevenSevenBit.Operator.SharedKernel.Json.Converters;

using System.Text.Json;
using System.Text.Json.Serialization;
using Nethereum.ABI.EIP712;

public class MemberValueConverter : JsonConverter<MemberValue>
{
    public override MemberValue Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        using var doc = JsonDocument.ParseValue(ref reader);

        return new MemberValue
        {
            TypeName = doc.RootElement.GetProperty("typeName").GetString(),
            Value = doc.RootElement.GetProperty("value").GetString(),
        };
    }

    public override void Write(Utf8JsonWriter writer, MemberValue value, JsonSerializerOptions options)
    {
        writer.WriteStartObject();
        writer.WriteString("typeName", value.TypeName);
        writer.WriteString("value", value.Value.ToString());
        writer.WriteEndObject();
    }
}