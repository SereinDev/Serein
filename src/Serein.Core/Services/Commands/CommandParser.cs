using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Serein.Core.Models.Commands;
using Serein.Core.Services.Bindings;
using Serein.Core.Services.Plugins;
using Serein.Core.Services.Servers;
using Serein.Core.Utils.Extensions;

namespace Serein.Core.Services.Commands;

public partial class CommandParser(
    SereinApp sereinApp,
    PluginManager pluginManager,
    ServerManager servers,
    BindingManager bindingManager,
    HardwareInfoProvider hardwareInfoProvider
)
{
    [GeneratedRegex(@"^\[(?<name>[a-zA-Z]+)(:(?<argument>[\w\-\s,=\.:]+))?\](?<body>.+)$")]
    private static partial Regex GetGeneralCommandRegex();

    [GeneratedRegex(@"\{([a-zA-Z][a-zA-Z0-9\.]*(@\w+)?)\}")]
    private static partial Regex GetVariableRegex();

    public static readonly Regex Variable = GetVariableRegex();
    public static readonly Regex GeneralCommand = GetGeneralCommandRegex();

    /// <summary>
    /// 解析命令
    /// </summary>
    /// <param name="origin">命令来源</param>
    /// <param name="command">命令文本</param>
    /// <param name="throws">是否在解析出错时抛出异常</param>
    /// <returns>命令对象</returns>
    public static Command Parse(CommandOrigin origin, string? command, bool throws = false)
    {
        var cmd = command?.TrimStart();
        try
        {
            if (string.IsNullOrEmpty(cmd) || string.IsNullOrWhiteSpace(cmd))
            {
                throw new ArgumentException("命令为空", nameof(command));
            }

            if (cmd.Length < 2 || !cmd.StartsWith('[') || !cmd.Contains(']'))
            {
                throw new ArgumentException("缺少命令标识：'['和']'", nameof(command));
            }

            var matchResult = GeneralCommand.Match(cmd);

            if (!matchResult.Success || matchResult.Groups.Count != 5)
            {
                throw new NotSupportedException("命令语法不正确");
            }

            var name = matchResult.Groups["name"].Value;
            var body = matchResult.Groups["body"].Value;
            var argument = matchResult.Groups["argument"].Value;

            var type = name.ToLowerInvariant() switch
            {
                "cmd" => CommandType.ExecuteShellCommand,

                "s" => CommandType.InputServer,
                "server" => CommandType.InputServer,

                "g" => CommandType.SendGroupMsg,
                "group" => CommandType.SendGroupMsg,
                "p" => CommandType.SendPrivateMsg,
                "private" => CommandType.SendPrivateMsg,
                "d" => CommandType.SendData,
                "data" => CommandType.SendData,

                "b" => CommandType.Bind,
                "bind" => CommandType.Bind,
                "ub" => CommandType.Unbind,
                "unbind" => CommandType.Unbind,

                "js" => CommandType.ExecuteJavascriptCodes,
                "javascript" => CommandType.ExecuteJavascriptCodes,

                "debug" => CommandType.Debug,
                _ => throw new NotSupportedException("命令类型无效"),
            };

            return new()
            {
                Origin = origin,
                Type = type,
                Body = body,
                Arguments = ParseCommandArguments(argument),
            };
        }
        catch
        {
            if (throws)
            {
                throw;
            }

            return new() { Origin = origin, Type = CommandType.Invalid };
        }
    }

    private static CommandArguments ParseCommandArguments(string arguments)
    {
        if (string.IsNullOrEmpty(arguments))
        {
            return new();
        }

        if (!arguments.Contains(',') && !arguments.Contains('='))
        {
            return new() { Target = arguments.Trim() };
        }

        var args = arguments.Split(
            ',',
            StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries
        );

        var commandArguments = new CommandArguments();

        foreach (var arg in args)
        {
            var parts = arg.Split('=', 2, StringSplitOptions.TrimEntries);

            if (parts.Length != 2)
            {
                continue;
            }

            var key = parts[0].ToLowerInvariant();
            var value = parts[1];

            switch (key)
            {
                case "target":
                    commandArguments.Target = value;
                    break;

                case "as_segments":
                case "assegments":
                    commandArguments.AsSegments = GetArgumentValueAsBool(value);
                    break;

                case "auto_escape":
                case "autoescape":
                    commandArguments.AutoEscape = GetArgumentValueAsBool(value);
                    break;

                case "use_unicode":
                case "useunicode":
                    commandArguments.UseUnicode = GetArgumentValueAsBool(value);
                    break;

                case "self":
                    var selfParts = value.Split(
                        '.',
                        2,
                        StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries
                    );
                    if (selfParts.Length == 2)
                    {
                        commandArguments.Self = new()
                        {
                            Platform = selfParts[0],
                            UserId = selfParts[1],
                        };
                    }
                    break;

                case "self_id":
                case "selfid":
                    commandArguments.Self ??= new();
                    commandArguments.Self.UserId = value;
                    break;

                case "self_platform":
                case "selfplatform":
                    commandArguments.Self ??= new();
                    commandArguments.Self.Platform = value;
                    break;
            }
        }

        return commandArguments;
    }

    private static bool? GetArgumentValueAsBool(string value)
    {
        return bool.TryParse(value, out bool result) ? result : null;
    }

    /// <summary>
    /// 格式化命令
    /// </summary>
    /// <param name="command">命令对象</param>
    /// <param name="commandContext">上下文</param>
    /// <returns>格式化后的命令主体部分</returns>
    public string Format(Command command, CommandContext? commandContext)
    {
        if (string.IsNullOrEmpty(command.Body))
        {
            return string.Empty;
        }

        var body = ApplyVariables(command.Body, commandContext);

        if (commandContext?.Match is not null)
        {
            foreach (
                KeyValuePair<string, Group> kv in (IEnumerable<KeyValuePair<string, Group>>)
                    commandContext.Value.Match.Groups
            )
            {
                body = body.Replace("$" + kv.Key, kv.Value.Value);
            }
        }

        return command.Type == CommandType.InputServer ? body : body.Replace("\\n", "\n");
    }

#pragma warning disable IDE0046

    /// <summary>
    /// 应用变量
    /// </summary>
    /// <param name="input">输入</param>
    /// <param name="commandContext">命令上下文</param>
    /// <param name="removeInvalidVariablePatten">删除无效的命令片段</param>
    /// <returns>应用变量后的文本</returns>
    public string ApplyVariables(
        string input,
        CommandContext? commandContext,
        bool removeInvalidVariablePatten = false
    )
    {
        if (!input.Contains('{') || !input.Contains('}'))
        {
            return input;
        }

        var currentTime = DateTime.Now;

        var text = Variable.Replace(
            input,
            (match) =>
            {
                var name = match.Groups[1].Value.ToLowerInvariant();

                if (
                    commandContext?.Variables is not null
                    && commandContext.Value.Variables.TryGetValue(name, out string? value)
                    && value is not null
                )
                {
                    return value;
                }

                if (
                    pluginManager.CommandVariables.TryGetValue(name, out value) && value is not null
                )
                {
                    return value;
                }

                object? obj = name switch
                {
                    #region Serein
                    "serein.type" => sereinApp.Type,
                    "serein.version" => sereinApp.Version,
                    "serein.fullversion" => sereinApp.FullVersion,
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
                    "os.name" => hardwareInfoProvider.Info?.OperatingSystem.Name,
                    "os.version" => hardwareInfoProvider.Info?.OperatingSystem.VersionString,
                    #endregion

                    #region CPU
                    "cpu.name" => hardwareInfoProvider.Info?.CpuList.FirstOrDefault()?.Name,
                    "cpu.description" => hardwareInfoProvider
                        .Info?.CpuList.FirstOrDefault()
                        ?.Description,
                    "cpu.caption" => hardwareInfoProvider.Info?.CpuList.FirstOrDefault()?.Caption,
                    "cpu.manufacturer" => hardwareInfoProvider
                        .Info?.CpuList.FirstOrDefault()
                        ?.Manufacturer,
                    "cpu.cores" => hardwareInfoProvider
                        .Info?.CpuList.FirstOrDefault()
                        ?.NumberOfCores,
                    "cpu.logicalprocessors" => hardwareInfoProvider
                        .Info?.CpuList.FirstOrDefault()
                        ?.NumberOfLogicalProcessors,
                    "cpu.percent" => hardwareInfoProvider
                        .Info?.CpuList.FirstOrDefault()
                        ?.PercentProcessorTime,
                    #endregion

                    #region RAM
                    "ram.total" => hardwareInfoProvider.Info?.MemoryStatus.TotalPhysical / 1024,
                    "ram.totalgb" => hardwareInfoProvider.Info?.MemoryStatus.TotalPhysical
                        / 1024
                        / 1024,
                    "ram.available" => hardwareInfoProvider.Info?.MemoryStatus.AvailablePhysical
                        / 1024,
                    "ram.availablegb" => hardwareInfoProvider.Info?.MemoryStatus.AvailablePhysical
                        / 1024
                        / 1024,
                    "ram.used" => (
                        hardwareInfoProvider.Info?.MemoryStatus.TotalPhysical
                        - hardwareInfoProvider.Info?.MemoryStatus.AvailablePhysical
                    ) / 1024,
                    "ram.usedgb" => (
                        hardwareInfoProvider.Info?.MemoryStatus.TotalPhysical
                        - hardwareInfoProvider.Info?.MemoryStatus.AvailablePhysical
                    )
                        / 1024
                        / 1024,
                    "ram.percent" => 100
                        * (
                            hardwareInfoProvider.Info?.MemoryStatus.TotalPhysical
                            - hardwareInfoProvider.Info?.MemoryStatus.AvailablePhysical
                        )
                        / hardwareInfoProvider.Info?.MemoryStatus.TotalPhysical,
                    #endregion

                    #region 消息
                    "msg.id" => commandContext.HasValue
                        ? StringExtension.SelectValueNotNullOrEmpty(
                            commandContext.Value.OneBotV11MessagePacket?.MessageId.ToString(),
                            commandContext.Value.OneBotV12MessagePacket?.MessageId.ToString(),
                            commandContext.Value.SatoriV1MessagePacket?.Message?.Id
                        )
                        : null,
                    "sender.id" => commandContext?.UserId,
                    "sender.nickname" => commandContext.HasValue
                        ? StringExtension.SelectValueNotNullOrEmpty(
                            commandContext.Value.OneBotV11MessagePacket?.Sender.Nickname,
                            commandContext.Value.SatoriV1MessagePacket?.User?.Nick
                        )
                        : null,
                    "sender.title" => commandContext?.OneBotV11MessagePacket?.Sender?.Title,
                    "sender.avatar" => commandContext?.SatoriV1MessagePacket?.User?.Avatar,
                    "sender.card" => commandContext?.OneBotV11MessagePacket?.Sender?.Card,
                    "sender.role" => commandContext?.OneBotV11MessagePacket?.Sender?.RoleName,
                    "sender.gameid" => GetGameId(commandContext?.UserId),
                    "sender.shownname" => commandContext.HasValue
                        ? StringExtension.SelectValueNotNullOrEmpty(
                            commandContext.Value.OneBotV11MessagePacket?.Sender.Card,
                            commandContext.Value.OneBotV11MessagePacket?.Sender.Nickname,
                            commandContext.Value.SatoriV1MessagePacket?.User?.Nick,
                            commandContext.Value.SatoriV1MessagePacket?.User?.Name
                        )
                        : null,
                    #endregion

                    _ => GetServerVariables(match.Groups[1].Value, commandContext?.ServerId),
                };

                var r = obj?.ToString();

                if (r is not null)
                {
                    return r;
                }

                return removeInvalidVariablePatten ? string.Empty : match.Value;
            }
        );

        return text;
    }

    private string? GetGameId(string? userId)
    {
        if (userId is null)
        {
            return null;
        }

        return bindingManager.TryGetValue(userId, out var binding)
            ? binding.GameIds.FirstOrDefault()
            : null;
    }

#pragma warning restore IDE0046

    private object? GetServerVariables(string input, string? id = null)
    {
        if (!input.StartsWith("server.", StringComparison.InvariantCultureIgnoreCase))
        {
            return null;
        }

        var i = input.IndexOf('@');

        Server? server;

        if (i < 0)
        {
            return Switch(
                input,
                !string.IsNullOrEmpty(id) && servers.Servers.TryGetValue(id, out server)
                    ? server
                    : servers.Servers.Values.FirstOrDefault()
            );
        }

        if (i == 0 || i >= input.Length)
        {
            return null;
        }

        var key = input[..i];
        id = input[(i + 1)..];

        return !servers.Servers.TryGetValue(id, out server) ? null : Switch(key, server);

        static object? Switch(string key, Server? server)
        {
            return server is null
                ? null
                : key.ToLowerInvariant() switch
                {
                    "server.id" => server.Id,
                    "server.name" => server.Configuration.Name,
                    "server.status" => server.Status ? "已启动" : "未启动",
                    "server.usage" => server.Info.CpuUsage,
                    "server.output" => server.Info.OutputLines,
                    "server.input" => server.Info.InputLines,
                    "server.time" => (DateTime.Now - server.Info.StartTime).ToCommonString(),
                    "server.version" => server.Info.Stat?.Version,
                    "server.motd" => server.Info.Stat?.Stripped_Motd,
                    "server.players.max" => server.Info.Stat?.MaximumPlayers,
                    "server.players.current" => server.Info.Stat?.CurrentPlayers,
                    "server.players.percent" => server.Info.Stat is not null
                        ? server.Info.Stat.MaximumPlayersInt > 0
                            ? 100
                                * server.Info.Stat.CurrentPlayersInt
                                / server.Info.Stat.MaximumPlayersInt
                            : 0
                        : null,

                    _ => null,
                };
        }
    }
}
