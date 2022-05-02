using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Serein
{
    public class Message
    {
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
            if(JsonObject["post_type"].ToString()== "message")
            {
                Global.ui.BotWebBrowser_Invoke(
                "<span style=\"color:#239B56;font-weight: bold;\">[↓]</span>" +
                $"{JsonObject["sender"]["nickname"]}({JsonObject["sender"]["user_id"]})"+":"+
                JsonObject["raw_message"].ToString());
                foreach(RegexItem Item in Global.RegexItems)
                {
                    if (string.IsNullOrEmpty(Item.Regex) || Item.Area<=1)
                    {
                        continue;
                    }
                    if(Regex.IsMatch(JsonObject["raw_message"].ToString(), Item.Regex))
                    {
                        string MessageType = JsonObject["message_type"].ToString();
                        int r;
                        int GroupId = int.TryParse(JsonObject["group_id"].ToString(), out r) ? r : -1;
                        int UserId = int.TryParse(JsonObject["sender"]["user_id"].ToString(), out r) ? r : -1;
                        if (Item.IsAdmin)
                        {
                            bool IsAdmin = false;
                            if (Global.Settings_bot.PermissionList.Contains(UserId))
                            {
                                IsAdmin = true;
                            }
                            else if (Global.Settings_bot.GivePermissionToAllAdmin && (JsonObject["sender"]["role"].ToString() == "admin" || JsonObject["sender"]["role"].ToString() == "owner"))
                            {
                                IsAdmin = true;
                            }
                            if (!IsAdmin)
                            {
                                continue;
                            }
                            if (MessageType == "group" && Global.Settings_bot.GroupList.Contains(GroupId))
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
                                Command.test(Item);
                            }
                            else if(MessageType == "private")
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
                                Command.test(Item);
                            }
                        }
                        else
                        {
                            if (MessageType == "group" && Global.Settings_bot.GroupList.Contains(GroupId))
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
                                Command.test(Item);
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
                                Command.test(Item);
                            }
                        }
                    }
                }
            }
        }
    }
}

