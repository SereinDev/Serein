using System.Text.Json.Serialization;

using Serein.Core.Utils.Json;

namespace Serein.Core.Models.Network.OneBot.Messages;

[JsonConverter(typeof(EnumNumberConverter<MessageType>))]
public enum MessageType
{
    Unknown,
    Private,
    Group,
}
