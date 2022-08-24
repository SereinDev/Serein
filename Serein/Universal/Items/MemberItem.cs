using Newtonsoft.Json;

namespace Serein.Items
{
    [JsonObject(MemberSerialization.OptOut)]
    internal class MemberItem
    {
        public long ID { get; set; } = 0;
        public string Card { get; set; } = string.Empty;
        public string Nickname { get; set; } = string.Empty;
        public int Role { get; set; } = 2;
        public string GameID { get; set; } = string.Empty;

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
