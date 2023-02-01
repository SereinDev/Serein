using Newtonsoft.Json;
using Serein.Extensions;

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
        public int Area { get; set; }

        /// <summary>
        /// 需要管理权限
        /// </summary>
        public bool IsAdmin { get; set; }

        /// <summary>
        /// 忽略的对象
        /// </summary>
        public long[] Ignored = { };

        /// <summary>
        /// 作用域文本列表
        /// </summary>
        public static readonly string[] AreaArray = { "禁用", "控制台", "消息（群聊）", "消息（私聊）", "消息（自身发送）" };

        /// <summary>
        /// 作用域 - 文本
        /// </summary>
        [JsonIgnore]
        public string Area_Text
        {
            get
            {
                return AreaArray[Area];
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
        /// <param name="text">TSV格式文本</param>
        public void FromText(string text)
        {
            string[] args = text.Split('\t');
            if (args.Length != 5)
            {
                return;
            }
            Expression = args[0];
            Area = int.TryParse(args[1], out int s) ? s : 0;
            IsAdmin = args[2] == "True";
            Remark = args[3];
            Command = args[4];
        }

        /// <summary>
        /// 检查是否合法
        /// </summary>
        /// <returns>是否合法</returns>
        public bool Check()
            => Base.Command.GetType(Command) != CommandType.Invalid && Expression.TestRegex();
    }
}
