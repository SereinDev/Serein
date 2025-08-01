using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serein.Core.Models.Bindings;
using Serein.Core.Models.Commands;
using Serein.Core.Models.Network.Connection;
using Serein.Core.Services.Bindings;
using Serein.Core.Services.Data;
using Serein.Core.Services.Network.Connection;
using Serein.Core.Services.Plugins.Js;
using Serein.Core.Services.Servers;
using Serein.Core.Utils.Extensions;

namespace Serein.Core.Services.Commands;

/// <summary>
/// 命令运行器
/// </summary>
public sealed class CommandRunner
{
    private readonly Lazy<CommandParser> _commandParser;
    private readonly Lazy<ConnectionManager> _connectionManager;
    private readonly Lazy<ServerManager> _serverManager;
    private readonly Lazy<ReactionTrigger> _reactionTrigger;
    private readonly ILogger _logger;
    private readonly SettingProvider _settingProvider;
    private readonly JsPluginLoader _jsPluginLoader;
    private readonly BindingManager _bindingManager;

    public CommandRunner(
        IHost host,
        ILogger<CommandRunner> logger,
        SettingProvider settingProvider,
        JsPluginLoader jsPluginLoader,
        BindingManager bindingManager
    )
    {
        var services = host.Services;
        _connectionManager = new(services.GetRequiredService<ConnectionManager>);
        _reactionTrigger = new(services.GetRequiredService<ReactionTrigger>);
        _serverManager = new(services.GetRequiredService<ServerManager>);
        _commandParser = new(services.GetRequiredService<CommandParser>);
        _logger = logger;
        _settingProvider = settingProvider;
        _jsPluginLoader = jsPluginLoader;
        _bindingManager = bindingManager;
    }

    /// <summary>
    /// 运行命令
    /// </summary>
    /// <param name="command">命令</param>
    /// <param name="commandContext">命令上下文</param>
    public async Task RunAsync(Command command, CommandContext? commandContext = null)
    {
        if (command.Type == CommandType.Invalid)
        {
            return;
        }

        _logger.LogDebug(
            "运行命令：Type={}, Body='{}', Arguments='{}', Id={}",
            command.Type,
            command.Body,
            command.Arguments,
            command.GetHashCode()
        );

        var body = _commandParser.Value.Format(command, commandContext);

        _logger.LogDebug("格式化后：Body='{}'", body);

        switch (command.Type)
        {
            case CommandType.ExecuteShellCommand:
                await ExecuteShellCommand(body);
                break;

            case CommandType.InputServer:
                InputServer(command, commandContext, body);
                break;

            case CommandType.SendGroupMsg:
                await SendGroupMsgAsync(command, commandContext, body);
                break;

            case CommandType.SendPrivateMsg:
                await SendPrivateMsgAsync(command, commandContext, body);
                break;

            case CommandType.SendChannelMsg:
                await SendChannelMsgAsync(command, commandContext, body);
                break;

            case CommandType.SendData:
                await _connectionManager.Value.SendDataAsync(body);
                break;

            case CommandType.SendReply:
                if (commandContext.HasValue)
                {
                    await FastReply(commandContext.Value, body);
                }
                break;

            case CommandType.Bind:
            case CommandType.Unbind:
                if (string.IsNullOrEmpty(commandContext?.Packets.UserId))
                {
                    break;
                }

                try
                {
                    if (command.Type == CommandType.Bind)
                    {
                        _bindingManager.Add(
                            commandContext.Value.Packets.UserId,
                            body,
                            StringExtension.SelectValueNotNullOrEmpty(
                                commandContext.Value.Packets.OneBotV11?.Sender.Card,
                                commandContext.Value.Packets.OneBotV11?.Sender.Nickname,
                                commandContext.Value.Packets.SatoriV1?.User?.Nick,
                                commandContext.Value.Packets.SatoriV1?.User?.Name
                            )
                        );

                        await _reactionTrigger.Value.TriggerAsync(
                            ReactionType.BindingSucceeded,
                            new(
                                UserId: commandContext.Value.Packets.UserId,
                                GroupId: commandContext.Value.Packets.GroupId
                            )
                        );
                    }
                    else
                    {
                        _bindingManager.Remove(commandContext.Value.Packets.UserId, body);
                        await _reactionTrigger.Value.TriggerAsync(
                            ReactionType.UnbindingSucceeded,
                            new(
                                UserId: commandContext.Value.Packets.UserId,
                                GroupId: commandContext.Value.Packets.GroupId
                            )
                        );
                    }
                }
                catch (Exception e) when (e is BindingFailureException or ArgumentException)
                {
                    _logger.LogError(e, "通过命令绑定失败");

                    await FastReply(commandContext.Value, e.Message);
                }
                break;

            case CommandType.ExecuteJavascriptCodes:
                if (
                    string.IsNullOrEmpty(command.Arguments?.Target)
                    || !_jsPluginLoader.Plugins.TryGetValue(
                        command.Arguments.Target,
                        out var jsPlugin
                    )
                )
                {
                    break;
                }

                var entered = false;
                try
                {
                    if (!Monitor.TryEnter(jsPlugin.Engine, 1000))
                    {
                        throw new TimeoutException("等待引擎超时");
                    }
                    entered = true;
                    jsPlugin.Engine.Execute(body);
                }
                finally
                {
                    if (entered)
                    {
                        Monitor.Exit(jsPlugin.Engine);
                    }
                }
                break;

            case CommandType.Debug:
                _logger.LogDebug("{}", body);
                break;

            default:
                throw new NotSupportedException();
        }

        _logger.LogDebug("命令（Id={}）运行结束", command.GetHashCode());
    }

