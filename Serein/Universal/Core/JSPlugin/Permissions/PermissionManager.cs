using Jint;
using Jint.Native;
using Serein.Utils.IO;
using System.Collections.Generic;
using System.Linq;

namespace Serein.Core.JSPlugin.Permission
{
    internal static class PermissionManager
    {
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="groupName">权限组名称</param>
        /// <param name="permissionGroup">权限组对象</param>
        /// <param name="overwrite">覆写</param>
        /// <returns>添加结果</returns>
        public static bool Add(string groupName, PermissionGroup permissionGroup, bool overwrite)
        {
            if (
                string.IsNullOrEmpty(groupName)
                || string.IsNullOrWhiteSpace(groupName)
                || permissionGroup is null
            )
            {
                return false;
            }
            lock (Global.PermissionGroups)
            {
                if (Global.PermissionGroups.ContainsKey(groupName))
                {
                    if (overwrite)
                    {
                        Global.PermissionGroups[groupName] = permissionGroup;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    Global.PermissionGroups.Add(groupName, permissionGroup);
                }
                Data.SavePermissionGroups();
            }
            return true;
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="groupName">权限组名称</param>
        /// <returns>删除结果</returns>
        public static bool Remove(string groupName)
        {
            if (string.IsNullOrEmpty(groupName))
            {
                return false;
            }
            lock (Global.PermissionGroups)
            {
                if (Global.PermissionGroups.Remove(groupName))
                {
                    Data.SavePermissionGroups();
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 计算权限组
        /// </summary>
        /// <param name="type">类型</param>
        /// <param name="userId">用户ID</param>
        /// <param name="groupId">群号</param>
        /// <returns>权限内容</returns>
        public static IDictionary<string, object> Calculate(
            string type,
            long userId,
            long groupId = -1
        )
        {
            Dictionary<string, (int, object)> tempPermissions = new();
            lock (Global.PermissionGroups)
            {
                foreach (PermissionGroup permissionGroup in Global.PermissionGroups.Values)
                {
                    if (!permissionGroup.IsMatchedWith(type, userId, groupId))
                    {
                        continue;
                    }
                    Inherit(permissionGroup, 0)
                        .ToList()
                        .ForEach(kv =>
                        {
                            if (!tempPermissions.ContainsKey(kv.Key))
                            {
                                tempPermissions.Add(kv.Key, (permissionGroup.Priority, kv.Value));
                            }
                        });
                    foreach (string key in permissionGroup.Permissions.Keys)
                    {
                        if (!tempPermissions.TryGetValue(key, out (int, object) conflict))
                        {
                            tempPermissions.Add(
                                key,
                                (permissionGroup.Priority, permissionGroup.Permissions[key])
                            );
                            continue;
                        }
                        if (conflict.Item1 < permissionGroup.Priority)
                        {
                            tempPermissions[key] = (
                                permissionGroup.Priority,
                                permissionGroup.Permissions[key]
                            );
                        }
                    }
                }
            }
            return tempPermissions
                .Select((kv) => new KeyValuePair<string, object>(kv.Key, kv.Value.Item2))
                .ToDictionary(kv => kv.Key, kv => kv.Value);
        }

        /// <summary>
        /// 继承
        /// </summary>
        /// <param name="permissionGroup">根权限组</param>
        /// <param name="depth">深度</param>
        /// <returns>权限内容</returns>
        private static IDictionary<string, object> Inherit(
            PermissionGroup permissionGroup,
            int depth
        )
        {
            if (depth > 3)
            {
                return permissionGroup.Permissions;
            }
            IDictionary<string, object> tempPermissions = permissionGroup.Permissions;
            lock (Global.PermissionGroups)
            {
                foreach (string parent in permissionGroup.Parents)
                {
                    if (
                        Global.PermissionGroups.TryGetValue(
                            parent,
                            out PermissionGroup? parentPermissionGroup
                        )
                    )
                    {
                        Inherit(parentPermissionGroup, depth + 1)
                            .ToList()
                            .ForEach(kv =>
                            {
                                if (!tempPermissions.ContainsKey(kv.Key))
                                {
                                    tempPermissions.Add(kv.Key, kv.Value);
                                }
                            });
                    }
                }
            }
            return tempPermissions;
        }

        /// <summary>
        /// 设置权限
        /// </summary>
        /// <param name="groupName">权限组名</param>
        /// <param name="permissionKey">权限key</param>
        /// <param name="value">值</param>
        /// <returns>设置结果</returns>
        public static bool SetPermission(string groupName, string permissionKey, JsValue value)
        {
            if (
                !Global.PermissionGroups.TryGetValue(
                    groupName,
                    out PermissionGroup? permissionGroup
                )
                || permissionGroup.Permissions is null
                || string.IsNullOrEmpty(groupName)
                || string.IsNullOrEmpty(permissionKey)
            )
            {
                return false;
            }
            bool result = false;
            lock (Global.PermissionGroups)
            {
                if (!permissionGroup.Permissions.ContainsKey(permissionKey))
                {
                    Global.PermissionGroups[groupName].Permissions.Add(
                        permissionKey,
                        value.ToObject()
                    );
                    result = true;
                }
                else if (value is null || value.IsNull() || value.IsUndefined())
                {
                    result = Global.PermissionGroups[groupName].Permissions.Remove(permissionKey);
                }
                else
                {
                    Global.PermissionGroups[groupName].Permissions[permissionKey] =
                        value.ToObject();
                    result = true;
                }
            }
            Data.SavePermissionGroups();
            return result;
        }
    }
}
