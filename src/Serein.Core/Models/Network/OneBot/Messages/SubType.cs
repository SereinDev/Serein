using System.Text.Json.Serialization;

using Serein.Core.Utils.Json;

namespace Serein.Core.Models.Network.OneBot.Messages;

[JsonConverter(typeof(EnumNumberConverter<SubType>))]
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
