using Serein.Core.Models.Abstractions;

namespace Serein.Core.Models.Commands;

/// <summary>
/// 命令
/// </summary>
public class Command : NotifyPropertyChangedModelBase
{
    public Command() { }

    public Command(Command command)
    {
        Type = command.Type;
        Arguments = new(command.Arguments);
        Body = command.Body;
    }

    /// <summary>
    /// 类型
    /// </summary>
    public CommandType Type { get; set; }

    /// <summary>
    /// 参数
    /// </summary>
    public CommandArguments Arguments { get; set; } = new();

    /// <summary>
    /// 主体
    /// </summary>
    public string Body { get; set; } = string.Empty;
}
