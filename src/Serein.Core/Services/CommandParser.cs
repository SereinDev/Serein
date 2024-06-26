using System;
using System.Linq;
using System.Text.RegularExpressions;

using Serein.Core.Models.Commands;
using Serein.Core.Models.Server;
using Serein.Core.Services.Plugins;
using Serein.Core.Services.Servers;
using Serein.Core.Utils.Extensions;

namespace Serein.Core.Services;

public partial class CommandParser(
    PluginHost pluginHost,
    ServerManager servers,
    HardwareInfoProvider hardwareInfoProvider
)
{
    [GeneratedRegex(@"^\[(?<name>[a-zA-Z]+)(:(?<argument>[\w\-\s]+))?\](?<body>.+)$")]
    private static partial Regex GetGeneralCommand();

    [GeneratedRegex(@"\{([a-zA-Z]+(\.[a-zA-Z]+)?(@\w+)?)\}")]
    private static partial Regex GetVariable();

    public static readonly Regex Variable = GetVariable();
    public static readonly Regex GeneralCommand = GetGeneralCommand();

    private readonly PluginHost _pluginHost = pluginHost;
    private readonly HardwareInfoProvider _hardwareInfoProvider = hardwareInfoProvider;
    private readonly ServerManager _serverManager = servers;

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
    internal string ApplyVariables(
        string line,
        CommandContext? commandContext,
        bool removeInvalidVariablePatten = false
    )
    {
        if (!line.Contains('{') || !line.Contains('}'))
            return line.Replace("\\n", "\n");

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
                    #region Serein
                    "serein.type" => SereinApp.Type,
                    "serein.version" => SereinApp.Version,
                    "serein.fullversion" => SereinApp.FullVersion,
                    #endregion

                    #region 时间
                    "datetime" => currentTime.ToString(),
                    "datetime.date" => currentTime.Date.ToString("d"),
                    "datetime.time" => currentTime.ToString("T"),
                    "datetime.year" => currentTime.Year,
                    "datetime.month" => currentTime.Month,
                    "datetime.day" => currentTime.Day,
                    "datetime.hour" => currentTime.Hour,
                    "datetime.minute" => currentTime.Minute,
                    "datetime.second" => currentTime.Second,
                    "datetime.millisecond" => currentTime.Millisecond,
                    "datetime.microsecond" => currentTime.Microsecond,
                    "datetime.nanosecond" => currentTime.Nanosecond,
                    "datetime.iso" => currentTime.ToString("O"),
                    "datetime.dayofweek" => currentTime.DayOfWeek.ToString(),
                    #endregion

                    #region 操作系统
                    "os.name" => _hardwareInfoProvider.Info?.OperatingSystem.Name,
                    "os.version" => _hardwareInfoProvider.Info?.OperatingSystem.VersionString,
                    #endregion

                    #region CPU
                    "cpu.name" => _hardwareInfoProvider.Info?.CpuList.FirstOrDefault()?.Name,
                    "cpu.description"
                        => _hardwareInfoProvider.Info?.CpuList.FirstOrDefault()?.Description,
                    "cpu.caption" => _hardwareInfoProvider.Info?.CpuList.FirstOrDefault()?.Caption,
                    "cpu.manufacturer"
                        => _hardwareInfoProvider.Info?.CpuList.FirstOrDefault()?.Manufacturer,
                    "cpu.cores"
                        => _hardwareInfoProvider.Info?.CpuList.FirstOrDefault()?.NumberOfCores,
                    "cpu.logicalprocessors"
                        => _hardwareInfoProvider.Info?.CpuList
                            .FirstOrDefault()
                            ?.NumberOfLogicalProcessors,
                    "cpu.percent"
                        => _hardwareInfoProvider.Info?.CpuList
                            .FirstOrDefault()
                            ?.PercentProcessorTime,
                    #endregion

                    #region RAM
                    "ram.total" => _hardwareInfoProvider.Info?.MemoryStatus.TotalPhysical / 1024,
                    "ram.totalgb"
                        => _hardwareInfoProvider.Info?.MemoryStatus.TotalPhysical / 1024 / 1024,
                    "ram.available"
                        => _hardwareInfoProvider.Info?.MemoryStatus.AvailablePhysical / 1024,
                    "ram.availablegb"
                        => _hardwareInfoProvider.Info?.MemoryStatus.AvailablePhysical / 1024 / 1024,
                    "ram.used"
                        => (
                            _hardwareInfoProvider.Info?.MemoryStatus.TotalPhysical
                            - _hardwareInfoProvider.Info?.MemoryStatus.AvailablePhysical
                        ) / 1024,
                    "ram.usedgb"
                        => (
                            _hardwareInfoProvider.Info?.MemoryStatus.TotalPhysical
                            - _hardwareInfoProvider.Info?.MemoryStatus.AvailablePhysical
                        )
                            / 1024
                            / 1024,
                    "ram.percent"
                        => 100
                            * (
                                _hardwareInfoProvider.Info?.MemoryStatus.TotalPhysical
                                - _hardwareInfoProvider.Info?.MemoryStatus.AvailablePhysical
                            )
                            / _hardwareInfoProvider.Info?.MemoryStatus.TotalPhysical,
                    #endregion

                    #region 消息
                    "msg.id" => commandContext?.MessagePacket?.MessageId,
                    "sender.id" => commandContext?.MessagePacket?.UserId,
                    "sender.nickname" => commandContext?.MessagePacket?.Sender?.Nickname,
                    "sender.title" => commandContext?.MessagePacket?.Sender?.Title,
                    "sender.role" => commandContext?.MessagePacket?.Sender?.RoleName,
                    "sender.shownname"
                        => string.IsNullOrEmpty(commandContext?.MessagePacket?.Sender?.Card)
                            ? commandContext?.MessagePacket?.Sender?.Nickname
                            : commandContext.MessagePacket?.Sender?.Card,
                    #endregion

                    _ => GetServerVariables(name)
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

    private object? GetServerVariables(string input)
    {
        var i = input.IndexOf('@');
        if (i < 0)
            return null;

        var name = input[..(i + 1)];
        var id = input[(i + 1)..];

        return !_serverManager.Servers.TryGetValue(id, out Server? server)
            ? null
            : name.ToLowerInvariant() switch
            {
                "server.status" => server.Status == ServerStatus.Running ? "已启动" : "未启动",
                "server.usage" => server.ServerInfo.CPUUsage,
                "server.output" => server.ServerInfo.OutputLines,
                "server.input" => server.ServerInfo.InputLines,
                "server.time" => (DateTime.Now - server.ServerInfo.StartTime).ToCommonString(),
                "server.version" => server.ServerInfo.Stat?.Version,
                "server.motd" => server.ServerInfo.Stat?.Stripped_Motd,
                "server.players.max" => server.ServerInfo.Stat?.MaximumPlayers,
                "server.players.current" => server.ServerInfo.Stat?.CurrentPlayers,
                "server.players.percent"
                    => server.ServerInfo.Stat is not null
                        ? 100
                            * server.ServerInfo.Stat.CurrentPlayersInt
                            / server.ServerInfo.Stat.MaximumPlayersInt
                        : null,

                _ => null
            };
    }
}
