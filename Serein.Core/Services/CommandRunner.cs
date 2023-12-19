using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

using Serein.Core.Models;
using Serein.Core.Models.Commands;
using Serein.Core.Services.Networks;
using Serein.Core.Services.Server;

namespace Serein.Core.Services;

public class CommandRunner
{
    private readonly CommandParser _commandParser;
    private readonly IHost _host;
    private IServiceProvider Services => _host.Services;
    private WsNetwork WsNetwork => Services.GetRequiredService<WsNetwork>();
    private ServerManager ServerManager => Services.GetRequiredService<ServerManager>();
    private IOutputHandler Output => Services.GetRequiredService<IOutputHandler>();

    public CommandRunner(IHost host, CommandParser commandParser)
    {
        _host = host;
        _commandParser = commandParser;
    }

    public async Task Run(Command command, CommandContext commandContext)
    {
        if (command.Type == CommandType.Invalid)
            return;

        var body = _commandParser.Format(command.Body, commandContext);

        var callerType = command.Origin switch
        {
            CommandOrigin.Msg => CallerType.Command,
            CommandOrigin.ServerInput => CallerType.Command,
            CommandOrigin.ServerOutput => CallerType.Command,
            CommandOrigin.Schedule => CallerType.Command,
            CommandOrigin.Plugin => CallerType.Plugin,
            CommandOrigin.ConsoleExecute => CallerType.User,
            _ => CallerType.Unknown
        };

        switch (command.Type)
        {
            case CommandType.ExecuteShellCommand:
                await ExecuteShellCommand(body);
                break;

            case CommandType.ServerInput:
                ServerManager.Input(body, callerType);
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

            case CommandType.RequestMotdpe:

                break;

            case CommandType.RequestMotdje:

                break;

            case CommandType.ExecuteJavascriptCodes:

                break;

            case CommandType.Debug:
                Output.LogDebug("{}", body);
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
        var process = new Process()
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