using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Serein.Items;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using WebSocket4Net;

namespace Serein.Base
{
    public class Websocket
    {
        /// <summary>
        /// 连接状态
        /// </summary>
        public static bool Status = false;

        /// <summary>
        /// 重连状态
        /// </summary>
        private static bool Reconnect = false;

        /// <summary>
        /// WS客户端
        /// </summary>
        private static WebSocket WSClient;

        /// <summary>
        /// 启动时刻
        /// </summary>
        public static DateTime StartTime = DateTime.Now;

        /// <summary>
        /// 连接WS
        /// </summary>
        /// <param name="ExecutedByUser">被用户执行</param>
        public static void Connect(bool ExecutedByUser = true)
        {
            if (ExecutedByUser && Status)
            {
                Logger.MsgBox(":(\nWebsocket已连接", "Serein", 0, 48);
            }
            else if (ExecutedByUser && Reconnect)
            {
                Logger.MsgBox(":(\n请先结束重启倒计时", "Serein", 0, 48);
            }
            else if (!Status)
            {
                Logger.Out(LogType.Bot_Clear);
                Matcher.MessageReceived = "-";
                Matcher.MessageSent = "-";
                Matcher.SelfId = "-";
                try
                {
                    WSClient = new WebSocket(
                        "ws://" + Global.Settings.Bot.Uri,
                        "",
                        null,
                        new List<KeyValuePair<string, string>> {
                            new KeyValuePair<string, string>("Authorization", Global.Settings.Bot.Authorization)
                            }
                        );
                    WSClient.MessageReceived += Receive;
                    WSClient.Error += (sender, e) =>
                    {
                        Logger.Out(LogType.Bot_Error, e.Exception.Message);
                    };
                    WSClient.Closed += (sender, e) =>
                    {
                        Status = false;
                        Logger.Out(LogType.Bot_Output);
                        Logger.Out(LogType.Bot_Notice, "WebSocket连接已断开");
                        if (Global.Settings.Bot.AutoReconnect && Reconnect)
                        {
                            System.Threading.Tasks.Task.Run(() =>
                            {
                                Logger.Out(LogType.Bot_Notice, "将于10秒后（{DateTime.Now.AddSeconds(10):T}）尝试重新连接");
                                Logger.Out(LogType.Bot_Notice, "你可以按下断开按钮来取消重启");
                                for (int i = 0; i < 20; i++)
                                {
                                    Thread.CurrentThread.Join(500);
                                    if (!Reconnect || Status)
                                    {
                                        break;
                                    }
                                }
                                if (Reconnect && !Status)
                                {
                                    Connect(false);
                                }
                            });
                        }
                    };
                    WSClient.Opened += (sender, e) =>
                    {
                        Reconnect = true;
                        Logger.Out(LogType.Bot_Notice, $"连接到{Global.Settings.Bot.Uri}");
                    };
                    WSClient.Open();
                    StartTime = DateTime.Now;
                    Status = true;
                }
                catch (Exception e)
                {
                    Logger.Out(LogType.Bot_Error, e.Message);
                }
            }
        }

        /// <summary>
        /// 发送文本
        /// </summary>
        /// <param name="Msg">文本</param>
        /// <returns>是否成功发送</returns>
        public static bool Send(string Msg)
        {
            if (Status)
            {
                WSClient.Send(Msg);
            }
            return Status;
        }

        /// <summary>
        /// 发送数据包
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
                JObject ParamsJObject = new JObject
                {
                    { IsPrivate ? "user_id" : "group_id", Target_Long },
                    { "message", Message },
                    { "auto_escape", Global.Settings.Bot.AutoEscape && AutoEscape }
                };
                JObject TextJObject = new JObject
                {
                    { "action", IsPrivate ? "send_private_msg" : "send_group_msg" },
                    { "params", ParamsJObject }
                };
                WSClient.Send(TextJObject.ToString());
                if (Global.Settings.Bot.EnbaleOutputData)
                    Logger.Out(LogType.Bot_Send, TextJObject.ToString());
                else
                    Logger.Out(LogType.Bot_Send, $"{(IsPrivate ? "私聊" : "群聊")}({Target}):{Message}");
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
                Reconnect = false;
                WSClient.Close();
            }
            else if (Reconnect)
            {
                Reconnect = false;
                Logger.Out(LogType.Bot_Notice, "重启已取消");
            }
            else
            {
                Logger.MsgBox(":(\nWebsocket未连接", "Serein", 0, 48);
            }
        }

        /// <summary>
        /// 消息接收处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e">消息接收事件参数</param>
        public static void Receive(object sender, MessageReceivedEventArgs e)
        {
            if (Global.Settings.Bot.EnbaleOutputData)
                Logger.Out(LogType.Bot_Receive, e.Message);
            if (Global.Settings.Bot.EnableLog)
            {
                if (!Directory.Exists(IO.GetPath("logs", "msg")))
                    Directory.CreateDirectory(IO.GetPath("logs", "msg"));
                try
                {
                    File.AppendAllText(
                        IO.GetPath("logs", "msg", $"{DateTime.Now:yyyy-MM-dd}.log"),
                        $"{DateTime.Now:T}  {Log.OutputRecognition(e.Message)}\n",
                        Encoding.UTF8
                        );
                }
                catch { }
            }
            try
            {
                Matcher.Process((JObject)JsonConvert.DeserializeObject(WebUtility.HtmlDecode(DeUnicode(e.Message))));
            }
            catch { }
        }

        /// <summary>
        /// 处理Unicode转义
        /// </summary>
        /// <param name="str">文本</param>
        /// <returns>处理后文本</returns>
        public static string DeUnicode(string str)
        {
            System.Text.RegularExpressions.Regex reg = new System.Text.RegularExpressions.Regex(@"(?i)\\[uU]([0-9a-f]{4})");
            return reg.Replace(str, delegate (Match m) { return ((char)Convert.ToInt32(m.Groups[1].Value, 16)).ToString(); });
        }
    }
}
