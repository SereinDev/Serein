using System;
using System.Text.Json.Serialization;

namespace Serein.ConnectionProtocols.Models.OneBot.V11.Messages;

public class Sender
{
    public string Nickname { get; set; } = string.Empty;

    public string Card { get; set; } = string.Empty;

    public string Title { get; set; } = string.Empty;

    public Role Role { get; set; }

    [JsonIgnore]
    public string RoleName =>
        Role switch
        {
            Role.Member => "成员",
            Role.Admin => "管理员",
            Role.Owner => "群主",
            _ => throw new ArgumentOutOfRangeException(),
        };

    public string TinyId { get; set; } = string.Empty;

    public long UserId { get; set; }
}
