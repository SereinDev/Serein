using System;
using System.Collections.Generic;
using System.Linq;

using Serein.Core.Models.Permissions;
using Serein.Core.Services.Data;

namespace Serein.Core.Services.Permissions;

public partial class GroupManager(
    PermissionManager permissionManager,
    PermissionGroupProvider permissionGroupProvider
)
{
    [System.Text.RegularExpressions.GeneratedRegex(@"^\w{3,}$")]
    private static partial System.Text.RegularExpressions.Regex GetGroupIdRegex();

    private static readonly System.Text.RegularExpressions.Regex GroupIdRegex = GetGroupIdRegex();

    private readonly PermissionManager _permissionManager = permissionManager;
    private readonly PermissionGroupProvider _permissionGroupProvider = permissionGroupProvider;

    public string[] Ids => [.. _permissionGroupProvider.Value.Keys];

    public static void ValidateGroupId(string? id)
    {
        if (string.IsNullOrEmpty(id) || !GroupIdRegex.IsMatch(id))
            throw new InvalidOperationException("权限组Id格式不正确");
    }

    public void Add(string id, Group group)
    {
        if (!_permissionGroupProvider.Value.TryAdd(id, group))
            throw new InvalidOperationException("已经存在了相同Id的权限组");

        _permissionGroupProvider.SaveAsyncWithDebounce();
    }

    public bool Remove(string id)
    {
        _permissionGroupProvider.SaveAsyncWithDebounce();
        return _permissionGroupProvider.Value.Remove(id);
    }

    public Group this[string id]
    {
        get => _permissionGroupProvider.Value[id];
        set
        {
            _permissionGroupProvider.Value[id] = value;
            _permissionGroupProvider.SaveAsyncWithDebounce();
        }
    }

    public Dictionary<string, bool?> GetAllNodes(long userId, bool ignoreWildcard = false)
    {
        var result = new Dictionary<string, (int Priority, bool? Value)>();
        var visitedGroups = new HashSet<string>();
        var nodes = _permissionManager.Nodes.Keys;

        lock (_permissionGroupProvider.Value)
        {
            foreach (var kv in _permissionGroupProvider.Value)
            {
                if (visitedGroups.Contains(kv.Key))
                    continue;

                if (kv.Key == "everyone" || kv.Value.Members.Contains(userId))
                    ProcessGroup(kv.Key, kv.Value);
            }
        }

        return result.ToDictionary(kv => kv.Key, kv => kv.Value.Value);

        void ProcessGroup(string groupId, Group group)
        {
            if (visitedGroups.Contains(groupId))
                return;

            visitedGroups.Add(groupId);

            foreach (var parent in group.Parents)
                if (_permissionGroupProvider.Value.TryGetValue(parent, out var parentGroup))
                    ProcessGroup(parent, parentGroup);

            AddNodes(group.Priority, group.Nodes);
        }

        void AddNodes(int priority, Dictionary<string, bool?> permissions)
        {
            foreach (var permission in permissions)
            {
                if (!ignoreWildcard && permission.Key.EndsWith(".*"))
                {
                    var keyWithoutWildcard = permission.Key[..^2];
                    foreach (var key in nodes)
                        if (key.StartsWith(keyWithoutWildcard))
                            CompareAndAdd(key, priority, permission.Value);
                }
                else
                    CompareAndAdd(permission.Key, priority, permission.Value);
            }
        }

        void CompareAndAdd(string key, int priority, bool? value)
        {
            if (!result.TryGetValue(key, out var permission) || permission.Priority <= priority)
                result[key] = (priority, value);
        }
    }
}
