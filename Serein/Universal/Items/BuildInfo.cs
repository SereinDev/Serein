using Serein.Properties;

namespace Serein.Items
{
    internal class BuildInfo
    {
        public BuildInfo()
        {
            string[] Arg = System.Text.RegularExpressions.Regex.Replace(
                (Resources.Build_Info ?? string.Empty).Trim(' ', '\n', '\r').Replace("\r", string.Empty),
                @"[^\u0000-\u007f]+", string.Empty).Split('\n');
            if (Arg.Length >= 5)
            {
                Type = Arg[0].Trim();
                Time = Arg[1].Trim();
                Dir = Arg[2].Trim();
                OS = Arg[3].Trim();
                Detail = Arg[4].Trim();
            }
        }

        public override string ToString()
        {
            return "" +
                $"编译类型：{Type}\r\n" +
                $"编译时间：{Time}\r\n" +
                $"编译路径：{Dir}\r\n" +
                $"系统类型：{OS}\r\n" +
                $"详细信息：{Detail}";
        }

        /// <summary>
        /// 编译类型
        /// </summary>
        public string Type { get; set; } = "未指定";

        /// <summary>
        /// 编译时间
        /// </summary>
        public string Time { get; set; } = "-";

        /// <summary>
        /// 系统
        /// </summary>
        public string OS { get; set; } = "-";

        /// <summary>
        /// 路径
        /// </summary>
        public string Dir { get; set; } = "-";

        /// <summary>
        /// 详细信息
        /// </summary>
        public string Detail { get; set; } = "-";
    }
}
