namespace Serein.Core.Models.Commands;

public enum CommandOrigin
{
    /// <summary>
    /// 空
    /// </summary>
    Null,

    /// <summary>
    /// 消息
    /// </summary>
    Message,

    /// <summary>
    /// 服务器输出
    /// </summary>
    ServerOutput,

    /// <summary>
    /// 服务器输入
    /// </summary>
    ServerInput,

    /// <summary>
    /// 定时任务
    /// </summary>
    Schedule,

    /// <summary>
    /// 反应
    /// </summary>
    Reaction,

    /// <summary>
    /// 插件
    /// </summary>
    Plugin,
}
