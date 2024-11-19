using System.Threading;
using System.Threading.Tasks;

using Microsoft.Extensions.Hosting;

namespace Serein.Core.Services.Plugins;

internal class PluginService(PluginManager pluginManager) : IHostedService
{
    private bool _unloaded;

    private readonly PluginManager _pluginManager = pluginManager;

    public Task StartAsync(CancellationToken cancellationToken)
    {
        Task.Run(_pluginManager.Load, cancellationToken);

        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        if (!_unloaded)
        {
            Task.Run(_pluginManager.Unload, cancellationToken);
        }

        _unloaded = true;

        return Task.CompletedTask;
    }
}
