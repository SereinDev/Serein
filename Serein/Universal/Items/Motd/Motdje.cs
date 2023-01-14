using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Serein.Base;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Serein.Items.Motd
{
    internal class Motdje : Motd
    {
        /// <summary>
        /// Java版Motd获取入口
        /// </summary>
        /// <param name="newIp">IP</param>
        /// <param name="newPort">端口</param>
        public Motdje(string newIP = "127.0.0.1", string newPort = "25565")
        {
            if (!Init(newIP, newPort))
            {
                return;
            }
            try
            {
                Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReceiveTimeout, 5000);
                socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.SendTimeout, 5000);
                socket.Connect(new IPEndPoint(IP, Port));
                DateTime startTime = DateTime.Now;
                socket.Send(new byte[] { 6, 0, 0, 0, 0x63, 0xdd, 1, 1, 0 });
                byte[] buffer = new byte[1024 * 1024];
                int length = socket.Receive(buffer);
                Delay = DateTime.Now - startTime;
                string Data = length > 5 ?
                    Encoding.UTF8.GetString(buffer, 5, length - 5) :
                    Encoding.UTF8.GetString(buffer, 0, length);
                socket.Close();
                Origin = Data;
                Logger.Out(LogType.Debug, $"Original: {Data}");
                if (!string.IsNullOrEmpty(Data))
                {
                    JObject jsonObject = (JObject)JsonConvert.DeserializeObject(Data);
                    OnlinePlayer = jsonObject["players"]["online"].ToString();
                    MaxPlayer = jsonObject["players"]["max"].ToString();
                    Version = jsonObject["version"]["name"].ToString();
                    Protocol = jsonObject["version"]["protocol"].ToString();
                    if (jsonObject["description"]["text"] != null)
                    {
                        Description = jsonObject["description"]["text"].ToString();
                    }
                    if (jsonObject["description"]["extra"] != null)
                    {
                        Description = string.Empty;
                        foreach (JObject childrenJObject in jsonObject["description"]["extra"])
                        {
                            Description += childrenJObject["text"].ToString();
                        }
                    }
                    if (jsonObject["favicon"] != null)
                    {
                        Favicon = jsonObject["favicon"].ToString() ?? string.Empty;
                        if (Favicon.Contains(","))
                        {
                            Favicon = $"[CQ:image,file=base64://{Favicon.Substring(Favicon.IndexOf(',') + 1)}]";
                        }
                        else
                        {
                            Favicon = string.Empty;
                        }
                    }
                    Description = System.Text.RegularExpressions.Regex.Replace(System.Text.RegularExpressions.Regex.Unescape(Description), "§.", string.Empty);
                    IsSuccessful = true;
                }
            }
            catch (Exception e)
            {
                Logger.Out(LogType.Debug, e);
                Exception = e.Message;
                return;
            }
        }
    }
}