    #region 发送消息

    private async Task SendMessageAsync(
        TargetType targetType,
        Command command,
        CommandContext? commandContext,
        string content,
        Func<CommandContext, string?> targetIdExtractor,
        string[] fallbackPrefixes
    )
    {
        // 优先使用命令参数中指定的目标
        if (!string.IsNullOrEmpty(command.Arguments?.Target))
        {
            await _connectionManager.Value.SendMessageAsync(
                targetType,
                command.Arguments.Target,
                content,
                command.Arguments,
                commandContext?.Packets.Self
            );
            return;
        }

        // 从上下文中提取目标ID
        if (commandContext.HasValue)
        {
            var targetId = targetIdExtractor(commandContext.Value);
            if (!string.IsNullOrEmpty(targetId))
            {
                await _connectionManager.Value.SendMessageAsync(
                    targetType,
                    targetId,
                    content,
                    command.Arguments,
                    commandContext?.Packets.Self
                );
                return;
            }
        }

        // 回退到配置的默认ID（仅限非消息来源的命令）
        if (
            command.Origin != CommandOrigin.Message
            && _settingProvider.Value.Connection.ListenedIds.Length > 0
        )
        {
            var targetId = GetFallbackTargetId(fallbackPrefixes);
            if (!string.IsNullOrEmpty(targetId))
            {
                await _connectionManager.Value.SendMessageAsync(
                    targetType,
                    targetId,
                    content,
                    command.Arguments,
                    commandContext?.Packets.Self
                );
            }
        }
    }

    private string? GetFallbackTargetId(string[] prefixes)
    {
        var first = _settingProvider.Value.Connection.ListenedIds.FirstOrDefault(id =>
            prefixes.Any(prefix => id.StartsWith(prefix)) || !id.Contains(':')
        );

        if (string.IsNullOrEmpty(first))
        {
            return null;
        }

        if (first.Contains(':'))
        {
            first = first[(first.IndexOf(':') + 1)..];
        }

        return first;
    }

    private async Task SendPrivateMsgAsync(
        Command command,
        CommandContext? commandContext,
        string body
    )
    {
        await SendMessageAsync(
            TargetType.Private,
            command,
            commandContext,
            body,
            ctx => ctx.Packets.UserId,
            []
        );
    }

