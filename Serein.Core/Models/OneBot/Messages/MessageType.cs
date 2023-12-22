using System.Text.Json.Serialization;

using Serein.Core.Utils.Json;

namespace Serein.Core.Models.OneBot.Messages;

[JsonConverter(typeof(EnumConverter<MessageType>))]
public enum MessageType
{
    Unknown,
    Private,
    Group,
}
