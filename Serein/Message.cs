using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text.RegularExpressions;

namespace Serein
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
                    Command.Run(Item.Command, Regex.Match(CommandLine, Item.Regex));
                }
            }
        }
        public static void ProcessMsgFromBot(string Json)
        {
            Json = Json.Replace("&#44;", ",");
            Json = Json.Replace("&amp;", "#");
            Json = Json.Replace("&#91;", "[");
            Json = Json.Replace("&#93;", "]");
            JObject JsonObject = (JObject)JsonConvert.DeserializeObject(Json);
            if (
                JsonObject["post_type"].ToString() == "message" ||
                JsonObject["post_type"].ToString() == "message_sent"
                )
            {
                Global.Ui.BotWebBrowser_Invoke(
                    "<span style=\"color:#239B56;font-weight: bold;\">[↓]</span>" +
                    $"{JsonObject["sender"]["nickname"]}({JsonObject["sender"]["user_id"]})" + ":" +
                    JsonObject["raw_message"].ToString()
                    );
                bool IsSelfMessage = JsonObject["post_type"].ToString() == "message_sent";
                foreach (RegexItem Item in Global.RegexItems)
                {
                    if (
                        string.IsNullOrEmpty(Item.Regex) ||
                        Item.Area <= 1)
                    {
                        continue;
                    }
                    if (
                        !(
                            (IsSelfMessage && Item.Area == 4) ||
                            (!IsSelfMessage && Item.Area != 4)
                        ))
                    {
                        continue;
                    }
                    if (Regex.IsMatch(JsonObject["raw_message"].ToString(), Item.Regex))
                    {
                        string MessageType = JsonObject["message_type"].ToString();
                        long UserId = long.TryParse(JsonObject["sender"]["user_id"].ToString(), out long Result) ? Result : -1;
                        long GroupId = MessageType == "group" && long.TryParse(JsonObject["group_id"].ToString(), out Result) ? Result : -1;
                        if (Item.IsAdmin && !IsSelfMessage)
                        {
                            bool IsAdmin = false;
                            if (Global.Settings_Bot.PermissionList.Contains(UserId))
                            {
                                IsAdmin = true;
                            }
                            else if (Global.Settings_Bot.GivePermissionToAllAdmin && (JsonObject["sender"]["role"].ToString() == "admin" || JsonObject["sender"]["role"].ToString() == "owner"))
                            {
                                IsAdmin = true;
                            }
                            if (!IsAdmin)
                            {
                                continue;
                            }
                            if (Item.Area == 2 && MessageType == "group" && Global.Settings_Bot.GroupList.Contains(GroupId))
                            {
                                Command.Run(
                                    JsonObject,
                                    Item.Command,
                                    Regex.Match(
                                        JsonObject["raw_message"].ToString(),
                                        Item.Regex
                                    ),
                                    UserId,
                                    GroupId
                                );
                            }
                            else if (Item.Area == 3 && MessageType == "private")
                            {
                                Command.Run(
                                    JsonObject,
                                    Item.Command,
                                    Regex.Match(
                                        JsonObject["raw_message"].ToString(),
                                        Item.Regex
                                        ),
                                    UserId
                                );
                            }
                        }
                        else
                        {
                            if (Item.Area == 2 && MessageType == "group" && Global.Settings_Bot.GroupList.Contains(GroupId))
                            {
                                Command.Run(
                                    JsonObject,
                                    Item.Command,
                                    Regex.Match(
                                        JsonObject["raw_message"].ToString(),
                                        Item.Regex
                                    ),
                                    UserId,
                                    GroupId
                                );
                            }
                            else if (Item.Area == 3 && MessageType == "private")
                            {
                                Command.Run(
                                    JsonObject,
                                    Item.Command,
                                    Regex.Match(
                                        JsonObject["raw_message"].ToString(),
                                        Item.Regex
                                        ),
                                    UserId
                                );
                            }
                        }
                    }
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
        }
    }
}

