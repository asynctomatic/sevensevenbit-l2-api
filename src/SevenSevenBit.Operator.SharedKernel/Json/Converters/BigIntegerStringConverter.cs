namespace SevenSevenBit.Operator.SharedKernel.Json.Converters;

using System.Globalization;
using System.Numerics;
using System.Text.Json;
using System.Text.Json.Serialization;

public class BigIntegerStringConverter : JsonConverter<BigInteger>
{
    public override BigInteger Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType is not (JsonTokenType.String or JsonTokenType.Number))
        {
            throw new JsonException($"Found token {reader.TokenType} but expected token {JsonTokenType.String} or {JsonTokenType.Number}");
        }

        using var doc = JsonDocument.ParseValue(ref reader);

        var value = reader.TokenType == JsonTokenType.String
            ? doc.RootElement.GetString()
            : doc.RootElement.GetRawText();

        if (value is null)
        {
            // TODO - Unsure of the best solution to handle this case without an adhoc solution,
            // this works for now because our models don't accept negative numbers in other attributes
            return BigInteger.MinusOne;
        }

        // TODO - Unsure of the best solution to handle this case without an adhoc solution,
        // this works for now because our models don't accept negative numbers in other attributes
        return BigInteger.TryParse(value, out var bigInteger) ?
            bigInteger :
            BigInteger.MinusOne;
    }

    public override void Write(Utf8JsonWriter writer, BigInteger value, JsonSerializerOptions options)
    {
        var s = value.ToString(NumberFormatInfo.InvariantInfo);
        writer.WriteStringValue(s);
    }
}