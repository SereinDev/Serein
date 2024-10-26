using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Serein.Core.Services.Permissions;

public partial class PermissionManager
{
    [GeneratedRegex(@"^([a-zA-Z][a-zA-Z0-9\-]*\.)*[a-zA-Z][a-zA-Z0-9\-]*$")]
    private static partial Regex GetKeyRegex();

    private readonly Dictionary<string, string?> _permissions = [];

    public IReadOnlyDictionary<string, string?> Permissions => _permissions;

    public void Register(string id, string key, string? description = null)
    {
        if (!GetKeyRegex().IsMatch(key))
            throw new ArgumentException("权限键不合法", nameof(key));

        _permissions.Add($"{id}.{key}", description);
    }

    public void Unregister(string id, string key)
    {
        if (!_permissions.Remove($"{id}.{key}"))
            throw new KeyNotFoundException();
    }

    internal void Clear() => _permissions.Clear();
}