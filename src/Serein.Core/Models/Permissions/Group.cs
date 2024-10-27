using System.Collections.Generic;

namespace Serein.Core.Models.Permissions;

public class Group
{
    public string Name { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public int Priority { get; set; }

    public Dictionary<string, bool?> Nodes { get; set; } = [];

    public string[] Parents { get; set; } = [];

    public long[] Members { get; set; } = [];
}