using System.Linq;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

using Serein.Cli.Models;
using Serein.Core.Models.Server;
using Serein.Core.Services.Servers;

namespace Serein.Cli.Services.Interaction.Handlers;

[CommandDescription("exit", "关闭并退出", Priority = -114514)]
public class ExitHandler(IHost host) : CommandHandler
{
    private readonly IHost _host = host;
    private readonly ILogger _logger =  host.Services.GetRequiredService<ILogger<ExitHandler>>();
    private readonly ServerManager _serverManager = host.Services.GetRequiredService<ServerManager>();

    public override void Invoke(string[] args)
    {
        if (!_serverManager.AnyRunning)
        {
            _host.StopAsync().Wait();
            return;
        };

        var servers = _serverManager.Servers.Where((kv) => kv.Value.Status == ServerStatus.Running);

        _logger.LogError("当前还有以下{}个服务器未关闭", servers.Count());
        foreach (var kv in servers)
            _logger.LogError("- {} (Id:{})", kv.Value.Configuration.Name, kv.Key);
    }
}
