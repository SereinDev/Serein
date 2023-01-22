using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Serein.Extensions;
using Serein.Items;
using Serein.JSPlugin;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using WebSocket4Net;

namespace Serein.Base
{
    internal static class Websocket
    {
        /// <summary>
        /// 连接状态
        /// </summary>
        public static bool Status;

        /// <summary>
        /// 重连状态
        /// </summary>
        private static bool Reconnect;

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
        /// <param name="executedByUser">被用户执行</param>
        public static void Connect(bool executedByUser = true)
        {
            if (executedByUser && Status)
            {
                Logger.MsgBox("Websocket已连接", "Serein", 0, 48);
            }
            else if (!Status)
            {
                Logger.Output(LogType.Bot_Clear);
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
                    WSClient.Error += (_, e) =>
                    {
                        Logger.Output(LogType.Bot_Error, e.Exception.Message);
                    };
                    WSClient.Closed += (_, _) =>
                    {
                        Status = false;
                        Logger.Output(LogType.Bot_Notice, "WebSocket连接已断开");
                        if (Global.Settings.Bot.AutoReconnect && Reconnect)
                        {
                            System.Threading.Tasks.Task.Run(() =>
                            {
                                Logger.Output(LogType.Bot_Notice, $"将于10秒后（{DateTime.Now.AddSeconds(10):T}）尝试重新连接");
                                Logger.Output(LogType.Bot_Notice, "你可以按下断开按钮来取消重连");
                                for (int i = 0; i < 20; i++)
                                {
                                    500.ToSleepFor();
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
                    WSClient.Opened += (_, _) =>
                    {
                        Reconnect = true;
                        Logger.Output(LogType.Bot_Notice, $"成功连接到{Global.Settings.Bot.Uri}");
                    };
                    WSClient.Open();
                    StartTime = DateTime.Now;
                    Status = true;
                }
                catch (Exception e)
                {
                    Logger.Output(LogType.Bot_Error, e.Message);
                }
            }
        }

        /// <summary>
        /// 发送文本
        /// </summary>
        /// <param name="text">文本</param>
        /// <returns>是否成功发送</returns>
        public static bool Send(string text)
        {
            if (Status)
            {
                WSClient.Send(text);
            }
            return Status;
        }

        /// <summary>
        /// 发送数据包
        /// </summary>
        /// <param name="isPrivate">是否私聊消息</param>
        /// <param name="message">消息内容</param>
        /// <param name="target">目标对象</param>
        /// <param name="canBeEscaped">纯文本发送</param>
        /// <returns>发送结果</returns>
        public static bool Send(bool isPrivate, string message, object target, bool canBeEscaped = true)
        {
            if (Status)
            {
                long targetNumber = long.TryParse(target.ToString(), out long result) ? result : -1;
                JObject textJObject = new()
                {
                    { "action", isPrivate ? "send_private_msg" : "send_group_msg" },
                    {
                        "params",
                        new JObject
                        {
                            { isPrivate ? "user_id" : "group_id", targetNumber },
                            { "message", message },
                            { "auto_escape", canBeEscaped && Global.Settings.Bot.AutoEscape }
                        }
                    }
                };
                WSClient.Send(textJObject.ToString());
                if (Global.Settings.Bot.EnbaleOutputData)
                {
                    Logger.Output(LogType.Bot_Send, textJObject);
                }
                else
                {
                    Logger.Output(LogType.Bot_Send, $"{(isPrivate ? "私聊" : "群聊")}({target}):{message}");
                }
                IO.MsgLog($"[Sent] {textJObject}");
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
                Logger.Output(LogType.Bot_Notice, "重连已取消");
            }
            else
            {
                Logger.MsgBox("Websocket未连接", "Serein", 0, 48);
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
            {
                Logger.Output(LogType.Bot_Receive, e.Message);
            }
            IO.MsgLog($"[Received] {e.Message}");
            try
            {
                System.Threading.Tasks.Task.Run(() => JSFunc.Trigger(EventType.ReceivePacket, e.Message));
                Matcher.Process((JObject)JsonConvert.DeserializeObject(
                    WebUtility.HtmlDecode(
                        new System.Text.RegularExpressions.Regex(@"(?i)\\[uU]([0-9a-f]{4})").Replace(e.Message, match => ((char)Convert.ToInt32(match.Groups[1].Value, 16)).ToString())
                )));
            }
            catch (Exception exception)
            {
                Logger.Output(LogType.Debug, exception);
            }
        }
    }
}
