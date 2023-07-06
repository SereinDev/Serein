using Newtonsoft.Json;
using Serein.Utils;
using System;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Serein.Base.Motd
{
    internal class Motdje : Motd
    {
        public static readonly byte[] SentPacket = new byte[] { 6, 0, 0, 0, 0x63, 0xdd, 1, 1, 0 };

        /// <summary>
        /// Java版Motd获取入口
        /// </summary>
        /// <param name="addr">地址</param>
        public Motdje(string addr)
        {
            if (!TryParse(addr))
            {
                return;
            }
            if (Port == -1)
            {
                Port = 25565;
            }
            TryGet();
        }

        /// <summary>
        /// Java版Motd获取入口（本地）
        /// </summary>
        /// <param name="port">端口</param>
        public Motdje(int port)
        {
            Port = port;
            TryGet();
        }

        /// <summary>
        /// 尝试获取信息
        /// </summary>
        internal override bool TryGet()
        {
            try
            {
                Get();
                return true;
            }
            catch (Exception e)
            {
                Logger.Output(LogType.Debug, e);
                Exception = e.Message;
                return false;
            }
        }

        /// <summary>
        /// 获取信息
        /// </summary>
        internal override void Get()
        {
            byte[] datas = new byte[1024 * 1024];
            int totalLength = 0;

            using (Socket socket = new(IP.AddressFamily, SocketType.Stream, ProtocolType.Tcp)
            {
                SendTimeout = 1000,
                ReceiveTimeout = 1000
            })
            {
                socket.Connect(new IPEndPoint(IP, Port));
                DateTime startTime = DateTime.Now;

                socket.Send(SentPacket);

                while (true)
                {
                    try
                    {
                        byte[] buffer = new byte[1024 * 16];
                        int length = socket.Receive(buffer);
                        if (length == 0)
                        {
                            break; // 空包
                        }
                        Array.Copy(buffer, 0, datas, totalLength, length);
                        totalLength += length;

                        Origin = Encoding.UTF8.GetString(datas, 0, totalLength);
                        if (Origin.Length > 0 && Origin.EndsWith("}") && new[] { ']', '}', '"' }.ToList().Contains(Origin[Origin.Length - 2]))
                        {
                            break; // 接收完毕
                        }
                    }
                    catch
                    {
                        break;
                    }
                }
                Latency = (DateTime.Now - startTime).TotalMilliseconds;
            }

            Origin = Encoding.UTF8.GetString(datas, 0, totalLength);


            if (Origin.Contains("{"))
            {
                Origin = Origin.Substring(Origin.IndexOf('{'));
                Logger.Output(LogType.Debug, $"Origin: {Origin}");

                MotdjePacket.Packet packet = JsonConvert.DeserializeObject<MotdjePacket.Packet>(Origin) ?? throw new ArgumentNullException();

                IsSuccessful = true;
                OnlinePlayer = packet.Players.Online;
                MaxPlayer = packet.Players.Max;
                Version = packet.Version.Name;
                Protocol = packet.Version.Protocol.ToString();
                Description = packet.Description?.Text;
                Favicon = packet.Favicon;
            }
            else
            {
                throw new NotSupportedException("数据包格式貌似不正确");
            }
        }
    }
}
