using System.Text.Json.Serialization;

namespace Serein.ConnectionProtocols.Models.OneBot.V11.Messages;

[JsonConverter(typeof(JsonStringEnumConverter<Role>))]
public enum Role
{
    Member,
    Admin,
    Owner,
}
