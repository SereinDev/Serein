using System.Threading;
using System.Threading.Tasks;

using Microsoft.Extensions.Hosting;

namespace Serein.Core.Services.Plugins;

public class PluginService(PluginManager pluginManager) : IHostedService
{
    private readonly PluginManager _pluginManager = pluginManager;

    public Task StartAsync(CancellationToken cancellationToken)
    {
        Task
            .Delay(1000, cancellationToken)
            .ContinueWith((_) => _pluginManager.Load(), cancellationToken);

        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        Task.Run(_pluginManager.Unload, cancellationToken);

        return Task.CompletedTask;
    }
}