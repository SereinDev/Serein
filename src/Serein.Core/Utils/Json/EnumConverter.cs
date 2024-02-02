using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Serein.Core.Utils.Json;

public class EnumConverter<T> : JsonConverter<T>
    where T : struct, Enum
{
    public override T Read(
        ref Utf8JsonReader reader,
        Type typeToConvert,
        JsonSerializerOptions options
    )
    {
        return reader.TokenType switch
        {
            JsonTokenType.String
                => Enum.TryParse(reader.GetString() ?? string.Empty, true, out T e)
                    ? e
                    : Enum.TryParse(
                        reader.GetString()?.Replace("_", null) ?? string.Empty,
                        true,
                        out e
                    )
                        ? e
                        : default,
            JsonTokenType.Number => Enum.Parse<T>(reader.GetInt64().ToString()),
            _ => throw new InvalidOperationException(),
        };
    }

    public override void Write(Utf8JsonWriter writer, T value, JsonSerializerOptions options)
    {
        writer.WriteNumberValue(Convert.ToInt32(value));
    }
}
