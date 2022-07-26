using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Serein.Items;
using System.Text.RegularExpressions;
using System;
using System.Net;
using Serein.Plugin;

namespace Serein.Base
{
    public class Message
    {
        public static string MessageReceived, MessageSent, SelfId;
        public static void ProcessMsgFromConsole(string CommandLine)
        {
            foreach (RegexItem Item in Global.RegexItems)
            {
                if (string.IsNullOrEmpty(Item.Regex) || Item.Area != 1)
                {
                    continue;
                }
                if (Regex.IsMatch(CommandLine, Item.Regex))
                {
                    Command.Run(
                        2,
                        Item.Command,
                        MsgMatch: Regex.Match(CommandLine, Item.Regex)
                        );
                }
            }
        }
        public static void ProcessMsgFromBot(string Package)
        {
            string Json = Package ?? "";
            Json = DeUnicode(Json);
            Json = WebUtility.HtmlDecode(Json);
            JObject JsonObject = (JObject)JsonConvert.DeserializeObject(Json);
            if (JsonObject["post_type"] == null)
            {
                return;
            }
            if (
                JsonObject["post_type"].ToString() == "message" ||
                JsonObject["post_type"].ToString() == "message_sent"
                )
            {
                bool IsSelfMessage = JsonObject["post_type"].ToString() == "message_sent";
                string MessageType = JsonObject["message_type"].ToString();
                string RawMessage = JsonObject["raw_message"].ToString();
                long UserId = long.TryParse(JsonObject["sender"]["user_id"].ToString(), out long Result) ? Result : -1;
                long GroupId = MessageType == "group" && long.TryParse(JsonObject["group_id"].ToString(), out Result) ? Result : -1;
                Global.Ui.BotWebBrowser_Invoke(
                    "<span style=\"color:#239B56;font-weight: bold;\">[↓]</span>" +
                    $"{JsonObject["sender"]["nickname"]}({JsonObject["sender"]["user_id"]})" + ":" +
                    RawMessage
                    ); 
                foreach (RegexItem Item in Global.RegexItems)
                {
                    if (
                        string.IsNullOrEmpty(Item.Regex) ||
                        Item.Area <= 1 ||
                        !(
                            IsSelfMessage && Item.Area == 4 ||
                            !IsSelfMessage && Item.Area != 4
                        ) ||
                        MessageType == "group" && !Global.Settings.Bot.GroupList.Contains(GroupId) ||
                        !Regex.IsMatch(RawMessage, Item.Regex)
                        )
                        continue;
                    if (
                        !(
                        Global.Settings.Bot.PermissionList.Contains(UserId) ||
                        Global.Settings.Bot.GivePermissionToAllAdmin &&
                        MessageType == "group" && (
                            JsonObject["sender"]["role"].ToString() == "admin" ||
                            JsonObject["sender"]["role"].ToString() == "owner")
                        ) &&
                        Item.IsAdmin &&
                        !IsSelfMessage
                        )
                    {
                        switch (Item.Area)
                        {
                            case 2:
                                EventTrigger.Trigger("PermissionDenied_Group", GroupId, UserId);
                                break;
                            case 3:
                                EventTrigger.Trigger("PermissionDenied_Private", UserId: UserId);
                                break;
                        }
                        continue;
                    }
                    if (Regex.IsMatch(RawMessage, Item.Regex))
                    {
                        if ((Item.Area == 4 || Item.Area == 2) && MessageType == "group")
                        {
                            Command.Run(
                                1,
                                Item.Command,
                                JsonObject,
                                Regex.Match(
                                    RawMessage,
                                    Item.Regex
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
                                JsonObject,
                                Regex.Match(
                                    RawMessage,
                                    Item.Regex
                                    ),
                                UserId
                            );
                        }
                    }
                }
                if (!IsSelfMessage)
                {
                    if (MessageType == "private")
                        JSFunc.Trigger("onReceivePrivateMessage", UserId, RawMessage, JsonObject["sender"]["nickname"].ToString());
                    else if (MessageType == "group" && Global.Settings.Bot.GroupList.Contains(GroupId))
                        JSFunc.Trigger("onReceiveGroupMessage", GroupId, UserId, RawMessage, string.IsNullOrEmpty(JsonObject["sender"]["card"].ToString()) ? JsonObject["sender"]["nickname"].ToString() : JsonObject["sender"]["card"].ToString());
                }
            }
            else if (
                JsonObject["post_type"].ToString() == "meta_event"
                &&
                JsonObject["meta_event_type"].ToString() == "heartbeat"
                )
            {
                SelfId = JsonObject["self_id"].ToString();
                MessageReceived = (
                    JsonObject["status"]["stat"]["message_received"] ??
                    JsonObject["status"]["stat"]["MessageReceived"]
                    ).ToString();
                MessageSent = (
                    JsonObject["status"]["stat"]["message_sent"] ??
                    JsonObject["status"]["stat"]["MessageSent"]
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
            else if (JsonObject["post_type"].ToString() == "notice")
            {
                long UserId = long.TryParse(JsonObject["user_id"].ToString(), out long Result) ? Result : -1;
                long GroupId = long.TryParse(JsonObject["group_id"].ToString(), out Result) ? Result : -1;
                if (Global.Settings.Bot.GroupList.Contains(GroupId))
                {
                    if (
                        JsonObject["notice_type"].ToString() == "group_decrease" ||
                        JsonObject["notice_type"].ToString() == "group_increase")
                    {
                        switch (JsonObject["notice_type"].ToString())
                        {
                            case "group_decrease":
                                EventTrigger.Trigger("Group_Decrease", GroupId, UserId);
                                JSFunc.Trigger("onGroupDecrease", GroupId, UserId);
                                break;
                            case "group_increase":
                                EventTrigger.Trigger("Group_Increase", GroupId, UserId);
                                JSFunc.Trigger("onGroupIncrease", GroupId, UserId);
                                break;
                        }
                    }
                    else if (
                        JsonObject["notice_type"].ToString() == "notify" &&
                        JsonObject["sub_type"].ToString() == "poke" &&
                        JsonObject["target_id"].ToString() == SelfId)
                    {
                        EventTrigger.Trigger("Group_Poke", GroupId, UserId);
                        JSFunc.Trigger("onGroupPoke", GroupId, UserId);
                    }
                }
            }
            JSFunc.Trigger("onReceivePackage", Package);
        }
        public static string DeUnicode(string str)
        {
            Regex reg = new Regex(@"(?i)\\[uU]([0-9a-f]{4})");
            return reg.Replace(str, delegate (Match m) { return ((char)Convert.ToInt32(m.Groups[1].Value, 16)).ToString(); });
        }
    }
}
