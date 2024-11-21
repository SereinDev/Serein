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

    private readonly ILogger<ServerSwitcher> _logger = logger;
    private readonly ILogger<Server> _serverLogger = serverLogger;
    private readonly ServerManager _serverManager = serverManager;
    private readonly SettingProvider _settingProvider = settingProvider;
    private readonly object _lock = new();

    public void SwitchTo(string id)
    {
        lock (_lock)
        {
            if (id == CurrentId)
            {
                _logger.LogWarning("选择的服务器Id没有变化");
                return;
            }

            if (!_serverManager.Servers.TryGetValue(id, out var server))
            {
                throw new InvalidOperationException("选择的服务器不存在");
            }

            if (
                !string.IsNullOrEmpty(CurrentId)
                && _serverManager.Servers.TryGetValue(CurrentId, out var oldServer)
            )
            {
                oldServer.ServerOutput -= LogToConsole;
            }

            server.ServerOutput += LogToConsole;
            CurrentId = id;
            _logger.LogInformation("成功选择到\"{}\"(Id={})", server.Configuration.Name, id);

            if (server.Status)
            {
                if (string.IsNullOrEmpty(_settingProvider.Value.Application.CliCommandHeader))
                {
                    _settingProvider.Value.Application.CliCommandHeader = "//";
                }
                _logger.LogWarning(
                    "此服务器正在运行中，输入的命令将转发至服务器。若要执行Serein的命令，你需要在命令前加上\"{}\"",
                    _settingProvider.Value.Application.CliCommandHeader
                );
            }
        }
    }

    public void Initialize()
    {
        if (_serverManager.Servers.Count == 1)
        {
            _logger.LogWarning("当前仅有一个服务器配置，该服务器的所有输出都将输出到控制台");
            _logger.LogWarning(
                "添加更多服务器配置后，你可以用\"server switch <id>\"选择要进行操作的服务器"
            );

            SwitchTo(_serverManager.Servers.First().Key);
        }
        else if (_serverManager.Servers.Count > 1)
        {
            _logger.LogWarning(
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
        switch (e.OutputType)
        {
            case ServerOutputType.Raw:
                Console.WriteLine(e.Data);
                break;

            case ServerOutputType.Information:
                _serverLogger.LogInformation(
                    "[{}(Id={})] {}",
                    server.Configuration.Name,
                    server.Id,
                    e.Data
                );
                break;

            case ServerOutputType.Error:
                _serverLogger.LogError(
                    "[{}(Id={})] {}",
                    server.Configuration.Name,
                    server.Id,
                    e.Data
                );
                break;
        }
    }
}
