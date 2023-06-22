using Newtonsoft.Json.Linq;
using Serein.Base.Packets;
using Serein.Core.JSPlugin;
using Serein.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Serein.Core.Generic
{
    internal static class Matcher
    {
        /// <summary>
        /// 已接收消息
        /// </summary>
        public static string MessageReceived => !Websocket.Status || string.IsNullOrEmpty(_messageReceived) ? "-" : _messageReceived!;


        /// <summary>
        /// 已发送消息
        /// </summary>
        public static string MessageSent => !Websocket.Status || string.IsNullOrEmpty(_messageSent) ? "-" : _messageSent!;

        /// <summary>
        /// 当前ID
        /// </summary>
        public static string SelfId => !Websocket.Status || string.IsNullOrEmpty(_selfId) ? "-" : _selfId!;

        private static string? _messageReceived, _messageSent, _selfId;

        /// <summary>
        /// 处理来自控制台的消息
        /// </summary>
        /// <param name="line">控制台的消息</param>
        public static void Process(string line)
        {
            lock (Global.RegexList)
            {
                foreach (Base.Regex regex in Global.RegexList)
                {
                    if (string.IsNullOrEmpty(regex.Expression) || regex.Area != 1 || !System.Text.RegularExpressions.Regex.IsMatch(line, regex.Expression))
                    {
                        continue;
                    }
                    Task.Run(() => Command.Run(Base.CommandOrigin.Console, regex.Command, System.Text.RegularExpressions.Regex.Match(line, regex.Expression)));
                }
            }
        }

        /// <summary>
        /// 处理来自机器人的消息
        /// </summary>
        /// <param name="packet">数据包</param>
        public static void Process(JObject packet)
        {
            string? postType = packet.SelectToken("post_type")?.ToString();
            if (string.IsNullOrEmpty(postType))
            {
                return;
            }
            switch (postType)
            {
                case "message":
                case "message_sent":
                    HandleMsg(packet.ToObject<Message>());
                    break;

                case "notice":
                    HandleNotice(packet.ToObject<Notice>());
                    break;

                case "meta_event":

                    break;

            }
        }

        /// <summary>
        /// 处理元事件
        /// </summary>
        /// <param name="packet">数据包</param>
        private static void HandleMetaEvent(JObject packet)
        {
            if (packet.SelectToken("meta_event_type")?.ToString() == "heartbeat")
            {
                _selfId = packet.SelectToken("self_id")?.ToString();
                _messageReceived = (packet.SelectToken("status.stat.message_received") ?? packet.SelectToken("status.stat.MessageReceived"))?.ToString();
                _messageSent = (packet.SelectToken("status.stat.message_sent") ?? packet.SelectToken("status.stat.MessageSent"))?.ToString();
                if ((long.TryParse(MessageReceived, out long tempNumber) ? tempNumber : 0) > 10000000)
                {
                    _messageReceived = (tempNumber / 10000).ToString("N1") + "w";
                }
                if ((long.TryParse(MessageSent, out tempNumber) ? tempNumber : 0) > 10000000)
                {
                    _messageSent = (tempNumber / 10000).ToString("N1") + "w";
                }
            }
        }

        /// <summary>
        /// 处理通知
        /// </summary>
        /// <param name="notice">通知数据包</param>
        private static void HandleNotice(Notice? notice)
        {
            if (notice is not null && Global.Settings.Bot.GroupList.Contains(notice.GroupId))
            {
                switch (notice.NoticeType)
                {
                    case "GroupDecrease":
                    case "group_decrease":
                        EventTrigger.Trigger(Base.EventType.GroupDecrease, notice);
                        JSFunc.Trigger(Base.EventType.GroupDecrease, notice.GroupId, notice.UserId);
                        break;

                    case "GroupIncrease":
                    case "group_increase":
                        EventTrigger.Trigger(Base.EventType.GroupIncrease, notice);
                        JSFunc.Trigger(Base.EventType.GroupIncrease, notice.GroupId, notice.UserId);
                        break;

                    case "notify":
                        if (notice.SubType == "poke" &&
                            notice.TargetId?.ToString() == SelfId)
                        {
                            EventTrigger.Trigger(Base.EventType.GroupPoke, notice);
                            JSFunc.Trigger(Base.EventType.GroupPoke, notice.GroupId, notice.UserId);
                        }
                        break;
                }
            }
        }

        /// <summary>
        /// 处理消息
        /// </summary>
        /// <param name="messagePacket">数据包</param>
        private static void HandleMsg(Message? messagePacket)
        {
            if (messagePacket is null || messagePacket?.Sender is null)
            {
                return;
            }
            bool isSelfMessage = messagePacket.MessageType == "message_sent";
            if (!isSelfMessage)
            {
                bool interdicted = false;
                if (messagePacket.MessageType == "private")
                {
                    interdicted = JSFunc.Trigger(
                        Base.EventType.ReceivePrivateMessage,
                        messagePacket.UserId,
                        messagePacket.RawMessage,
                        messagePacket.Sender.Nickname ?? string.Empty,
                        messagePacket.MessageId
                        );
                }
                else if (messagePacket.MessageType == "group")
                {
                    interdicted = JSFunc.Trigger(
                        Base.EventType.ReceiveGroupMessage,
                        messagePacket.GroupId,
                        messagePacket.UserId,
                        messagePacket.RawMessage,
                        messagePacket.Sender.Card ?? messagePacket.Sender.Nickname ?? string.Empty,
                        messagePacket.MessageId
                        );
                }
                if (interdicted)
                {
                    return;
                }
            }
            Logger.Output(Base.LogType.Bot_Receive, $"{messagePacket.Sender.Nickname ?? string.Empty}({messagePacket.UserId})" + ":" + messagePacket.RawMessage);

            if (messagePacket.MessageType == "group" ^ Global.Settings.Bot.GroupList.Contains(messagePacket.GroupId))
            {
                return;
            }

            lock (Global.RegexList)
            {
                foreach (Base.Regex regex in Global.RegexList)
                {
                    if (
                        string.IsNullOrEmpty(messagePacket?.RawMessage) ||
                        string.IsNullOrEmpty(regex.Expression) || // 表达式为空
                        regex.Area <= 1 ||  // 禁用或控制台
                        isSelfMessage ^ regex.Area == 4 || // 自身消息与定义域矛盾
                        !System.Text.RegularExpressions.Regex.IsMatch(messagePacket!.RawMessage, regex.Expression) || // 不匹配
                        regex.Area == 2 && regex.Ignored.ToList().Contains(messagePacket.GroupId) ||
                        regex.Area == 3 && regex.Ignored.ToList().Contains(messagePacket.UserId) // 忽略
                        )
                    {
                        continue;
                    }
                    if (
                        !(
                        Global.Settings.Bot.PermissionList.Contains(messagePacket.UserId) ||
                        Global.Settings.Bot.GivePermissionToAllAdmin &&
                        messagePacket.MessageType == "group" &&
                        messagePacket?.Sender.RoleIndex < 2 &&
                        regex.IsAdmin &&
                        !isSelfMessage
                        ))
                    {
                        switch (regex.Area)
                        {
                            case 2:
                                EventTrigger.Trigger(Base.EventType.PermissionDeniedFromGroupMsg, messagePacket!);
                                break;

                            case 3:
                                EventTrigger.Trigger(Base.EventType.PermissionDeniedFromPrivateMsg, messagePacket!);
                                break;
                        }
                        continue;
                    }
                    if (System.Text.RegularExpressions.Regex.IsMatch(messagePacket.RawMessage, regex.Expression))
                    {
                        if ((regex.Area == 4 || regex.Area == 2) && messagePacket.MessageType == "group" ||
                            (regex.Area == 4 || regex.Area == 3) && messagePacket.MessageType == "private")
                        {
                            Command.Run(
                                Base.CommandOrigin.Msg,
                                regex.Command,
                                System.Text.RegularExpressions.Regex.Match(
                                    messagePacket.RawMessage,
                                    regex.Expression
                                ),
                                messagePacket,
                                false
                            );
                        }
                    }
                }
            }

            if (messagePacket.MessageType != "group")
            {
                return;
            }
            lock (Global.GroupCache)
            {
                if (!Global.GroupCache.ContainsKey(messagePacket.GroupId))
                {
                    Global.GroupCache.Add(messagePacket.GroupId, new Dictionary<long, Base.Member>());
                }
                if (!Global.GroupCache[messagePacket.GroupId].ContainsKey(messagePacket.UserId))
                {
                    Global.GroupCache[messagePacket.GroupId].Add(messagePacket.UserId, new Base.Member());
                }
                Global.GroupCache[messagePacket.GroupId][messagePacket.UserId].ID = messagePacket.UserId;
                Global.GroupCache[messagePacket.GroupId][messagePacket.UserId].Nickname = messagePacket.Sender.Nickname ?? string.Empty;
                Global.GroupCache[messagePacket.GroupId][messagePacket.UserId].Card = messagePacket.Sender.Card ?? string.Empty;
                Global.GroupCache[messagePacket.GroupId][messagePacket.UserId].Role = messagePacket.Sender.RoleIndex;
                Global.GroupCache[messagePacket.GroupId][messagePacket.UserId].GameID = Binder.GetGameID(messagePacket.UserId);
            }
        }

        /// <summary>
        /// 重置统计
        /// </summary>
        public static void ResetStatisics()
        {
            _messageReceived = null;
            _messageSent = null;
            _selfId = null;
        }
    }
}
