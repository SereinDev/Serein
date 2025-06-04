using System.Text.Json.Serialization;

namespace Serein.ConnectionProtocols.Models.OneBot.V11.Messages;

[JsonConverter(typeof(JsonStringEnumConverter<MessageType>))]
public enum MessageType
{
    Unknown,
    Private,
    Group,
}
