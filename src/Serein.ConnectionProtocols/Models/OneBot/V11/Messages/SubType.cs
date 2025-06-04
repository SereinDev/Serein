using System.Text.Json.Serialization;

namespace Serein.ConnectionProtocols.Models.OneBot.V11.Messages;

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
