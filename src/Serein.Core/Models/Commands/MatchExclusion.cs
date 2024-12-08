using System.Collections.Generic;

namespace Serein.Core.Models.Commands;

public record MatchExclusion
{
    public List<long> Groups { get; init; } = [];

    public List<long> Users { get; init; } = [];

    public List<string> Servers { get; init; } = [];
}
