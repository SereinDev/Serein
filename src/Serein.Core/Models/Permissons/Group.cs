using System.Collections.Generic;

namespace Serein.Core.Models.Permissions;

public class Group
{
    public string Name { get; set; } = string.Empty;

    public string? Description { get; set; }

    public int Priority { get; set; }

    public Dictionary<string, bool?> Permissions { get; init; } = [];

    public List<string> Parents { get; init; } = [];

    public List<long> Members { get; init; } = [];
}