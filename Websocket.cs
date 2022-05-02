using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.IO;
using System.Text;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Forms;
using System.Net.NetworkInformation;
using System.Net;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using WebSocketSharp;

namespace Serein
{
    public class Websocket
    {
        public static bool Status = false;
        public static WebSocket webSocket;
        static StreamWriter LogWriter;
        public static void Connect()
        {
            if (Status)
            {
                MessageBox.Show(":(\nWebsocket已连接.", "Serein", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else if (!CheckPort(Global.Settings_bot.Port))
            {
                MessageBox.Show(":(\nWebsocket目标端口未开启.", "Serein", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                Global.Ui.BotWebBrowser_Invoke("#clear");
                Status = true;
                webSocket = new WebSocket($"ws://127.0.0.1:{Global.Settings_bot.Port}");
                webSocket.OnMessage += Recieve;
                webSocket.OnError += (sender, e) =>
                {
                    Global.Ui.BotWebBrowser_Invoke(
                        "<span style=\"color:#BA4A00;font-weight: bold;\">[×]</span>" +
                        e.Message);
                };
                webSocket.OnClose += (sender, e) =>
                {
                    Status = false;
                    Global.Ui.BotWebBrowser_Invoke("<span style=\"color:#4B738D;font-weight: bold;\">[Serein]</span>WebSocket连接已断开");
                };
                webSocket.OnOpen += (sender, e) =>
                {
                    Global.Ui.BotWebBrowser_Invoke($"<span style=\"color:#4B738D;font-weight: bold;\">[Serein]</span>连接到ws://127.0.0.1:{Global.Settings_bot.Port}");
                };
                webSocket.ConnectAsync();
            }
        }
        public static void Send(bool IsPrivate, string Message, string Target)
        {
            if (Status)
            {
                int IntTarget = int.TryParse(Target, out int t) ? t : -1;
                JObject TextJObject = new JObject();
                JObject ParamsJObject = new JObject();
                if (IsPrivate)
                {
                    TextJObject.Add("action", "send_private_msg");
                    ParamsJObject.Add("user_id", IntTarget);
                    ParamsJObject.Add("message", Message);
                    TextJObject.Add("params", ParamsJObject);
                }
                else
                {
                    TextJObject.Add("action", "send_group_msg");
                    ParamsJObject.Add("group_id", IntTarget);
                    ParamsJObject.Add("message", Message);
                    TextJObject.Add("params", ParamsJObject);
                }
                webSocket.SendAsync(
                    TextJObject.ToString(),
                    (Sent) => {
                        if (Sent)
                        {
                            Global.Ui.BotWebBrowser_Invoke(
                           "<span style=\"color:#2874A6;font-weight: bold;\">[↑]</span>" +
                           TextJObject.ToString());
                        }
                    }
                    );
            }
        }
        public static void Close()
        {
            if (Status)
            {
                webSocket.CloseAsync();
            }
            else
            {
                MessageBox.Show(":(\nWebsocket未连接.", "Serein", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        public static void Recieve(object sender, MessageEventArgs e)
        {
            Global.Ui.BotWebBrowser_Invoke(
                "<span style=\"color:#239B56;font-weight: bold;\">[↓]</span>" +
                e.Data);
            if (Global.Settings_bot.EnableLog)
            {
                if (!Directory.Exists(Global.Path + "\\logs\\msg"))
                {
                    Directory.CreateDirectory(Global.Path + "\\logs\\msg");
                }
                try
                {
                    LogWriter = new StreamWriter(
                    Global.Path + $"\\logs\\msg\\{DateTime.Now:yyyy-MM-dd}.log",
                    true,
                    Encoding.UTF8
                    );
                    LogWriter.WriteLine($"{DateTime.Now.TimeOfDay}  {Log.OutputRecognition(e.Data)}");
                    LogWriter.Flush();
                    LogWriter.Close();
                }
                catch { }
                Message.ProcessMsgFromBot(e.Data);
            }
        }
        private static bool CheckPort(int port)
        {
            IPGlobalProperties iPGlobalProperties = IPGlobalProperties.GetIPGlobalProperties();
            IPEndPoint[] iPEndPoints = iPGlobalProperties.GetActiveTcpListeners();
            foreach (IPEndPoint iPEndPoint in iPEndPoints)
            {
                if (iPEndPoint.Port == port)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
