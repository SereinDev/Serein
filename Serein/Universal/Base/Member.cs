using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Serein.Core.Generic;

namespace Serein.Base
{
    [JsonObject(MemberSerialization.OptOut, NamingStrategyType = typeof(CamelCaseNamingStrategy))]
    internal class Member
    {
        /// <summary>
        /// 用户ID
        /// </summary>
        public long ID { get; set; }

        /// <summary>
        /// 群名片
        /// </summary>
        public string Card { get; set; } = string.Empty;

        /// <summary>
        /// 昵称
        /// </summary>
        public string Nickname { get; set; } = string.Empty;

        /// <summary>
        /// 群角色
        /// </summary>
        public int Role { get; set; } = 2;

        /// <summary>
        /// 游戏ID
        /// </summary>
        public string GameID { get; set; } = string.Empty;

        /// <summary>
        /// 显示名字
        /// </summary>
        [JsonIgnore]
        public string ShownName => string.IsNullOrEmpty(Card) ? Nickname : Card;

        /// <summary>
        /// 群角色 - 文本
        /// </summary>
        [JsonIgnore]
        internal string Role_Text => Command.Roles_Chinese[Role];
    }
}
