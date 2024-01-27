using System.Text.Json.Serialization;

using Serein.Core.Utils.Json;

namespace Serein.Core.Models.Network.OneBot.Messages;

[JsonConverter(typeof(EnumConverter<Role>))]
public enum Role
{
    Member,
    Admin,
    Owner,
}
