using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

using Serein.Core.Models.Commands;
using Serein.Core.Services.Data;
using Serein.Core.Services.Network.Connection;
using Serein.Core.Services.Servers;

namespace Serein.Core.Services.Commands;

public class CommandRunner
{
    private readonly Lazy<CommandParser> _commandParser;
    private readonly Lazy<WsConnectionManager> _wsConnectionManager;
    private readonly Lazy<ServerManager> _serverManager;
    private readonly ILogger _logger;
    private readonly SettingProvider _settingProvider;

    public CommandRunner(IHost host, ILogger logger, SettingProvider settingProvider)
    {
        var services = host.Services;
        _wsConnectionManager = new(services.GetRequiredService<WsConnectionManager>);
        _serverManager = new(services.GetRequiredService<ServerManager>);
        _commandParser = new(services.GetRequiredService<CommandParser>);
        _logger = logger;
        _settingProvider = settingProvider;
    }

    public async Task RunAsync(Command command, CommandContext? commandContext = null)
    {
        if (command.Type == CommandType.Invalid)
            return;

        var body = _commandParser.Value.Format(command, commandContext);

        switch (command.Type)
        {
            case CommandType.ExecuteShellCommand:
                await ExecuteShellCommand(body);
                break;

            case CommandType.InputServer:
                Server? server = null;
                if (!string.IsNullOrEmpty(command.Argument))
                    _serverManager.Value.Servers.TryGetValue(command.Argument, out server);
                else if (
                    !string.IsNullOrEmpty(commandContext?.ServerId)
                    && command.Origin == CommandOrigin.ServerOutput
                )
                    _serverManager.Value.Servers.TryGetValue(commandContext?.ServerId!, out server);
                else if (_serverManager.Value.Servers.Count == 1)
                    server = _serverManager.Value.Servers.Values.First();

                server?.Input(body, null, command.Origin == CommandOrigin.ConsoleExecute);
                break;

            case CommandType.SendGroupMsg:
                if (!string.IsNullOrEmpty(command.Argument))
                    await _wsConnectionManager.Value.SendGroupMsgAsync(command.Argument, body);
                else if (commandContext?.MessagePacket?.GroupId is long groupId)
                    await _wsConnectionManager.Value.SendGroupMsgAsync(groupId, body);
                else if (
                    command.Origin != CommandOrigin.Msg
                    && _settingProvider.Value.Connection.Groups.Length > 0
                )
                    await _wsConnectionManager.Value.SendGroupMsgAsync(
                        _settingProvider.Value.Connection.Groups[0],
                        body
                    );
                break;

            case CommandType.SendPrivateMsg:
                if (!string.IsNullOrEmpty(command.Argument))
                    await _wsConnectionManager.Value.SendPrivateMsgAsync(command.Argument, body);
                else if (commandContext?.MessagePacket?.UserId is long userId)
                    await _wsConnectionManager.Value.SendGroupMsgAsync(userId, body);
                break;

            case CommandType.SendText:
                await _wsConnectionManager.Value.SendTextAsync(body);
                break;

            case CommandType.Bind:

                break;

            case CommandType.Unbind:

                break;

            case CommandType.ExecuteJavascriptCodes:

                break;

            case CommandType.Debug:
                _logger.LogDebug("{}", body);
                break;

            case CommandType.Reload:
                break;

            case CommandType.Invalid:
            default:
                throw new NotSupportedException();
        }
    }

    private static async Task ExecuteShellCommand(string line)
    {
        var process = new Process
        {
            StartInfo = Environment.OSVersion.Platform == PlatformID.Win32NT ? new()
            {
                UseShellExecute = false,
                RedirectStandardInput = true,
                CreateNoWindow = true,
                WorkingDirectory = Directory.GetCurrentDirectory(),
                FileName = "cmd.exe",
                Arguments = "/c " + line
            } : new()
            {
                UseShellExecute = false,
                RedirectStandardInput = true,
                CreateNoWindow = true,
                WorkingDirectory = Directory.GetCurrentDirectory(),
                FileName = "sh",
                Arguments = "--noprofile --norc " + line
            }
        };
        process.Start();

        await process.WaitForExitAsync().WaitAsync(TimeSpan.FromMinutes(1));

        if (!process.HasExited)
            process.Kill(true);

        process.Dispose();
    }
}
