using System.Text.Json.Serialization;

namespace Serein.Core.Models.Network.Connection.OneBot.Messages;

[JsonConverter(typeof(JsonStringEnumConverter<MessageFormat>))]
public enum MessageFormat
{
    String,
    Array,
}
