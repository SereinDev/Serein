using Newtonsoft.Json.Linq;
using Serein.JSPlugin;

namespace Serein.Base
{
    public static class Matcher
    {
        /// <summary>
        /// 统计信息
        /// </summary>
        public static string MessageReceived, MessageSent, SelfId;

        /// <summary>
        /// 处理来自控制台的消息
        /// </summary>
        /// <param name="line">控制台的消息</param>
        public static void Process(string line)
        {
            foreach (Items.Regex regex in Global.RegexItems)
            {
                if (string.IsNullOrEmpty(regex.Expression) || regex.Area != 1)
                {
                    continue;
                }
                if (System.Text.RegularExpressions.Regex.IsMatch(line, regex.Expression))
                {
                    Command.Run(2, regex.Command, msgMatch: System.Text.RegularExpressions.Regex.Match(line, regex.Expression));
                }
            }
        }

        /// <summary>
        /// 处理来自机器人的消息
        /// </summary>
        /// <param name="packet">数据包</param>
        public static void Process(JObject packet)
        {
            if (packet["post_type"] == null)
            {
                return;
            }
            string postType = packet["post_type"].ToString();
            long result, userId, groupId;
            switch (postType)
            {
                case "message":
                case "message_sent":
                    bool isSelfMessage = postType == "message_sent";
                    string messageType = packet["message_type"].ToString();
                    string rawMessage = packet["raw_message"].ToString();
                    userId = long.TryParse(packet["sender"]["user_id"].ToString(), out result) ? result : -1;
                    groupId = messageType == "group" && long.TryParse(packet["group_id"].ToString(), out result) ? result : -1;
                    Logger.Out(Items.LogType.Bot_Receive, $"{packet["sender"]["nickname"]}({packet["sender"]["user_id"]})" + ":" + rawMessage);
                    foreach (Items.Regex regex in Global.RegexItems)
                    {
                        if (
                            string.IsNullOrEmpty(regex.Expression) ||
                            regex.Area <= 1 ||
                            !(
                                isSelfMessage && regex.Area == 4 ||
                                !isSelfMessage && regex.Area != 4
                            ) ||
                            messageType == "group" && !Global.Settings.Bot.GroupList.Contains(groupId) ||
                            !System.Text.RegularExpressions.Regex.IsMatch(rawMessage, regex.Expression)
                            )
                        {
                            continue;
                        }
                        if (
                            !(
                            Global.Settings.Bot.PermissionList.Contains(userId) ||
                            Global.Settings.Bot.GivePermissionToAllAdmin &&
                            messageType == "group" && (
                                packet["sender"]["role"].ToString() == "admin" ||
                                packet["sender"]["role"].ToString() == "owner")
                            ) &&
                            regex.IsAdmin &&
                            !isSelfMessage
                            )
                        {
                            switch (regex.Area)
                            {
                                case 2:
                                    EventTrigger.Trigger(Items.EventType.PermissionDeniedFromGroupMsg, groupId, userId);
                                    break;
                                case 3:
                                    EventTrigger.Trigger(Items.EventType.PermissionDeniedFromPrivateMsg, -1, userId);
                                    break;
                            }
                            continue;
                        }
                        if (System.Text.RegularExpressions.Regex.IsMatch(rawMessage, regex.Expression))
                        {
                            if ((regex.Area == 4 || regex.Area == 2) && messageType == "group")
                            {
                                Command.Run(
                                    1,
                                    regex.Command,
                                    packet,
                                    System.Text.RegularExpressions.Regex.Match(
                                        rawMessage,
                                        regex.Expression
                                    ),
                                    userId,
                                    groupId
                                );
                            }
                            else if ((regex.Area == 4 || regex.Area == 3) && messageType == "private")
                            {
                                Command.Run(
                                    1,
                                    regex.Command,
                                    packet,
                                    System.Text.RegularExpressions.Regex.Match(
                                        rawMessage,
                                        regex.Expression
                                        ),
                                    userId
                                );
                            }
                        }
                    }
                    if (!isSelfMessage)
                    {
                        if (messageType == "private")
                        {
                            JSFunc.Trigger(Items.EventType.ReceivePrivateMessage, userId, rawMessage, packet["sender"]["nickname"].ToString());
                        }
                        else if (messageType == "group")
                        {
                            JSFunc.Trigger(Items.EventType.ReceiveGroupMessage, groupId, userId, rawMessage, string.IsNullOrEmpty(packet["sender"]["card"].ToString()) ? packet["sender"]["nickname"].ToString() : packet["sender"]["card"].ToString());
                        }
                    }
                    break;
                case "meta_event":
                    if (packet["meta_event_type"].ToString() == "heartbeat")
                    {
                        SelfId = packet["self_id"].ToString();
                        MessageReceived = (
                            packet["status"]["stat"]["message_received"] ??
                            packet["status"]["stat"]["MessageReceived"] ?? "-"
                            ).ToString();
                        MessageSent = (
                            packet["status"]["stat"]["message_sent"] ??
                            packet["status"]["stat"]["MessageSent"] ?? "-"
                            ).ToString();
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
                    userId = long.TryParse(packet["user_id"].ToString(), out result) ? result : -1;
                    groupId = long.TryParse(packet["group_id"].ToString(), out result) ? result : -1;
                    if (Global.Settings.Bot.GroupList.Contains(groupId))
                    {
                        switch (packet["notice_type"].ToString())
                        {
                            case "GroupDecrease":
                            case "group_decrease":
                                EventTrigger.Trigger(Items.EventType.GroupDecrease, groupId, userId);
                                JSFunc.Trigger(Items.EventType.GroupDecrease, groupId, userId);
                                break;
                            case "GroupIncrease":
                            case "group_increase":
                                EventTrigger.Trigger(Items.EventType.GroupIncrease, groupId, userId);
                                JSFunc.Trigger(Items.EventType.GroupIncrease, groupId, userId);
                                break;
                            case "notify":
                                if (packet["sub_type"].ToString() == "poke" &&
                                    packet["target_id"].ToString() == SelfId)
                                {
                                    EventTrigger.Trigger(Items.EventType.GroupPoke, groupId, userId);
                                    JSFunc.Trigger(Items.EventType.GroupPoke, groupId, userId);
                                }
                                break;
                        }
                    }
                    break;
            }
        }
    }
}
