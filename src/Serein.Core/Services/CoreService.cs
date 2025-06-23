using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serein.Core.Models.Plugins;
using Serein.Core.Services.Data;
using Serein.Core.Services.Network;
using Serein.Core.Services.Network.Connection;
using Serein.Core.Services.Network.Web;
using Serein.Core.Services.Plugins;
using Serein.Core.Services.Servers;
using Serein.Core.Utils;

namespace Serein.Core.Services;

internal sealed partial class CoreService : IHostedService
{
    private readonly ILogger<CoreService> _logger;
    private readonly SettingProvider _settingProvider;
    private readonly ConnectionManager _connectionManager;
    private readonly CancellationTokenProvider _cancellationTokenProvider;
    private readonly ServerManager _serverManager;
    private readonly UpdateChecker _updateChecker;
    private readonly WebServer _httpServer;

    public CoreService(
        ILogger<CoreService> logger,
        SereinApp sereinApp,
        WebServer httpServer,
        ServerManager serverManager,
        UpdateChecker updateChecker,
        SettingProvider settingProvider,
        EventDispatcher eventDispatcher,
        ConnectionManager connectionManager,
        CancellationTokenProvider cancellationTokenProvider
    )
    {
        _logger = logger;
        _httpServer = httpServer;
        _serverManager = serverManager;
        _updateChecker = updateChecker;
        _settingProvider = settingProvider;
        _connectionManager = connectionManager;
        _cancellationTokenProvider = cancellationTokenProvider;

        _logger.LogInformation("Serein.{} {}", sereinApp.Type, sereinApp.Version);
        _logger.LogInformation("");
        _logger.LogInformation("仓库: {}", UrlConstants.Repository);
        _logger.LogInformation("Copyright (C) 2022 Zaitonn. All rights reserved.");
        _logger.LogInformation("");

        _logger.LogDebug("Path={}", AppDomain.CurrentDomain.BaseDirectory);
        _logger.LogDebug("Pid={}", Environment.ProcessId);
        _logger.LogDebug("版本：{}", sereinApp.FullVersion);
        _logger.LogDebug("程序集：{}", sereinApp.AssemblyName);
        _logger.LogDebug("首次启动：{}", SereinAppBuilder.StartForTheFirstTime);

        AppDomain.CurrentDomain.UnhandledException += (_, _) =>
            eventDispatcher.Dispatch(Event.SereinCrashed);
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _updateChecker.StartAsync();

        foreach (var (_, server) in _serverManager.Servers)
        {
            if (server.Configuration.StartWhenSettingUp)
            {
                _logger.LogInformation(
                    "正在启动服务器: {}({})",
                    server.Configuration.Name,
                    server.Id
                );
                Try(server.Start, $"服务器 {server.Configuration.Name}({server.Id})");
            }
        }

        if (_settingProvider.Value.WebApi.IsEnabled)
        {
            _logger.LogInformation("正在启动Web服务器");
            Try(_httpServer.Start, "Web Server");
        }

        if (_settingProvider.Value.Connection.ConnectWhenSettingUp)
        {
            _logger.LogInformation("正在启动连接");
            Try(_connectionManager.Start, "Connection");
        }

        _logger.LogInformation("Serein启动成功！");

        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("正在停止");

        _cancellationTokenProvider.Cancel();

        return Task.CompletedTask;
    }

    private void Try(Action action, string name)
    {
        try
        {
            action();
        }
        catch (Exception e)
        {
            _logger.LogError(e, "{}启动失败", name);
        }
    }
}
