using Serein.Base.Motd;
using Serein.Base.Packets;
using Serein.Core.JSPlugin;
using Serein.Core.Server;
using Serein.Utils;
using Serein.Utils.IO;
using Serein.Utils.Output;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Serein.Core.Common
{
    internal static class Command
    {
        /// <summary>
        /// 启动cmd.exe
        /// </summary>
        /// <param name="command">执行的命令</param>
        private static void StartShell(string command)
        {
            Process process =
                new()
                {
                    StartInfo = new()
                    {
                        FileName =
                            Environment.OSVersion.Platform == PlatformID.Win32NT
                                ? "cmd.exe"
                                : "/bin/bash",
                        UseShellExecute = false,
                        RedirectStandardInput = true,
                        CreateNoWindow = true,
                        WorkingDirectory = Global.PATH
                    }
                };
            process.Start();
            StreamWriter commandWriter =
                new(process.StandardInput.BaseStream, Encoding.Default) { AutoFlush = true };
            commandWriter.WriteLine(command.TrimEnd('\r', '\n'));
            commandWriter.Close();
            Task.Run(() =>
            {
                process.WaitForExit(600000);
                if (!process.HasExited)
                {
#if NET
                    process.Kill(true);
#else
                    process.Kill();
#endif
                }
                process.Dispose();
            });
        }

        /// <summary>
        /// 处理Serein命令
        /// </summary>
        /// <param name="originType">输入类型</param>
        /// <param name="command">命令</param>
        public static void Run(Base.CommandOrigin originType, string command) =>
            Run(originType, command, null, false);

        /// <summary>
        /// 处理Serein命令
        /// </summary>
        /// <param name="originType">输入类型</param>
        /// <param name="command">命令</param>
        /// <param name="message">数据包</param>
        /// <param name="disableMotd">禁用Motd获取</param>
        public static void Run(
            Base.CommandOrigin originType,
            string command,
            Message? message,
            bool disableMotd = false
        ) => Run(originType, command, null, message, disableMotd);

        /// <summary>
        /// 处理Serein命令
        /// </summary>
        /// <param name="originType">输入类型</param>
        /// <param name="command">命令</param>
        /// <param name="msgMatch">消息匹配对象</param>
        public static void Run(Base.CommandOrigin originType, string command, Match msgMatch) =>
            Run(originType, command, msgMatch, null, false);

        /// <summary>
        /// 处理Serein命令
        /// </summary>
        /// <param name="originType">输入类型</param>
        /// <param name="command">命令</param>
        /// <param name="msgMatch">消息匹配对象</param>
        /// <param name="message">数据包</param>
        /// <param name="disableMotd">禁用Motd获取</param>
        public static void Run(
            Base.CommandOrigin originType,
            string command,
            Match? msgMatch,
            Message? message,
            bool disableMotd,
            long groupId = 0
        )
        {
            Logger.Output(
                Base.LogType.Debug,
                "命令运行",
                $"originType:{originType} ",
                $"command:{command}"
            );

            if (groupId == 0)
            {
                if (message is null || message.GroupId == 0)
                {
                    if (Global.Settings.Bot.GroupList.Length >= 1)
                    {
                        groupId = Global.Settings.Bot.GroupList[0];
                    }
                }
                else
                {
                    groupId = message.GroupId;
                }
            }

            Base.CommandType type = GetType(command);
            if (
                type != Base.CommandType.Invalid
                && !(
                    (
                        type == Base.CommandType.RequestMotdpe
                        || type == Base.CommandType.RequestMotdje
                    ) && disableMotd
                )
            ) // EventTrigger的Motd回执
            {
                ExecuteCommand(
                    type,
                    originType,
                    command,
                    ApplyVariables(Format(command, msgMatch), message),
                    groupId,
                    message?.UserId ?? 0,
                    message
                );
            }
            if (
                originType == Base.CommandOrigin.Msg
                && type != Base.CommandType.Bind
                && type != Base.CommandType.Unbind
                && message is not null
                && message.GroupId != 0
            )
            {
                Binder.Update(message);
            }
        }

        /// <summary>
        /// 执行指定的命令
        /// </summary>
        private static void ExecuteCommand(
            Base.CommandType type,
            Base.CommandOrigin originType,
            string command,
            string value,
            long groupId,
            long userId,
            Message? message
        )
        {
            switch (type)
            {
                case Base.CommandType.ExecuteShellCmd:
                    StartShell(value);
                    break;

                case Base.CommandType.ServerInput:
                case Base.CommandType.ServerInputWithUnicode:
                    if (Global.Settings.Bot.EnbaleParseAt && originType == Base.CommandOrigin.Msg)
                    {
                        value = ParseAt(value, groupId);
                    }
                    value = Regex.Replace(
                        Regex.Replace(value, @"\[CQ:face.+?\]", "[表情]"),
                        @"\[CQ:([^,]+?),.+?\]",
                        "[$1]"
                    );
                    ServerManager.InputCommand(
                        value,
                        type == Base.CommandType.ServerInputWithUnicode,
                        true
                    );
                    break;

                case Base.CommandType.SendGivenGroupMsg:
                    Websocket.Send(
                        false,
                        value,
                        Regex.Match(command, @"(\d+)\|").Groups[1].Value,
                        originType != Base.CommandOrigin.EventTrigger
                    );
                    break;

                case Base.CommandType.SendGivenPrivateMsg:
                    Websocket.Send(
                        true,
                        value,
                        Regex.Match(command, @"(\d+)\|").Groups[1].Value,
                        originType != Base.CommandOrigin.EventTrigger
                    );
                    break;

                case Base.CommandType.SendGroupMsg:
                    Websocket.Send(
                        false,
                        value,
                        groupId,
                        originType != Base.CommandOrigin.EventTrigger
                    );
                    break;

                case Base.CommandType.SendPrivateMsg:
                    if (
                        originType == Base.CommandOrigin.Msg
                        || originType == Base.CommandOrigin.EventTrigger
                    )
                    {
                        Websocket.Send(
                            true,
                            value,
                            userId,
                            originType != Base.CommandOrigin.EventTrigger
                        );
                    }
                    break;

                case Base.CommandType.SendTempMsg:
                    if (originType == Base.CommandOrigin.Msg && groupId > 0 && userId > 0)
                    {
                        Websocket.Send(groupId, userId, value);
                    }
                    break;

                case Base.CommandType.Bind:
                    if (
                        (
                            originType == Base.CommandOrigin.Msg
                            || originType == Base.CommandOrigin.EventTrigger
                        )
                        && message?.MessageType == "group"
                    )
                    {
                        Binder.Bind(message, value);
                    }
                    break;

                case Base.CommandType.Unbind:
                    if (
                        (
                            originType == Base.CommandOrigin.Msg
                            || originType == Base.CommandOrigin.EventTrigger
                        )
                        && message?.MessageType == "group"
                    )
                    {
                        Binder.UnBind(message);
                    }
                    break;

                case Base.CommandType.RequestMotdpe:
                    if (originType == Base.CommandOrigin.Msg && message?.MessageType == "group")
                    {
                        Motd motd = new Motdpe(value);
                        EventTrigger.Trigger(
                            motd.IsSuccessful
                                ? Base.EventType.RequestingMotdpeSucceed
                                : Base.EventType.RequestingMotdFail,
                            message,
                            motd
                        );
                    }
                    break;

                case Base.CommandType.RequestMotdje:
                    if (originType == Base.CommandOrigin.Msg && message?.MessageType == "group")
                    {
                        Motd motd = new Motdje(value);
                        EventTrigger.Trigger(
                            motd.IsSuccessful
                                ? Base.EventType.RequestingMotdjeSucceed
                                : Base.EventType.RequestingMotdFail,
                            message,
                            motd
                        );
                    }
                    break;

                case Base.CommandType.ExecuteJavascriptCodes:
                    if (originType != Base.CommandOrigin.Javascript)
                    {
                        Task.Run(() => JSEngineFactory.Create().Execute(value));
                    }
                    break;

                case Base.CommandType.ExecuteJavascriptCodesWithNamespace:
                    if (originType != Base.CommandOrigin.Javascript)
                    {
                        string key = Regex.Match(command, @"^(javascript|js):([^\|]+)\|").Groups[
                            2
                        ].Value;
                        Task.Run(() =>
                        {
                            if (
                                JSPluginManager.PluginDict.TryGetValue(key, out Plugin? plugin)
                                && plugin.Available
                            )
                            {
                                string e;
                                lock (plugin.Engine!)
                                {
                                    plugin.Engine.Run(value, out e);
                                }
                                if (!string.IsNullOrEmpty(e))
                                {
                                    Logger.Output(
                                        Base.LogType.Plugin_Error,
                                        $"[{key}]",
                                        "通过命令执行时错误：\n",
                                        e
                                    );
                                }
                            }
                        });
                    }
                    break;

                case Base.CommandType.Reload:
                    try
                    {
                        FileSaver.Reload(value, originType == Base.CommandOrigin.Msg);
                        if (originType == Base.CommandOrigin.Msg)
                        {
                            Websocket.Send(
                                groupId == 0,
                                "重新加载成功",
                                groupId == 0 ? userId : groupId,
                                false
                            );
                        }
                    }
                    catch (Exception e)
                    {
                        Logger.Output(Base.LogType.Debug, e);
                        if (originType == Base.CommandOrigin.Msg)
                        {
                            Websocket.Send(
                                groupId == 0,
                                $"重新加载失败：{e.Message}",
                                groupId == 0 ? userId : groupId,
                                false
                            );
                        }
                        else if (originType == Base.CommandOrigin.Javascript)
                        {
                            throw new NotSupportedException("重新加载失败", e);
                        }
                    }
                    break;

                case Base.CommandType.DebugOutput:
                    Logger.Output(Base.LogType.Debug, "[DebugOutput]", value);
                    break;

                default:
                    Logger.Output(Base.LogType.Debug, "[Unknown]", value);
                    break;
            }
        }

        /// <summary>
        /// 获取命令类型
        /// </summary>
        /// <param name="command">命令</param>
        /// <returns>类型</returns>
        public static Base.CommandType GetType(string command)
        {
            if (
                string.IsNullOrEmpty(command)
                || !command.Contains("|")
                || !Regex.IsMatch(command, @"^.+?\|[\s\S]+$", RegexOptions.IgnoreCase)
            )
            {
                return Base.CommandType.Invalid;
            }
            switch (Regex.Match(command, @"^([^\|]+?)\|").Groups[1].Value.ToLowerInvariant())
            {
                case "cmd":
                    return Base.CommandType.ExecuteShellCmd;
                case "s":
                case "server":
                    return Base.CommandType.ServerInput;
                case "s:unicode":
                case "server:unicode":
                case "s:u":
                case "server:u":
                    return Base.CommandType.ServerInputWithUnicode;
                case "g":
                case "group":
                    return Base.CommandType.SendGroupMsg;
                case "p":
                case "private":
                    return Base.CommandType.SendPrivateMsg;
                case "t":
                case "temp":
                    return Base.CommandType.SendTempMsg;
                case "b":
                case "bind":
                    return Base.CommandType.Bind;
                case "ub":
                case "unbind":
                    return Base.CommandType.Unbind;
                case "motdpe":
                    return Base.CommandType.RequestMotdpe;
                case "motdje":
                    return Base.CommandType.RequestMotdje;
                case "js":
                case "javascript":
                    return Base.CommandType.ExecuteJavascriptCodes;
                case "reload":
                    return Base.CommandType.Reload;
                case "debug":
                    return Base.CommandType.DebugOutput;
                default:
                    if (Regex.IsMatch(command, @"^(g|group):\d+\|", RegexOptions.IgnoreCase))
                    {
                        return Base.CommandType.SendGivenGroupMsg;
                    }
                    if (Regex.IsMatch(command, @"^(p|private):\d+\|", RegexOptions.IgnoreCase))
                    {
                        return Base.CommandType.SendGivenPrivateMsg;
                    }
                    if (
                        Regex.IsMatch(
                            command,
                            @"^(js|javascript):[^\|]+\|",
                            RegexOptions.IgnoreCase
                        )
                    )
                    {
                        return Base.CommandType.ExecuteJavascriptCodesWithNamespace;
                    }
                    if (
                        Regex.IsMatch(
                            command,
                            @"^(reload)\|(all|regex|schedule|member|groupcache)",
                            RegexOptions.IgnoreCase
                        )
                    )
                    {
                        return Base.CommandType.Reload;
                    }
                    return Base.CommandType.Invalid;
            }
        }

        /// <summary>
        /// 格式化命令
        /// </summary>
        /// <param name="command">命令</param>
        /// <param name="match">正则匹配对象</param>
        /// <returns>值</returns>
        public static string Format(string command, Match? match = null)
        {
            string str = command.Substring(command.IndexOf('|') + 1);
            if (match != null)
            {
                lock (match)
                {
                    for (int i = match.Groups.Count; i >= 0; i--)
                    {
                        str = System.Text.RegularExpressions.Regex.Replace(
                            str,
                            $"\\${i}(?!\\d)",
                            match.Groups[i].Value
                        );
                    }
#if NET
                    // 正则表达式中的分组构造（NET5+)
                    foreach (string key in match.Groups.Keys)
                    {
                        str = str.Replace($"${{{key}}}", match.Groups[key].Value);
                    }
#endif
                }
            }
            Logger.Output(Base.LogType.Debug, str);
            return str;
        }

        /// <summary>
        /// 应用变量
        /// </summary>
        /// <param name="text">文本</param>
        /// <param name="message">数据包</param>
        /// <returns>应用变量后的文本</returns>
        private static string ApplyVariables(string text, Message? message = null)
        {
            if (!text.Contains("%"))
            {
                return text.Replace("\\n", "\n");
            }
            bool serverStatus = ServerManager.Status;
            DateTime currentTime = DateTime.Now;
            text = Patterns.Variable.Replace(
                text,
                (match) =>
                {
                    object? obj = match.Groups[1].Value.ToLowerInvariant() switch
                    {
                        #region 时间
                        "year" => currentTime.Year,
                        "month" => currentTime.Month,
                        "day" => currentTime.Day,
                        "hour" => currentTime.Hour,
                        "minute" => currentTime.Minute,
                        "second" => currentTime.Second,
                        "time" => currentTime.ToString("T"),
                        "date" => currentTime.Date.ToString("d"),
                        "dayofweek" => currentTime.DayOfWeek.ToString(),
                        "datetime" => currentTime.ToString(),
                        #endregion

                        "sereinversion" => Global.VERSION,

                        #region motd
                        "gamemode" => ServerManager.Motd?.GameMode,
                        "description" => ServerManager.Motd?.Description,
                        "protocol" => ServerManager.Motd?.Protocol,
                        "onlineplayer" => ServerManager.Motd?.OnlinePlayer,
                        "maxplayer" => ServerManager.Motd?.MaxPlayer,
                        "original" => ServerManager.Motd?.Origin,
                        "latency" => ServerManager.Motd?.Latency.ToString("N1"),
                        "version" => ServerManager.Motd?.Version,
                        "favicon" => ServerManager.Motd?.FaviconCQCode,
                        "exception" => ServerManager.Motd?.Exception,
                        #endregion

                        #region 系统信息
                        "net" => Environment.Version.ToString(),
                        "cpuusage" => SystemInfo.CPUUsage.ToString("N1"),
                        "os" => SystemInfo.OS,
                        "uploadspeed" => SystemInfo.UploadSpeed,
                        "downloadspeed" => SystemInfo.DownloadSpeed,
                        "cpuname" => SystemInfo.CPUName,
                        "cpubrand" => SystemInfo.CPUBrand,
                        "cpufrequency" => SystemInfo.CPUFrequency.ToString("N1"),
                        "usedram" => SystemInfo.UsedRAM,
                        "usedramgb" => SystemInfo.UsedRAM / 1024,
                        "totalram" => SystemInfo.TotalRAM,
                        "totalramgb" => SystemInfo.TotalRAM / 1024,
                        "freeram" => (SystemInfo.TotalRAM - SystemInfo.UsedRAM),
                        "freeramgb" => (SystemInfo.TotalRAM - SystemInfo.UsedRAM) / 1024,
                        "ramusage"
                            => SystemInfo.RAMUsage > 100
                                ? "100"
                                : SystemInfo.RAMUsage.ToString("N1"),
                        #endregion

                        #region 服务器
                        "levelname" => serverStatus ? ServerManager.LevelName : "-",
                        "difficulty" => serverStatus ? ServerManager.Difficulty : "-",
                        "runtime" => serverStatus ? ServerManager.Time : "-",
                        "servercpuusage"
                            => serverStatus ? ServerManager.CPUUsage.ToString("N1") : "-",
                        "filename" => serverStatus ? ServerManager.StartFileName : "-",
                        "status" => serverStatus ? "已启动" : "未启动",
                        #endregion

                        #region 消息
                        "msgid" => message?.MessageId,
                        "id" => message?.UserId,
                        "gameid" => Binder.GetGameID(message?.UserId ?? 0),
                        "sex" => message?.Sender?.SexName,
                        "nickname" => message?.Sender?.Nickname,
                        "age" => message?.Sender?.Age,
                        "area" => message?.Sender?.Area,
                        "card" => message?.Sender?.Card,
                        "level" => message?.Sender?.Level,
                        "title" => message?.Sender?.Title,
                        "role" => message?.Sender?.RoleName,
                        "shownname"
                            => string.IsNullOrEmpty(message?.Sender?.Card)
                                ? message?.Sender?.Nickname
                                : message?.Sender?.Card,
                        #endregion

                        _
                            => JSPluginManager.CommandVariablesDict.TryGetValue(
                                match.Groups[1].Value,
                                out string? variable
                            )
                                ? variable
                                : match.Groups[0].Value
                    };
                    return obj?.ToString() ?? string.Empty;
                }
            );
            text = Patterns.GameID.Replace(
                text,
                (match) => Binder.GetGameID(long.Parse(match.Groups[1].Value))
            );
            text = Patterns.ID.Replace(
                text,
                (match) => Binder.GetID(match.Groups[1].Value).ToString()
            );
            return text.Replace("\\n", "\n");
        }

        /// <summary>
        /// 转换艾特的CQ码
        /// </summary>
        /// <param name="text">待转换文本</param>
        /// <param name="groupID">群号</param>
        /// <returns>转换后文本</returns>
        public static string ParseAt(string text, long groupID)
        {
            text = text.Replace("[CQ:at,qq=all]", "@全体成员");
            text = Patterns.CQAt.Replace(text, "@$1");
            if (groupID <= 0)
            {
                return text;
            }
            text = Regex.Replace(
                text,
                @"(?<=@)(\d+)",
                (match) =>
                {
                    long userID = long.TryParse(match.Groups[1].Value, out long result)
                        ? result
                        : 0;
                    return
                        Global.GroupCache.TryGetValue(
                            groupID,
                            out Dictionary<long, Base.Member>? groupinfo
                        ) && groupinfo.TryGetValue(userID, out Base.Member? member)
                        ? member.ShownName
                        : match.Groups[1].Value;
                }
            );
            return text;
        }

        public static class Patterns
        {
            public static readonly Regex CQAt = new(@"\[CQ:at,qq=(\d+)\]", RegexOptions.Compiled);

            /// <summary>
            /// 变量正则
            /// </summary>
            public static readonly Regex Variable = new(@"%(\w+)%", RegexOptions.Compiled);

            /// <summary>
            /// 游戏ID正则
            /// </summary>
            public static readonly Regex GameID =
                new(@"%GameID:(\d+)%", RegexOptions.Compiled | RegexOptions.IgnoreCase);

            /// <summary>
            /// ID正则
            /// </summary>
            public static readonly Regex ID =
                new(@"%ID:([^%]+?)%", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        }
    }
}
