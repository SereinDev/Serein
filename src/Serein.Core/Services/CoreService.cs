using System;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

using Serein.Core.Services.Data;
using Serein.Core.Services.Network;
using Serein.Core.Services.Network.Ssh;
using Serein.Core.Services.Network.WebApi;
using Serein.Core.Services.Servers;
using Serein.Core.Utils;

namespace Serein.Core.Services;

public class CoreService : IHostedService
{
    private readonly ILogger<CoreService> _logger;
    private readonly SettingProvider _settingProvider;
    private readonly ServerManager _serverManager;
    private readonly UpdateChecker _updateChecker;
    private readonly HttpServer _httpServer;
    private readonly SshHost _sshServiceProvider;

    public CoreService(
        ILogger<CoreService> logger,
        SettingProvider settingProvider,
        ServerManager serverManager,
        UpdateChecker updateChecker,
        HttpServer httpServer,
        SshHost sshServiceProvider
)
    {
        _logger = logger;
        _settingProvider = settingProvider;
        _serverManager = serverManager;
        _updateChecker = updateChecker;
        _httpServer = httpServer;
        _sshServiceProvider = sshServiceProvider;

        _logger.LogInformation("Serein.{} {}", SereinApp.Type, SereinApp.Version);
        _logger.LogInformation("仓库: {}", UrlConstants.Repository);
        _logger.LogInformation("Copyright (C) 2022 Zaitonn. All rights reserved.");
        _logger.LogInformation("");

        _logger.LogDebug("Path={}", AppDomain.CurrentDomain.BaseDirectory);
        _logger.LogDebug("Pid={}", Environment.ProcessId);
        _logger.LogDebug("版本：{}", SereinApp.FullVersion);
        _logger.LogDebug("程序集：{}", typeof(SereinApp).Assembly.GetName());
        _logger.LogDebug("首次启动：{}", SereinApp.StartForTheFirstTime);
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _updateChecker.StartAsync();

        foreach (var (_, server) in _serverManager.Servers)
            if (server.Configuration.StartWhenSettingUp)
                Try(server.Start);

        if (_settingProvider.Value.WebApi.Enable)
            Try(_httpServer.Start);

        if (_settingProvider.Value.Ssh.Enable)
            Try(_sshServiceProvider.Start);

        _logger.LogInformation("Serein启动成功！");

        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("正在停止");
        return Task.CompletedTask;
    }

    private static void Try(Action action)
    {
        try
        {
            action();
        }
        catch { }
    }
}
