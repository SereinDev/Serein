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
    internal static class PacketHandler
    {
        const string _emptyString = "-";

        /// <summary>
        /// 已接收消息数
        /// </summary>
        public static string MessageReceived
        {
            get
            {
                if (!Websocket.Status || !_messageReceived.HasValue)
                {
                    return "-";
                }
                if (_messageReceived > 10000)
                {
                    return (_messageReceived / 10000).ToString() ?? _emptyString;
                }
                return _messageReceived?.ToString() ?? _emptyString;
            }
        }

        /// <summary>
        /// 已接收消息数（Int64）
        /// </summary>
        public static long? MessageReceivedInt64 => Websocket.Status ? _messageReceived : null;

        /// <summary>
        /// 已发送消息数
        /// </summary>
        public static string MessageSent
        {
            get
            {
                if (!Websocket.Status || !_messageSent.HasValue)
                {
                    return "-";
                }
                if (_messageSent > 10000)
                {
                    return (_messageSent / 10000).ToString() ?? _emptyString;
                }
                return _messageSent?.ToString() ?? _emptyString;
            }
        }

        /// <summary>
        /// 已发送消息数（Int64）
        /// </summary>
        public static long? MessageSentInt64 => Websocket.Status ? _messageSent : null;

        /// <summary>
        /// 当前ID
        /// </summary>
        public static string SelfId => Websocket.Status ? _selfId.ToString() ?? _emptyString : _emptyString;

        /// <summary>
        /// 当前ID（Int64）
        /// </summary>
        public static long? SelfIdInt64 => Websocket.Status ? _selfId : null;

        private static long? _messageReceived, _messageSent, _selfId;


        /// <summary>
        /// 处理来自机器人的消息
        /// </summary>
        /// <param name="packet">数据包</param>
        public static void Handle(JObject packet)
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
                    PacketHandler.HandleMsg(packet.ToObject<Message>());
                    break;

                case "notice":
                    PacketHandler.HandleNotice(packet.ToObject<Notice>());
                    break;

                case "meta_event":
                    PacketHandler.HandleMetaEvent(packet);
                    break;

            }
        }

        /// <summary>
        /// 处理元事件
        /// </summary>
        /// <param name="packet">数据包</param>
        public static void HandleMetaEvent(JObject packet)
        {
            if (packet.SelectToken("meta_event_type")?.ToString() == "heartbeat")
            {
                _selfId = (long?)packet.SelectToken("self_id");
                _messageReceived = (long?)packet.SelectToken("status.stat.message_received");
                _messageSent = ((long?)packet.SelectToken("status.stat.message_sent"));
            }
        }

        /// <summary>
        /// 处理通知
        /// </summary>
        /// <param name="notice">通知数据包</param>
        public static void HandleNotice(Notice? notice)
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
        public static void HandleMsg(Message? messagePacket)
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

            Matcher.MatchMsg(messagePacket, isSelfMessage);
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