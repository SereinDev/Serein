using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Serein.Extensions;
using System;

namespace Serein.Base.Packets
{
    [JsonObject(NamingStrategyType = typeof(SnakeCaseNamingStrategy))]
    internal class Sender
    {
        /// <summary>
        /// 用户ID
        /// </summary>
        public long UserId;

        /// <summary>
        /// 年龄
        /// </summary>
        public long Age;

        public string? Nickname,
            Area,
            Level,
            Role,
            Sex,
            Title,
            Card;

        /// <summary>
        /// 性别下标
        /// </summary>
        internal int SexIndex => Array.IndexOf(Sexs, Sex);

        /// <summary>
        /// 角色下标
        /// </summary>
        internal int RoleIndex => Array.IndexOf(Roles, Role);

        /// <summary>
        /// 性别名称
        /// </summary>
        internal string SexName => SexNames.GetValue(SexIndex)?.ToString() ?? "未知";

        /// <summary>
        /// 角色名称
        /// </summary>
        internal string RoleName => Roles.GetValue(RoleIndex)?.ToString() ?? "成员";

        public static readonly string[] Sexs = { "unknown", "male", "female" };
        public static readonly string[] SexNames = { "未知", "男", "女" };
        public static readonly string[] Roles = { "owner", "admin", "member" };
        public static readonly string[] RoleNames = { "群主", "管理员", "成员" };

        public override string ToString() => this.ToJson();
    }
}
