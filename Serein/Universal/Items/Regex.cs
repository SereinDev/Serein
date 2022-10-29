using Newtonsoft.Json;

namespace Serein.Items
{
    [JsonObject(MemberSerialization.OptOut)]
    internal class Regex
    {
        /// <summary>
        /// 正则表达式
        /// </summary>
        [JsonProperty(PropertyName = "Regex")]
        public string Expression { get; set; } = string.Empty;

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; } = string.Empty;

        /// <summary>
        /// 命令
        /// </summary>
        public string Command { get; set; } = string.Empty;

        /// <summary>
        /// 作用域
        /// </summary>
        public int Area { get; set; } = 0;

        /// <summary>
        /// 需要管理权限
        /// </summary>
        public bool IsAdmin { get; set; } = false;

        /// <summary>
        /// 作用域 - 文本
        /// </summary>
        [JsonIgnore]
        public string Area_Text
        {
            get
            {
                return new[] { "禁用", "控制台", "消息（群聊）", "消息（私聊）", "消息（自身发送）" }[Area];
            }
        }

        /// <summary>
        /// 需要管理权限 - 文本
        /// </summary>
        [JsonIgnore]
        public string IsAdmin_Text
        {
            get
            {
                return Area == 2 || Area == 3 ? IsAdmin ? "是" : "否" : "-";
            }
        }

        /// <summary>
        /// 转为对象
        /// </summary>
        /// <param name="Text">TSV格式文本</param>
        public void ToObject(string Text)
        {
            string[] Texts = Text.Split('\t');
            if (Texts.Length != 5)
            {
                return;
            }
            Expression = Texts[0];
            Area = int.TryParse(Texts[1], out int s) ? s : 0;
            IsAdmin = Texts[2] == "True";
            Remark = Texts[3];
            Command = Texts[4];
        }

        /// <summary>
        /// 检查是否合法
        /// </summary>
        /// <returns>是否合法</returns>
        public bool Check()
        {
            if (
                !(string.IsNullOrWhiteSpace(Expression) || string.IsNullOrEmpty(Expression) ||
                string.IsNullOrWhiteSpace(Command) || string.IsNullOrEmpty(Command)
                )
                && Base.Command.GetType(Command) != -1)
            {
                try
                {
                    new System.Text.RegularExpressions.Regex(Expression).Match(string.Empty);
                    return true;
                }
                catch
                {
                    return false;
                }
            }
            return false;
        }
    }
}
