using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;

namespace Serein.Core.JSPlugin.Permission
{
    [JsonObject(MemberSerialization.OptOut, NamingStrategyType = typeof(CamelCaseNamingStrategy))]
    internal class PermissionGroup
    {
        /// <summary>
        /// 权限组描述
        /// </summary>
        public string Description;

        /// <summary>
        /// 匹配对象
        /// </summary>
        public Match[] Matches = { };

        /// <summary>
        /// 权限内容
        /// </summary>
        public Dictionary<string, object> Permissions = new();

        /// <summary>
        /// 优先级
        /// </summary>
        public int Priority;

        /// <summary>
        /// 父权限组
        /// </summary>
        public string[] Parents = { };

        public PermissionGroup()
        {
            Matches ??= Array.Empty<Match>();
            Parents ??= Array.Empty<string>();
            Permissions ??= new();
            if (Priority < 0)
            {
                Priority = 0;
            }
        }
    }
}