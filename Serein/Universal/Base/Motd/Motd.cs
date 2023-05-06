using Serein.Utils;
using System;
using System.Net;

namespace Serein.Base.Motd
{
    internal abstract class Motd
    {
        /// <summary>
        /// IP
        /// </summary>
        internal IPAddress IP { get; private set; } = IPAddress.Parse("127.0.0.1");

        /// <summary>
        /// 端口
        /// </summary>
        internal int Port = -1;

        /// <summary>
        /// 最大玩家数
        /// </summary>
        internal int MaxPlayer;

        /// <summary>
        /// 在线玩家数
        /// </summary>
        internal int OnlinePlayer;

        /// <summary>
        /// 服务器描述
        /// </summary>
        internal string Description = string.Empty;

        /// <summary>
        /// 协议
        /// </summary>
        internal string Protocol = string.Empty;

        /// <summary>
        /// 版本
        /// </summary>
        internal string Version = string.Empty;

        /// <summary>
        /// 存档名称
        /// </summary>
        internal string LevelName = string.Empty;

        /// <summary>
        /// 游戏模式
        /// </summary>
        internal string GameMode = string.Empty;

        /// <summary>
        /// 延迟
        /// </summary>
        internal double Delay;

        /// <summary>
        /// 图标
        /// </summary>
        internal string Favicon = string.Empty;

        /// <summary>
        /// 原文
        /// </summary>
        internal string Origin = string.Empty;

        /// <summary>
        /// 错误消息
        /// </summary>
        internal string Exception = string.Empty;

        /// <summary>
        /// 获取成功
        /// </summary>
        internal bool IsSuccessful;

        private static readonly System.Text.RegularExpressions.Regex IPv4Patten = new(@"((?:(?:25[0-5]|2[0-4]\d|((1\d{2})|([1-9]?\d)))\.){3}(?:25[0-5]|2[0-4]\d|((1\d{2})|([1-9]?\d))))", System.Text.RegularExpressions.RegexOptions.Compiled);

        private static readonly System.Text.RegularExpressions.Regex IPv6Patten = new(@"^\s*((([0-9A-Fa-f]{1,4}:){7}([0-9A-Fa-f]{1,4}|:))|(([0-9A-Fa-f]{1,4}:){6}(:[0-9A-Fa-f]{1,4}|((25[0-5]|2[0-4]\d|1\d\d|[1-9]?\d)(\.(25[0-5]|2[0-4]\d|1\d\d|[1-9]?\d)){3})|:))|(([0-9A-Fa-f]{1,4}:){5}(((:[0-9A-Fa-f]{1,4}){1,2})|:((25[0-5]|2[0-4]\d|1\d\d|[1-9]?\d)(\.(25[0-5]|2[0-4]\d|1\d\d|[1-9]?\d)){3})|:))|(([0-9A-Fa-f]{1,4}:){4}(((:[0-9A-Fa-f]{1,4}){1,3})|((:[0-9A-Fa-f]{1,4})?:((25[0-5]|2[0-4]\d|1\d\d|[1-9]?\d)(\.(25[0-5]|2[0-4]\d|1\d\d|[1-9]?\d)){3}))|:))|(([0-9A-Fa-f]{1,4}:){3}(((:[0-9A-Fa-f]{1,4}){1,4})|((:[0-9A-Fa-f]{1,4}){0,2}:((25[0-5]|2[0-4]\d|1\d\d|[1-9]?\d)(\.(25[0-5]|2[0-4]\d|1\d\d|[1-9]?\d)){3}))|:))|(([0-9A-Fa-f]{1,4}:){2}(((:[0-9A-Fa-f]{1,4}){1,5})|((:[0-9A-Fa-f]{1,4}){0,3}:((25[0-5]|2[0-4]\d|1\d\d|[1-9]?\d)(\.(25[0-5]|2[0-4]\d|1\d\d|[1-9]?\d)){3}))|:))|(([0-9A-Fa-f]{1,4}:){1}(((:[0-9A-Fa-f]{1,4}){1,6})|((:[0-9A-Fa-f]{1,4}){0,4}:((25[0-5]|2[0-4]\d|1\d\d|[1-9]?\d)(\.(25[0-5]|2[0-4]\d|1\d\d|[1-9]?\d)){3}))|:))|(:(((:[0-9A-Fa-f]{1,4}){1,7})|((:[0-9A-Fa-f]{1,4}){0,5}:((25[0-5]|2[0-4]\d|1\d\d|[1-9]?\d)(\.(25[0-5]|2[0-4]\d|1\d\d|[1-9]?\d)){3}))|:)))(%.+)?\s*$", System.Text.RegularExpressions.RegexOptions.Compiled);

        /// <summary>
        /// 尝试转换IP
        /// </summary>
        /// <param name="addr">IP</param>
        /// <returns>结果</returns>
        internal bool TryParse(string addr)
        {
            try
            {
                addr = addr.Trim();
                if (addr.Contains(":") && !IPv6Patten.IsMatch(addr) && addr.LastIndexOf(':') != addr.Length - 1)
                {
                    // 分离端口号和IP/域名
                    Port = int.Parse(addr.Substring(addr.LastIndexOf(':') + 1, addr.Length - addr.LastIndexOf(':') - 1));
                    if (Port < 0 || Port > 65535)
                    {
                        throw new ArgumentOutOfRangeException(nameof(addr), "无效的端口号");
                    }
                    addr = addr.Substring(0, addr.LastIndexOf(':')).Trim('[', ']');
                }
                if (!IPv4Patten.IsMatch(addr) && !IPv6Patten.IsMatch(addr))
                {
                    IP = Dns.GetHostAddresses(addr)[0];
                }
                else
                {
                    IP = IPAddress.Parse(addr);
                }
                return true;
            }
            catch (Exception e)
            {
                Logger.Output(LogType.Debug, e);
                Exception = e.Message;
                return false;
            }
        }

        internal abstract void Get();
        internal abstract bool TryGet();
    }
}
