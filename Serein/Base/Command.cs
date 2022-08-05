using Newtonsoft.Json.Linq;
using Serein.Items.Motd;
using Serein.Plugin;
using Serein.Server;
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

        /// <summary>
        /// 启动cmd.exe
        /// </summary>
        /// <param name="Command">执行的命令</param>
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
                    WorkingDirectory = Global.Path
                }
            };
            CMDProcess.Start();
            StreamWriter CommandWriter = new StreamWriter(CMDProcess.StandardInput.BaseStream, Encoding.Default)
            {
                AutoFlush = true,
                NewLine = "\r\n"
            };
            CommandWriter.WriteLine(Command.TrimEnd('\r', '\n'));
            CommandWriter.Close();
            Task.Run(() =>
            {
                CMDProcess.WaitForExit(600000);
                if (!CMDProcess.HasExited)
                {
                    CMDProcess.Kill();
                }
                CMDProcess.Dispose();
            });
        }

        /// <summary>
        /// 处理Serein命令
        /// </summary>
        /// <param name="InputType">输入类型</param>
        /// <param name="Command">命令</param>
        /// <param name="JsonObject">消息JSON对象</param>
        /// <param name="MsgMatch">消息匹配对象</param>
        /// <param name="UserId">用户ID</param>
        /// <param name="GroupId">群聊ID</param>
        /// <param name="DisableMotd">禁用Motd获取</param>
        public static void Run(
            int InputType,
            string Command,
            JObject JsonObject = null,
            Match MsgMatch = null,
            long UserId = -1,
            long GroupId = -1,
            bool DisableMotd = false
            )
        {
            /*
                1   QQ消息
                2   控制台输出
                3   定时任务
                4   EventTrigger
            */
            Global.Logger(999,"[Command:Run()]",$"InputType:{InputType} | Command:\"{Command}\" | UserId:\"{UserId}\" | GroupId:\"{GroupId}\"");
            if (GroupId == -1 && Global.Settings.Bot.GroupList.Count >= 1)
            {
                GroupId = Global.Settings.Bot.GroupList[0];
            }
            int Type = GetType(Command);
            if (Type == -1)
            {
                return;
            }
            string Value = GetValue(Command, MsgMatch);
            Value = ApplyVariables(Value, JsonObject, DisableMotd);
            switch (Type)
            {
                case 1:
                    StartCmd(Value);
                    break;
                case 2:
                    Value = Regex.Replace(Value, @"\[CQ:face.+?\]", "[表情]");
                    Value = Regex.Replace(Value, @"\[CQ:([^,]+?),.+?\]", "[CQ:$1]");
                    ServerManager.InputCommand(Value, true);
                    break;
                case 3:
                    Value = Regex.Replace(Value, @"\[CQ:face.+?\]", "[表情]");
                    Value = Regex.Replace(Value, @"\[CQ:([^,]+?),.+?\]", "[CQ:$1]");
                    ServerManager.InputCommand(Value, true, true);
                    break;
                case 11:
                    if (Websocket.Status)
                        Websocket.Send(false, Value, Regex.Match(Command, @"(\d+)\|").Groups[1].Value, InputType != 4);
                    break;
                case 12:
                    if (Websocket.Status)
                        Websocket.Send(true, Value, Regex.Match(Command, @"(\d+)\|").Groups[1].Value, InputType != 4);
                    break;
                case 13:
                    if (Websocket.Status)
                        Websocket.Send(false, Value, GroupId, InputType != 4);
                    break;
                case 14:
                    if ((InputType == 1 || InputType == 4) && Websocket.Status)
                        Websocket.Send(true, Value, UserId, InputType != 4);
                    break;
                case 20:
                    if ((InputType == 1 || InputType == 4) && GroupId != -1)
                        Members.Bind(
                            JsonObject,
                            Value,
                            UserId,
                            GroupId
                            );
                    break;
                case 21:
                    if ((InputType == 1 || InputType == 4) && GroupId != -1)
                        Members.UnBind(
                            long.TryParse(Value, out long i) ? i : -1, GroupId
                            );
                    break;
                case 30:
                    if (InputType == 1 && GroupId != -1)
                    {
                        Motd motd = new Motdpe(Value);
                        EventTrigger.Trigger(
                            motd.Success ? "Motdpe_Success" : "Motd_Failure",
                            GroupId,
                            motd: motd);
                    }
                    break;
                case 31:
                    if (InputType == 1 && GroupId != -1)
                    {
                        Motd motd = new Motdje(Value);
                        EventTrigger.Trigger(
                            motd.Success ? "Motdje_Success" : "Motd_Failure",
                            GroupId,
                            motd: motd);
                    }
                    break;
                case 40:
                    if (Type != 5)
                    {
                        Task.Run(() =>
                        {
                            JSEngine.Init(true).Execute(Value);
                        });
                    }
                    break;
                case 50:
                    Global.Logger(999,"[DebugOutput]", Value);
                    break;
            }
            if (InputType == 1 && Type != 20 && Type != 21 && GroupId != -1)
            {
                Members.Update(JsonObject, UserId);
            }
        }

        /// <summary>
        /// 获取命令类型
        /// </summary>
        /// <param name="Command">命令</param>
        /// <returns>类型</returns>
        public static int GetType(string Command)
        {
            /*
            Type类型     描述
            -1          错误的，会谢的，栓q的，yyds的，暴风吸入的，绝绝子的，属于是的，剁jiojio的，homo特有的，现充的，一整个的，乌鱼子的，集美的，咱就是说的，退退退的，别急的，抛开事实不谈的，9敏的
            1           cmd
            2           服务器命令
            3           服务器命令 with Unicode
            11          群聊（带参）
            12          私聊（带参）
            13          群聊
            14          私聊
            20          绑定id
            21          解绑id
            30          Motdpe
            31          Motdje
            50          debug
            */
            if (
                !Command.Contains("|") ||
                !Regex.IsMatch(Command, @"^.+?\|[\s\S]+$", RegexOptions.IgnoreCase)
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
            if (Regex.IsMatch(Command, @"^motdpe\|", RegexOptions.IgnoreCase))
            {
                return 30;
            }
            if (Regex.IsMatch(Command, @"^motdje\|", RegexOptions.IgnoreCase))
            {
                return 31;
            }
            if (Regex.IsMatch(Command, @"^js\|", RegexOptions.IgnoreCase) ||
                Regex.IsMatch(Command, @"^javascript\|", RegexOptions.IgnoreCase))
            {
                return 40;
            }
            if (Regex.IsMatch(Command, @"^debug\|", RegexOptions.IgnoreCase))
            {
                return 50;
            }
            return -1;
        }

        /// <summary>
        /// 获取命令的值
        /// </summary>
        /// <param name="command">命令</param>
        /// <param name="MsgMatch">消息匹配对象</param>
        /// <returns>值</returns>
        public static string GetValue(string command, Match MsgMatch = null)
        {
            int index = command.IndexOf('|');
            string Value = command.Substring(index + 1);
            if (MsgMatch != null)
            {
                for (int i = MsgMatch.Groups.Count; i >= 0; i--)
                {
                    Value = Value.Replace($"${i}", MsgMatch.Groups[i].Value);
                }
            }
            Global.Logger(999,"[Command:GetValue()]",$"Value:{Value}");
            return Value;
        }

        /// <summary>
        /// 应用变量
        /// </summary>
        /// <param name="Text">文本</param>
        /// <param name="JsonObject">消息JSON对象</param>
        /// <param name="DisableMotd">禁用Motd获取</param>
        /// <returns>应用变量后的文本</returns>
        public static string ApplyVariables(string Text, JObject JsonObject = null, bool DisableMotd = false)
        {
            if (!Text.Contains("%"))
            {
                return Text.Replace("\\n", "\n");
            }
            if (!DisableMotd && Regex.IsMatch(Text, @"%(GameMode|OnlinePlayer|MaxPlayer|Description|Protocol|Original|Delay)%", RegexOptions.IgnoreCase))
            {
                switch (Global.Settings.Server.Type)
                {
                    case 1:
                        Motdpe motdpe = new Motdpe(newPort: Global.Settings.Server.Port.ToString());
                        Text = Regex.Replace(Text, "%GameMode%", motdpe.GameMode, RegexOptions.IgnoreCase);
                        Text = Regex.Replace(Text, "%Description%", motdpe.Description, RegexOptions.IgnoreCase);
                        Text = Regex.Replace(Text, "%Protocol%", motdpe.Protocol, RegexOptions.IgnoreCase);
                        Text = Regex.Replace(Text, "%OnlinePlayer%", motdpe.OnlinePlayer, RegexOptions.IgnoreCase);
                        Text = Regex.Replace(Text, "%MaxPlayer%", motdpe.MaxPlayer, RegexOptions.IgnoreCase);
                        Text = Regex.Replace(Text, "%Original%", motdpe.Original, RegexOptions.IgnoreCase);
                        Text = Regex.Replace(Text, "%Delay%", motdpe.Delay.Milliseconds.ToString(), RegexOptions.IgnoreCase);
                        break;
                    case 2:
                        Motdje motdje = new Motdje(newPort: Global.Settings.Server.Port.ToString());
                        Text = Regex.Replace(Text, "%Description%", motdje.Description, RegexOptions.IgnoreCase);
                        Text = Regex.Replace(Text, "%Protocol%", motdje.Protocol, RegexOptions.IgnoreCase);
                        Text = Regex.Replace(Text, "%OnlinePlayer%", motdje.OnlinePlayer, RegexOptions.IgnoreCase);
                        Text = Regex.Replace(Text, "%MaxPlayer%", motdje.MaxPlayer, RegexOptions.IgnoreCase);
                        Text = Regex.Replace(Text, "%Original%", motdje.Original, RegexOptions.IgnoreCase);
                        Text = Regex.Replace(Text, "%Delay%", motdje.Delay.Milliseconds.ToString(), RegexOptions.IgnoreCase);
                        Text = Regex.Replace(Text, "%Favicon%", motdje.Favicon, RegexOptions.IgnoreCase);
                        break;
                }
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
                    Text = Regex.Replace(Text, "%ID%", JsonObject["sender"]["user_id"].ToString(), RegexOptions.IgnoreCase);
                    Text = Regex.Replace(Text, "%GameID%", Members.GetGameID(long.TryParse(JsonObject["sender"]["user_id"].ToString(), out long result) ? result : -1), RegexOptions.IgnoreCase);
                    Text = Regex.Replace(Text, "%Sex%", Sexs_Chinese[Array.IndexOf(Sexs, JsonObject["sender"]["sex"].ToString())], RegexOptions.IgnoreCase);
                    Text = Regex.Replace(Text, "%Nickname%", JsonObject["sender"]["nickname"].ToString(), RegexOptions.IgnoreCase);
                    Text = Regex.Replace(Text, "%Age%", JsonObject["sender"]["age"].ToString(), RegexOptions.IgnoreCase);
                    Text = Regex.Replace(Text, "%Area%", JsonObject["sender"]["area"].ToString(), RegexOptions.IgnoreCase);
                    Text = Regex.Replace(Text, "%Card%", JsonObject["sender"]["card"].ToString(), RegexOptions.IgnoreCase);
                    Text = Regex.Replace(Text, "%Level%", JsonObject["sender"]["level"].ToString(), RegexOptions.IgnoreCase);
                    Text = Regex.Replace(Text, "%Title%", JsonObject["sender"]["title"].ToString(), RegexOptions.IgnoreCase);
                    Text = Regex.Replace(Text, "%Role%", Roles_Chinese[Array.IndexOf(Roles, JsonObject["sender"]["role"].ToString())], RegexOptions.IgnoreCase);
                    Text = Regex.Replace(Text, "%ShownName%", string.IsNullOrEmpty(JsonObject["sender"]["card"].ToString()) ? JsonObject["sender"]["nickname"].ToString() : JsonObject["sender"]["card"].ToString(), RegexOptions.IgnoreCase);
                }
                catch (Exception e)
                {
                    Global.Logger(999,"[Command:GetVariables()]",e.ToString());
                }
            }
            Text = Regex.Replace(Text, "%NET%", SystemInfo.NET, RegexOptions.IgnoreCase);
            Text = Regex.Replace(Text, "%OS%", SystemInfo.OS, RegexOptions.IgnoreCase);
            Text = Regex.Replace(Text, "%CPUName%", SystemInfo.CPUName, RegexOptions.IgnoreCase);
            Text = Regex.Replace(Text, "%UsedRAM%", SystemInfo.UsedRAM, RegexOptions.IgnoreCase);
            Text = Regex.Replace(Text, "%TotalRAM%", SystemInfo.TotalRAM, RegexOptions.IgnoreCase);
            Text = Regex.Replace(Text, "%RAMPercentage%", SystemInfo.RAMPercentage, RegexOptions.IgnoreCase);
            Text = Regex.Replace(Text, "%CPUPercentage%", SystemInfo.CPUPercentage, RegexOptions.IgnoreCase);
            if (ServerManager.Status)
            {
                Text = Regex.Replace(Text, "%LevelName%", ServerManager.LevelName, RegexOptions.IgnoreCase);
                Text = Regex.Replace(Text, "%Version%", ServerManager.Version, RegexOptions.IgnoreCase);
                Text = Regex.Replace(Text, "%Difficulty%", ServerManager.Difficulty, RegexOptions.IgnoreCase);
                Text = Regex.Replace(Text, "%RunTime%", ServerManager.GetTime(), RegexOptions.IgnoreCase);
                Text = Regex.Replace(Text, "%Percentage%", ServerManager.CPUPersent.ToString("N1"), RegexOptions.IgnoreCase);
                Text = Regex.Replace(Text, "%FileName%", ServerManager.StartFileName, RegexOptions.IgnoreCase);
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
                        ).ToString(),
                    RegexOptions.IgnoreCase
                    );
            }
            return Text.Replace("\\n", "\n");
        }
    }
}
