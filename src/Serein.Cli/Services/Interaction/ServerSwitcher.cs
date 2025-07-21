using System;
using System.Linq;
using Microsoft.Extensions.Logging;
using Serein.Core.Models.Server;
using Serein.Core.Services.Data;
using Serein.Core.Services.Servers;

namespace Serein.Cli.Services.Interaction;

public sealed class ServerSwitcher
{
    public string? CurrentId => _server?.Id;
    private Server? _server;

    private readonly object _lock = new();
    private readonly ILogger<ServerSwitcher> _logger;
    private readonly ILogger<Server> _serverLogger;
    private readonly ServerManager _serverManager;
    private readonly SettingProvider _settingProvider;

    public ServerSwitcher(
        ILogger<ServerSwitcher> logger,
        ILogger<Server> serverLogger,
        ServerManager serverManager,
        SettingProvider settingProvider
    )
    {
        _logger = logger;
        _serverLogger = serverLogger;
        _serverManager = serverManager;
        _settingProvider = settingProvider;

        _serverManager.ServersUpdated += (_, e) =>
        {
            if (e.Type == ServersUpdatedType.Removed && e.Id == CurrentId)
            {
                _logger.LogWarning(
                    "当前选择的服务器\"{}\"(Id={})已被删除",
                    _server?.Configuration.Name,
                    _server?.Id
                );

                if (_serverManager.Servers.Count > 0)
                {
                    SwitchTo(_serverManager.Servers.First().Key);
                }
            }
        };
    }

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

            if (_server is not null)
            {
                _server.Logger.Output -= LogToConsole;
            }

            _server = server;

            _server.Logger.Output += LogToConsole;
            _logger.LogInformation("成功选择到\"{}\"(Id={})", _server.Configuration.Name, id);

            if (_server.Status)
            {
                if (string.IsNullOrWhiteSpace(_settingProvider.Value.Application.CliCommandHeader))
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
            _logger.LogInformation("当前仅有一个服务器配置，该服务器的所有输出都将输出到控制台");
            _logger.LogInformation(
                "添加更多服务器配置后，你可以用\"server list\"查看所有的服务器信息或用\"server switch <id>\"选择要进行操作的服务器"
            );

            SwitchTo(_serverManager.Servers.First().Key);
        }
        else if (_serverManager.Servers.Count > 1)
        {
            _logger.LogInformation(
                "当前有多个服务器配置，你可以用\"server list\"查看所有的服务器信息或用\"server switch <id>\"选择要进行操作的服务器"
            );
        }
    }

    private void LogToConsole(object? sender, ServerOutputEventArgs e)
    {
        if (_server is null)
        {
            return;
        }

        switch (e.Type)
        {
            case ServerOutputType.StandardOutput:
                Console.WriteLine(e.Data);
                break;

            case ServerOutputType.InternalInfo:
                _serverLogger.LogInformation(
                    "[{}(Id={})] {}",
                    _server.Configuration.Name,
                    _server.Id,
                    e.Data
                );
                break;

            case ServerOutputType.InternalError:
                _serverLogger.LogError(
                    "[{}(Id={})] {}",
                    _server.Configuration.Name,
                    _server.Id,
                    e.Data
                );
                break;
        }
    }
}
