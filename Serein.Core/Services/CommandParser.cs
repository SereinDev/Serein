using System;
using System.Linq;
using System.Text.RegularExpressions;
using RegexMatch = System.Text.RegularExpressions.Match;

using Serein.Core.Models.Commands;
using Serein.Core.Services.Server;
using Serein.Core.Models.Server;

namespace Serein.Core.Services;

public class CommandParser
{
    private readonly Regex _generalCommand = new(@"^.+?\|[\s\S]+$", RegexOptions.Compiled);
    private readonly Regex _commandHeader = new(@"^.+?\|[\s\S]+$", RegexOptions.Compiled);
    private readonly Regex _variable = new(@"%(\w+)%", RegexOptions.Compiled);
    private readonly SystemInfoFactory _systemInfoFactory;
    private readonly ServerManager _serverManager;

    public CommandParser(SystemInfoFactory systemInfoFactory, ServerManager serverManager)
    {
        _systemInfoFactory = systemInfoFactory;
        _serverManager = serverManager;
    }

    public static string Format(string command, RegexMatch? match = null)
    {
        var str = command[(command.IndexOf('|') + 1)..];
        if (match != null)
        {
            lock (match)
            {
                for (int i = match.Groups.Count; i >= 0; i--)
                {
                    str = Regex.Replace(str, $"\\${i}(?!\\d)", match.Groups[i].Value);
                }

                foreach (var key in match.Groups.Keys)
                {
                    str = str.Replace($"${{{key}}}", match.Groups[key].Value);
                }
            }
        }
        return str;
    }

    public CommandType GetCommandType(string command)
    {
        if (
            string.IsNullOrEmpty(command)
            || !command.Contains('|')
            || !_generalCommand.IsMatch(command)
        )
            return CommandType.Invalid;

        return _commandHeader.Match(command).Groups[1].Value.ToLowerInvariant() switch
        {
            "cmd" => CommandType.ExecuteShellCmd,

            "s" => CommandType.ServerInput,
            "server" => CommandType.ServerInput,
            "s:unicode" => CommandType.ServerInputWithUnicode,
            "server:unicode" => CommandType.ServerInputWithUnicode,
            "s:u" => CommandType.ServerInputWithUnicode,
            "server:u" => CommandType.ServerInputWithUnicode,

            "g" => CommandType.SendGroupMsg,
            "group" => CommandType.SendGroupMsg,
            "p" => CommandType.SendPrivateMsg,
            "private" => CommandType.SendPrivateMsg,
            "t" => CommandType.SendTempMsg,
            "temp" => CommandType.SendTempMsg,

            "b" => CommandType.Bind,
            "bind" => CommandType.Bind,
            "ub" => CommandType.Unbind,
            "unbind" => CommandType.Unbind,

            "motdpe" => CommandType.RequestMotdpe,
            "motdje" => CommandType.RequestMotdje,

            "js" => CommandType.ExecuteJavascriptCodes,
            "javascript" => CommandType.ExecuteJavascriptCodes,

            "reload" => CommandType.Reload,
            "debug" => CommandType.Debug,
            _ => HandleOtherSituations()
        };

        CommandType HandleOtherSituations()
        {
            if (Regex.IsMatch(command, @"^(g|group):\d+\|", RegexOptions.IgnoreCase))
                return CommandType.SendGivenGroupMsg;
            if (Regex.IsMatch(command, @"^(p|private):\d+\|", RegexOptions.IgnoreCase))
                return CommandType.SendGivenPrivateMsg;
            if (Regex.IsMatch(command, @"^(js|javascript):[^\|]+\|", RegexOptions.IgnoreCase))
                return CommandType.ExecuteJavascriptCodesWithNamespace;
            if (
                Regex.IsMatch(
                    command,
                    @"^(reload)\|(all|regex|schedule|member|groupcache)",
                    RegexOptions.IgnoreCase
                )
            )
                return CommandType.Reload;

            return CommandType.Invalid;
        }
    }

