using System;
using System.Linq;
using Microsoft.Extensions.Logging;
using Serein.Core.Models.Server;
using Serein.Core.Services.Data;
using Serein.Core.Services.Servers;

namespace Serein.Cli.Services.Interaction;

public sealed class ServerSwitcher(
    ILogger<ServerSwitcher> logger,
    ILogger<Server> serverLogger,
    ServerManager serverManager,
    SettingProvider settingProvider
)
{
    public string? CurrentId { get; private set; }

    private readonly object _lock = new();

    public void SwitchTo(string id)
    {
        lock (_lock)
        {
            if (id == CurrentId)
            {
                logger.LogWarning("选择的服务器Id没有变化");
                return;
            }

            if (!serverManager.Servers.TryGetValue(id, out var server))
            {
                throw new InvalidOperationException("选择的服务器不存在");
            }

            if (
                !string.IsNullOrEmpty(CurrentId)
                && serverManager.Servers.TryGetValue(CurrentId, out var oldServer)
            )
            {
                oldServer.Logger.Output -= LogToConsole;
            }

            server.Logger.Output += LogToConsole;
            CurrentId = id;
            logger.LogInformation("成功选择到\"{}\"(Id={})", server.Configuration.Name, id);

            if (server.Status)
            {
                if (string.IsNullOrEmpty(settingProvider.Value.Application.CliCommandHeader))
                {
                    settingProvider.Value.Application.CliCommandHeader = "//";
                }
                logger.LogWarning(
                    "此服务器正在运行中，输入的命令将转发至服务器。若要执行Serein的命令，你需要在命令前加上\"{}\"",
                    settingProvider.Value.Application.CliCommandHeader
                );
            }
        }
    }

    public void Initialize()
    {
        if (serverManager.Servers.Count == 1)
        {
            logger.LogWarning("当前仅有一个服务器配置，该服务器的所有输出都将输出到控制台");
            logger.LogWarning(
                "添加更多服务器配置后，你可以用\"server switch <id>\"选择要进行操作的服务器"
            );

            SwitchTo(serverManager.Servers.First().Key);
        }
        else if (serverManager.Servers.Count > 1)
        {
            logger.LogWarning(
                "当前有多个服务器配置，你可以用\"server switch <id>\"选择要进行操作的服务器"
            );
        }
    }

    private void LogToConsole(object? sender, ServerOutputEventArgs e)
    {
        if (sender is not Server server)
        {
            return;
        }
        switch (e.Type)
        {
            case ServerOutputType.StandardOutput:
                Console.WriteLine(e.Data);
                break;

            case ServerOutputType.InternalInfo:
                serverLogger.LogInformation(
                    "[{}(Id={})] {}",
                    server.Configuration.Name,
                    server.Id,
                    e.Data
                );
                break;

            case ServerOutputType.InternalError:
                serverLogger.LogError(
                    "[{}(Id={})] {}",
                    server.Configuration.Name,
                    server.Id,
                    e.Data
                );
                break;
        }
    }
}
