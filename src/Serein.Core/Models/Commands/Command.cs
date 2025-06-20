namespace Serein.Core.Models.Commands;

/// <summary>
/// 命令
/// </summary>
public class Command
{
    public Command() { }

    public Command(Command command)
    {
        Origin = command.Origin;
        Type = command.Type;
        Arguments = command.Arguments;
        Body = command.Body;
    }

    /// <summary>
    /// 来源
    /// </summary>
    public CommandOrigin Origin { get; init; }

    /// <summary>
    /// 类型
    /// </summary>
    public CommandType Type { get; init; }

    /// <summary>
    /// 参数
    /// </summary>
    public CommandArguments? Arguments { get; init; }

    /// <summary>
    /// 主体
    /// </summary>
    public string Body { get; set; } = string.Empty;
}
