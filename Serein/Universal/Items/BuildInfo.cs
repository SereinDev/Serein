using Serein.Properties;

namespace Serein.Items
{
    internal class BuildInfo
    {
        public BuildInfo()
        {
            string[] arg = System.Text.RegularExpressions.Regex.Replace(
                (Resources.buildinfo ?? string.Empty).Trim(' ', '\n', '\r').Replace("\r", string.Empty),
                @"[^\u0000-\u007f]+", string.Empty).Split('\n');
            if (arg.Length >= 3)
            {
                Type = arg[0].Trim();
                Time = arg[1].Trim();
                Detail = arg[2].Trim().Replace("\\n", "\n");
            }
        }

        public override string ToString()
        {
            return "" +
                $"编译类型：{Type}\r\n" +
                $"编译时间：{Time}\r\n" +
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
        /// 详细信息
        /// </summary>
        public string Detail { get; set; } = "-";
    }
}
