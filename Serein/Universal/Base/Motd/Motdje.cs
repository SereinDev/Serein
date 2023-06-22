using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
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
        /// <summary>
        /// Java版Motd获取入口
        /// </summary>
        /// <param name="addr">地址</param>
        public Motdje(string addr)
        {
            if (!base.TryParse(addr))
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
            using (Socket socket = new(IP.AddressFamily, SocketType.Stream, ProtocolType.Tcp))
            {
                socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReceiveTimeout, 1000);
                socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.SendTimeout, 1000);
                socket.Connect(new IPEndPoint(IP, Port));
                DateTime startTime = DateTime.Now;
                socket.Send(new byte[] { 6, 0, 0, 0, 0x63, 0xdd, 1, 1, 0 });
                while (true)
                {
                    try
                    {
                        byte[] buffer = new byte[1024 * 8];
                        int length = socket.Receive(buffer);
                        if (length == 0)
                        {
                            break; // 空包
                        }
                        Array.Copy(buffer, 0, datas, totalLength, length);
                        totalLength += length;
                        Origin = totalLength > 5 ?
                            Encoding.UTF8.GetString(datas, 5, totalLength - 5) :
                            Encoding.UTF8.GetString(datas, 0, totalLength);
                        Logger.Output(LogType.Debug, length);
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
                Delay = (DateTime.Now - startTime).TotalMilliseconds;
            }
            Origin = totalLength > 5 ?
                Encoding.UTF8.GetString(datas, 5, totalLength - 5) :
                Encoding.UTF8.GetString(datas, 0, totalLength);
            Logger.Output(LogType.Debug, $"Origin: {Origin}");
            if (!string.IsNullOrEmpty(Origin))
            {
                JObject jsonObject = (JObject)JsonConvert.DeserializeObject(Origin)!;
                OnlinePlayer = long.TryParse(jsonObject.SelectToken("players.online")?.ToString(), out long number) ? number : -1;
                MaxPlayer = long.TryParse(jsonObject.SelectToken("players.max")?.ToString(), out number) ? number : -1;
                Version = jsonObject.SelectToken("version.name")?.ToString();
                Protocol = jsonObject.SelectToken("version.protocol")?.ToString();
                if (jsonObject.SelectToken("description.text") != null)
                {
                    Description = jsonObject.SelectToken("description.text")?.ToString() ?? string.Empty;
                    Description = System.Text.RegularExpressions.Regex.Replace(System.Text.RegularExpressions.Regex.Unescape(Description), "§.", string.Empty);
                }
                if (jsonObject.SelectToken("description.extra")?.HasValues ?? false)
                {
                    Description = string.Empty;
                    foreach (JObject childrenJObject in jsonObject.SelectToken("description.extra")!)
                    {
                        Description += childrenJObject["text"]?.ToString();
                    }
                }
                if (jsonObject["favicon"] != null)
                {
                    Favicon = jsonObject["favicon"]?.ToString();
                    if (!string.IsNullOrEmpty(Favicon) && Favicon!.Contains(","))
                    {
                        Favicon = $"[CQ:image,file=base64://{Favicon.Substring(Favicon.IndexOf(',') + 1)}]";
                    }
                }
                IsSuccessful = true;
            }
        }
    }
}
