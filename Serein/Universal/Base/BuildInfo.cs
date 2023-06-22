using Serein.Properties;
using Serein.Utils;

namespace Serein.Base
{
    internal class BuildInfo
    {
        public BuildInfo()
        {
            string[] arg = System.Text.RegularExpressions.Regex.Replace(
                (Resources.buildinfo ?? string.Empty).Trim(' ', '\n', '\r').Replace("\r", string.Empty),
                @"[^\u0000-\u007f]+", string.Empty).Split('\n');
            switch (arg.Length)
            {
                case 1:
                    Type = arg[0].Trim();
                    break;
                case 2:
                    Type = arg[0].Trim();
                    Time = arg[1].Trim();
                    break;
                case 3:
                    Type = arg[0].Trim();
                    Time = arg[1].Trim();
                    Detail = arg[2].Trim().Replace("\\n", "\n");
                    break;
                default:
                    Logger.Output(LogType.Debug, "未找到有效的编译信息");
                    break;
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
        public string Type { get; private set; } = "未知";

        /// <summary>
        /// 编译时间
        /// </summary>
        public string Time
        {
            get => string.IsNullOrEmpty(_time) ? "-" : _time!;
            set => _time = value;
        }
        private string? _time;

        /// <summary>
        /// 详细信息
        /// </summary>
        public string Detail
        {
            get => string.IsNullOrEmpty(_detail) ? "-" : _detail!;
            set => _detail = value;
        }

        private string? _detail;
    }
}
