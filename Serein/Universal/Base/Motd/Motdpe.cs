using Serein.Utils.Output;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Serein.Base.Motd
{
    internal class Motdpe : Motd
    {
        /// <summary>
        /// 基岩版Motd获取入口
        /// </summary>
        /// <param name="addr">地址</param>
        public Motdpe(string addr)
        {
            if (!TryParse(addr))
            {
                return;
            }
            if (Port == -1)
            {
                Port = 19132;
            }
            TryGet();
        }

        /// <summary>
        /// 基岩版Motd获取入口（本地）
        /// </summary>
        /// <param name="port">端口</param>
        public Motdpe(int port)
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
            int length = 0;
            string data;
            using (Socket socket = new(IP.AddressFamily, SocketType.Dgram, ProtocolType.Udp))
            {
                socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReceiveTimeout, 1000);
                socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.SendTimeout, 1000);
                socket.Bind(new IPEndPoint(IPAddress.Any, 0));
                string[] guid = Guid.NewGuid().ToString().ToUpperInvariant().Split('-');
                string text = $"01{Environment.TickCount.ToString("X").PadLeft(16, '0')}00FFFF00FEFEFEFEFDFDFDFD12345678{guid[2]}{guid[4]}";
                byte[] sendBytes = new byte[text.Length / 2];
                for (int i = 0; i < text.Length / 2; i++)
                {
                    sendBytes[i] = Convert.ToByte(text.Substring(i * 2, 2), 16);
                }
                DateTime startTime = DateTime.Now;
                socket.SendTo(sendBytes, new IPEndPoint(IP, Port));
                EndPoint endPoint = new IPEndPoint(IPAddress.Any, 0);
                byte[] buffer = new byte[1024 * 8];
                length = socket.ReceiveFrom(buffer, ref endPoint);
                Latency = (DateTime.Now - startTime).TotalMilliseconds;
                data = length > 35 ?
                    Encoding.UTF8.GetString(buffer, 35, length - 35) :
                    Encoding.UTF8.GetString(buffer, 0, length);
            }

            if (length > 35)
            {
                Origin = data;
                string[] datas = data.Split(';');
                if (datas.Length >= 12)
                {
                    Description = System.Text.RegularExpressions.Regex.Replace(datas[1], "§.", string.Empty);
                    Protocol = datas[2];
                    Version = datas[3];
                    OnlinePlayer = long.Parse(datas[4]);
                    MaxPlayer = long.Parse(datas[5]);
                    LevelName = datas[7];
                    GameMode = datas[8];
                    IsSuccessful = true;
                }
            }
        }
    }
}
