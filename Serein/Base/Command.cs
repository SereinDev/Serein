using Newtonsoft.Json.Linq;
using Serein.Items;
using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Serein.Base
{
    internal class Command
    {
        public static string[] Sexs = { "unknown", "male", "female" };
        public static string[] Sexs_Chinese = { "未知", "男", "女" };
        public static string[] Roles = { "owner", "admin", "member" };
        public static string[] Roles_Chinese = { "群主", "管理员", "成员" };
        public static void StartCmd(string Command)
        {
            Process CMDProcess = new Process()
            {
                StartInfo = new ProcessStartInfo()
                {
                    FileName = "cmd.exe",
                    UseShellExecute = false,
                    RedirectStandardInput = true,
                    CreateNoWindow = true,
                    WorkingDirectory = Global.Path,
                    StandardErrorEncoding = Encoding.UTF8,
                    StandardOutputEncoding = Encoding.UTF8
                }
            };
            CMDProcess.Start();
            StreamWriter CommandWriter = new StreamWriter(CMDProcess.StandardInput.BaseStream, Encoding.Default)
            {
                AutoFlush = true,
                NewLine = "\r\n"
            };
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
                new Task(() =>
                {
                    StartCmd(Value);
                }).Start();
            }
            else if (Type == 2)
            {
                Server.InputCommand(Value, true);
            }
            else if (Type == 3)
            {
                Server.InputCommand(Value, true, true);
            }
            else if (Type == 11 && Websocket.Status)
            {
                Websocket.Send(false, Value, Regex.Match(Command, @"(\d+)\|").Groups[1].Value);
            }
            else if (Type == 12 && Websocket.Status)
            {
                Websocket.Send(true, Value, Regex.Match(Command, @"(\d+)\|").Groups[1].Value);
            }
            else if (Type == 13 && Websocket.Status && Global.Settings.Bot.GroupList.Count >= 1)
            {
                Websocket.Send(false, Value, Global.Settings.Bot.GroupList[0].ToString());
            }
            else if (Type == 50)
            {
                Global.Debug("[DebugOutput]" + Value);
            }
        }

        public static void Run(JObject JsonObject, string Command, Match MsgMatch, long UserId, long GroupId = -1)
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
                new Task(() =>
                {
                    StartCmd(Value);
                }).Start();
            }
            else if (Type == 2)
            {
                Value = Regex.Replace(Value, @"\[CQ:face.+?\]", "[表情]");
                Value = Regex.Replace(Value, @"\[CQ:([^,]+?),.+?\]", "[CQ:$1]");
                Server.InputCommand(Value, true);
            }
            else if (Type == 3)
            {
                Value = Regex.Replace(Value, @"\[CQ:face.+?\]", "[表情]");
                Value = Regex.Replace(Value, @"\[CQ:([^,]+?),.+?\]", "[CQ:$1]");
                Server.InputCommand(Value, true, true);
            }
            else if (Type == 11 && Websocket.Status)
            {
                Websocket.Send(false, Value, Regex.Match(Command, @"(\d+)\|").Groups[1].Value);
            }
            else if (Type == 12 && Websocket.Status)
            {
                Websocket.Send(true, Value, Regex.Match(Command, @"(\d+)\|").Groups[1].Value);
            }
            else if (Type == 13 && Websocket.Status)
            {
                Websocket.Send(false, Value, GroupId.ToString());
            }
            else if (Type == 14 && Websocket.Status)
            {
                Websocket.Send(true, Value, UserId.ToString());
            }
            else if (Type == 20 && GroupId != -1)
            {
                Websocket.Send(
                    false,
                    $"[CQ:at,qq={UserId}] " +
                    Members.Bind(
                        JsonObject,
                        Value,
                        UserId
                        ),
                    GroupId.ToString()
                    );
            }
            else if (Type == 21 && GroupId != -1)
            {
                Websocket.Send(
                    false,
                    $"[CQ:at,qq={UserId}] " +
                    Members.UnBind(
                        long.TryParse(Value, out long i) ? i : -1
                        ),
                    GroupId.ToString()
                    );
            }
            else if (Type == 50)
            {
                Global.Debug("[DebugOutput]" + Value);
            }
            if (Type != 20 && Type != 21 && GroupId != -1)
            {
                Members.Update(JsonObject, UserId);
            }
        }

        public static int GetType(string Command)
        {
            /*
            Type类型     描述
            -1          错误的、不合法的、未知的
            1           cmd
            2           服务器命令
            3           服务器命令 with Unicode
            11          群聊（带参）
            12          私聊（带参）
            13          群聊
            14          私聊
            20          绑定id
            21          解绑id
            50          debug
            */
            if (
                !Command.Contains("|") ||
                !Regex.IsMatch(Command, @"^.+?\|.+$", RegexOptions.IgnoreCase)
                )
            {
                return -1;
            }
            if (Regex.IsMatch(Command, @"^cmd\|", RegexOptions.IgnoreCase))
            {
                return 1;
            }
            if (Regex.IsMatch(Command, @"^s\|", RegexOptions.IgnoreCase) ||
                Regex.IsMatch(Command, @"^server\|", RegexOptions.IgnoreCase))
            {
                return 2;
            }
            if (Regex.IsMatch(Command, @"^s:unicode\|", RegexOptions.IgnoreCase) ||
                Regex.IsMatch(Command, @"^server:unicode\|", RegexOptions.IgnoreCase) ||
                Regex.IsMatch(Command, @"^s:u\|", RegexOptions.IgnoreCase) ||
                Regex.IsMatch(Command, @"^server:u\|", RegexOptions.IgnoreCase))
            {
                return 3;
            }
            if (Regex.IsMatch(Command, @"^g:\d+\|", RegexOptions.IgnoreCase) ||
                Regex.IsMatch(Command, @"^group:\d+\|", RegexOptions.IgnoreCase))
            {
                return 11;
            }
            if (Regex.IsMatch(Command, @"^p:\d+\|", RegexOptions.IgnoreCase) ||
                Regex.IsMatch(Command, @"^private:\d+\|", RegexOptions.IgnoreCase))
            {
                return 12;
            }
            if (Regex.IsMatch(Command, @"^g\|", RegexOptions.IgnoreCase) ||
                Regex.IsMatch(Command, @"^group\|", RegexOptions.IgnoreCase))
            {
                return 13;
            }
            if (Regex.IsMatch(Command, @"^p\|", RegexOptions.IgnoreCase) ||
                Regex.IsMatch(Command, @"^private\|", RegexOptions.IgnoreCase))
            {
                return 14;
            }
            if (Regex.IsMatch(Command, @"^b\|", RegexOptions.IgnoreCase) ||
                Regex.IsMatch(Command, @"^bind\|", RegexOptions.IgnoreCase))
            {
                return 20;
            }
            if (Regex.IsMatch(Command, @"^ub\|", RegexOptions.IgnoreCase) ||
                Regex.IsMatch(Command, @"^unbind\|", RegexOptions.IgnoreCase))
            {
                return 21;
            }
            if (Regex.IsMatch(Command, @"^debug\|", RegexOptions.IgnoreCase))
            {
                return 50;
            }

            return -1;
        }
        public static string GetValue(string command, Match MsgMatch)
        {
            int index = command.IndexOf('|');
            string Value = command.Substring(index + 1);
            if (MsgMatch == null)
            {
                return Value;
            }
            for (int i = MsgMatch.Groups.Count; i >= 0; i--)
            {
                Value = Value.Replace($"${i}", MsgMatch.Groups[i].Value);
            }
            Global.Debug($"[Command] Command:{command} Value:{Value}");
            return Value;
        }
        public static string GetVariables(string Text, JObject JsonObject = null)
        {
            if (!Text.Contains("%"))
            {
                return Text.Replace("\\n", "\n");
            }
            if (Regex.IsMatch(Text, @"%(GameMode|OnlinePlayer|MaxPlayer|Description|Protocol|Original)%", RegexOptions.IgnoreCase))
            {
                Motdpe motdpe = new Motdpe(newPort: Server.Port);
                Text = Regex.Replace(Text, "%GameMode%", motdpe.GameMode, RegexOptions.IgnoreCase);
                Text = Regex.Replace(Text, "%Description%", motdpe.Description, RegexOptions.IgnoreCase);
                Text = Regex.Replace(Text, "%Protocol%", motdpe.Protocol, RegexOptions.IgnoreCase);
                Text = Regex.Replace(Text, "%OnlinePlayer%", motdpe.OnlinePlayer, RegexOptions.IgnoreCase);
                Text = Regex.Replace(Text, "%MaxPlayer%", motdpe.MaxPlayer, RegexOptions.IgnoreCase);
                Text = Regex.Replace(Text, "%Original%", motdpe.Original, RegexOptions.IgnoreCase);
            }
            DateTime CurrentTime = DateTime.Now;
            Text = Regex.Replace(Text, "%Year%", CurrentTime.Year.ToString(), RegexOptions.IgnoreCase);
            Text = Regex.Replace(Text, "%Month%", CurrentTime.Month.ToString(), RegexOptions.IgnoreCase);
            Text = Regex.Replace(Text, "%Day%", CurrentTime.Day.ToString(), RegexOptions.IgnoreCase);
            Text = Regex.Replace(Text, "%Hour%", CurrentTime.Hour.ToString(), RegexOptions.IgnoreCase);
            Text = Regex.Replace(Text, "%Minute%", CurrentTime.Minute.ToString(), RegexOptions.IgnoreCase);
            Text = Regex.Replace(Text, "%Second%", CurrentTime.Second.ToString(), RegexOptions.IgnoreCase);
            Text = Regex.Replace(Text, "%Time%", CurrentTime.ToString("T"), RegexOptions.IgnoreCase);
            Text = Regex.Replace(Text, "%Date%", CurrentTime.Date.ToString("d"), RegexOptions.IgnoreCase);
            Text = Regex.Replace(Text, "%DayOfWeek%", CurrentTime.DayOfWeek.ToString(), RegexOptions.IgnoreCase);
            Text = Regex.Replace(Text, "%DateTime%", CurrentTime.ToString(), RegexOptions.IgnoreCase);
            Text = Regex.Replace(Text, "%SereinVersion%", Global.VERSION, RegexOptions.IgnoreCase);
            if (JsonObject != null)
            {
                try
                {
                    Text = Regex.Replace(Text, "%Sex%", Sexs_Chinese[Array.IndexOf(Sexs, JsonObject["sender"]["sex"].ToString())], RegexOptions.IgnoreCase);
                    Text = Regex.Replace(Text, "%Nickname%", JsonObject["sender"]["nickname"].ToString(), RegexOptions.IgnoreCase);
                    Text = Regex.Replace(Text, "%Age%", JsonObject["sender"]["age"].ToString(), RegexOptions.IgnoreCase);
                    Text = Regex.Replace(Text, "%ID%", JsonObject["sender"]["user_id"].ToString(), RegexOptions.IgnoreCase);
                    Text = Regex.Replace(Text, "%Area%", JsonObject["sender"]["area"].ToString(), RegexOptions.IgnoreCase);
                    Text = Regex.Replace(Text, "%Card%", JsonObject["sender"]["card"].ToString(), RegexOptions.IgnoreCase);
                    Text = Regex.Replace(Text, "%Level%", JsonObject["sender"]["level"].ToString(), RegexOptions.IgnoreCase);
                    Text = Regex.Replace(Text, "%Title%", JsonObject["sender"]["title"].ToString(), RegexOptions.IgnoreCase);
                    Text = Regex.Replace(Text, "%Role%", Roles_Chinese[Array.IndexOf(Roles, JsonObject["sender"]["role"].ToString())], RegexOptions.IgnoreCase);
                    Text = Regex.Replace(Text, "%ShownName%", string.IsNullOrEmpty(JsonObject["sender"]["card"].ToString()) ? JsonObject["sender"]["nickname"].ToString() : JsonObject["sender"]["card"].ToString(), RegexOptions.IgnoreCase);
                }
                catch { }
            }
            Text = Regex.Replace(Text, "%NET%", SystemInfo.NET, RegexOptions.IgnoreCase);
            Text = Regex.Replace(Text, "%OS%", SystemInfo.OS, RegexOptions.IgnoreCase);
            Text = Regex.Replace(Text, "%CPUName%", SystemInfo.CPUName, RegexOptions.IgnoreCase);
            Text = Regex.Replace(Text, "%UsedRAM%", SystemInfo.UsedRAM, RegexOptions.IgnoreCase);
            Text = Regex.Replace(Text, "%TotalRAM%", SystemInfo.TotalRAM, RegexOptions.IgnoreCase);
            Text = Regex.Replace(Text, "%RAMPercentage%", SystemInfo.RAMPercentage, RegexOptions.IgnoreCase);
            Text = Regex.Replace(Text, "%CPUPercentage%", SystemInfo.CPUPercentage, RegexOptions.IgnoreCase);
            if (Server.Status)
            {
                Text = Regex.Replace(Text, "%LevelName%", Server.LevelName, RegexOptions.IgnoreCase);
                Text = Regex.Replace(Text, "%Version%", Server.Version, RegexOptions.IgnoreCase);
                Text = Regex.Replace(Text, "%Difficulty%", Server.Difficulty, RegexOptions.IgnoreCase);
                Text = Regex.Replace(Text, "%RunTime%", Server.GetTime(), RegexOptions.IgnoreCase);
                Text = Regex.Replace(Text, "%Percentage%", Server.CPUPersent.ToString("N1"), RegexOptions.IgnoreCase);
                Text = Regex.Replace(Text, "%FileName%", Server.StartFileName, RegexOptions.IgnoreCase);
                Text = Regex.Replace(Text, "%Status%", "已启动", RegexOptions.IgnoreCase);
            }
            else
            {
                Text = Regex.Replace(Text, "%LevelName%", "-", RegexOptions.IgnoreCase);
                Text = Regex.Replace(Text, "%Version%", "-", RegexOptions.IgnoreCase);
                Text = Regex.Replace(Text, "%Difficulty%", "-", RegexOptions.IgnoreCase);
                Text = Regex.Replace(Text, "%RunTime%", "-", RegexOptions.IgnoreCase);
                Text = Regex.Replace(Text, "%Percentage%", "-", RegexOptions.IgnoreCase);
                Text = Regex.Replace(Text, "%FileName%", "-", RegexOptions.IgnoreCase);
                Text = Regex.Replace(Text, "%Status%", "未启动", RegexOptions.IgnoreCase);
            }
            if (Regex.IsMatch(Text, @"%GameID:\d+%", RegexOptions.IgnoreCase))
            {
                long UserId = long.TryParse(
                    Regex.Match(
                        Text,
                        @"%GameID:(\d+?)%",
                        RegexOptions.IgnoreCase
                        ).Groups[1].Value,
                    out long i) ? i : -1;
                Text = Regex.Replace(
                    Text,
                    @"%GameID:(\d+?)%",
                    Members.GetGameID(UserId),
                    RegexOptions.IgnoreCase
                    );
            }
            if (Regex.IsMatch(Text, @"%ID:.+?%", RegexOptions.IgnoreCase))
            {
                Text = Regex.Replace(
                    Text,
                    @"%ID:(.+?)%",
                    Members.GetID(
                        Regex.Match(
                            Text,
                            @"%ID:(.+?)%",
                            RegexOptions.IgnoreCase
                            ).Groups[1].Value
                        ),
                    RegexOptions.IgnoreCase
                    );
            }
            return Text.Replace("\\n", "\n");
        }
    }
}
