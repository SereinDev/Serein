using System.Threading;
using System.Threading.Tasks;

using Microsoft.Extensions.Hosting;

using Serein.Core.Services.Data;
using Serein.Core.Services.Network;
using Serein.Core.Services.Network.Ssh;
using Serein.Core.Services.Network.WebApi;
using Serein.Core.Services.Servers;

namespace Serein.Core.Services;

public class StartUpService(
    SettingProvider settingProvider,
    ServerManager serverManager,
    UpdateChecker updateChecker,
    HttpServer httpServer,
    SshHost sshServiceProvider
    ) : IHostedService
{
    private readonly SettingProvider _settingProvider = settingProvider;
    private readonly ServerManager _serverManager = serverManager;
    private readonly UpdateChecker _updateChecker = updateChecker;
    private readonly HttpServer _httpServer = httpServer;
    private readonly SshHost _sshServiceProvider = sshServiceProvider;

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _updateChecker.StartAsync();

        foreach (var (_, server) in _serverManager.Servers)
        {
            if (server.Configuration.AutoRestart)
                try
                {
                    server.Start();
                }
                catch { }
        }

        if (_settingProvider.Value.WebApi.Enable)
            try
            {
                _httpServer.Start();
            }
            catch
            { }

        if (_settingProvider.Value.Ssh.Enable)
            try
            {
                _sshServiceProvider.Start();
            }
            catch
            { }

        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}
