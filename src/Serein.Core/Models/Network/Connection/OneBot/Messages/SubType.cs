using System.Text.Json.Serialization;

namespace Serein.Core.Models.Network.Connection.OneBot.Messages;

[JsonConverter(typeof(JsonStringEnumConverter<SubType>))]
public enum SubType
{
    Unknown,
    Friend,
    Normal,
    Anonymous,
    Group,
    GroupSelf,
    Notice,
}
