using Newtonsoft.Json.Linq;
using Serein.Items.Motd;
using Serein.JSPlugin;
using Serein.Server;
using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Serein.Base
{
    internal static class Command
    {
        public static readonly string[] Sexs = { "unknown", "male", "female" };
        public static readonly string[] Sexs_Chinese = { "未知", "男", "女" };
        public static readonly string[] Roles = { "owner", "admin", "member" };
        public static readonly string[] Roles_Chinese = { "群主", "管理员", "成员" };

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
                    CMDProcess.Kill();
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
                5   Javascript
            */
            Logger.Out(
                Items.LogType.Debug,
                    "命令运行",
                    $"InputType:{InputType} ",
                    $"Command:{Command}",
                    $"UserId:{UserId}",
                    $"GroupId:{GroupId}");
            if (GroupId == -1 && Global.Settings.Bot.GroupList.Count >= 1)
                GroupId = Global.Settings.Bot.GroupList[0];
            Items.CommandType Type = GetType(Command);
            if (Type == Items.CommandType.Invalid || ((Type == Items.CommandType.RequestMotdpe || Type == Items.CommandType.RequestMotdje) && DisableMotd))
                return;
            string Value = GetValue(Command, MsgMatch);
            Value = ApplyVariables(Value, JsonObject, DisableMotd);
            switch (Type)
            {
                case Items.CommandType.ExecuteCmd:
                    StartCmd(Value);
                    break;
                case Items.CommandType.ServerInput:
                case Items.CommandType.ServerInputWithUnicode:
                    Value = Regex.Replace(Value, @"\[CQ:face.+?\]", "[表情]");
                    Value = Regex.Replace(Value, @"\[CQ:([^,]+?),.+?\]", "[CQ:$1]");
                    ServerManager.InputCommand(Value, true, Type == Items.CommandType.ServerInputWithUnicode);
                    break;
                case Items.CommandType.SendGivenGroupMsg:
                    if (Websocket.Status)
                        Websocket.Send(false, Value, Regex.Match(Command, @"(\d+)\|").Groups[1].Value, InputType != 4);
                    break;
                case Items.CommandType.SendGivenPrivateMsg:
                    if (Websocket.Status)
                        Websocket.Send(true, Value, Regex.Match(Command, @"(\d+)\|").Groups[1].Value, InputType != 4);
                    break;
                case Items.CommandType.SendGroupMsg:
                    if (Websocket.Status)
                        Websocket.Send(false, Value, GroupId, InputType != 4);
                    break;
                case Items.CommandType.SendPrivateMsg:
                    if ((InputType == 1 || InputType == 4) && Websocket.Status)
                        Websocket.Send(true, Value, UserId, InputType != 4);
                    break;
                case Items.CommandType.Bind:
                    if ((InputType == 1 || InputType == 4) && GroupId != -1)
                        Binder.Bind(
                            JsonObject,
                            Value,
                            UserId,
                            GroupId
                            );
                    break;
                case Items.CommandType.Unbind:
                    if ((InputType == 1 || InputType == 4) && GroupId != -1)
                        Binder.UnBind(
                            long.TryParse(Value, out long i) ? i : -1, GroupId
                            );
                    break;
                case Items.CommandType.RequestMotdpe:
                    if (InputType == 1 && (GroupId != -1 || UserId != -1))
                    {
                        Motd _Motd = new Motdpe(Value);
                        EventTrigger.Trigger(
                            _Motd.Success ? Items.EventType.RequestingMotdpeSucceed : Items.EventType.RequestingMotdFail,
                            GroupId, UserId, _Motd);
                    }
                    break;
                case Items.CommandType.RequestMotdje:
                    if (InputType == 1 && (GroupId != -1 || UserId != -1))
                    {
                        Motd _Motd = new Motdje(Value);
                        EventTrigger.Trigger(
                            _Motd.Success ? Items.EventType.RequestingMotdjeSucceed : Items.EventType.RequestingMotdFail,
                            GroupId, UserId, _Motd);
                    }
                    break;
                case Items.CommandType.ExecuteJavascriptCodes:
                    if (InputType != 5)
                        Task.Run(() => JSEngine.Init(true).Execute(Value));
                    break;
                case Items.CommandType.DebugOutput:
                    Logger.Out(Items.LogType.Debug, "[DebugOutput]", Value);
                    break;
            }
            if (InputType == 1 && Type != Items.CommandType.Bind && Type != Items.CommandType.Unbind && GroupId != -1)
                Binder.Update(JsonObject, UserId);
        }

        /// <summary>
        /// 获取命令类型
        /// </summary>
        /// <param name="Command">命令</param>
        /// <returns>类型</returns>
        public static Items.CommandType GetType(string Command)
        {
            if (
                !Command.Contains("|") ||
                !Regex.IsMatch(Command, @"^.+?\|[\s\S]+$", RegexOptions.IgnoreCase)
                )
                return Items.CommandType.Invalid;
            if (Regex.IsMatch(Command, @"^cmd\|", RegexOptions.IgnoreCase))
                return Items.CommandType.ExecuteCmd;
            if (Regex.IsMatch(Command, @"^s\|", RegexOptions.IgnoreCase) ||
                Regex.IsMatch(Command, @"^server\|", RegexOptions.IgnoreCase))
                return Items.CommandType.ServerInput;
            if (Regex.IsMatch(Command, @"^s:unicode\|", RegexOptions.IgnoreCase) ||
                Regex.IsMatch(Command, @"^server:unicode\|", RegexOptions.IgnoreCase) ||
                Regex.IsMatch(Command, @"^s:u\|", RegexOptions.IgnoreCase) ||
                Regex.IsMatch(Command, @"^server:u\|", RegexOptions.IgnoreCase))
                return Items.CommandType.ServerInputWithUnicode;
            if (Regex.IsMatch(Command, @"^g:\d+\|", RegexOptions.IgnoreCase) ||
                Regex.IsMatch(Command, @"^group:\d+\|", RegexOptions.IgnoreCase))
                return Items.CommandType.SendGivenGroupMsg;
            if (Regex.IsMatch(Command, @"^p:\d+\|", RegexOptions.IgnoreCase) ||
                Regex.IsMatch(Command, @"^private:\d+\|", RegexOptions.IgnoreCase))
                return Items.CommandType.SendGivenPrivateMsg;
            if (Regex.IsMatch(Command, @"^g\|", RegexOptions.IgnoreCase) ||
                Regex.IsMatch(Command, @"^group\|", RegexOptions.IgnoreCase))
                return Items.CommandType.SendGroupMsg;
            if (Regex.IsMatch(Command, @"^p\|", RegexOptions.IgnoreCase) ||
                Regex.IsMatch(Command, @"^private\|", RegexOptions.IgnoreCase))
                return Items.CommandType.SendPrivateMsg;
            if (Regex.IsMatch(Command, @"^b\|", RegexOptions.IgnoreCase) ||
                Regex.IsMatch(Command, @"^bind\|", RegexOptions.IgnoreCase))
                return Items.CommandType.Bind;
            if (Regex.IsMatch(Command, @"^ub\|", RegexOptions.IgnoreCase) ||
                Regex.IsMatch(Command, @"^unbind\|", RegexOptions.IgnoreCase))
                return Items.CommandType.Unbind;
            if (Regex.IsMatch(Command, @"^motdpe\|", RegexOptions.IgnoreCase))
                return Items.CommandType.RequestMotdpe;
            if (Regex.IsMatch(Command, @"^motdje\|", RegexOptions.IgnoreCase))
                return Items.CommandType.RequestMotdje;
            if (Regex.IsMatch(Command, @"^js\|", RegexOptions.IgnoreCase) ||
                Regex.IsMatch(Command, @"^javascript\|", RegexOptions.IgnoreCase))
                return Items.CommandType.ExecuteJavascriptCodes;
            if (Regex.IsMatch(Command, @"^debug\|", RegexOptions.IgnoreCase))
                return Items.CommandType.DebugOutput;
            return Items.CommandType.Invalid;
        }

        /// <summary>
        /// 获取命令的值
        /// </summary>
        /// <param name="command">命令</param>
        /// <param name="MsgMatch">消息匹配对象</param>
        /// <returns>值</returns>
        public static string GetValue(string command, Match MsgMatch = null)
        {
            int Index = command.IndexOf('|');
            string Value = command.Substring(Index + 1);
            if (MsgMatch != null)
                for (int i = MsgMatch.Groups.Count; i >= 0; i--)
                {
                    Value = Value.Replace($"${i}", MsgMatch.Groups[i].Value);
                }
            Logger.Out(Items.LogType.Debug, $"Value:{Value}");
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
            if (Global.Settings.Bot.EnbaleParseAt && JsonObject != null)
            {
                foreach (Match match in Regex.Matches(Text, @"(\[CQ:at,qq=|@)(\d{5,14})\]?"))
                {
                    if (
                        match.Groups.Count >= 3 &&
                        long.TryParse(match.Groups[2].Value, out long ID) &&
                        Global.MemberItems.TryGetValue(ID, out Items.Member Member)
                        )
                    {
                        Text = Text.Replace(
                            match.Value,
                            "@" + (!string.IsNullOrEmpty(Member.Card) ? Member.Card : !string.IsNullOrEmpty(Member.Nickname) ? Member.Nickname : ID.ToString())
                            );
                    }
                }
            }
            if (!Text.Contains("%"))
                return Text.Replace("\\n", "\n");
            if (!DisableMotd && Regex.IsMatch(Text, @"%(GameMode|OnlinePlayer|MaxPlayer|Description|Protocol|Original|Delay|Favicon)%", RegexOptions.IgnoreCase))
                switch (Global.Settings.Server.Type)
                {
                    case 1:
                        Motdpe _Motdpe = new Motdpe(NewPort: Global.Settings.Server.Port.ToString());
                        Text = Regex.Replace(Text, "%GameMode%", _Motdpe.GameMode, RegexOptions.IgnoreCase);
                        Text = Regex.Replace(Text, "%Description%", _Motdpe.Description, RegexOptions.IgnoreCase);
                        Text = Regex.Replace(Text, "%Protocol%", _Motdpe.Protocol, RegexOptions.IgnoreCase);
                        Text = Regex.Replace(Text, "%OnlinePlayer%", _Motdpe.OnlinePlayer, RegexOptions.IgnoreCase);
                        Text = Regex.Replace(Text, "%MaxPlayer%", _Motdpe.MaxPlayer, RegexOptions.IgnoreCase);
                        Text = Regex.Replace(Text, "%Origin%", _Motdpe.Origin, RegexOptions.IgnoreCase);
                        Text = Regex.Replace(Text, "%Delay%", _Motdpe.Delay.Milliseconds.ToString(), RegexOptions.IgnoreCase);
                        break;
                    case 2:
                        Motdje _Motdje = new Motdje(NewPort: Global.Settings.Server.Port.ToString());
                        Text = Regex.Replace(Text, "%Description%", _Motdje.Description, RegexOptions.IgnoreCase);
                        Text = Regex.Replace(Text, "%Protocol%", _Motdje.Protocol, RegexOptions.IgnoreCase);
                        Text = Regex.Replace(Text, "%OnlinePlayer%", _Motdje.OnlinePlayer, RegexOptions.IgnoreCase);
                        Text = Regex.Replace(Text, "%MaxPlayer%", _Motdje.MaxPlayer, RegexOptions.IgnoreCase);
                        Text = Regex.Replace(Text, "%Origin%", _Motdje.Origin, RegexOptions.IgnoreCase);
                        Text = Regex.Replace(Text, "%Delay%", _Motdje.Delay.Milliseconds.ToString(), RegexOptions.IgnoreCase);
                        Text = Regex.Replace(Text, "%Favicon%", _Motdje.Favicon, RegexOptions.IgnoreCase);
                        break;
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
                    Text = Regex.Replace(Text, "%GameID%", Binder.GetGameID(long.TryParse(JsonObject["sender"]["user_id"].ToString(), out long result) ? result : -1), RegexOptions.IgnoreCase);
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
                catch { }
            }
            Text = Regex.Replace(Text, "%NET%", SystemInfo.NET, RegexOptions.IgnoreCase);
            Text = Regex.Replace(Text, "%OS%", SystemInfo.OS, RegexOptions.IgnoreCase);
            Text = Regex.Replace(Text, "%CPUName%", SystemInfo.CPUName, RegexOptions.IgnoreCase);
            Text = Regex.Replace(Text, "%UsedRAM%", SystemInfo.UsedRAM.ToString(), RegexOptions.IgnoreCase);
            Text = Regex.Replace(Text, "%TotalRAM%", SystemInfo.TotalRAM.ToString(), RegexOptions.IgnoreCase);
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
                    Binder.GetGameID(UserId),
                    RegexOptions.IgnoreCase
                    );
            }
            if (Regex.IsMatch(Text, @"%ID:.+?%", RegexOptions.IgnoreCase))
                Text = Regex.Replace(Text, @"%ID:(.+?)%", Binder.GetID(Regex.Match(Text, @"%ID:(.+?)%", RegexOptions.IgnoreCase).Groups[1].Value).ToString(), RegexOptions.IgnoreCase);
            return Text.Replace("\\n", "\n");
        }
    }
}
