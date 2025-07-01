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

    public string[] Ids => [.. permissionGroupProvider.Value.Keys];

    public static void ValidateGroupId(string? id)
    {
        if (string.IsNullOrEmpty(id) || !GroupIdRegex.IsMatch(id))
        {
            throw new InvalidOperationException("权限组Id格式不正确");
        }
    }

    public void Add(string id, Group group)
    {
        if (!permissionGroupProvider.Value.TryAdd(id, group))
        {
            throw new InvalidOperationException("已经存在了相同Id的权限组");
        }

        permissionGroupProvider.SaveAsyncWithDebounce();
    }

    public bool Remove(string id)
    {
        permissionGroupProvider.SaveAsyncWithDebounce();
        return permissionGroupProvider.Value.Remove(id);
    }

    public Group this[string id]
    {
        get => permissionGroupProvider.Value[id];
        set
        {
            permissionGroupProvider.Value[id] = value;
            permissionGroupProvider.SaveAsyncWithDebounce();
        }
    }

    public Dictionary<string, bool?> GetAllNodes(string userId, bool ignoreWildcard = false)
    {
        var result = new Dictionary<string, (int Priority, bool? Value)>();
        var visitedGroups = new HashSet<string>();
        var nodes = permissionManager.Nodes.Keys;

        lock (permissionGroupProvider.Value)
        {
            foreach (var kv in permissionGroupProvider.Value)
            {
                if (visitedGroups.Contains(kv.Key))
                {
                    continue;
                }

                if (kv.Key == "everyone" || kv.Value.Users.Contains(userId))
                {
                    ProcessGroup(kv.Key, kv.Value);
                }
            }
        }

        return result.ToDictionary(kv => kv.Key, kv => kv.Value.Value);

        void ProcessGroup(string groupId, Group group)
        {
            if (visitedGroups.Contains(groupId))
            {
                return;
            }
            visitedGroups.Add(groupId);

            foreach (var parent in group.Parents)
            {
                if (permissionGroupProvider.Value.TryGetValue(parent, out var parentGroup))
                {
                    ProcessGroup(parent, parentGroup);
                }
            }

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
                    {
                        if (key.StartsWith(keyWithoutWildcard))
                        {
                            CompareAndAdd(key, priority, permission.Value);
                        }
                    }
                }
                else
                {
                    CompareAndAdd(permission.Key, priority, permission.Value);
                }
            }
        }

        void CompareAndAdd(string key, int priority, bool? value)
        {
            if (!result.TryGetValue(key, out var permission) || permission.Priority <= priority)
            {
                result[key] = (priority, value);
            }
        }
    }
}
