using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Linq;
using System.Text.RegularExpressions;

namespace Serein.baseFunction
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
            JObject JsonObject = (JObject)JsonConvert.DeserializeObject(Json);
            if (JsonObject["post_type"].ToString() == "message")
            {
                Global.Ui.BotWebBrowser_Invoke(
                "<span style=\"color:#239B56;font-weight: bold;\">[↓]</span>" +
                $"{JsonObject["sender"]["nickname"]}({JsonObject["sender"]["user_id"]})" + ":" +
                JsonObject["raw_message"].ToString());
                foreach (RegexItem Item in Global.RegexItems)
                {
                    if (string.IsNullOrEmpty(Item.Regex) || Item.Area <= 1)
                    {
                        continue;
                    }
                    if (Regex.IsMatch(JsonObject["raw_message"].ToString(), Item.Regex))
                    {
                        string MessageType = JsonObject["message_type"].ToString();
                        long GroupId = long.TryParse(JsonObject["group_id"].ToString(), out long Result) ? Result : -1;
                        long UserId = long.TryParse(JsonObject["sender"]["user_id"].ToString(), out Result) ? Result : -1;
                        if (Item.IsAdmin)
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
                            if (MessageType == "group" && Global.Settings_Bot.GroupList.Contains(GroupId))
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
                            else if (MessageType == "private")
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
                            if (MessageType == "group" && Global.Settings_Bot.GroupList.Contains(GroupId))
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
                            else if (MessageType == "private")
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
                JsonObject["meta_event_type"].ToString() == "heartbeat")
            {
                SelfId = JsonObject["self_id"].ToString();
                MessageReceived = JsonObject["status"]["stat"]["MessageReceived"].ToString();
                MessageSent = JsonObject["status"]["stat"]["MessageSent"].ToString();
                if ((long.TryParse(MessageReceived, out long TempNumber) ? TempNumber : 0) > 10000000)
                {
                    MessageReceived = (TempNumber / 10000).ToString("N1") + "W";
                }
                if ((long.TryParse(MessageSent, out TempNumber) ? TempNumber : 0) > 10000000)
                {
                    MessageSent = (TempNumber / 10000).ToString("N1") + "W";
                }
            }
        }
    }
}

