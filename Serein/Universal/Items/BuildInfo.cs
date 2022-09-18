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

        public string Type { get; set; } = "未指定";
        public string Time { get; set; } = "-";
        public string OS { get; set; } = "-";
        public string Dir { get; set; } = "-";
        public string Detail { get; set; } = "-";
    }
}
