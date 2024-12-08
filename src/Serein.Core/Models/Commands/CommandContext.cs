using System.Collections.Generic;
using Serein.Core.Models.Network.Connection.OneBot.Packets;
using RegexMatch = System.Text.RegularExpressions.Match;

namespace Serein.Core.Models.Commands;

/// <summary>
/// 命令上下文
/// </summary>
/// <param name="Match">匹配对象（来自匹配功能）</param>
/// <param name="MessagePacket">消息数据包（来自消息匹配）</param>
/// <param name="ServerId">服务器Id（与服务器相关的命令）</param>
/// <param name="Variables">自定义变量</param>
public record CommandContext(
    RegexMatch? Match = null,
    MessagePacket? MessagePacket = null,
    string? ServerId = null,
    IReadOnlyDictionary<string, string?>? Variables = null
);
