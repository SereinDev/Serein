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
        public static DateTime StartTime = DateTime.Now;
        private static bool Restart = false;

        /// <summary>
        /// 连接WS
        /// </summary>
        /// <param name="ExecutedByUser">被用户执行</param>
        public static void Connect(bool ExecutedByUser = true)
        {
            if (ExecutedByUser && Status)
            {
                MessageBox.Show(":(\nWebsocket已连接.", "Serein", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else if (ExecutedByUser && Restart)
            {
                MessageBox.Show(":(\n请先结束重启倒计时.", "Serein", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else if (!Status)
            {
                Global.Logger(20,"#clear");
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
                        Global.Logger(24,e.Exception.Message);
                    };
                    webSocket.Closed += (sender, e) =>
                    {
                        Status = false;
                        Global.Logger(20,"");
                        Global.Logger(21,"WebSocket连接已断开");
                        if (Global.Settings.Bot.Restart && Restart)
                        {
                            Task.Run(() =>
                            {
                                Global.Logger(21,"将于10秒后（{DateTime.Now.AddSeconds(10):T}）尝试重新连接");
                                Global.Logger(21,"你可以按下断开按钮来取消重启");
                                for (int i = 0; i < 20; i++)
                                {
                                    Thread.CurrentThread.Join(500);
                                    if (!Restart || Status)
                                    {
                                        break;
                                    }
                                }
                                if (Restart && !Status)
                                {
                                    Connect(false);
                                }
                            });
                        }
                    };
                    webSocket.Opened += (sender, e) =>
                    {
                        Restart = true;
                        Global.Logger(21, $"连接到{Global.Settings.Bot.Uri}");
                    };
                    webSocket.Open();
                    StartTime = DateTime.Now;
                    Status = true;
                }
                catch (Exception e)
                {
                    Global.Logger(24,e.Message);
                }
            }
        }

        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="IsPrivate">是否私聊消息</param>
        /// <param name="Message">消息内容</param>
        /// <param name="Target">目标对象</param>
        /// <param name="AutoEscape">纯文本发送</param>
        /// <returns>发送结果</returns>
        public static bool Send(bool IsPrivate, string Message, object Target, bool AutoEscape = true)
        {
            if (Status)
            {
                long Target_Long = long.TryParse(Target.ToString(), out long t) ? t : -1;
                JObject TextJObject = new JObject();
                JObject ParamsJObject = new JObject();
                TextJObject.Add("action", IsPrivate ? "send_private_msg" : "send_group_msg");
                ParamsJObject.Add(IsPrivate ? "user_id" : "group_id", Target_Long);
                ParamsJObject.Add("message", Message);
                ParamsJObject.Add("auto_escape", Global.Settings.Bot.AutoEscape && AutoEscape);
                TextJObject.Add("params", ParamsJObject);
                webSocket.Send(TextJObject.ToString());
                if (Global.Settings.Bot.EnbaleOutputData)
                {
                    Global.Logger(23, TextJObject.ToString());
                }
                else
                {
                    Global.Logger(23, $"{(IsPrivate ? "私聊" : "群聊")}({Target}):{Message}");
                }
            }
            return Status;
        }

        /// <summary>
        /// 断开WS
        /// </summary>
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
                Global.Logger(21,"重启已取消.");
            }
            else
            {
                MessageBox.Show(":(\nWebsocket未连接.", "Serein", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        /// <summary>
        /// 消息接收处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e">消息接收事件参数</param>
        public static void Recieve(object sender, MessageReceivedEventArgs e)
        {
            if (Global.Settings.Bot.EnbaleOutputData)
            {
                Global.Logger(22, e.Message);
            }
            if (Global.Settings.Bot.EnableLog)
            {
                if (!Directory.Exists(Global.Path + "\\logs\\msg"))
                {
                    Directory.CreateDirectory(Global.Path + "\\logs\\msg");
                }
                try
                {
                    File.AppendAllText(
                        Global.Path + $"\\logs\\msg\\{DateTime.Now:yyyy-MM-dd}.log",
                        $"{DateTime.Now.TimeOfDay}  {Log.OutputRecognition(e.Message)}",
                        Encoding.UTF8
                        );
                }
                catch { }
            }
            try
            {
                Message.ProcessMsgFromBot(e.Message);
            }
            catch { }
        }
    }
}
