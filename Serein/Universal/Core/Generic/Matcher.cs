using Newtonsoft.Json.Linq;
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
        public static string MessageReceived
        {
            get => !Websocket.Status || string.IsNullOrEmpty(_messageReceived) ? "-" : _messageReceived;
            set => _messageReceived = value;
        }

        /// <summary>
        /// 已发送消息
        /// </summary>
        public static string MessageSent
        {
            get => !Websocket.Status || string.IsNullOrEmpty(_messageSent) ? "-" : _messageSent;
            set => _messageSent = value;
        }

        /// <summary>
        /// 当前ID
        /// </summary>
        public static string SelfId
        {
            get => !Websocket.Status || string.IsNullOrEmpty(_selfId) ? "-" : _selfId;
            set => _selfId = value;
        }

        private static string _messageReceived, _messageSent, _selfId;

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
                    Task.Run(() => Command.Run(Base.CommandOrigin.Console, regex.Command, msgMatch: System.Text.RegularExpressions.Regex.Match(line, regex.Expression)));
                }
            }
        }

        /// <summary>
        /// 处理来自机器人的消息
        /// </summary>
        /// <param name="packet">数据包</param>
        public static void Process(JObject packet)
        {
            string postType = packet.SelectToken("post_type")?.ToString();
            if (string.IsNullOrEmpty(postType))
            {
                return;
            }
            long result, userID, groupID;
            switch (postType)
            {
                case "message":
                case "message_sent":
                    string messageType = packet.SelectToken("message_type")?.ToString();
                    HandleMsg(
                        packet,
                        long.TryParse(packet.SelectToken("message_id").ToString(), out result) ? result : 0,
                        messageType == "group" && long.TryParse(packet.SelectToken("group_id").ToString(), out result) ? result : -1,
                        long.TryParse(packet.SelectToken("sender.user_id").ToString(), out result) ? result : -1,
                        packet.SelectToken("raw_message").ToString(),
                        postType == "message_sent",
                        messageType
                        );
                    break;

                case "meta_event":
                    if (packet.SelectToken("meta_event_type").ToString() == "heartbeat")
                    {
                        SelfId = packet.SelectToken("self_id").ToString();
                        MessageReceived = (packet.SelectToken("status.stat.message_received") ?? packet.SelectToken("status.stat.MessageReceived")).ToString();
                        MessageSent = (packet.SelectToken("status.stat.message_sent") ?? packet.SelectToken("status.stat.MessageSent")).ToString();
                        if ((long.TryParse(MessageReceived, out long TempNumber) ? TempNumber : 0) > 10000000)
                        {
                            MessageReceived = (TempNumber / 10000).ToString("N1") + "w";
                        }
                        if ((long.TryParse(MessageSent, out TempNumber) ? TempNumber : 0) > 10000000)
                        {
                            MessageSent = (TempNumber / 10000).ToString("N1") + "w";
                        }
                    }
                    break;

                case "notice":
                    userID = long.TryParse(packet.SelectToken("user_id").ToString(), out result) ? result : -1;
                    groupID = long.TryParse(packet.SelectToken("group_id").ToString(), out result) ? result : -1;
                    if (Global.Settings.Bot.GroupList.Contains(groupID))
                    {
                        switch (packet.SelectToken("notice_type").ToString())
                        {
                            case "GroupDecrease":
                            case "group_decrease":
                                EventTrigger.Trigger(Base.EventType.GroupDecrease, groupID, userID);
                                JSFunc.Trigger(Base.EventType.GroupDecrease, groupID, userID);
                                break;

                            case "GroupIncrease":
                            case "group_increase":
                                EventTrigger.Trigger(Base.EventType.GroupIncrease, groupID, userID);
                                JSFunc.Trigger(Base.EventType.GroupIncrease, groupID, userID);
                                break;

                            case "notify":
                                if (packet.SelectToken("sub_type").ToString() == "poke" &&
                                    packet.SelectToken("target_id").ToString() == SelfId)
                                {
                                    EventTrigger.Trigger(Base.EventType.GroupPoke, groupID, userID);
                                    JSFunc.Trigger(Base.EventType.GroupPoke, groupID, userID);
                                }
                                break;
                        }
                    }
                    break;
            }
        }

        /// <summary>
        /// 处理消息
        /// </summary>
        /// <param name="packet">数据包</param>
        /// <param name="messageID">消息ID</param>
        /// <param name="groupID">群号</param>
        /// <param name="userID">用户ID</param>
        /// <param name="rawMessage">原始消息</param>
        /// <param name="isSelfMessage">自身消息</param>
        /// <param name="messageType">消息类型</param>
        private static void HandleMsg(JObject packet, long messageID, long groupID, long userID, string rawMessage, bool isSelfMessage, string messageType)
        {
            if (!isSelfMessage)
            {
                bool interdicted = false;
                if (messageType == "private")
                {
                    interdicted = JSFunc.Trigger(Base.EventType.ReceivePrivateMessage, userID, rawMessage, packet.SelectToken("sender.nickname"), messageID);
                }
                else if (messageType == "group")
                {
                    interdicted = JSFunc.Trigger(Base.EventType.ReceiveGroupMessage, groupID, userID, rawMessage,
                        (packet.SelectToken("sender.card") ?? packet.SelectToken("sender.nickname")).ToString() ?? string.Empty, messageID);
                }
                if (interdicted) { return; }
            }
            Logger.Output(Base.LogType.Bot_Receive, $"{packet.SelectToken("sender.nickname")}({packet.SelectToken("sender.user_id")})" + ":" + rawMessage);

            if (messageType == "group" ^ Global.Settings.Bot.GroupList.Contains(groupID))
            {
                return;
            }
            lock (Global.RegexList)
            {
                foreach (Base.Regex regex in Global.RegexList)
                {
                    if (
                        string.IsNullOrEmpty(regex.Expression) || // 表达式为空
                        regex.Area <= 1 ||  // 禁用或控制台
                        isSelfMessage ^ regex.Area == 4 || // 自身消息与定义域矛盾
                        !System.Text.RegularExpressions.Regex.IsMatch(rawMessage, regex.Expression) || // 不匹配
                        regex.Area == 2 && regex.Ignored.ToList().Contains(groupID) ||
                        regex.Area == 3 && regex.Ignored.ToList().Contains(userID) // 忽略
                        )
                    {
                        continue;
                    }
                    if (
                        !(
                        Global.Settings.Bot.PermissionList.Contains(userID) ||
                        Global.Settings.Bot.GivePermissionToAllAdmin &&
                        messageType == "group" && (
                            packet.SelectToken("sender.role").ToString() == "admin" ||
                            packet.SelectToken("sender.role").ToString() == "owner")
                        ) &&
                        regex.IsAdmin &&
                        !isSelfMessage
                        )
                    {
                        switch (regex.Area)
                        {
                            case 2:
                                EventTrigger.Trigger(Base.EventType.PermissionDeniedFromGroupMsg, groupID, userID, packet);
                                break;

                            case 3:
                                EventTrigger.Trigger(Base.EventType.PermissionDeniedFromPrivateMsg, -1, userID, jobject: packet);
                                break;
                        }
                        continue;
                    }
                    if (System.Text.RegularExpressions.Regex.IsMatch(rawMessage, regex.Expression))
                    {
                        if ((regex.Area == 4 || regex.Area == 2) && messageType == "group")
                        {
                            Command.Run(
                                Base.CommandOrigin.Msg,
                                regex.Command,
                                packet,
                                System.Text.RegularExpressions.Regex.Match(
                                    rawMessage,
                                    regex.Expression
                                ),
                                userID,
                                groupID
                            );
                        }
                        else if ((regex.Area == 4 || regex.Area == 3) && messageType == "private")
                        {
                            Command.Run(
                                Base.CommandOrigin.Msg,
                                regex.Command,
                                packet,
                                System.Text.RegularExpressions.Regex.Match(
                                    rawMessage,
                                    regex.Expression
                                    ),
                                userID
                            );
                        }
                    }
                }
            }

            if (messageType != "group")
            {
                return;
            }
            lock (Global.GroupCache)
            {
                if (!Global.GroupCache.ContainsKey(groupID))
                {
                    Global.GroupCache.Add(groupID, new Dictionary<long, Base.Member>());
                }
                if (!Global.GroupCache[groupID].ContainsKey(userID))
                {
                    Global.GroupCache[groupID].Add(userID, new Base.Member());
                }
                Global.GroupCache[groupID][userID].ID = userID;
                Global.GroupCache[groupID][userID].Nickname = packet.SelectToken("sender.nickname").ToString();
                Global.GroupCache[groupID][userID].Card = packet.SelectToken("sender.card").ToString();
                Global.GroupCache[groupID][userID].Role = Array.IndexOf(Command.Roles, packet.SelectToken("sender.role"));
                Global.GroupCache[groupID][userID].GameID = Binder.GetGameID(userID);
            }
        }
    }
}
