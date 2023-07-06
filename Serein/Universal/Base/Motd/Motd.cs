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
        public IPAddress IP { get; private set; } = IPAddress.Loopback;

        /// <summary>
        /// 端口
        /// </summary>
        public int Port = -1;

        /// <summary>
        /// 最大玩家数
        /// </summary>
        public long MaxPlayer;

        /// <summary>
        /// 在线玩家数
        /// </summary>
        public long OnlinePlayer;

        /// <summary>
        /// 服务器描述
        /// </summary>
        public string? Description;

        /// <summary>
        /// 协议
        /// </summary>
        public string? Protocol;

        /// <summary>
        /// 版本
        /// </summary>
        public string? Version;

        /// <summary>
        /// 存档名称
        /// </summary>
        public string? LevelName;

        /// <summary>
        /// 游戏模式
        /// </summary>
        public string? GameMode;

        /// <summary>
        /// 延迟
        /// </summary>
        public double Latency;

        /// <summary>
        /// 图标
        /// </summary>
        public string? Favicon;

        /// <summary>
        /// 图标
        /// </summary>
        public string? FaviconCQCode => string.IsNullOrEmpty(Favicon) ? string.Empty : $"[CQ:image,file=base64://{Favicon!.Substring(Favicon.IndexOf(',') + 1)}]";

        /// <summary>
        /// 原文
        /// </summary>
        public string? Origin;

        /// <summary>
        /// 错误消息
        /// </summary>
        public string? Exception;

        /// <summary>
        /// 获取成功
        /// </summary>
        public bool IsSuccessful;

        private static readonly System.Text.RegularExpressions.Regex _IPv4Patten = new(@"((?:(?:25[0-5]|2[0-4]\d|((1\d{2})|([1-9]?\d)))\.){3}(?:25[0-5]|2[0-4]\d|((1\d{2})|([1-9]?\d))))", System.Text.RegularExpressions.RegexOptions.Compiled);

        private static readonly System.Text.RegularExpressions.Regex _IPv6Patten = new(@"^\s*((([0-9A-Fa-f]{1,4}:){7}([0-9A-Fa-f]{1,4}|:))|(([0-9A-Fa-f]{1,4}:){6}(:[0-9A-Fa-f]{1,4}|((25[0-5]|2[0-4]\d|1\d\d|[1-9]?\d)(\.(25[0-5]|2[0-4]\d|1\d\d|[1-9]?\d)){3})|:))|(([0-9A-Fa-f]{1,4}:){5}(((:[0-9A-Fa-f]{1,4}){1,2})|:((25[0-5]|2[0-4]\d|1\d\d|[1-9]?\d)(\.(25[0-5]|2[0-4]\d|1\d\d|[1-9]?\d)){3})|:))|(([0-9A-Fa-f]{1,4}:){4}(((:[0-9A-Fa-f]{1,4}){1,3})|((:[0-9A-Fa-f]{1,4})?:((25[0-5]|2[0-4]\d|1\d\d|[1-9]?\d)(\.(25[0-5]|2[0-4]\d|1\d\d|[1-9]?\d)){3}))|:))|(([0-9A-Fa-f]{1,4}:){3}(((:[0-9A-Fa-f]{1,4}){1,4})|((:[0-9A-Fa-f]{1,4}){0,2}:((25[0-5]|2[0-4]\d|1\d\d|[1-9]?\d)(\.(25[0-5]|2[0-4]\d|1\d\d|[1-9]?\d)){3}))|:))|(([0-9A-Fa-f]{1,4}:){2}(((:[0-9A-Fa-f]{1,4}){1,5})|((:[0-9A-Fa-f]{1,4}){0,3}:((25[0-5]|2[0-4]\d|1\d\d|[1-9]?\d)(\.(25[0-5]|2[0-4]\d|1\d\d|[1-9]?\d)){3}))|:))|(([0-9A-Fa-f]{1,4}:){1}(((:[0-9A-Fa-f]{1,4}){1,6})|((:[0-9A-Fa-f]{1,4}){0,4}:((25[0-5]|2[0-4]\d|1\d\d|[1-9]?\d)(\.(25[0-5]|2[0-4]\d|1\d\d|[1-9]?\d)){3}))|:))|(:(((:[0-9A-Fa-f]{1,4}){1,7})|((:[0-9A-Fa-f]{1,4}){0,5}:((25[0-5]|2[0-4]\d|1\d\d|[1-9]?\d)(\.(25[0-5]|2[0-4]\d|1\d\d|[1-9]?\d)){3}))|:)))(%.+)?\s*$", System.Text.RegularExpressions.RegexOptions.Compiled);

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
                if (addr.Contains(":") && !_IPv6Patten.IsMatch(addr) && addr.LastIndexOf(':') != addr.Length - 1)
                {
                    // 分离端口号和IP/域名
                    Port = int.Parse(addr.Substring(addr.LastIndexOf(':') + 1, addr.Length - addr.LastIndexOf(':') - 1));
                    if (Port < 0 || Port > 65535)
                    {
                        throw new ArgumentOutOfRangeException(nameof(addr), "无效的端口号");
                    }
                    addr = addr.Substring(0, addr.LastIndexOf(':')).Trim('[', ']');
                }
                if (!_IPv4Patten.IsMatch(addr) && !_IPv6Patten.IsMatch(addr))
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
