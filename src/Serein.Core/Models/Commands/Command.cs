namespace Serein.Core.Models.Commands;

/// <summary>
/// 命令
/// </summary>
public class Command
{
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
    public object? Argument { get; set; }

    /// <summary>
    /// 主体
    /// </summary>
    public string Body { get; set; } = string.Empty;
}
