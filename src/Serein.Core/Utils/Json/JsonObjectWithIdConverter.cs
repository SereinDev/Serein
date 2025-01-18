using System;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Serein.Core.Utils.Json;

public class JsonObjectWithIdConverter<T> : JsonConverter<T>
    where T : notnull
{
    public override T? Read(
        ref Utf8JsonReader reader,
        Type typeToConvert,
        JsonSerializerOptions options
    )
    {
        throw new NotImplementedException();
    }

    public override void Write(Utf8JsonWriter writer, T value, JsonSerializerOptions options)
    {
        writer.WriteStartObject();

        foreach (PropertyInfo prop in typeof(T).GetProperties())
        {
            if (prop.GetCustomAttribute<JsonIgnoreAttribute>() is not null)
            {
                continue;
            }

            var propValue = prop.GetValue(value);

            writer.WritePropertyName(
                options.PropertyNamingPolicy?.ConvertName(prop.Name) ?? prop.Name
            );

            JsonSerializer.Serialize(writer, propValue, options);
        }

        writer.WriteNumber("id", value.GetHashCode());
        writer.WriteEndObject();
    }
}
