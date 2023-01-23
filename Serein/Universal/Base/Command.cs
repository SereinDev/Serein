using Newtonsoft.Json.Linq;
using Serein.Extensions;
using Serein.Items.Motd;
using Serein.JSPlugin;
using Serein.Server;
using System;
using System.Collections.Generic;
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
        /// <param name="userID">用户ID</param>
        /// <param name="groupID">群聊ID</param>
        /// <param name="disableMotd">禁用Motd获取</param>
        public static void Run(
            int inputType,
            string command,
            JObject json = null,
            Match msgMatch = null,
            long userID = -1,
            long groupID = -1,
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
            Logger.Output(
                Items.LogType.Debug,
                    "命令运行",
                    $"inputType:{inputType} ",
                    $"command:{command}",
                    $"userID:{userID}",
                    $"groupID:{groupID}");
            if (groupID == -1 && Global.Settings.Bot.GroupList.Count >= 1)
            {
                groupID = Global.Settings.Bot.GroupList[0];
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
                    if (Global.Settings.Bot.EnbaleParseAt
                        && inputType == 1
                        )
                    {
                        value = ParseAt(value, groupID);
                    }
                    value = Regex.Replace(value, @"\[CQ:face.+?\]", "[表情]");
                    value = Regex.Replace(value, @"\[CQ:([^,]+?),.+?\]", "[$1]");
                    ServerManager.InputCommand(value, type == Items.CommandType.ServerInputWithUnicode, true);
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
                        Websocket.Send(false, value, groupID, inputType != 4);
                    }
                    break;
                case Items.CommandType.SendPrivateMsg:
                    if ((inputType == 1 || inputType == 4) && Websocket.Status)
                    {
                        Websocket.Send(true, value, userID, inputType != 4);
                    }
                    break;
                case Items.CommandType.Bind:
                    if ((inputType == 1 || inputType == 4) && groupID != -1)
                    {
                        Binder.Bind(
                        json,
                        value,
                        userID,
                        groupID
                        );
                    }
                    break;
                case Items.CommandType.Unbind:
                    if ((inputType == 1 || inputType == 4) && groupID != -1)
                    {
                        Binder.UnBind(
                            long.TryParse(value, out long i) ? i : -1, groupID
                            );
                    }
                    break;
                case Items.CommandType.RequestMotdpe:
                    if (inputType == 1 && (groupID != -1 || userID != -1))
                    {
                        Motd _Motd = new Motdpe(value);
                        EventTrigger.Trigger(
                            _Motd.IsSuccessful ? Items.EventType.RequestingMotdpeSucceed : Items.EventType.RequestingMotdFail,
                            groupID, userID, _Motd);
                    }
                    break;
                case Items.CommandType.RequestMotdje:
                    if (inputType == 1 && (groupID != -1 || userID != -1))
                    {
                        Motd _Motd = new Motdje(value);
                        EventTrigger.Trigger(
                            _Motd.IsSuccessful ? Items.EventType.RequestingMotdjeSucceed : Items.EventType.RequestingMotdFail,
                            groupID, userID, _Motd);
                    }
                    break;
                case Items.CommandType.ExecuteJavascriptCodes:
                    if (inputType != 5)
                    {
                        Task.Run(() => JSEngine.Init(true).Execute(value));
                    }
                    break;
                case Items.CommandType.DebugOutput:
                    Logger.Output(Items.LogType.Debug, "[DebugOutput]", value);
                    break;
                default:
                    Logger.Output(Items.LogType.Debug, "[Unknown]", value);
                    break;
            }
            if (inputType == 1 && type != Items.CommandType.Bind && type != Items.CommandType.Unbind && groupID != -1)
            {
                Binder.Update(json, userID);
            }
        }

        /// <summary>
        /// 获取命令类型
        /// </summary>
        /// <param name="command">命令</param>
        /// <returns>类型</returns>
        public static Items.CommandType GetType(string command)
        {
            if (!command.Contains("|") ||
                !Regex.IsMatch(command, @"^.+?\|[\s\S]+$", RegexOptions.IgnoreCase))
            {
                return Items.CommandType.Invalid;
            }
            switch (Regex.Match(command, @"^([^\|]+?)\|").Groups[1].Value.ToLowerInvariant())
            {
                case "cmd":
                    return Items.CommandType.ExecuteCmd;
                case "s":
                case "server":
                    return Items.CommandType.ServerInput;
                case "s:unicode":
                case "server:unicode":
                case "s:u":
                case "server:u":
                    return Items.CommandType.ServerInputWithUnicode;
                case "g":
                case "group":
                    return Items.CommandType.SendGroupMsg;
                case "p":
                case "private":
                    return Items.CommandType.SendPrivateMsg;
                case "b":
                case "bind":
                    return Items.CommandType.Bind;
                case "ub":
                case "unbind":
                    return Items.CommandType.Unbind;
                case "motdpe":
                    return Items.CommandType.RequestMotdpe;
                case "motdje":
                    return Items.CommandType.RequestMotdje;
                case "js":
                case "javascript":
                    return Items.CommandType.ExecuteJavascriptCodes;
                case "debug":
                    return Items.CommandType.DebugOutput;
                default:
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
                    return Items.CommandType.Invalid;
            }
        }

        /// <summary>
        /// 获取命令的值
        /// </summary>
        /// <param name="command">命令</param>
        /// <param name="match">消息匹配对象</param>
        /// <returns>值</returns>
        public static string GetValue(string command, Match match = null)
        {
            string value = command.Substring(command.IndexOf('|') + 1);
            if (match != null)
            {
                for (int i = match.Groups.Count; i >= 0; i--)
                {
                    value = value.Replace($"${i}", match.Groups[i].Value);
                }
            }
            Logger.Output(Items.LogType.Debug, value);
            return value;
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
            if (!text.Contains("%"))
            {
                return text.Replace("\\n", "\n");
            }
            bool serverStatus = ServerManager.Status;
            DateTime CurrentTime = DateTime.Now;
            text = Patterns.Variable.Replace(text, (match) =>
                match.Groups[1].Value.ToLowerInvariant() switch
                {
                    #region 时间
                    "year" => CurrentTime.Year.ToString(),
                    "month" => CurrentTime.Month.ToString(),
                    "day" => CurrentTime.Day.ToString(),
                    "hour" => CurrentTime.Hour.ToString(),
                    "minute" => CurrentTime.Minute.ToString(),
                    "second" => CurrentTime.Second.ToString(),
                    "time" => CurrentTime.ToString("T"),
                    "date" => CurrentTime.Date.ToString("d"),
                    "dayofweek" => CurrentTime.DayOfWeek.ToString(),
                    "datetime" => CurrentTime.ToString(),
                    #endregion

                    "sereinversion" => Global.VERSION,

                    #region motd
                    "gamemode" => ServerManager.Motd.GameMode,
                    "description" => ServerManager.Motd.Description,
                    "protocol" => ServerManager.Motd.Protocol,
                    "onlineplayer" => ServerManager.Motd.OnlinePlayer.ToString(),
                    "maxplayer" => ServerManager.Motd.MaxPlayer.ToString(),
                    "original" => ServerManager.Motd.Origin,
                    "delay" => ServerManager.Motd.Delay.TotalMilliseconds.ToString("N1"),
                    "version" => ServerManager.Motd.Version,
                    "favicon" => ServerManager.Motd.Favicon,
                    "exception" => ServerManager.Motd.Exception,
                    #endregion

                    #region 系统信息
                    "net" => Environment.Version.ToString(),
#if !UNIX
                    "cpuusage" => SystemInfo.CPUUsage.ToString("N1"),
#else
                    "cpuusage" => "0",
#endif
                    "os" => SystemInfo.OS,
                    "uploadspeed" => SystemInfo.UploadSpeed,
                    "downloadspeed" => SystemInfo.DownloadSpeed,
                    "cpuname" => SystemInfo.CPUName,
                    "cpubrand" => SystemInfo.CPUBrand,
                    "cpufrequency" => SystemInfo.CPUFrequency.ToString("N1"),
                    "usedram" => SystemInfo.UsedRAM.ToString(),
                    "usedramgb" => (SystemInfo.UsedRAM / 1024).ToString("N1"),
                    "totalram" => SystemInfo.TotalRAM.ToString(),
                    "totalramgb" => (SystemInfo.TotalRAM / 1024).ToString("N1"),
                    "freeram" => (SystemInfo.Info.Hardware.RAM.Free / 1024 / 1024).ToString("N1"),
                    "freeramgb" => (SystemInfo.Info.Hardware.RAM.Free / 1024 / 1024 / 1024).ToString("N1"),
                    "ramusage" => SystemInfo.RAMUsage.ToString("N1"),
                    #endregion

                    #region 服务器
                    "levelname" => serverStatus ? ServerManager.LevelName : "-",
                    "difficulty" => serverStatus ? ServerManager.Difficulty : "-",
                    "runtime" => serverStatus ? ServerManager.GetTime() : "-",
                    "servercpuusage" => serverStatus ? ServerManager.CPUUsage.ToString("N1") : "-",
                    "filename" => serverStatus ? ServerManager.StartFileName : "-",
                    "status" => serverStatus ? "已启动" : "未启动",
                    #endregion

                    #region 消息
                    "id" => jsonObject.TryGetString("sender", "user_id"),
                    "gameid" => Binder.GetGameID(long.TryParse(jsonObject.TryGetString("sender", "user_id"), out long result) ? result : -1),
                    "sex" => Sexs_Chinese[Array.IndexOf(Sexs, jsonObject.TryGetString("sender", "sex"))],
                    "nickname" => jsonObject.TryGetString("sender", "nickname"),
                    "age" => jsonObject.TryGetString("sender", "age"),
                    "area" => jsonObject.TryGetString("sender", "area"),
                    "card" => jsonObject.TryGetString("sender", "card"),
                    "level" => jsonObject.TryGetString("sender", "level"),
                    "title" => jsonObject.TryGetString("sender", "title"),
                    "role" => Roles_Chinese[Array.IndexOf(Roles, jsonObject.TryGetString("sender", "role"))],
                    "shownname" => string.IsNullOrEmpty(jsonObject.TryGetString("sender", "card")) ? jsonObject.TryGetString("sender", "nickname") : jsonObject.TryGetString("sender", "card"),
                    #endregion

                    _ => match.Groups[0].Value
                }
            );
            text = Patterns.GameID.Replace(text,
                (match) => Binder.GetGameID(long.Parse(match.Groups[1].Value)));
            text = Patterns.ID.Replace(text,
                (match) => Binder.GetID(match.Groups[1].Value).ToString());
            return text.Replace("\\n", "\n");
        }

        public static string ParseAt(string text, long groupID)
        {
            text = text.Replace("[CQ:at,qq=all]", "@全体成员");
            text = Patterns.CQAt.Replace(text, "@$1 ");
            if (groupID > 0)
            {
                text = Regex.Replace(text, @"(?<=@)(\d+)(?=\s)", (match) =>
                {
                    long userID = long.TryParse(match.Groups[1].Value, out long result) ? result : 0;
                    return Global.GroupCache.TryGetValue(groupID, out Dictionary<long, string> groupinfo) &&
                        groupinfo.TryGetValue(userID, out string shownname) ? shownname : match.Groups[1].Value;
                });
            }
            return text;
        }

        protected static class Patterns
        {
            public static readonly Regex CQAt = new(@"\[CQ:at,qq=(\d+)\]", RegexOptions.Compiled);

            /// <summary>
            /// 变量正则
            /// </summary>
            public static readonly Regex Variable = new(@"%(\w+)%", RegexOptions.Compiled);

            /// <summary>
            /// 游戏ID正则
            /// </summary>
            public static readonly Regex GameID = new(@"%GameID:(\d+)%", RegexOptions.Compiled);

            /// <summary>
            /// ID正则
            /// </summary>
            public static readonly Regex ID = new(@"%ID:(.+)%", RegexOptions.Compiled);
        }
    }
}