    public string ApplyVariables(string input)
    {
        if (!input.Contains('%'))
            return input.Replace("\\n", "\n");

        var serverStatus = _serverManager.Status;
        var serverInfo = _serverManager.ServerInfo;
        var currentTime = DateTime.Now;

        var text = _variable.Replace(
            input,
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

                    "sereinversion" => App.Version,

                    #region 服务器
                    "gamemode" => _serverManager.ServerInfo?.Motd?.GameMode,
                    "description" => _serverManager.ServerInfo?.Motd?.Description,
                    "protocol" => _serverManager.ServerInfo?.Motd?.Protocol,
                    "onlineplayer" => _serverManager.ServerInfo?.Motd?.OnlinePlayers,
                    "maxplayer" => _serverManager.ServerInfo?.Motd?.PlayerCapacity,
                    "original" => _serverManager.ServerInfo?.Motd?.OriginText,
                    "latency" => _serverManager.ServerInfo?.Motd?.Latency.ToString("N1"),
                    "version" => _serverManager.ServerInfo?.Motd?.Version,
                    "favicon" => _serverManager.ServerInfo?.Motd?.FaviconCQCode,
                    "exception" => _serverManager.ServerInfo?.Motd?.Exception,
                    "status" => serverStatus == ServerStatus.Running ? "已启动" : "未启动",
                    #endregion

                    #region 系统信息
                    "net" => Environment.Version.ToString(),
                    "cpuusage" => null,
                    "os" => _systemInfoFactory.Info.Name,
                    "uploadspeed" => _systemInfoFactory.UploadSpeed,
                    "downloadspeed" => _systemInfoFactory.DownloadSpeed,
                    "cpuname" => _systemInfoFactory.Info.Hardware.CPUs.FirstOrDefault()?.Name,
                    "cpubrand" => _systemInfoFactory.Info.Hardware.CPUs.FirstOrDefault()?.Brand,
                    "cpufrequency"
                        => _systemInfoFactory.Info.Hardware.CPUs.FirstOrDefault()?.Frequency,
                    "usedram"
                        => (
                            _systemInfoFactory.Info.Hardware.RAM.Total
                            - _systemInfoFactory.Info.Hardware.RAM.Free
                        ) / 1024,
                    "usedramgb"
                        => (
                            (
                                (double)_systemInfoFactory.Info.Hardware.RAM.Total
                                - _systemInfoFactory.Info.Hardware.RAM.Free
                            )
                            / 1024
                            / 1024
                        ).ToString("N1"),
                    "totalram" => _systemInfoFactory.Info.Hardware.RAM.Total / 1024,
                    "totalramgb"
                        => (
                            (double)_systemInfoFactory.Info.Hardware.RAM.Total / 1024 / 1024
                        ).ToString("N1"),
                    "freeram" => _systemInfoFactory.Info.Hardware.RAM.Free / 1024,
                    "freeramgb"
                        => (
                            (double)_systemInfoFactory.Info.Hardware.RAM.Free / 1024 / 1024
                        ).ToString("N1"),
                    "ramusage"
                        => _systemInfoFactory.Info.Hardware.RAM.Total == 0
                            ? null
                            : (
                                100
                                * (
                                    (double)_systemInfoFactory.Info.Hardware.RAM.Total
                                    - _systemInfoFactory.Info.Hardware.RAM.Free
                                )
                                / _systemInfoFactory.Info.Hardware.RAM.Total
                            ).ToString("N1"),
                    #endregion

                    // #region 消息
                    // "msgid" => message?.MessageId,
                    // "id" => message?.UserId,
                    // "sex" => message?.Sender?.SexName,
                    // "nickname" => message?.Sender?.Nickname,
                    // "age" => message?.Sender?.Age,
                    // "area" => message?.Sender?.Area,
                    // "card" => message?.Sender?.Card,
                    // "level" => message?.Sender?.Level,
                    // "title" => message?.Sender?.Title,
                    // "role" => message?.Sender?.RoleName,
                    // "shownname"
                    //     => string.IsNullOrEmpty(message?.Sender?.Card)
                    //         ? message?.Sender?.Nickname
                    //         : message?.Sender?.Card,
                    // #endregion

                    _ => null
                };
                return obj?.ToString() ?? string.Empty;
            }
        );

        return text.Replace("\\n", "\n");
    }
}
