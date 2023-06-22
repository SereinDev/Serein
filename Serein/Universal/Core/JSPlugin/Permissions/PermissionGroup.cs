using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Serein.Core.JSPlugin.Permission
{
    [JsonObject(MemberSerialization.OptOut, NamingStrategyType = typeof(CamelCaseNamingStrategy))]
    internal class PermissionGroup
    {
        /// <summary>
        /// 权限组描述
        /// </summary>
        public string? Description { get; init; }

        /// <summary>
        /// 匹配条件
        /// </summary>
        public Condition[] Conditions { get; init; } = { };

        /// <summary>
        /// 权限内容
        /// </summary>
        public IDictionary<string, object> Permissions { get; init; }

        /// <summary>
        /// 优先级
        /// </summary>
        public int Priority { get; init; }

        /// <summary>
        /// 父权限组
        /// </summary>
        public string[] Parents { get; init; } = Array.Empty<string>();

        public PermissionGroup()
        {
            Conditions ??= Array.Empty<Condition>();
            Parents ??= Array.Empty<string>();
            Permissions ??= new Dictionary<string, object>();
            if (Priority < 0)
            {
                Priority = 0;
            }
        }

        /// <summary>
        /// 是否匹配（JS）
        /// </summary>
        /// <param name="type">类型</param>
        /// <param name="userId">用户ID</param>
        /// <param name="groupId">群号</param>
        /// <returns>匹配结果</returns>
        public bool isMatchedWith(string type, long userId, long groupId = -1)
            => IsMatchedWith(type, userId, groupId);

        /// <summary>
        /// 是否匹配
        /// </summary>
        /// <param name="type">类型</param>
        /// <param name="userId">用户ID</param>
        /// <param name="groupId">群号</param>
        /// <returns>匹配结果</returns>
        public bool IsMatchedWith(string type, long userId, long groupId = -1)
            => Conditions.Where((condition) =>
                condition.Type.ToLowerInvariant() == type && // 类型相同
                (condition.Users?.Contains(userId) ?? true) && // 用户判断，数组为空则为全体
                (groupId > 0 && (condition.Groups?.Contains(groupId) ?? true) && type == "group" || type != "group") // 群聊判断（前提为type是group且groupId>0），数组为空则为全体
                ).Count() > 0;
    }
}