using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;

namespace Serein.Core.Services.Plugins;

internal sealed class PluginService(PluginManager pluginManager) : IHostedService
{
    private bool _unloaded;

    public Task StartAsync(CancellationToken cancellationToken)
    {
        Task.Run(pluginManager.Load, cancellationToken);

        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        if (!_unloaded)
        {
            Task.Run(pluginManager.Unload, cancellationToken);
        }

        _unloaded = true;

        return Task.CompletedTask;
    }
}
