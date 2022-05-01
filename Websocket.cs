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
using WebSocketSharp;
namespace Serein
{
    public class Websocket
    {
        public static bool Status = false;
        public static WebSocket ws;
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
                Global.ui.BotWebBrowser_Invoke("#clear");
                Status = true;
                ws = new WebSocket($"ws://127.0.0.1:{Global.Settings_bot.Port}");
                ws.OnMessage += Recieve;
                ws.OnError += (sender, e) =>
                {
                    Global.ui.BotWebBrowser_Invoke(
                        "<span style=\"color:#BA4A00;font-weight: bold;\">[×]</span>" +
                        e.Message);
                };
                ws.OnClose += (sender, e) =>
                {
                    Status = false;
                    Global.ui.BotWebBrowser_Invoke("<span style=\"color:#4B738D;font-weight: bold;\">[Serein]</span>WebSocket连接已断开");
                };
                ws.OnOpen += (sender, e) =>
                {
                    Global.ui.BotWebBrowser_Invoke($"<span style=\"color:#4B738D;font-weight: bold;\">[Serein]</span>连接到ws://127.0.0.1:{Global.Settings_bot.Port}");
                };
                ws.ConnectAsync();
            }
        }
        public static void Send(bool isPrivate, string message, string target)
        {
            if (Status)
            {
                string text = "";
                if (isPrivate)
                {
                    text =
                    "{" +
                        "\"action\":\"send_private_msg\"," +
                        "\"params\":" +
                        "{" +
                            "\"user_id\":" + target + "," +
                            "\"message\":\"" + message + "\"" +
                        "}" +
                    "}";
                }
                else
                {
                    text =
                    "{" +
                        "\"action\":\"send_group_msg\"," +
                        "\"params\":" +
                        "{" +
                            "\"group_id\":" + target + "," +
                            "\"message\":\"" + message + "\"" +
                        "}" +
                    "}";
                }
                ws.SendAsync(
                    text,
                    (sent) => {
                        if (sent)
                        {
                            Global.ui.BotWebBrowser_Invoke(
                           "<span style=\"color:#2874A6;font-weight: bold;\">[↑]</span>" +
                           text);
                        }
                    }
                    );
            }
        }
        public static void Close()
        {
            if (Status)
            {
                ws.CloseAsync();
            }
            else
            {
                MessageBox.Show(":(\nWebsocket未连接.", "Serein", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        public static void Recieve(object sender, MessageEventArgs e)
        {
            Global.ui.BotWebBrowser_Invoke(
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
