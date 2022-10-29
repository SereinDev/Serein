using System;
using System.Net;

namespace Serein.Items.Motd
{
    internal class Motd
    {
        /// <summary>
        /// IP
        /// </summary>
        public IPAddress IP { get; set; } = IPAddress.Parse("127.0.0.1");

        /// <summary>
        /// 端口
        /// </summary>
        public int Port { get; set; } = 0;

        /// <summary>
        /// 最大玩家数
        /// </summary>
        public string MaxPlayer { get; set; } = "-";

        /// <summary>
        /// 在线玩家数
        /// </summary>
        public string OnlinePlayer { get; set; } = "-";

        /// <summary>
        /// 服务器描述
        /// </summary>
        public string Description { get; set; } = "-";

        /// <summary>
        /// 协议
        /// </summary>
        public string Protocol { get; set; } = "-";

        /// <summary>
        /// 版本
        /// </summary>
        public string Version { get; set; } = "-";

        /// <summary>
        /// 存档名称
        /// </summary>
        public string LevelName { get; set; } = "-";

        /// <summary>
        /// 游戏模式
        /// </summary>
        public string GameMode { get; set; } = "-";

        /// <summary>
        /// 延迟
        /// </summary>
        public TimeSpan Delay { get; set; } = TimeSpan.Zero;

        /// <summary>
        /// 图标
        /// </summary>
        public string Favicon { get; set; } = "-";

        /// <summary>
        /// 原文
        /// </summary>
        public string Original { get; set; } = "-";

        /// <summary>
        /// 错误消息
        /// </summary>
        public string Exception { get; set; } = string.Empty;

        /// <summary>
        /// 获取成功
        /// </summary>
        public bool Success { get; set; } = false;

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="NewIp">IP</param>
        /// <param name="NewPort">端口</param>
        /// <returns>是否成功</returns>
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
