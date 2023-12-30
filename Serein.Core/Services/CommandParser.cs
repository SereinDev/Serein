using System;
using System.Linq;
using System.Text.RegularExpressions;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using Serein.Core.Models.Commands;
using Serein.Core.Models.Server;
using Serein.Core.Services.Server;

namespace Serein.Core.Services;

public class CommandParser
{
    private static readonly Regex GeneralCommand =
        new(
            @"^\[(?<name>[a-zA-Z]+)(:(?<argument>[\w\-\s]+))?\](?<body>.+)$",
            RegexOptions.Compiled
        );
    private static readonly Regex Variable = new(@"\{(\w+)\}", RegexOptions.Compiled);
    private readonly IHost _host;
    private readonly SystemInfoFactory _systemInfoFactory;
    private ServerManager ServerManager => _host.Services.GetRequiredService<ServerManager>();

    public CommandParser(IHost host, SystemInfoFactory systemInfoFactory)
    {
        _host = host;
        _systemInfoFactory = systemInfoFactory;
    }

    public static bool Validate(string? command)
    {
        return !string.IsNullOrEmpty(command) && GeneralCommand.IsMatch(command);
    }

    public static Command Parse(CommandOrigin origin, string? command)
    {
        if (
            string.IsNullOrEmpty(command)
            || command.Length < 3
            || !command.StartsWith('[')
            || !command.Contains(']')
            || !GeneralCommand.IsMatch(command)
        )
            return new() { Origin = origin, Type = CommandType.Invalid };

        var matchResult = GeneralCommand.Match(command);

        if (!matchResult.Success || matchResult.Groups.Count != 5)
            return new() { Origin = origin, Type = CommandType.Invalid };

        var name = matchResult.Groups["name"].Value;
        var body = matchResult.Groups["body"].Value;
        var argument = matchResult.Groups["argument"].Value;

        var type = name switch
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

            "motdpe" => CommandType.RequestMotdpe,
            "motdje" => CommandType.RequestMotdje,

            "js" => CommandType.ExecuteJavascriptCodes,
            "javascript" => CommandType.ExecuteJavascriptCodes,

            "reload" => CommandType.Reload,

            "debug" => CommandType.Debug,
            _ => CommandType.Invalid
        };

        return new()
        {
            Origin = origin,
            Type = type,
            Body = body,
            Argument = argument
        };
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

    private string ApplyVariables(string commandBody, CommandContext? commandContext)
    {
        if (!commandBody.Contains('%'))
            return commandBody.Replace("\\n", "\n");

        var serverStatus = ServerManager.Status;
        var serverInfo = ServerManager.ServerInfo;
        var currentTime = DateTime.Now;

        var text = Variable.Replace(
            commandBody,
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
                    "gamemode" => ServerManager.ServerInfo?.Motd?.GameMode,
                    "description" => ServerManager.ServerInfo?.Motd?.Description,
                    "protocol" => ServerManager.ServerInfo?.Motd?.Protocol,
                    "onlineplayer" => ServerManager.ServerInfo?.Motd?.OnlinePlayers,
                    "maxplayer" => ServerManager.ServerInfo?.Motd?.PlayerCapacity,
                    "original" => ServerManager.ServerInfo?.Motd?.OriginText,
                    "latency" => ServerManager.ServerInfo?.Motd?.Latency.ToString("N1"),
                    "version" => ServerManager.ServerInfo?.Motd?.Version,
                    "favicon" => ServerManager.ServerInfo?.Motd?.FaviconCQCode,
                    "exception" => ServerManager.ServerInfo?.Motd?.Exception,
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
                return obj?.ToString() ?? string.Empty;
            }
        );

        return text.Replace("\\n", "\n");
    }
}
