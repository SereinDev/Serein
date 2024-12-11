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
using Serein.Core.Models.Network.Connection.OneBot.Messages;
using Serein.Core.Models.Network.Connection.OneBot.Packets;
using Serein.Core.Models.Settings;
using Serein.Core.Services.Bindings;
using Serein.Core.Services.Data;
using Serein.Core.Services.Network.Connection;
using Serein.Core.Services.Plugins.Js;
using Serein.Core.Services.Servers;

namespace Serein.Core.Services.Commands;

/// <summary>
/// 命令运行器
/// </summary>
public sealed class CommandRunner
{
    private readonly Lazy<CommandParser> _commandParser;
    private readonly Lazy<WsConnectionManager> _wsConnectionManager;
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
        _wsConnectionManager = new(services.GetRequiredService<WsConnectionManager>);
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
    /// <exception cref="TimeoutException"></exception>
    /// <exception cref="NotSupportedException"></exception>
    public async Task RunAsync(Command command, CommandContext? commandContext = null)
    {
        if (command.Type == CommandType.Invalid)
        {
            return;
        }
        _logger.LogDebug(
            "运行命令：command.Body='{}'; command.Argument='{}'",
            command.Body,
            command.Argument
        );

        var body = _commandParser.Value.Format(command, commandContext);
        var argumentStr = command.Argument as string;

        switch (command.Type)
        {
            case CommandType.ExecuteShellCommand:
                await ExecuteShellCommand(body);
                break;

            case CommandType.InputServer:
                Server? server = null;
                if (!string.IsNullOrEmpty(argumentStr))
                {
                    _serverManager.Value.Servers.TryGetValue(argumentStr, out server);
                }
                else if (!string.IsNullOrEmpty(commandContext?.ServerId))
                {
                    _serverManager.Value.Servers.TryGetValue(commandContext.ServerId, out server);
                }
                else if (_serverManager.Value.Servers.Count == 1)
                {
                    server = _serverManager.Value.Servers.Values.First();
                }
                server?.InputFromCommand(body, null);
                break;

            case CommandType.SendGroupMsg:
                if (!string.IsNullOrEmpty(argumentStr))
                {
                    await _wsConnectionManager.Value.SendGroupMsgAsync(argumentStr, body);
                }
                else if (command.Argument is long groupId1)
                {
                    await _wsConnectionManager.Value.SendGroupMsgAsync(groupId1, body);
                }
                else if (commandContext?.MessagePacket?.GroupId is long groupId2)
                {
                    await _wsConnectionManager.Value.SendGroupMsgAsync(groupId2, body);
                }
                else if (
                    command.Origin != CommandOrigin.Msg
                    && _settingProvider.Value.Connection.Groups.Length > 0
                )
                {
                    await _wsConnectionManager.Value.SendGroupMsgAsync(
                        _settingProvider.Value.Connection.Groups[0],
                        body
                    );
                }
                break;

            case CommandType.SendPrivateMsg:
                if (!string.IsNullOrEmpty(argumentStr))
                {
                    await _wsConnectionManager.Value.SendPrivateMsgAsync(argumentStr, body);
                }
                else if (command.Argument is long userId1)
                {
                    await _wsConnectionManager.Value.SendGroupMsgAsync(userId1, body);
                }
                else if (commandContext?.MessagePacket?.UserId is long userId2)
                {
                    await _wsConnectionManager.Value.SendPrivateMsgAsync(userId2, body);
                }
                break;

            case CommandType.SendData:
                await _wsConnectionManager.Value.SendDataAsync(body);
                break;

            case CommandType.Bind:
            case CommandType.Unbind:
                if (commandContext?.MessagePacket?.UserId is not long userId3)
                {
                    break;
                }
                try
                {
                    if (command.Type == CommandType.Bind)
                    {
                        _bindingManager.Add(
                            userId3,
                            body,
                            string.IsNullOrEmpty(commandContext.MessagePacket.Sender.Card)
                                ? commandContext.MessagePacket.Sender.Nickname
                                : commandContext.MessagePacket.Sender.Card
                        );
                        await _reactionTrigger.Value.TriggerAsync(
                            ReactionType.BindingSucceeded,
                            new(UserId: userId3, GroupId: commandContext.MessagePacket.GroupId)
                        );
                    }
                    else
                    {
                        _bindingManager.Remove(userId3, body);
                        await _reactionTrigger.Value.TriggerAsync(
                            ReactionType.UnbindingSucceeded,
                            new(UserId: userId3, GroupId: commandContext.MessagePacket.GroupId)
                        );
                    }
                }
                catch (BindingFailureException e)
                {
                    _logger.LogWarning(e, "通过命令绑定失败");
                    await FastReply(commandContext.MessagePacket, e.Message);
                }
                break;

            case CommandType.ExecuteJavascriptCodes:
                if (
                    command.Argument is not string id
                    || !_jsPluginLoader.JsPlugins.TryGetValue(id, out var jsPlugin)
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

            case CommandType.Invalid:
            default:
                throw new NotSupportedException();
        }
    }

    private async Task FastReply(MessagePacket messagePacket, string msg)
    {
        if (messagePacket.MessageType == MessageType.Group && messagePacket.GroupId is long groupId)
        {
            await _wsConnectionManager.Value.SendGroupMsgAsync(groupId, msg);
        }
        else if (
            messagePacket.MessageType == MessageType.Private
            && messagePacket.UserId is long userId
        )
        {
            await _wsConnectionManager.Value.SendPrivateMsgAsync(userId, msg);
        }
    }

    private async Task ExecuteShellCommand(string line)
    {
        _logger.LogDebug("运行命令行：{}", line);
        var process = new Process
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
        _logger.LogDebug("进程Id：{}", process.Id);
        await process.WaitForExitAsync().WaitAsync(TimeSpan.FromMinutes(1));

        if (!process.HasExited)
        {
            process.Kill(true);
        }
        process.Dispose();
    }
}
