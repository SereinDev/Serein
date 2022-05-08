using Newtonsoft.Json.Linq;
using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Serein
{
    class Command
    {
        public static string[] Sexs = { "unknown", "male", "female" };
        public static string[] Sexs_Chinese = { "未知", "男", "女" };
        public static string[] Roles = { "owner", "admin", "member" };
        public static string[] Roles_Chinese = { "群主", "管理员", "成员" };
        public static void StartCmd(string Command)
        {
            Process CMDProcess = new Process();
            CMDProcess.StartInfo.FileName = "cmd.exe";
            CMDProcess.StartInfo.UseShellExecute = false;
            CMDProcess.StartInfo.RedirectStandardInput = true;
            CMDProcess.StartInfo.CreateNoWindow = true;
            CMDProcess.Start();
            StreamWriter CommandWriter = new StreamWriter(CMDProcess.StandardInput.BaseStream, Encoding.Default);
            CommandWriter.AutoFlush = true;
            CommandWriter.NewLine = "\r\n";
            CommandWriter.WriteLine("chcp 936");
            CommandWriter.WriteLine($"cd \"{Global.Path}\"");
            CommandWriter.WriteLine("cls");
            CommandWriter.WriteLine(Command.TrimEnd('\r', '\n') + "&exit");
            CommandWriter.Close();
            CMDProcess.WaitForExit();
            CMDProcess.Close();
        }
        public static void Run(string Command, Match InputMatch = null)
        {
            int Type = GetType(Command);
            if (Type == -1)
            {
                return;
            }
            string Value = GetValue(Command, InputMatch);
            Value = GetVariables(Value);
            if (Type == 1)
            {
                Task CMDTask = new Task(() =>
                  {
                      StartCmd(Value);
                  });
                CMDTask.Start();
            }
            else if (Type == 2 && Server.Status)
            {
                Server.InputCommand(Value);
            }
            else if (Type == 3 && Websocket.Status)
            {
                Websocket.Send(false, Value, Regex.Match(Command, @"(\d+)\|").Groups[1].Value);
            }
            else if (Type == 4 && Websocket.Status)
            {
                Websocket.Send(true, Value, Regex.Match(Command, @"(\d+)\|").Groups[1].Value);
            }
            else if (Type == 5 && Websocket.Status && Global.Settings_bot.GroupList.Length >= 1)
            {
                Websocket.Send(false, Value, Global.Settings_bot.GroupList[0].ToString());
            }
        }

        public static void Run(JObject JsonObject, string Command, Match MsgMatch, int UserId, int GroupId = -1)
        {
            int Type = GetType(Command);
            if (Type == -1)
            {
                return;
            }
            string Value = GetValue(Command, MsgMatch);
            Value = GetVariables(Value, JsonObject);
            if (Type == 1)
            {
                Task CMDTask = new Task(() =>
                {
                    StartCmd(Value);
                });
                CMDTask.Start();
            }
            else if (Type == 2 && Server.Status)
            {
                Server.InputCommand(Value);
            }
            else if (Type == 3 && Websocket.Status)
            {
                Websocket.Send(false, Value, Regex.Match(Command, @"(\d+)\|").Groups[1].Value);
            }
            else if (Type == 4 && Websocket.Status)
            {
                Websocket.Send(true, Value, Regex.Match(Command, @"(\d+)\|").Groups[1].Value);
            }
            else if (Type == 5 && Websocket.Status)
            {
                Websocket.Send(false, Value, GroupId.ToString());
            }
            else if (Type == 6 && Websocket.Status)
            {
                Websocket.Send(true, Value, UserId.ToString());
            }
        }

        public static int GetType(string Command)
        {
            if (!Command.Contains("|"))
            {
                return -1;
            }
            if (!Regex.IsMatch(Command, @"^.+?\|.+$", RegexOptions.IgnoreCase))
            {
                return -1;
            }
            if (Regex.IsMatch(Command, @"^cmd\|", RegexOptions.IgnoreCase))
            {
                return 1;
            }
            else if (Regex.IsMatch(Command, @"^s\|", RegexOptions.IgnoreCase) || Regex.IsMatch(Command, @"^server\|", RegexOptions.IgnoreCase))
            {
                return 2;
            }
            else if (Regex.IsMatch(Command, @"^g:\d+\|", RegexOptions.IgnoreCase) || Regex.IsMatch(Command, @"^group:\d+\|", RegexOptions.IgnoreCase))
            {
                return 3;
            }
            else if (Regex.IsMatch(Command, @"^p:\d+\|", RegexOptions.IgnoreCase) || Regex.IsMatch(Command, @"^private:\d+\|", RegexOptions.IgnoreCase))
            {
                return 4;
            }
            else if (Regex.IsMatch(Command, @"^g\|", RegexOptions.IgnoreCase) || Regex.IsMatch(Command, @"^group\|", RegexOptions.IgnoreCase))
            {
                return 5;
            }
            else if (Regex.IsMatch(Command, @"^p\|", RegexOptions.IgnoreCase) || Regex.IsMatch(Command, @"^private\|", RegexOptions.IgnoreCase))
            {
                return 6;
            }
            else
            {
                return -1;
            }
        }
        public static string GetValue(string command, Match MsgMatch)
        {
            int index = command.IndexOf('|');
            string Value = command.Substring(index + 1);
            if (MsgMatch == null)
            {
                return Value;
            }
            for (int i = 1; i < MsgMatch.Groups.Count; i++)
            {
                Value = Value.Replace($"${i}", MsgMatch.Groups[i].Value);
            }
            return Value;
        }
        public static string GetVariables(string Text, JObject JsonObject = null)
        {
            DateTime CurrentTime = DateTime.Now;
            Text = Text.Replace("%DateTime-Year%", CurrentTime.Year.ToString());
            Text = Text.Replace("%DateTime-Month%", CurrentTime.Month.ToString());
            Text = Text.Replace("%DateTime-Day%", CurrentTime.Day.ToString());
            Text = Text.Replace("%DateTime-Hour%", CurrentTime.Hour.ToString());
            Text = Text.Replace("%DateTime-Minute%", CurrentTime.Minute.ToString());
            Text = Text.Replace("%DateTime-Second%", CurrentTime.Second.ToString());
            Text = Text.Replace("%DateTime%", CurrentTime.ToString());
            if (JsonObject != null)
            {
                try
                {
                    Text = Text.Replace(
                        "%QQ-Age%",
                        JsonObject["sender"]["age"].ToString()
                        );
                    Text = Text.Replace(
                        "%QQ-ID%",
                        JsonObject["sender"]["user_id"].ToString()
                        );
                    Text = Text.Replace(
                        "%QQ-Area%",
                        JsonObject["sender"]["area"].ToString());
                    Text = Text.Replace(
                        "%QQ-Card%",
                        JsonObject["sender"]["card"].ToString());
                    Text = Text.Replace(
                        "%QQ-Level%",
                        JsonObject["sender"]["level"].ToString());
                    Text = Text.Replace(
                        "%QQ-Nickname%",
                        JsonObject["sender"]["nickname"].ToString());
                    Text = Text.Replace(
                        "%QQ-Title%",
                        JsonObject["sender"]["title"].ToString());
                    Text = Text.Replace(
                        "%QQ-Role%",
                        Roles_Chinese[Array.IndexOf(
                            Roles,
                            JsonObject["sender"]["role"].ToString()
                            )]);
                    Text = Text.Replace(
                        "%QQ-Sex%",
                        Sexs_Chinese[Array.IndexOf(
                            Sexs,
                            JsonObject["sender"]["sex"].ToString()
                            )]);
                    Text = Text.Replace(
                        "%QQ-ShownName%",
                        string.IsNullOrEmpty(
                            JsonObject["sender"]["card"].ToString()
                            )
                        ? JsonObject["sender"]["nickname"].ToString()
                        : JsonObject["sender"]["card"].ToString()
                        );
                }
                catch
                {

                }
            }
            return Text.Replace("\\n", "\n");
        }
    }
}
