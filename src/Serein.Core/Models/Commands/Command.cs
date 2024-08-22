namespace Serein.Core.Models.Commands;

public class Command
{
    public CommandOrigin Origin { get; init; }

    public CommandType Type { get; init; }

    public object? Argument { get; set; }

    public string Body { get; set; } = string.Empty;
}
