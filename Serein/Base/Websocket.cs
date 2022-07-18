using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using WebSocket4Net;

namespace Serein.Base
{
    public class Websocket
    {
        public static bool Status = false;
        public static WebSocket webSocket;
        private static StreamWriter LogWriter;
        public static DateTime StartTime = DateTime.Now;
        private static bool Restart = false;
        public static void Connect(bool ExecutedByUser=true)
        {
            if (ExecutedByUser&&Status)
            {
                MessageBox.Show(":(\nWebsocket已连接.", "Serein", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else if (ExecutedByUser && Restart)
            {
                MessageBox.Show(":(\n请先结束重启倒计时.", "Serein", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else if(!Status)
            {
                Global.Ui.BotWebBrowser_Invoke("#clear");
                Message.MessageReceived = "-";
                Message.MessageSent = "-";
                Message.SelfId = "-";
                try
                {
                    webSocket = new WebSocket(
                        "ws://" + Global.Settings.Bot.Uri,
                        "",
                        null,
                        new List<KeyValuePair<string, string>> {
                            new KeyValuePair<string, string>("Authorization", Global.Settings.Bot.Authorization)
                            }
                        );
                    webSocket.MessageReceived += new EventHandler<MessageReceivedEventArgs>(Recieve);
                    webSocket.Error += (sender, e) =>
                    {
                        Global.Ui.BotWebBrowser_Invoke(
                            "<span style=\"color:#BA4A00;font-weight: bold;\">[×]</span>" +
                            Log.EscapeLog(e.Exception.Message));
                    };
                    webSocket.Closed += (sender, e) =>
                    {
                        Status = false;
                        Global.Ui.BotWebBrowser_Invoke("<br><span style=\"color:#4B738D;font-weight: bold;\">[Serein]</span>WebSocket连接已断开");
                        if (Global.Settings.Bot.Restart&&Restart)
                        {
                            Task.Run(() => 
                            {
                                Global.Ui.BotWebBrowser_Invoke($"<span style=\"color:#4B738D;font-weight: bold;\">[Serein]</span>将于10秒后（{DateTime.Now.AddSeconds(10):T}）尝试重新连接");
                                Global.Ui.BotWebBrowser_Invoke($"<span style=\"color:#4B738D;font-weight: bold;\">[Serein]</span>你可以按下断开按钮来取消重启");
                                for (int i = 0; i < 20; i++)
                                {
                                    Thread.CurrentThread.Join(500);
                                    if (!Restart|| Status)
                                    {
                                        break;
                                    }
                                }
                                if (Restart&&!Status)
                                {
                                    Connect(false);
                                }
                            });
                        }
                    };
                    webSocket.Opened += (sender, e) =>
                    {
                        Restart = true;
                        Global.Ui.BotWebBrowser_Invoke($"<span style=\"color:#4B738D;font-weight: bold;\">[Serein]</span>连接到{Global.Settings.Bot.Uri}");
                    };
                    webSocket.Open();
                    StartTime = DateTime.Now;
                    Status = true;
                }
                catch (Exception e)
                {
                    Global.Ui.BotWebBrowser_Invoke(
                            "<span style=\"color:#BA4A00;font-weight: bold;\">[×]</span>" + e.Message
                            );
                }
            }
        }
        public static void Send(bool IsPrivate, string Message, object Target)
        {
            if (Status)
            {
                long Target_Long = long.TryParse(Target.ToString(), out long t) ? t : -1;
                JObject TextJObject = new JObject();
                JObject ParamsJObject = new JObject();
                if (IsPrivate)
                {
                    TextJObject.Add("action", "send_private_msg");
                    ParamsJObject.Add("user_id", Target_Long);
                    ParamsJObject.Add("message", Message);
                    TextJObject.Add("params", ParamsJObject);
                }
                else
                {
                    TextJObject.Add("action", "send_group_msg");
                    ParamsJObject.Add("group_id", Target_Long);
                    ParamsJObject.Add("message", Message);
                    TextJObject.Add("params", ParamsJObject);
                }
                webSocket.Send(TextJObject.ToString());
                if (Global.Settings.Bot.EnbaleOutputData)
                {
                    Global.Ui.BotWebBrowser_Invoke(
                        "<span style=\"color:#2874A6;font-weight: bold;\">[↑]</span>" +
                        Log.EscapeLog(TextJObject.ToString())
                        );
                }
                else
                {
                    Global.Ui.BotWebBrowser_Invoke(
                        "<span style=\"color:#2874A6;font-weight: bold;\">[↑]</span>" +
                        Log.EscapeLog($"{(IsPrivate ? "私聊" : "群聊")}({Target}):{Message}")
                        );
                }
            }
        }
        public static void Close()
        {
            if (Status)
            {
                Restart = false;
                webSocket.Close();
            }
            else if (Restart)
            {
                Restart = false;
                Global.Ui.BotWebBrowser_Invoke($"<span style=\"color:#4B738D;font-weight: bold;\">[Serein]</span>重启已取消.");
            }
            else
            {
                MessageBox.Show(":(\nWebsocket未连接.", "Serein", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        public static void Recieve(object sender, MessageReceivedEventArgs e)
        {
            if (Global.Settings.Bot.EnbaleOutputData)
            {
                Global.Ui.BotWebBrowser_Invoke(
                    "<span style=\"color:#239B56;font-weight: bold;\">[↓]</span>" +
                    Log.EscapeLog(e.Message)
                    );
            }
            if (Global.Settings.Bot.EnableLog)
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
                    LogWriter.WriteLine($"{DateTime.Now.TimeOfDay}  {Log.OutputRecognition(e.Message)}");
                    LogWriter.Flush();
                    LogWriter.Close();
                }
                catch { }
            }
            new Task(() => Message.ProcessMsgFromBot(e.Message)).Start();
        }
    }
}
