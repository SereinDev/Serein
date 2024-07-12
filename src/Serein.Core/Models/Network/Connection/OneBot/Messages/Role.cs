using System.Text.Json.Serialization;

namespace Serein.Core.Models.Network.Connection.OneBot.Messages;

[JsonConverter(typeof(JsonStringEnumConverter<Role>))]
public enum Role
{
    Member,
    Admin,
    Owner,
}
