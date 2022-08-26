using Serein.Properties;

namespace Serein.Items
{
    internal class BuildInfo
    {
        public BuildInfo()
        {
            string[] Arg = (Resources.Build_Info ?? string.Empty).Trim(' ', '\n', '\r').Replace("\r", string.Empty).Split('\n');
            switch (Arg.Length)
            {
                case 1:
                    EventName = Arg[0];
                    break;
                case 2:
                    EventName = Arg[0];
                    Time = Arg[1];
                    break;
                case 3:
                    EventName = Arg[0];
                    Time = Arg[1];
                    SHA = Arg[2];
                    break;
            }
        }

        public string EventName { get; set; } = "Unknown";
        public string Time { get; set; } = string.Empty;
        public string SHA { get; set; } = string.Empty;
    }
}
