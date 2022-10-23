using System;
using System.Net;

namespace Serein.Items.Motd
{
    internal class Motd
    {
        public IPAddress IP { get; set; } = IPAddress.Parse("127.0.0.1");
        public int Port { get; set; } = 0;
        public string MaxPlayer { get; set; } = "-";
        public string OnlinePlayer { get; set; } = "-";
        public string Description { get; set; } = "-";
        public string Protocol { get; set; } = "-";
        public string Version { get; set; } = "-";
        public string LevelName { get; set; } = "-";
        public string GameMode { get; set; } = "-";
        public TimeSpan Delay { get; set; } = TimeSpan.Zero;
        public string Favicon { get; set; } = "-";
        public string Original { get; set; } = "-";
        public string Exception { get; set; } = string.Empty;
        public bool Success { get; set; } = false;

        public bool Init(string NewIp = "127.0.0.1", string NewPort = "19132")
        {
            try
            {
                if (NewIp.Contains(":"))
                {
                    NewPort = NewIp.Split(':')[1];
                    NewIp = NewIp.Split(':')[0];
                }
                if (!new System.Text.RegularExpressions.Regex(
                    @"((?:(?:25[0-5]|2[0-4]\d|((1\d{2})|([1-9]?\d)))\.){3}(?:25[0-5]|2[0-4]\d|((1\d{2})|([1-9]?\d))))")
                    .IsMatch(NewIp))
                {
                    IPAddress[] IPs = Dns.GetHostAddresses(NewIp);
                    IP = IPs[0];
                }
                else
                {
                    IP = IPAddress.Parse(NewIp);
                }
                Port = int.Parse(NewPort);
                return true;
            }
            catch (Exception e)
            {
                Logger.Out(LogType.Debug, "[Motd:Init()]", e.ToString());
                Exception = e.Message;
                return false;
            }
        }
    }
}
