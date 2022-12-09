using Newtonsoft.Json.Linq;
using Serein.JSPlugin;

namespace Serein.Base
{
    public class Matcher
    {
        /// <summary>
        /// 统计信息
        /// </summary>
        public static string MessageReceived, MessageSent, SelfId;

        /// <summary>
        /// 处理来自控制台的消息
        /// </summary>
        /// <param name="Line">控制台的消息</param>
        public static void Process(string Line)
        {
            foreach (Items.Regex Item in Global.RegexItems)
            {
                if (string.IsNullOrEmpty(Item.Expression) || Item.Area != 1)
                {
                    continue;
                }
                if (System.Text.RegularExpressions.Regex.IsMatch(Line, Item.Expression))
                {
                    Command.Run(
                        2,
                        Item.Command,
                        MsgMatch: System.Text.RegularExpressions.Regex.Match(Line, Item.Expression)
                        );
                }
            }
        }

        /// <summary>
        /// 处理来自机器人的消息
        /// </summary>
        /// <param name="Packet">数据包</param>
        public static void Process(JObject Packet)
        {
            if (Packet["post_type"] == null)
            {
                return;
            }
            if (
                Packet["post_type"].ToString() == "message" ||
                Packet["post_type"].ToString() == "message_sent"
                )
            {
                bool IsSelfMessage = Packet["post_type"].ToString() == "message_sent";
                string MessageType = Packet["message_type"].ToString();
                string RawMessage = Packet["raw_message"].ToString();
                long UserId = long.TryParse(Packet["sender"]["user_id"].ToString(), out long Result) ? Result : -1;
                long GroupId = MessageType == "group" && long.TryParse(Packet["group_id"].ToString(), out Result) ? Result : -1;
                Logger.Out(Items.LogType.Bot_Receive, $"{Packet["sender"]["nickname"]}({Packet["sender"]["user_id"]})" + ":" + RawMessage);
                foreach (Items.Regex Item in Global.RegexItems)
                {
                    if (
                        string.IsNullOrEmpty(Item.Expression) ||
                        Item.Area <= 1 ||
                        !(
                            IsSelfMessage && Item.Area == 4 ||
                            !IsSelfMessage && Item.Area != 4
                        ) ||
                        MessageType == "group" && !Global.Settings.Bot.GroupList.Contains(GroupId) ||
                        !System.Text.RegularExpressions.Regex.IsMatch(RawMessage, Item.Expression)
                        )
                        continue;
                    if (
                        !(
                        Global.Settings.Bot.PermissionList.Contains(UserId) ||
                        Global.Settings.Bot.GivePermissionToAllAdmin &&
                        MessageType == "group" && (
                            Packet["sender"]["role"].ToString() == "admin" ||
                            Packet["sender"]["role"].ToString() == "owner")
                        ) &&
                        Item.IsAdmin &&
                        !IsSelfMessage
                        )
                    {
                        switch (Item.Area)
                        {
                            case 2:
                                EventTrigger.Trigger(Items.EventType.PermissionDeniedFromGroupMsg, GroupId, UserId);
                                break;
                            case 3:
                                EventTrigger.Trigger(Items.EventType.PermissionDeniedFromPrivateMsg, -1, UserId);
                                break;
                        }
                        continue;
                    }
                    if (System.Text.RegularExpressions.Regex.IsMatch(RawMessage, Item.Expression))
                    {
                        if ((Item.Area == 4 || Item.Area == 2) && MessageType == "group")
                        {
                            Command.Run(
                                1,
                                Item.Command,
                                Packet,
                                System.Text.RegularExpressions.Regex.Match(
                                    RawMessage,
                                    Item.Expression
                                ),
                                UserId,
                                GroupId
                            );
                        }
                        else if ((Item.Area == 4 || Item.Area == 3) && MessageType == "private")
                        {
                            Command.Run(
                                1,
                                Item.Command,
                                Packet,
                                System.Text.RegularExpressions.Regex.Match(
                                    RawMessage,
                                    Item.Expression
                                    ),
                                UserId
                            );
                        }
                    }
                }
                if (!IsSelfMessage)
                {
                    if (MessageType == "private")
                        JSFunc.Trigger(Items.EventType.ReceivePrivateMessage, UserId, RawMessage, Packet["sender"]["nickname"].ToString());
                    else if (MessageType == "group" && Global.Settings.Bot.GroupList.Contains(GroupId))
                        JSFunc.Trigger(Items.EventType.ReceiveGroupMessage, GroupId, UserId, RawMessage, string.IsNullOrEmpty(Packet["sender"]["card"].ToString()) ? Packet["sender"]["nickname"].ToString() : Packet["sender"]["card"].ToString());
                }
            }
            else if (
                Packet["post_type"].ToString() == "meta_event"
                &&
                Packet["meta_event_type"].ToString() == "heartbeat"
                )
            {
                SelfId = Packet["self_id"].ToString();
                MessageReceived = (
                    Packet["status"]["stat"]["message_received"] ??
                    Packet["status"]["stat"]["MessageReceived"]
                    ).ToString();
                MessageSent = (
                    Packet["status"]["stat"]["message_sent"] ??
                    Packet["status"]["stat"]["MessageSent"]
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
            else if (Packet["post_type"].ToString() == "notice")
            {
                long UserId = long.TryParse(Packet["user_id"].ToString(), out long Result) ? Result : -1;
                long GroupId = long.TryParse(Packet["group_id"].ToString(), out Result) ? Result : -1;
                if (Global.Settings.Bot.GroupList.Contains(GroupId))
                {
                    switch (Packet["notice_type"].ToString())
                    {
                        case "GroupDecrease":
                        case "group_decrease":
                            EventTrigger.Trigger(Items.EventType.GroupDecrease, GroupId, UserId);
                            JSFunc.Trigger(Items.EventType.GroupDecrease, GroupId, UserId);
                            break;
                        case "GroupIncrease":
                        case "group_increase":
                            EventTrigger.Trigger(Items.EventType.GroupIncrease, GroupId, UserId);
                            JSFunc.Trigger(Items.EventType.GroupIncrease, GroupId, UserId);
                            break;
                        case "notify":
                            if (Packet["sub_type"].ToString() == "poke" &&
                                Packet["target_id"].ToString() == SelfId)
                            {
                                EventTrigger.Trigger(Items.EventType.GroupPoke, GroupId, UserId);
                                JSFunc.Trigger(Items.EventType.GroupPoke, GroupId, UserId);
                            }
                            break;
                    }
                }
            }
            JSFunc.Trigger(Items.EventType.ReceivePacket, Packet);
        }
    }
}
