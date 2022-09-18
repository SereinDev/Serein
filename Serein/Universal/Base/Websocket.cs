﻿using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WebSocket4Net;

namespace Serein.Base
{
    public class Websocket
    {
        public static bool Status = false;
        private static bool Reconnect = false;
        private static WebSocket WSClient;
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
                Logger.Out(20, "#clear");
                Message.MessageReceived = "-";
                Message.MessageSent = "-";
                Message.SelfId = "-";
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
                    WSClient.MessageReceived += new EventHandler<MessageReceivedEventArgs>(Recieve);
                    WSClient.Error += (sender, e) =>
                    {
                        Logger.Out(24, e.Exception.Message);
                    };
                    WSClient.Closed += (sender, e) =>
                    {
                        Status = false;
                        Logger.Out(20, "");
                        Logger.Out(21, "WebSocket连接已断开");
                        if (Global.Settings.Bot.AutoReconnect && Reconnect)
                        {
                            Task.Run(() =>
                            {
                                Logger.Out(21, "将于10秒后（{DateTime.Now.AddSeconds(10):T}）尝试重新连接");
                                Logger.Out(21, "你可以按下断开按钮来取消重启");
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
                        Logger.Out(21, $"连接到{Global.Settings.Bot.Uri}");
                    };
                    WSClient.Open();
                    StartTime = DateTime.Now;
                    Status = true;
                }
                catch (Exception e)
                {
                    Logger.Out(24, e.Message);
                }
            }
        }

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
                JObject TextJObject = new JObject();
                JObject ParamsJObject = new JObject();
                TextJObject.Add("action", IsPrivate ? "send_private_msg" : "send_group_msg");
                ParamsJObject.Add(IsPrivate ? "user_id" : "group_id", Target_Long);
                ParamsJObject.Add("message", Message);
                ParamsJObject.Add("auto_escape", Global.Settings.Bot.AutoEscape && AutoEscape);
                TextJObject.Add("params", ParamsJObject);
                WSClient.Send(TextJObject.ToString());
                if (Global.Settings.Bot.EnbaleOutputData)
                {
                    Logger.Out(23, TextJObject.ToString());
                }
                else
                {
                    Logger.Out(23, $"{(IsPrivate ? "私聊" : "群聊")}({Target}):{Message}");
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
                Reconnect = false;
                WSClient.Close();
            }
            else if (Reconnect)
            {
                Reconnect = false;
                Logger.Out(21, "重启已取消");
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
        public static void Recieve(object sender, MessageReceivedEventArgs e)
        {
            if (Global.Settings.Bot.EnbaleOutputData)
            {
                Logger.Out(22, e.Message);
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
                        $"{DateTime.Now:T}  {Log.OutputRecognition(e.Message)}\n",
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