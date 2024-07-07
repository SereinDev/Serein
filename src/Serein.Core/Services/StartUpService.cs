using System.Threading;
using System.Threading.Tasks;

using Microsoft.Extensions.Hosting;

using Serein.Core.Services.Data;
using Serein.Core.Services.Network;
using Serein.Core.Services.Servers;

namespace Serein.Core.Services;

public class StartUpService(
    SettingProvider settingProvider,
    ServerManager serverManager,
    UpdateChecker updateChecker
    ) : IHostedService
{
    private readonly SettingProvider _settingProvider = settingProvider;
    private readonly ServerManager _serverManager = serverManager;
    private readonly UpdateChecker _updateChecker = updateChecker;

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _updateChecker.Start();

        foreach (var (_, server) in _serverManager.Servers)
        {
            try
            {
                if (server.Configuration.AutoRestart)
                    server.Start();
            }
            catch { }
        }

        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}
