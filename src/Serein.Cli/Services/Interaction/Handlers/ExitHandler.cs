using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serein.Cli.Models;
using Serein.Core.Models.Plugins;
using Serein.Core.Services.Plugins;
using Serein.Core.Services.Servers;
using Serein.Core.Utils.Extensions;

namespace Serein.Cli.Services.Interaction.Handlers;

[CommandName("exit", "退出")]
[CommandDescription(["停止所有服务并退出Serein.Cli"])]
public sealed class ExitHandler(
    IHost host,
    ILogger<ExitHandler> logger,
    ServerManager serverManager,
    EventDispatcher eventDispatcher
) : CommandHandler
{
    public override void Invoke(IReadOnlyList<string> args)
    {
        if (!serverManager.AnyRunning)
        {
            eventDispatcher.Dispatch(Event.SereinClosed);

            host.StopAsync().Await();
            return;
        }

        var servers = serverManager.Servers.Where((kv) => kv.Value.Status);

        logger.LogError("当前还有以下{}个服务器未关闭", servers.Count());

        foreach (var kv in servers)
        {
            logger.LogError("- {} (Id={})", kv.Value.Configuration.Name, kv.Key);
        }
    }
}
