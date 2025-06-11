using System.Collections.Generic;

namespace Serein.Core.Models.Commands;

public record MatchExclusion
{
    public List<string> Channels { get; init; } = [];

    public List<string> Groups { get; init; } = [];

    public List<string> Users { get; init; } = [];

    public List<string> Servers { get; init; } = [];
}
