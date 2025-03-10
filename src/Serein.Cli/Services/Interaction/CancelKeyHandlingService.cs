using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serein.Core.Services.Servers;

namespace Serein.Cli.Services.Interaction;

public sealed class CancelKeyHandlingService(
    IHost host,
    ILogger<CancelKeyHandlingService> logger,
    ServerManager serverManager
) : IHostedService
{
    private readonly ILogger _logger = logger;

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
        if (!serverManager.AnyRunning)
        {
            host.StopAsync().Wait();
            return;
        }

        e.Cancel = true;
        var servers = serverManager.Servers.Where((kv) => kv.Value.Status);

        _logger.LogError("当前还有以下{}个服务器未关闭", servers.Count());
        foreach (var kv in servers)
        {
            _logger.LogError("▫ {} (Id:{})", kv.Value.Configuration.Name, kv.Key);
        }
    }
}
