using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

using Serein.Core.Models.Commands;
using Serein.Core.Models.Output;
using Serein.Core.Services.Networks.Connection;
using Serein.Core.Services.Servers;

namespace Serein.Core.Services;

public class CommandRunner
{
    private readonly Lazy<CommandParser> _commandParser;
    private readonly IHost _host;
    private IServiceProvider Services => _host.Services;
    private WsConnectionManager WsNetwork => Services.GetRequiredService<WsConnectionManager>();
    private ServerManager ServerManager => Services.GetRequiredService<ServerManager>();
    private ISereinLogger Logger => Services.GetRequiredService<ISereinLogger>();

    public CommandRunner(IHost host)
    {
        _host = host;
        _commandParser = new(Services.GetRequiredService<CommandParser>);
    }

    public async Task RunAsync(Command command, CommandContext? commandContext = null)
    {
        if (command.Type == CommandType.Invalid)
            return;

        var body = _commandParser.Value.Format(command.Body, commandContext);

        switch (command.Type)
        {
            case CommandType.ExecuteShellCommand:
                await ExecuteShellCommand(body);
                break;

            case CommandType.ServerInput:
                if (ServerManager.Servers.TryGetValue(command.Argument, out Server? server))
                    server.Input(body, null, command.Origin == CommandOrigin.ConsoleExecute);
                break;

            case CommandType.SendGroupMsg:
                if (!string.IsNullOrEmpty(command.Argument))
                    await WsNetwork.SendGroupMsgAsync(command.Argument, body);
                break;

            case CommandType.SendPrivateMsg:
                if (!string.IsNullOrEmpty(command.Argument))
                    await WsNetwork.SendPrivateMsgAsync(command.Argument, body);
                break;

            case CommandType.SendText:
                await WsNetwork.SendTextAsync(body);
                break;

            case CommandType.Bind:

                break;

            case CommandType.Unbind:

                break;

            case CommandType.ExecuteJavascriptCodes:

                break;

            case CommandType.Debug:
                Logger.LogDebug("{}", body);
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
            StartInfo = new()
            {
                FileName =
                    Environment.OSVersion.Platform == PlatformID.Win32NT ? "cmd.exe" : "/bin/bash",
                UseShellExecute = false,
                RedirectStandardInput = true,
                CreateNoWindow = true,
                WorkingDirectory = Directory.GetCurrentDirectory()
            }
        };
        process.Start();
        var commandWriter = new StreamWriter(process.StandardInput.BaseStream, Encoding.Default)
        {
            AutoFlush = true
        };
        commandWriter.WriteLine(line.TrimEnd('\r', '\n'));
        commandWriter.Close();

        await process.WaitForExitAsync().WaitAsync(TimeSpan.FromMinutes(1));

        if (!process.HasExited)
            process.Kill(true);

        process.Dispose();
    }
}
