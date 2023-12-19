namespace Serein.Core.Models.Commands;

public class Command
{
    public CommandOrigin Origin { get; init; }

    public CommandType Type { get; init; }

    public string Argument { get; init; } = string.Empty;

    public string Body { get; set; } = string.Empty;
}