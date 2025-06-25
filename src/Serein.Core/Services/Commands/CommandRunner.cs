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
            "运行命令：type={}, command.Body='{}'; command.Argument='{}'",
            command.Type,
            command.Body,
            command.Arguments
        );

        var body = _commandParser.Value.Format(command, commandContext);

        _logger.LogDebug("格式化后：body='{}'", body);

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
                if (!commandContext.HasValue)
                {
                    break;
                }

                await FastReply(commandContext.Value, body);
                break;

            case CommandType.Bind:
            case CommandType.Unbind:
                if (!commandContext.HasValue || string.IsNullOrEmpty(commandContext.Value.UserId))
                {
                    break;
                }

                try
                {
                    if (command.Type == CommandType.Bind)
                    {
                        _bindingManager.Add(
                            commandContext.Value.UserId,
                            body,
                            StringExtension.SelectValueNotNullOrEmpty(
                                commandContext.Value.OneBotV11MessagePacket?.Sender.Card,
                                commandContext.Value.OneBotV11MessagePacket?.Sender.Nickname,
                                commandContext.Value.SatoriV1MessagePacket?.User?.Nick,
                                commandContext.Value.SatoriV1MessagePacket?.User?.Name
                            )
                        );

                        await _reactionTrigger.Value.TriggerAsync(
                            ReactionType.BindingSucceeded,
                            new(
                                UserId: commandContext.Value.UserId,
                                GroupId: commandContext.Value.GroupId
                            )
                        );
                    }
                    else
                    {
                        _bindingManager.Remove(commandContext.Value.UserId, body);
                        await _reactionTrigger.Value.TriggerAsync(
                            ReactionType.UnbindingSucceeded,
                            new(
                                UserId: commandContext.Value.UserId,
                                GroupId: commandContext.Value.GroupId
                            )
                        );
                    }
                }
                catch (BindingFailureException e)
                {
                    _logger.LogWarning(e, "通过命令绑定失败");

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
    }

    private async Task SendPrivateMsgAsync(
        Command command,
        CommandContext? commandContext,
        string body
    )
    {
        if (!string.IsNullOrEmpty(command.Arguments?.Target))
        {
            await _connectionManager.Value.SendMessageAsync(
                TargetType.Private,
                command.Arguments.Target,
                body,
                command.Arguments
            );
        }
        else if (!string.IsNullOrEmpty(commandContext?.UserId))
        {
            await _connectionManager.Value.SendMessageAsync(
                TargetType.Private,
                commandContext.Value.UserId,
                body,
                command.Arguments
            );
        }
    }

    private async Task SendChannelMsgAsync(
        Command command,
        CommandContext? commandContext,
        string body
    )
    {
        if (!string.IsNullOrEmpty(command.Arguments?.Target))
        {
            await _connectionManager.Value.SendMessageAsync(
                TargetType.Channel,
                command.Arguments.Target,
                body,
                command.Arguments
            );
        }
        else if (!string.IsNullOrEmpty(commandContext?.OneBotV12MessagePacket?.ChannelId))
        {
            await _connectionManager.Value.SendMessageAsync(
                TargetType.Channel,
                commandContext.Value.OneBotV12MessagePacket.ChannelId,
                body,
                command.Arguments
            );
        }
        else if (!string.IsNullOrEmpty(commandContext?.SatoriV1MessagePacket?.Channel?.Id))
        {
            await _connectionManager.Value.SendMessageAsync(
                TargetType.Channel,
                commandContext.Value.SatoriV1MessagePacket.Channel.Id,
                body,
                command.Arguments
            );
        }
        else if (
            command.Origin != CommandOrigin.Msg
            && _settingProvider.Value.Connection.ListenedIds.Length > 0
        )
        {
            var first = _settingProvider.Value.Connection.ListenedIds.FirstOrDefault(
                (id) => id.StartsWith("channel:") || id.StartsWith("c:") || !id.Contains(':')
            );

            if (string.IsNullOrEmpty(first))
            {
                return;
            }

            if (first.Contains(':'))
            {
                first = first[(first.IndexOf(':') + 1)..];
            }

            await _connectionManager.Value.SendMessageAsync(TargetType.Channel, first, body);
        }
    }

    private async Task SendGroupMsgAsync(
        Command command,
        CommandContext? commandContext,
        string body
    )
    {
        if (!string.IsNullOrEmpty(command.Arguments?.Target))
        {
            await _connectionManager.Value.SendMessageAsync(
                TargetType.Group,
                command.Arguments.Target,
                body,
                command.Arguments
            );
        }
        else if (!string.IsNullOrEmpty(commandContext?.GroupId))
        {
            await _connectionManager.Value.SendMessageAsync(
                TargetType.Group,
                commandContext.Value.GroupId,
                body,
                command.Arguments
            );
        }
        else if (
            command.Origin != CommandOrigin.Msg
            && _settingProvider.Value.Connection.ListenedIds.Length > 0
        )
        {
            var first = _settingProvider.Value.Connection.ListenedIds.FirstOrDefault(
                (id) => id.StartsWith("group:") || id.StartsWith("g:") || !id.Contains(':')
            );

            if (string.IsNullOrEmpty(first))
            {
                return;
            }

            if (first.Contains(':'))
            {
                first = first[(first.IndexOf(':') + 1)..];
            }

            await _connectionManager.Value.SendMessageAsync(
                TargetType.Group,
                first,
                body,
                command.Arguments
            );
        }
    }

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

        server?.InputFromCommand(body, null);
    }

    private async Task FastReply(CommandContext commandContext, string content)
    {
        if (
            commandContext.OneBotV11MessagePacket is not null
            || commandContext.OneBotV12MessagePacket is not null
        )
        {
            if (!string.IsNullOrEmpty(commandContext.GroupId))
            {
                await _connectionManager.Value.SendMessageAsync(
                    TargetType.Group,
                    commandContext.GroupId,
                    content
                );
            }
            else if (!string.IsNullOrEmpty(commandContext.OneBotV12MessagePacket?.ChannelId))
            {
                await _connectionManager.Value.SendMessageAsync(
                    TargetType.Channel,
                    commandContext.OneBotV12MessagePacket.ChannelId,
                    content
                );
            }
            else if (!string.IsNullOrEmpty(commandContext.OneBotV12MessagePacket?.GuildId))
            {
                await _connectionManager.Value.SendMessageAsync(
                    TargetType.Guild,
                    commandContext.OneBotV12MessagePacket.GuildId,
                    content
                );
            }
            else if (!string.IsNullOrEmpty(commandContext.UserId))
            {
                await _connectionManager.Value.SendMessageAsync(
                    TargetType.Private,
                    commandContext.UserId,
                    content
                );
            }
        }
        else if (commandContext.SatoriV1MessagePacket is not null)
        {
            if (commandContext.SatoriV1MessagePacket.Channel is not null)
            {
                await _connectionManager.Value.SendMessageAsync(
                    TargetType.Channel,
                    commandContext.SatoriV1MessagePacket.Channel.Id,
                    content
                );
            }
        }
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
