using System;
using System.Net;
using System.Text.RegularExpressions;

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
        public bool Init(string newip = "127.0.0.1", string newPort = "19132")
        {
            try
            {
                if (newip.Contains(":"))
                {
                    newPort = newip.Split(':')[1];
                    newip = newip.Split(':')[0];
                }
                if (!new Regex(
                    @"((?:(?:25[0-5]|2[0-4]\d|((1\d{2})|([1-9]?\d)))\.){3}(?:25[0-5]|2[0-4]\d|((1\d{2})|([1-9]?\d))))")
                    .IsMatch(newip))
                {
                    IPAddress[] IPs = Dns.GetHostAddresses(newip);
                    IP = IPs[0];
                }
                else
                {
                    IP = IPAddress.Parse(newip);
                }
                Port = int.Parse(newPort);
                return true;
            }
            catch (Exception e)
            {
                Global.Logger(999, "[Motd:Init()]", e.ToString()));
                Exception = e.Message;
                return false;
            }
        }
    }
}
