namespace Serein.Core.Models.OneBot.Messages;

public class Sender
{
    public string Nickname { get; set; } = string.Empty;

    public Role Role { get; set; }

    public string TinyId { get; set; } = string.Empty;

    public long UserId { get; set; }
}
