using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

using Serein.Core.Models.Server;
using Serein.Core.Services.Servers;

namespace Serein.Cli.Services.Interaction;

public class CancelKeyHandlingService(
    IHost host,
    ILogger<CancelKeyHandlingService> logger,
    ServerManager serverManager
) : IHostedService
{
    private readonly IHost _host = host;
    private readonly ILogger _logger = logger;
    private readonly ServerManager _serverManager = serverManager;

    public Task StartAsync(CancellationToken cancellationToken)
    {
        Console.CancelKeyPress += OnCancelKeyPress;
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    private void OnCancelKeyPress(object? sender, ConsoleCancelEventArgs e)
    {
        if (!_serverManager.AnyRunning)
        {
            _host.StopAsync().Wait();
            return;
        };

        e.Cancel = true;
        var servers = _serverManager.Servers.Where((kv) => kv.Value.Status == ServerStatus.Running);

        _logger.LogError("当前还有以下{}个服务器未关闭", servers.Count());
        foreach (var kv in servers)
            _logger.LogError("▫ {} (Id:{})", kv.Value.Configuration.Name, kv.Key);
    }
}
