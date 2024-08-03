using System;
using System.Collections.Generic;

using Serein.Core.Models.Permissions;
using Serein.Core.Services.Data;

namespace Serein.Core.Services.Permissions;

public class PermissionManager(PermissionGroupProvider permissionGroupProvider)
{
    private readonly PermissionGroupProvider _permissionGroupProvider = permissionGroupProvider;

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

    public Dictionary<string, bool?> GetAllPermissions(long userId)
    {
        var result = new Dictionary<string, bool?>();

        var list = new List<string>();

        lock (_permissionGroupProvider.Value)
        {
            foreach (var kv in _permissionGroupProvider.Value)
            {
                if (list.Contains(kv.Key))
                    continue;

                list.Add(kv.Key);

                if (!kv.Value.Members.Contains(userId) && kv.Key != "everyone")
                    foreach (var parent in kv.Value.Parents)
                    {
                        if (list.Contains(kv.Key))
                            continue;

                        list.Add(kv.Key);

                        if (_permissionGroupProvider.Value.TryGetValue(parent, out var group))
                            foreach (var permission in group.Permissions)
                                result[permission.Key] = permission.Value;
                    }
                else
                    foreach (var permission in kv.Value.Permissions)
                        result[permission.Key] = permission.Value;
            }
        }
        return result;
    }
}
