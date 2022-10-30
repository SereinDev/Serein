using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
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
        /// <param name="NewIp">IP</param>
        /// <param name="NewPort">端口</param>
        public Motdje(string NewIP = "127.0.0.1", string NewPort = "25565")
        {
            if (!Init(NewIP, NewPort))
                return;
            try
            {
                Socket Client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReceiveTimeout, 5000);
                Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.SendTimeout, 5000);
                Client.Connect(
                    new IPEndPoint(
                        IP,
                        Port
                        )
                    );
                DateTime StartTime = DateTime.Now;
                Client.Send(
                    new byte[] { 6, 0, 0, 0, 0x63, 0xdd, 1, 1, 0 }
                    );
                byte[] Buffer = new byte[1024 * 1024];
                int Length = Client.Receive(Buffer);
                Delay = DateTime.Now - StartTime;
                string Data = Length > 5 ?
                    Encoding.UTF8.GetString(Buffer, 5, Length - 5) :
                    Encoding.UTF8.GetString(Buffer, 0, Length);
                Client.Close();
                Original = Data;
                Logger.Out(LogType.Debug, "[Motdje]", $"Original: {Data}");
                if (string.IsNullOrEmpty(Data))
                {
                    JObject JsonObject = (JObject)JsonConvert.DeserializeObject(Data);
                    OnlinePlayer = JsonObject["players"]["online"].ToString();
                    MaxPlayer = JsonObject["players"]["max"].ToString();
                    Version = JsonObject["version"]["name"].ToString();
                    Protocol = JsonObject["version"]["protocol"].ToString();
                    if (JsonObject["description"]["text"] != null)
                        Description = JsonObject["description"]["text"].ToString();
                    if (JsonObject["description"]["extra"] != null)
                    {
                        Description = string.Empty;
                        foreach (JObject ChildrenJObject in JsonObject["description"]["extra"])
                        {
                            Description += ChildrenJObject["text"].ToString();
                        }
                    }
                    if (JsonObject["favicon"] != null)
                    {
                        Favicon = (string)JsonObject["favicon"];
                        if (Favicon.Contains(","))
                            Favicon = $"[CQ:image,file=base64://{Favicon.Substring(Favicon.IndexOf(',') + 1)}]";
                        else
                            Favicon = string.Empty;
                    }
                    Description = System.Text.RegularExpressions.Regex.Replace(System.Text.RegularExpressions.Regex.Unescape(Description), "§.", string.Empty);
                    Success = true;
                }
            }
            catch (Exception e)
            {
                Logger.Out(LogType.Debug, "[Motdje]", e.ToString());
                Exception = e.Message;
                return;
            }
        }
    }
}
