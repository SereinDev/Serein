using System.Text.Json.Serialization;

namespace Serein.ConnectionProtocols.Models.OneBot.V11.Messages;

[JsonConverter(typeof(JsonStringEnumConverter<MessageFormat>))]
public enum MessageFormat
{
    String,
    Array,
}
