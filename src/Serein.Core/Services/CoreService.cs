using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serein.Core.Models.Plugins;
using Serein.Core.Services.Data;
using Serein.Core.Services.Network;
using Serein.Core.Services.Network.Web;
using Serein.Core.Services.Plugins;
using Serein.Core.Services.Servers;
using Serein.Core.Utils;

namespace Serein.Core.Services;

internal sealed class CoreService : IHostedService
{
    private readonly ILogger<CoreService> _logger;
    private readonly SettingProvider _settingProvider;
    private readonly ServerManager _serverManager;
    private readonly UpdateChecker _updateChecker;
    private readonly WebServer _httpServer;

    public CoreService(
        ILogger<CoreService> logger,
        SereinApp sereinApp,
        SettingProvider settingProvider,
        ServerManager serverManager,
        UpdateChecker updateChecker,
        EventDispatcher eventDispatcher,
        WebServer httpServer
    )
    {
        _logger = logger;
        _settingProvider = settingProvider;
        _serverManager = serverManager;
        _updateChecker = updateChecker;
        _httpServer = httpServer;

        _logger.LogInformation("Serein.{} {}", sereinApp.Type, sereinApp.Version);
        _logger.LogInformation("仓库: {}", UrlConstants.Repository);
        _logger.LogInformation("Copyright (C) 2022 Zaitonn. All rights reserved.");
        _logger.LogInformation("");

        _logger.LogDebug("Path={}", AppDomain.CurrentDomain.BaseDirectory);
        _logger.LogDebug("Pid={}", Environment.ProcessId);
        _logger.LogDebug("版本：{}", sereinApp.FullVersion);
        _logger.LogDebug("程序集：{}", sereinApp.AssemblyName);
        _logger.LogDebug("首次启动：{}", SereinAppBuilder.StartForTheFirstTime);

        if (Debugger.IsAttached)
        {
            _logger.LogWarning("调试器已附加");
        }

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
                Try(server.Start, $"服务器{server.Configuration.Name}({server.Id})");
            }
        }

        if (_settingProvider.Value.WebApi.IsEnabled)
        {
            Try(_httpServer.Start, "Web Server");
        }

        _logger.LogInformation("Serein启动成功！");

        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("正在停止");
        _updateChecker.Dispose();

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
