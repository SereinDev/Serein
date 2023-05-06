using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Serein.Base;
using Serein.Core.JSPlugin;
using Serein.Extensions;
using Serein.Utils;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using WebSocket4Net;

namespace Serein.Core.Generic
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
        public static void Open()
        {
            if (Status)
            {
                Utils.Logger.MsgBox("Websocket已连接", "Serein", 0, 48);
                return;
            }
            Utils.Logger.Output(LogType.Bot_Clear);
            Matcher.MessageReceived = null;
            Matcher.MessageSent = null;
            Matcher.SelfId = null;
            try
            {
                WSClient = new(
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
                    Utils.Logger.Output(LogType.Bot_Error, e.Exception.Message);
                };
                WSClient.Closed += Closed;
                WSClient.Opened += (_, _) =>
                {
                    Reconnect = true;
                    Utils.Logger.Output(LogType.Bot_Notice, $"成功连接到{Global.Settings.Bot.Uri}");
                };
                WSClient.Open();
                StartTime = DateTime.Now;
                Status = true;
            }
            catch (Exception e)
            {
                Utils.Logger.Output(LogType.Bot_Error, e.Message);
            }
        }

        private static void Closed(object sender, EventArgs e)
        {
            Status = false;
            Utils.Logger.Output(LogType.Bot_Notice, "WebSocket连接已断开");
            if (Global.Settings.Bot.AutoReconnect && Reconnect)
            {
                Task.Run(() =>
                {
                    Utils.Logger.Output(LogType.Bot_Notice, $"将于10秒后（{DateTime.Now.AddSeconds(10):T}）尝试重新连接");
                    Utils.Logger.Output(LogType.Bot_Notice, "你可以按下断开按钮来取消重连");
                    for (int i = 0; i < 20; i++)
                    {
                        500.ToSleep();
                        if (!Reconnect || Status)
                        {
                            break;
                        }
                    }
                    if (Reconnect && !Status)
                    {
                        Open();
                    }
                });
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
        public static bool Send(bool isPrivate, string message, string target, bool canBeEscaped = true)
            => Send(isPrivate, message, long.TryParse(target, out long targetID) ? targetID : -1, canBeEscaped);

        /// <summary>
        /// 发送数据包
        /// </summary>
        /// <param name="isPrivate">是否私聊消息</param>
        /// <param name="message">消息内容</param>
        /// <param name="target">目标对象</param>
        /// <param name="canBeEscaped">纯文本发送</param>
        /// <returns>发送结果</returns>
        public static bool Send(bool isPrivate, string message, long target, bool canBeEscaped = true)
        {
            if (Status)
            {
                JObject textJObject = new()
                {
                    {
                        "action",
                        isPrivate ? "send_private_msg" : "send_group_msg"
                    },
                    {
                        "params",
                        new JObject
                        {
                            { isPrivate ? "user_id" : "group_id", target },
                            { "message", message },
                            { "auto_escape", canBeEscaped && Global.Settings.Bot.AutoEscape }
                        }
                    }
                };
                WSClient.Send(textJObject.ToString());
                if (Global.Settings.Bot.EnbaleOutputData)
                {
                    Utils.Logger.Output(LogType.Bot_Send, textJObject);
                }
                else
                {
                    Utils.Logger.Output(LogType.Bot_Send, $"{(isPrivate ? "私聊" : "群聊")}({target}):{message}");
                }
                IO.MsgLog($"[Sent] {textJObject}");
            }
            return Status;
        }


        /// <summary>
        /// 发送数据包
        /// </summary>
        /// <param name="groupID">群号</param>
        /// <param name="userID">用户ID</param>
        /// <param name="message">消息</param>
        /// <returns>发送结果</returns>
        public static bool Send(long groupID, long userID, string message)
        {
            if (Status)
            {
                JObject textJObject = new()
                {
                    { "action", "send_private_msg" },
                    {
                        "params",
                        new JObject
                        {
                            { "user_id" , userID },
                            { "group_id" , groupID },
                            { "message", message },
                            { "auto_escape", Global.Settings.Bot.AutoEscape }
                        }
                    }
                };
                WSClient.Send(textJObject.ToString());
                if (Global.Settings.Bot.EnbaleOutputData)
                {
                    Utils.Logger.Output(LogType.Bot_Send, textJObject);
                }
                else
                {
                    Utils.Logger.Output(LogType.Bot_Send, $"\"临时会话\"({userID}):{message}");
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
                Utils.Logger.Output(LogType.Bot_Notice, "重连已取消");
            }
            else
            {
                Utils.Logger.MsgBox("Websocket未连接", "Serein", 0, 48);
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
                Utils.Logger.Output(LogType.Bot_Receive, e.Message);
            }
            IO.MsgLog($"[Received] {e.Message}");
            string packet = WebUtility.HtmlDecode(new System.Text.RegularExpressions.Regex(@"(?i)\\[uU]([0-9a-f]{4})").Replace(e.Message, match => ((char)Convert.ToInt32(match.Groups[1].Value, 16)).ToString()));
            if (JSFunc.Trigger(EventType.ReceivePacket, packet))
            {
                return;
            }
            try
            {
                Matcher.Process((JObject)JsonConvert.DeserializeObject(packet));
            }
            catch (Exception exception)
            {
                Utils.Logger.Output(LogType.Debug, exception);
            }
        }
    }
}
