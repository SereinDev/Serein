using System.Collections.Generic;
using Serein.Core.Models.Network.Connection;
using RegexMatch = System.Text.RegularExpressions.Match;

namespace Serein.Core.Models.Commands;

/// <summary>
/// 命令上下文
/// </summary>
public readonly record struct CommandContext
{
    /// <summary>
    /// 来源
    /// </summary>
    public CommandOrigin Origin { get; init; }

    public RegexMatch? Match { get; init; }

    public Packets Packets { get; init; }

    public string? ServerId { get; init; }

    public IReadOnlyDictionary<string, string?>? Variables { get; init; }
}
