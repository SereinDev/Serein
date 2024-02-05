using System;
using System.Linq;
using System.Text.RegularExpressions;

using Microsoft.Extensions.Hosting;

using Serein.Core.Models.Commands;
using Serein.Core.Models.Server;
using Serein.Core.Services.Plugins;
using Serein.Core.Services.Server;

namespace Serein.Core.Services;

public class CommandParser
{
    private static readonly Regex GeneralCommand =
        new(
            @"^\[(?<name>[a-zA-Z]+)(:(?<argument>[\w\-\s]+))?\](?<body>.+)$",
            RegexOptions.Compiled
        );
    private static readonly Regex Variable = new(@"\{([a-zA-Z]\w+)\}", RegexOptions.Compiled);
    private readonly PluginHost _pluginHost;
    private readonly SystemInfoFactory _systemInfoFactory;
    private readonly ServerManager _serverManager;

    public CommandParser(
        PluginHost pluginHost,
        SystemInfoFactory systemInfoFactory,
        ServerManager serverManager
    )
    {
        _pluginHost = pluginHost;
        _systemInfoFactory = systemInfoFactory;
        _serverManager = serverManager;
    }

    public static Command Parse(CommandOrigin origin, string? command, bool throws = false)
    {
        var cmd = command?.TrimStart();
        try
        {
            if (string.IsNullOrEmpty(cmd) || string.IsNullOrWhiteSpace(cmd))
                throw new ArgumentException("命令为空", nameof(command));

            if (cmd.Length < 2 || !cmd.StartsWith('[') || !cmd.Contains(']'))
                throw new ArgumentException("缺少命令标识：'['和']'", nameof(command));

            var matchResult = GeneralCommand.Match(cmd);

            if (!matchResult.Success || matchResult.Groups.Count != 5)
                throw new NotSupportedException("命令语法不正确");

            var name = matchResult.Groups["name"].Value;
            var body = matchResult.Groups["body"].Value;
            var argument = matchResult.Groups["argument"].Value;

            var type = name.ToLowerInvariant() switch
            {
                "cmd" => CommandType.ExecuteShellCommand,

                "s" => CommandType.ServerInput,
                "server" => CommandType.ServerInput,

                "g" => CommandType.SendGroupMsg,
                "group" => CommandType.SendGroupMsg,
                "p" => CommandType.SendPrivateMsg,
                "private" => CommandType.SendPrivateMsg,

                "b" => CommandType.Bind,
                "bind" => CommandType.Bind,
                "ub" => CommandType.Unbind,
                "unbind" => CommandType.Unbind,

                "js" => CommandType.ExecuteJavascriptCodes,
                "javascript" => CommandType.ExecuteJavascriptCodes,

                "reload" => CommandType.Reload,

                "debug" => CommandType.Debug,
                _ => throw new NotSupportedException("命令类型无效")
            };

            return new()
            {
                Origin = origin,
                Type = type,
                Body = body,
                Argument = argument
            };
        }
        catch
        {
            if (throws)
                throw;

            return new() { Origin = origin, Type = CommandType.Invalid };
        }
    }

    public string Format(string? commandBody, CommandContext? commandContext)
    {
        if (string.IsNullOrEmpty(commandBody))
            return string.Empty;

        var argument = ApplyVariables(commandBody, commandContext);

        if (commandContext?.Match != null)
            foreach (var key in commandContext.Match.Groups.Keys)
            {
                argument = argument.Replace($"{{{key}}}", commandContext.Match.Groups[key].Value);
            }

        return argument;
    }

#pragma warning disable IDE0046
    public string ApplyVariables(
        string line,
        CommandContext? commandContext,
        bool removeInvalidVariablePatten = false
    )
    {
        if (!line.Contains('{') || !line.Contains('}'))
            return line.Replace("\\n", "\n");

        var serverStatus = _serverManager.Status;
        var serverInfo = _serverManager.ServerInfo;
        var currentTime = DateTime.Now;

        var text = Variable.Replace(
            line,
            (match) =>
            {
                var name = match.Groups[1].Value.ToLowerInvariant();

                if (
                    commandContext?.Variables is not null
                    && commandContext.Variables.TryGetValue(name, out string? value)
                    && value is not null
                )
                    return value;

                if (_pluginHost.Variables.TryGetValue(name, out value) && value is not null)
                    return value;

                object? obj = name switch
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

                    "sereinversion" => SereinApp.Version,
                    "sereintype" => SereinApp.Type,

                    #region 服务器
                    "filename" => serverInfo?.FileName,
                    "outputlines" => serverInfo?.OutputLines,
                    "inputlines" => serverInfo?.InputLines,
                    "gamemode" => serverInfo?.Stat?.Gamemode,
                    "description" => serverInfo?.Stat?.Stripped_Motd,
                    "protocol" => serverInfo?.Stat?.Protocol,
                    "currentplayers" => serverInfo?.Stat?.CurrentPlayers,
                    "maximumplayers" => serverInfo?.Stat?.MaximumPlayers,
                    "latency"
                        => serverInfo?.Stat is not null
                            ? (serverInfo.Stat.Latency / 1000).ToString("N1")
                            : "?",
                    "version" => serverInfo?.Stat?.Version,
                    "favicon" => serverInfo?.Stat?.Favicon,
                    "status" => serverStatus == ServerStatus.Running ? "已启动" : "未启动",
                    #endregion

                    #region 系统信息
                    "netversion" => Environment.Version.ToString(),
                    "cpuusage" => (_systemInfoFactory.CPUUsage * 100).ToString("N1"),
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

                    #region 消息
                    "msgid" => commandContext?.MessagePacket?.MessageId,
                    "id" => commandContext?.MessagePacket?.UserId,
                    "nickname" => commandContext?.MessagePacket?.Sender?.Nickname,
                    "title" => commandContext?.MessagePacket?.Sender?.Title,
                    "role" => commandContext?.MessagePacket?.Sender?.RoleName,
                    "shownname"
                        => string.IsNullOrEmpty(commandContext?.MessagePacket?.Sender?.Card)
                            ? commandContext?.MessagePacket?.Sender?.Nickname
                            : commandContext.MessagePacket?.Sender?.Card,
                    #endregion

                    _ => null
                };

                var r = obj?.ToString();

                if (r is not null)
                    return r;

                return removeInvalidVariablePatten ? string.Empty : match.Value;
            }
        );

        return text.Replace("\\n", "\n");
    }

#pragma warning restore IDE0046
}
