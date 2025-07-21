using System.Collections.Generic;
using Serein.Core.Models.Network.Connection;
using RegexMatch = System.Text.RegularExpressions.Match;

namespace Serein.Core.Models.Commands;

/// <summary>
/// 命令上下文
/// </summary>
public readonly record struct CommandContext
{
    public RegexMatch? Match { get; init; }

    public Packets Packets { get; init; }

    public string? ServerId { get; init; }

    public IReadOnlyDictionary<string, string?>? Variables { get; init; }
}
