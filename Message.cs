using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Web.Script.Serialization;

namespace Serein
{
    public class Message
    {
        public static void ProcessMsgFromConsole(string text)
        {

        }
        public static void ProcessMsgFromBot(string json)
        {
            JObject jsonObject = (JObject)JsonConvert.DeserializeObject(json);
            if(jsonObject["post_type"].ToString()== "message")
            {
                Global.ui.BotWebBrowser_Invoke(
                "<span style=\"color:#239B56;font-weight: bold;\">[↓]</span>" +
                $"{jsonObject["sender"]["nickname"]}({jsonObject["sender"]["user_id"]})"+":"+
                jsonObject["raw_message"].ToString());
                foreach(RegexItem item in Global.RegexItems)
                {
                    if (string.IsNullOrEmpty(item.Regex) || item.Area<=1)
                    {
                        continue;
                    }
                    if(Regex.IsMatch(jsonObject["raw_message"].ToString(), item.Regex))
                    {
                        string message_type = jsonObject["message_type"].ToString();
                        int r;
                        int group_id = int.TryParse(jsonObject["group_id"].ToString(), out r) ? r : -1;
                        int user_id = int.TryParse(jsonObject["sender"]["user_id"].ToString(), out r) ? r : -1;
                        if (item.IsAdmin)
                        {
                            bool Admin = false;
                            if (Global.Settings_bot.PermissionList.Contains(user_id))
                            {
                                Admin = true;
                            }
                            else if (Global.Settings_bot.GivePermissionToAllAdmin && (jsonObject["sender"]["role"].ToString() == "admin" || jsonObject["sender"]["role"].ToString() == "owner"))
                            {
                                Admin = true;
                            }
                            if (!Admin)
                            {
                                continue;
                            }
                            if (message_type == "group" && Global.Settings_bot.GroupList.Contains(group_id))
                            {
                                Command.command(
                                    item.Command,
                                    Regex.Match(
                                        jsonObject["raw_message"].ToString(),
                                        item.Regex
                                    ),
                                    user_id,
                                    group_id
                                );
                                Command.test(item);
                            }
                            else if(message_type == "private")
                            {
                                Command.command(
                                    item.Command,
                                    Regex.Match(
                                        jsonObject["raw_message"].ToString(),
                                        item.Regex
                                        ),
                                    user_id
                                );
                                Command.test(item);
                            }


                        }
                        else
                        {
                            if (message_type == "group" && Global.Settings_bot.GroupList.Contains(group_id))
                            {
                                Command.command(
                                    item.Command,
                                    Regex.Match(
                                        jsonObject["raw_message"].ToString(),
                                        item.Regex
                                    ),
                                    user_id,
                                    group_id
                                );
                                Command.test(item);
                            }
                            else if (message_type == "private")
                            {
                                Command.command(
                                    item.Command,
                                    Regex.Match(
                                        jsonObject["raw_message"].ToString(),
                                        item.Regex
                                        ),
                                    user_id
                                );
                                Command.test(item);
                            }
                        }
                    }
                }
            }
        }
        public static void ProcessMsgFromTask(string text)
        {

        }
    }
}

