namespace Serein.Core.JSPlugin.Permission
{
    internal static class Permission
    {

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="groupName">权限组名称</param>
        /// <param name="permissionGroup">权限组对象</param>
        /// <returns>添加结果</returns>
        public static bool Add(string groupName, PermissionGroup permissionGroup)
        {
            if (string.IsNullOrEmpty(groupName) || string.IsNullOrWhiteSpace(groupName) || Global.PermissionGroups.ContainsKey(groupName))
            {
                return false;
            }
            lock (Global.PermissionGroups)
            {
                Global.PermissionGroups.Add(groupName, permissionGroup);
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
                return Global.PermissionGroups.Remove(groupName);
            }
        }
    }
}