using System.Text.Json.Serialization;

using Serein.Core.Utils.Json;

namespace Serein.Core.Models.OneBot.Messages;

[JsonConverter(typeof(EnumConverter<SubType>))]
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
