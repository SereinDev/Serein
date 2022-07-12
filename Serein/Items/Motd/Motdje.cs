using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Serein.Items.Motd
{
    internal class Motdje : Motd
    {
        public Motdje(string newip = "127.0.0.1", string newPort = "25565")
        {
            Init(newip, newPort);
            string Data = string.Empty;
            DateTime StartTime = DateTime.Now;
            try
            {
                Socket client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReceiveTimeout, 5000);
                client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.SendTimeout, 5000);
                client.Connect(
                    new IPEndPoint(
                        IP,
                        Port
                        )
                    );
                StartTime = DateTime.Now;
                client.Send(
                    new byte[] { 6, 0, 0, 0, 0x63, 0xdd, 1, 1, 0 }
                    );
                byte[] buffer = new byte[1024 * 1024];
                int length = client.Receive(buffer);
                Delay = DateTime.Now - StartTime;
                Data = length > 5 ?
                    Encoding.UTF8.GetString(buffer, 5, length - 5) :
                    Encoding.UTF8.GetString(buffer, 0, length);
                client.Close();
                Original = Data;
                Global.Debug($"[Motdje] Original: {Data}");
            }
            catch (Exception e)
            {
                Global.Debug($"[Motdje] {e.Message}");
                Exception = e.Message;
                return;
            }
            if (Data != string.Empty)
            {
                try
                {
                    JObject JsonObject = (JObject)JsonConvert.DeserializeObject(Data);
                    OnlinePlayer = JsonObject["players"]["online"].ToString();
                    MaxPlayer = JsonObject["players"]["max"].ToString();
                    Version = JsonObject["version"]["name"].ToString();
                    Protocol = JsonObject["version"]["protocol"].ToString();
                    if (JsonObject["description"]["text"] != null)
                    {
                        Description = JsonObject["description"]["text"].ToString();
                    }
                    else if (JsonObject["description"]["extra"] != null)
                    {
                        Description = string.Empty;
                        foreach (JObject ChildrenJObject in (JArray)JsonObject["description"]["extra"])
                        {
                            Description += ChildrenJObject["text"].ToString();
                        }
                    }
                    Description = Regex.Replace(Regex.Unescape(Description), "§.", string.Empty);
                    Success = true;
                }
                catch (Exception e)
                {
                    Global.Debug($"[Motdje] {e.Message}");
                    Exception = e.Message;
                }
            }
        }
    }
}
