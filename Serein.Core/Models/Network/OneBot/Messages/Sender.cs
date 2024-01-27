using System;
using System.Text.Json.Serialization;

namespace Serein.Core.Models.Network.OneBot.Messages;

public class Sender
{
    public string Nickname { get; set; } = string.Empty;

    public string Card { get; set; } = string.Empty;

    public string? Title { get; set; }

    public Role Role { get; set; }

    [JsonIgnore]
    public string RoleName =>
        Role switch
        {
            Role.Member => "成员",
            Role.Admin => "管理员",
            Role.Owner => "群主",
            _ => throw new ArgumentOutOfRangeException()
        };

    public string TinyId { get; set; } = string.Empty;

    public long UserId { get; set; }
}