    private async Task SendChannelMsgAsync(
        Command command,
        CommandContext? commandContext,
        string body
    )
    {
        await SendMessageAsync(
            TargetType.Channel,
            command,
            commandContext,
            body,
            ctx => ctx.Packets.OneBotV12?.ChannelId ?? ctx.Packets.SatoriV1?.Channel?.Id,
            ["channel:", "c:"]
        );
    }

    private async Task SendGroupMsgAsync(
        Command command,
        CommandContext? commandContext,
        string body
    )
    {
        await SendMessageAsync(
            TargetType.Group,
            command,
            commandContext,
            body,
            ctx => ctx.Packets.GroupId,
            ["group:", "g:"]
        );
    }

    private async Task FastReply(CommandContext commandContext, string content)
    {
        var targetInfo = GetReplyTargetInfo(commandContext);
        if (targetInfo.HasValue)
        {
            await _connectionManager.Value.SendMessageAsync(
                targetInfo.Value.TargetType,
                targetInfo.Value.TargetId,
                content,
                null,
                commandContext.Packets.Self
            );
        }
    }

    private static (TargetType TargetType, string TargetId)? GetReplyTargetInfo(
        CommandContext commandContext
    )
    {
        if (
            commandContext.Packets.OneBotV11 is not null
            || commandContext.Packets.OneBotV12 is not null
        )
        {
            if (!string.IsNullOrEmpty(commandContext.Packets.GroupId))
            {
                return (TargetType.Group, commandContext.Packets.GroupId);
            }
            if (!string.IsNullOrEmpty(commandContext.Packets.OneBotV12?.ChannelId))
            {
                return (TargetType.Channel, commandContext.Packets.OneBotV12.ChannelId);
            }
            if (!string.IsNullOrEmpty(commandContext.Packets.OneBotV12?.GuildId))
            {
                return (TargetType.Guild, commandContext.Packets.OneBotV12.GuildId);
            }
            if (!string.IsNullOrEmpty(commandContext.Packets.UserId))
            {
                return (TargetType.Private, commandContext.Packets.UserId);
            }
        }
        else if (commandContext.Packets.SatoriV1?.Channel is not null)
        {
            return (TargetType.Channel, commandContext.Packets.SatoriV1.Channel.Id);
        }

        return null;
    }

    #endregion

    private void InputServer(Command command, CommandContext? commandContext, string body)
    {
        Server? server = null;

        if (!string.IsNullOrEmpty(command.Arguments?.Target))
        {
            _serverManager.Value.Servers.TryGetValue(command.Arguments.Target, out server);
        }
        else if (!string.IsNullOrEmpty(commandContext?.ServerId))
        {
            _serverManager.Value.Servers.TryGetValue(commandContext.Value.ServerId, out server);
        }
        else if (_serverManager.Value.Servers.Count == 1)
        {
            server = _serverManager.Value.Servers.Values.First();
        }

        server?.InputFromCommand(body, command.Arguments?.UseUnicode);
    }

    private async Task ExecuteShellCommand(string line)
    {
        _logger.LogDebug("运行命令行：{}", line);

        using var process = new Process
        {
            StartInfo =
                Environment.OSVersion.Platform == PlatformID.Win32NT
                    ? new()
                    {
                        UseShellExecute = false,
                        RedirectStandardInput = true,
                        CreateNoWindow = true,
                        WorkingDirectory = Directory.GetCurrentDirectory(),
                        FileName = "cmd.exe",
                        Arguments = "/c " + line,
                    }
                    : new()
                    {
                        UseShellExecute = false,
                        RedirectStandardInput = true,
                        CreateNoWindow = true,
                        WorkingDirectory = Directory.GetCurrentDirectory(),
                        FileName = "sh",
                        Arguments = "--noprofile --norc " + line,
                    },
        };

        process.Start();

        _logger.LogDebug("命令行进程Id：{}", process.Id);

        await process.WaitForExitAsync().WaitAsync(TimeSpan.FromMinutes(1));

        if (!process.HasExited)
        {
            _logger.LogDebug("运行超时，尝试强制结束进程（Id：{}）", process.Id);
            process.Kill(true);
        }
    }
}
