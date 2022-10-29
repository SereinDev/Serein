using Newtonsoft.Json;

namespace Serein.Items
{
    [JsonObject(MemberSerialization.OptOut)]
    internal class Member
    {
        /// <summary>
        /// 用户ID
        /// </summary>
        public long ID { get; set; } = 0;

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
        /// 群角色 - 文本
        /// </summary>
        [JsonIgnore]
        public string Role_Text
        {
            get
            {
                return new[] { "群主", "管理员", "成员" }[Role];
            }
        }
    }
}
