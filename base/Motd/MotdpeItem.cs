using System;
using System.Text.RegularExpressions;

namespace Serein
{
    internal class MotdpeItem
    {
        public string ip { get; set; } = "127.0.0.1";
        public int Port { get; set; } = 19132;
        public string MaxPlayer { get; set; } = "-";
        public string OnlinePlayer { get; set; } = "-";
        public string Description { get; set; } = "-";
        public string Protocol { get; set; } = "-";
        public string Version { get; set; } = "-";
        public string LevelName { get; set; } = "-";
        public string GameMode { get; set; } = "-";
        public TimeSpan Span { get; set; } = TimeSpan.Zero;
        public string ipv4 { get; set; } = "-";
        public string ipv6 { get; set; } = "-";
        public string Original { get; set; } = "-";
        public void Serialization(string Data)
        {
            Original = Data;
            string[] Datas = Data.Split(';');
            if (Datas.Length >= 12)
            {
                Description = Regex.Replace(Datas[1], "§.", string.Empty);
                Protocol = Datas[2];
                Version = Datas[3];
                OnlinePlayer = Datas[4];
                MaxPlayer = Datas[5];
                LevelName = Datas[7];
                GameMode = Datas[8];
                ipv4 = Datas[10];
                ipv6 = Datas[11];
            }
        }
    }
}
