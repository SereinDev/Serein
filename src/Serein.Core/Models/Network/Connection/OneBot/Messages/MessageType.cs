using System.Text.Json.Serialization;

namespace Serein.Core.Models.Network.Connection.OneBot.Messages;

[JsonConverter(typeof(JsonStringEnumConverter<MessageType>))]
public enum MessageType
{
    Unknown,
    Private,
    Group,
}
