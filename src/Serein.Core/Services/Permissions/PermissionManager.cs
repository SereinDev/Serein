using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Serein.Core.Services.Permissions;

public sealed partial class PermissionManager
{
    [GeneratedRegex(@"^([a-zA-Z][a-zA-Z0-9\-]*\.)*[a-zA-Z][a-zA-Z0-9\-]*$")]
    private static partial Regex GetKeyRegex();

    private readonly Dictionary<string, string?> _nodes = [];

    public IReadOnlyDictionary<string, string?> Nodes => _nodes;

    public void Register(string id, string node, string? description = null)
    {
        if (!GetKeyRegex().IsMatch(node))
        {
            throw new ArgumentException("权限节点不合法", nameof(node));
        }

        _nodes.Add($"{id}.{node}", description);
    }

    public void Unregister(string id, string node)
    {
        if (!_nodes.Remove($"{id}.{node}"))
        {
            throw new KeyNotFoundException();
        }
    }

    internal void Clear() => _nodes.Clear();
}
