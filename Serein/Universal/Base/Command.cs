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
        public static void StartCmd(string command)
        {
            Process process = new()
            {
                StartInfo = new()
                {
                    FileName = "cmd.exe",
                    UseShellExecute = false,
                    RedirectStandardInput = true,
                    CreateNoWindow = true,
                    WorkingDirectory = Global.Path
                }
            };
            process.Start();
            StreamWriter commandWriter = new(process.StandardInput.BaseStream, Encoding.Default)
            {
                AutoFlush = true,
                NewLine = "\r\n"
            };
            commandWriter.WriteLine(command.TrimEnd('\r', '\n'));
            commandWriter.Close();
            Task.Run(() =>
            {
                process.WaitForExit(600000);
                if (!process.HasExited)
                {
                    process.Kill();
                }
                process.Dispose();
            });
        }

        /// <summary>
        /// 处理Serein命令
        /// </summary>
        /// <param name="inputType">输入类型</param>
        /// <param name="command">命令</param>
        /// <param name="json">消息JSON对象</param>
        /// <param name="msgMatch">消息匹配对象</param>
        /// <param name="userId">用户ID</param>
        /// <param name="groupId">群聊ID</param>
        /// <param name="disableMotd">禁用Motd获取</param>
        public static void Run(
            int inputType,
            string command,
            JObject json = null,
            Match msgMatch = null,
            long userId = -1,
            long groupId = -1,
            bool disableMotd = false
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
                    $"inputType:{inputType} ",
                    $"command:{command}",
                    $"userId:{userId}",
                    $"groupId:{groupId}");
            if (groupId == -1 && Global.Settings.Bot.GroupList.Count >= 1)
            {
                groupId = Global.Settings.Bot.GroupList[0];
            }
            Items.CommandType type = GetType(command);
            if (type == Items.CommandType.Invalid || ((type == Items.CommandType.RequestMotdpe || type == Items.CommandType.RequestMotdje) && disableMotd))
            {
                return;
            }
            string value = GetValue(command, msgMatch);
            value = ApplyVariables(value, json, disableMotd);
            switch (type)
            {
                case Items.CommandType.ExecuteCmd:
                    StartCmd(value);
                    break;
                case Items.CommandType.ServerInput:
                case Items.CommandType.ServerInputWithUnicode:
                    value = Regex.Replace(value, @"\[CQ:face.+?\]", "[表情]");
                    value = Regex.Replace(value, @"\[CQ:([^,]+?),.+?\]", "[CQ:$1]");
                    ServerManager.InputCommand(value, true, type == Items.CommandType.ServerInputWithUnicode);
                    break;
                case Items.CommandType.SendGivenGroupMsg:
                    if (Websocket.Status)
                    {
                        Websocket.Send(false, value, Regex.Match(command, @"(\d+)\|").Groups[1].Value, inputType != 4);
                    }
                    break;
                case Items.CommandType.SendGivenPrivateMsg:
                    if (Websocket.Status)
                    {
                        Websocket.Send(true, value, Regex.Match(command, @"(\d+)\|").Groups[1].Value, inputType != 4);
                    }
                    break;
                case Items.CommandType.SendGroupMsg:
                    if (Websocket.Status)
                    {
                        Websocket.Send(false, value, groupId, inputType != 4);
                    }
                    break;
                case Items.CommandType.SendPrivateMsg:
                    if ((inputType == 1 || inputType == 4) && Websocket.Status)
                    {
                        Websocket.Send(true, value, userId, inputType != 4);
                    }
                    break;
                case Items.CommandType.Bind:
                    if ((inputType == 1 || inputType == 4) && groupId != -1)
                    {
                        Binder.Bind(
                        json,
                        value,
                        userId,
                        groupId
                        );
                    }
                    break;
                case Items.CommandType.Unbind:
                    if ((inputType == 1 || inputType == 4) && groupId != -1)
                    {
                        Binder.UnBind(
                            long.TryParse(value, out long i) ? i : -1, groupId
                            );
                    }
                    break;
                case Items.CommandType.RequestMotdpe:
                    if (inputType == 1 && (groupId != -1 || userId != -1))
                    {
                        Motd _Motd = new Motdpe(value);
                        EventTrigger.Trigger(
                            _Motd.IsSuccessful ? Items.EventType.RequestingMotdpeSucceed : Items.EventType.RequestingMotdFail,
                            groupId, userId, _Motd);
                    }
                    break;
                case Items.CommandType.RequestMotdje:
                    if (inputType == 1 && (groupId != -1 || userId != -1))
                    {
                        Motd _Motd = new Motdje(value);
                        EventTrigger.Trigger(
                            _Motd.IsSuccessful ? Items.EventType.RequestingMotdjeSucceed : Items.EventType.RequestingMotdFail,
                            groupId, userId, _Motd);
                    }
                    break;
                case Items.CommandType.ExecuteJavascriptCodes:
                    if (inputType != 5)
                    {
                        Task.Run(() => JSEngine.Init(true).Execute(value));
                    }
                    break;
                case Items.CommandType.DebugOutput:
                    Logger.Out(Items.LogType.Debug, "[DebugOutput]", value);
                    break;
                default:
                    Logger.Out(Items.LogType.Debug, "[Unknown]", value);
                    break;
            }
            if (inputType == 1 && type != Items.CommandType.Bind && type != Items.CommandType.Unbind && groupId != -1)
            {
                Binder.Update(json, userId);
            }
        }

        /// <summary>
        /// 获取命令类型
        /// </summary>
        /// <param name="command">命令</param>
        /// <returns>类型</returns>
        public static Items.CommandType GetType(string command)
        {
            if (
                !command.Contains("|") ||
                !Regex.IsMatch(command, @"^.+?\|[\s\S]+$", RegexOptions.IgnoreCase)
                )
            {
                return Items.CommandType.Invalid;
            }
            if (Regex.IsMatch(command, @"^cmd\|", RegexOptions.IgnoreCase))
            {
                return Items.CommandType.ExecuteCmd;
            }
            if (Regex.IsMatch(command, @"^s\|", RegexOptions.IgnoreCase) ||
                Regex.IsMatch(command, @"^server\|", RegexOptions.IgnoreCase))
            {
                return Items.CommandType.ServerInput;
            }
            if (Regex.IsMatch(command, @"^s:unicode\|", RegexOptions.IgnoreCase) ||
                Regex.IsMatch(command, @"^server:unicode\|", RegexOptions.IgnoreCase) ||
                Regex.IsMatch(command, @"^s:u\|", RegexOptions.IgnoreCase) ||
                Regex.IsMatch(command, @"^server:u\|", RegexOptions.IgnoreCase))
            {
                return Items.CommandType.ServerInputWithUnicode;
            }
            if (Regex.IsMatch(command, @"^g:\d+\|", RegexOptions.IgnoreCase) ||
                Regex.IsMatch(command, @"^group:\d+\|", RegexOptions.IgnoreCase))
            {
                return Items.CommandType.SendGivenGroupMsg;
            }
            if (Regex.IsMatch(command, @"^p:\d+\|", RegexOptions.IgnoreCase) ||
                Regex.IsMatch(command, @"^private:\d+\|", RegexOptions.IgnoreCase))
            {
                return Items.CommandType.SendGivenPrivateMsg;
            }
            if (Regex.IsMatch(command, @"^g\|", RegexOptions.IgnoreCase) ||
                Regex.IsMatch(command, @"^group\|", RegexOptions.IgnoreCase))
            {
                return Items.CommandType.SendGroupMsg;
            }
            if (Regex.IsMatch(command, @"^p\|", RegexOptions.IgnoreCase) ||
                Regex.IsMatch(command, @"^private\|", RegexOptions.IgnoreCase))
            {
                return Items.CommandType.SendPrivateMsg;
            }
            if (Regex.IsMatch(command, @"^b\|", RegexOptions.IgnoreCase) ||
                Regex.IsMatch(command, @"^bind\|", RegexOptions.IgnoreCase))
            {
                return Items.CommandType.Bind;
            }
            if (Regex.IsMatch(command, @"^ub\|", RegexOptions.IgnoreCase) ||
                Regex.IsMatch(command, @"^unbind\|", RegexOptions.IgnoreCase))
            {
                return Items.CommandType.Unbind;
            }
            if (Regex.IsMatch(command, @"^motdpe\|", RegexOptions.IgnoreCase))
            {
                return Items.CommandType.RequestMotdpe;
            }
            if (Regex.IsMatch(command, @"^motdje\|", RegexOptions.IgnoreCase))
            {
                return Items.CommandType.RequestMotdje;
            }
            if (Regex.IsMatch(command, @"^js\|", RegexOptions.IgnoreCase) ||
                Regex.IsMatch(command, @"^javascript\|", RegexOptions.IgnoreCase))
            {
                return Items.CommandType.ExecuteJavascriptCodes;
            }
            if (Regex.IsMatch(command, @"^debug\|", RegexOptions.IgnoreCase))
            {
                return Items.CommandType.DebugOutput;
            }
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
            {
                for (int i = MsgMatch.Groups.Count; i >= 0; i--)
                {
                    Value = Value.Replace($"${i}", MsgMatch.Groups[i].Value);
                }
            }
            Logger.Out(Items.LogType.Debug, $"Value:{Value}");
            return Value;
        }

        /// <summary>
        /// 应用变量
        /// </summary>
        /// <param name="text">文本</param>
        /// <param name="jsonObject">消息JSON对象</param>
        /// <param name="disableMotd">禁用Motd获取</param>
        /// <returns>应用变量后的文本</returns>
        public static string ApplyVariables(string text, JObject jsonObject = null, bool disableMotd = false)
        {
            if (Global.Settings.Bot.EnbaleParseAt && jsonObject != null)
            {
                foreach (Match match in Regex.Matches(text, @"(\[CQ:at,qq=|@)(\d{5,14})\]?"))
                {
                    if (
                        match.Groups.Count >= 3 &&
                        long.TryParse(match.Groups[2].Value, out long ID) &&
                        Global.MemberItems.TryGetValue(ID, out Items.Member Member)
                        )
                    {
                        text = text.Replace(
                            match.Value,
                            "@" + (!string.IsNullOrEmpty(Member.Card) ? Member.Card : !string.IsNullOrEmpty(Member.Nickname) ? Member.Nickname : ID.ToString())
                            );
                    }
                }
            }
            if (!text.Contains("%"))
            {
                return text.Replace("\\n", "\n");
            }
            if (!disableMotd && Global.Settings.Server.Type != 0 && Regex.IsMatch(text, @"%(GameMode|OnlinePlayer|MaxPlayer|Description|Protocol|Original|Delay|Favicon)%", RegexOptions.IgnoreCase))
            {
                Motd motd;
                if (Global.Settings.Server.Type == 1)
                {
                    motd = new Motdpe($"127.0.0.1:{Global.Settings.Server.Port}");
                }
                else
                {
                    motd = new Motdje($"127.0.0.1:{Global.Settings.Server.Port}");
                }
                if (motd.IsSuccessful) { }
                text = Regex.Replace(text, "%GameMode%", motd.GameMode, RegexOptions.IgnoreCase);
                text = Regex.Replace(text, "%Description%", motd.Description, RegexOptions.IgnoreCase);
                text = Regex.Replace(text, "%Protocol%", motd.Protocol, RegexOptions.IgnoreCase);
                text = Regex.Replace(text, "%OnlinePlayer%", motd.OnlinePlayer.ToString(), RegexOptions.IgnoreCase);
                text = Regex.Replace(text, "%MaxPlayer%", motd.MaxPlayer.ToString(), RegexOptions.IgnoreCase);
                text = Regex.Replace(text, "%Original%", motd.Origin, RegexOptions.IgnoreCase);
                text = Regex.Replace(text, "%Delay%", motd.Delay.TotalMilliseconds.ToString("N1"), RegexOptions.IgnoreCase);
                text = Regex.Replace(text, "%Version%", motd.Version, RegexOptions.IgnoreCase);
                text = Regex.Replace(text, "%Favicon%", motd.Favicon, RegexOptions.IgnoreCase);
                text = Regex.Replace(text, "%Exception%", motd.Exception, RegexOptions.IgnoreCase);
            }
            DateTime CurrentTime = DateTime.Now;
            text = Regex.Replace(text, "%Year%", CurrentTime.Year.ToString(), RegexOptions.IgnoreCase);
            text = Regex.Replace(text, "%Month%", CurrentTime.Month.ToString(), RegexOptions.IgnoreCase);
            text = Regex.Replace(text, "%Day%", CurrentTime.Day.ToString(), RegexOptions.IgnoreCase);
            text = Regex.Replace(text, "%Hour%", CurrentTime.Hour.ToString(), RegexOptions.IgnoreCase);
            text = Regex.Replace(text, "%Minute%", CurrentTime.Minute.ToString(), RegexOptions.IgnoreCase);
            text = Regex.Replace(text, "%Second%", CurrentTime.Second.ToString(), RegexOptions.IgnoreCase);
            text = Regex.Replace(text, "%Time%", CurrentTime.ToString("T"), RegexOptions.IgnoreCase);
            text = Regex.Replace(text, "%Date%", CurrentTime.Date.ToString("d"), RegexOptions.IgnoreCase);
            text = Regex.Replace(text, "%DayOfWeek%", CurrentTime.DayOfWeek.ToString(), RegexOptions.IgnoreCase);
            text = Regex.Replace(text, "%DateTime%", CurrentTime.ToString(), RegexOptions.IgnoreCase);
            text = Regex.Replace(text, "%SereinVersion%", Global.VERSION, RegexOptions.IgnoreCase);
            if (jsonObject != null)
            {
                try
                {
                    text = Regex.Replace(text, "%ID%", jsonObject["sender"]["user_id"].ToString(), RegexOptions.IgnoreCase);
                    text = Regex.Replace(text, "%GameID%", Binder.GetGameID(long.TryParse(jsonObject["sender"]["user_id"].ToString(), out long result) ? result : -1), RegexOptions.IgnoreCase);
                    text = Regex.Replace(text, "%Sex%", Sexs_Chinese[Array.IndexOf(Sexs, jsonObject["sender"]["sex"].ToString())], RegexOptions.IgnoreCase);
                    text = Regex.Replace(text, "%Nickname%", jsonObject["sender"]["nickname"].ToString(), RegexOptions.IgnoreCase);
                    text = Regex.Replace(text, "%Age%", jsonObject["sender"]["age"].ToString(), RegexOptions.IgnoreCase);
                    text = Regex.Replace(text, "%Area%", jsonObject["sender"]["area"].ToString(), RegexOptions.IgnoreCase);
                    text = Regex.Replace(text, "%Card%", jsonObject["sender"]["card"].ToString(), RegexOptions.IgnoreCase);
                    text = Regex.Replace(text, "%Level%", jsonObject["sender"]["level"].ToString(), RegexOptions.IgnoreCase);
                    text = Regex.Replace(text, "%Title%", jsonObject["sender"]["title"].ToString(), RegexOptions.IgnoreCase);
                    text = Regex.Replace(text, "%Role%", Roles_Chinese[Array.IndexOf(Roles, jsonObject["sender"]["role"].ToString())], RegexOptions.IgnoreCase);
                    text = Regex.Replace(text, "%ShownName%", string.IsNullOrEmpty(jsonObject["sender"]["card"].ToString()) ? jsonObject["sender"]["nickname"].ToString() : jsonObject["sender"]["card"].ToString(), RegexOptions.IgnoreCase);
                }
                catch (Exception e)
                {
                    Logger.Out(Items.LogType.Debug, e);
                }
            }
            text = Regex.Replace(text, "%NET%", Environment.Version.ToString(), RegexOptions.IgnoreCase);
#if !UNIX
            text = Regex.Replace(text, "%CPUUsage%", SystemInfo.CPUUsage.ToString("N1"), RegexOptions.IgnoreCase);
#else
            text = Regex.Replace(text, "%CPUUsage%", "-", RegexOptions.IgnoreCase);
#endif
            text = Regex.Replace(text, "%OS%", SystemInfo.OS, RegexOptions.IgnoreCase);
            text = Regex.Replace(text, "%UploadSpeed%", SystemInfo.UploadSpeed, RegexOptions.IgnoreCase);
            text = Regex.Replace(text, "%DownloadSpeed%", SystemInfo.DownloadSpeed, RegexOptions.IgnoreCase);
            text = Regex.Replace(text, "%CPUName%", SystemInfo.CPUName, RegexOptions.IgnoreCase);
            text = Regex.Replace(text, "%CPUBrand%", SystemInfo.CPUBrand, RegexOptions.IgnoreCase);
            text = Regex.Replace(text, "%CPUFrequency%", SystemInfo.CPUFrequency.ToString("N1"), RegexOptions.IgnoreCase);
            text = Regex.Replace(text, "%UsedRAM%", SystemInfo.UsedRAM.ToString(), RegexOptions.IgnoreCase);
            text = Regex.Replace(text, "%TotalRAM%", SystemInfo.TotalRAM.ToString(), RegexOptions.IgnoreCase);
            text = Regex.Replace(text, "%RAMUsage%", SystemInfo.RAMUsage.ToString("N1"), RegexOptions.IgnoreCase);
            if (ServerManager.Status)
            {
                text = Regex.Replace(text, "%LevelName%", ServerManager.LevelName, RegexOptions.IgnoreCase);
                text = Regex.Replace(text, "%Difficulty%", ServerManager.Difficulty, RegexOptions.IgnoreCase);
                text = Regex.Replace(text, "%RunTime%", ServerManager.GetTime(), RegexOptions.IgnoreCase);
                text = Regex.Replace(text, "%ServerCPUUsage%", ServerManager.CPUUsage.ToString("N1"), RegexOptions.IgnoreCase);
                text = Regex.Replace(text, "%FileName%", ServerManager.StartFileName, RegexOptions.IgnoreCase);
                text = Regex.Replace(text, "%Status%", "已启动", RegexOptions.IgnoreCase);
            }
            else
            {
                text = Regex.Replace(text, "%LevelName%", "-", RegexOptions.IgnoreCase);
                text = Regex.Replace(text, "%Difficulty%", "-", RegexOptions.IgnoreCase);
                text = Regex.Replace(text, "%RunTime%", "-", RegexOptions.IgnoreCase);
                text = Regex.Replace(text, "%ServerCPUUsage%", "0", RegexOptions.IgnoreCase);
                text = Regex.Replace(text, "%FileName%", "-", RegexOptions.IgnoreCase);
                text = Regex.Replace(text, "%Status%", "未启动", RegexOptions.IgnoreCase);
            }
            if (Regex.IsMatch(text, @"%GameID:\d+%", RegexOptions.IgnoreCase))
            {
                long userId = long.TryParse(
                    Regex.Match(
                        text,
                        @"%GameID:(\d+?)%",
                        RegexOptions.IgnoreCase
                        ).Groups[1].Value,
                    out long i) ? i : -1;
                text = Regex.Replace(
                    text,
                    @"%GameID:(\d+?)%",
                    Binder.GetGameID(userId),
                    RegexOptions.IgnoreCase
                    );
            }
            if (Regex.IsMatch(text, @"%ID:.+?%", RegexOptions.IgnoreCase))
            {
                text = Regex.Replace(text, @"%ID:(.+?)%", Binder.GetID(Regex.Match(text, @"%ID:(.+?)%", RegexOptions.IgnoreCase).Groups[1].Value).ToString(), RegexOptions.IgnoreCase);
            }
            return text.Replace("\\n", "\n");
        }
    }
}
