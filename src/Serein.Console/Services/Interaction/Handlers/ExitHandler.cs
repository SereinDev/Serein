using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serein.Console.Utils;
using Serein.Core.Models.Plugins;
using Serein.Core.Services.Plugins;
using Serein.Core.Services.Servers;
using Serein.Core.Utils.Extensions;
using Spectre.Console;

namespace Serein.Console.Services.Interaction.Handlers;

public sealed class ExitHandler(
    IHost host,
    ServerManager serverManager,
    EventDispatcher eventDispatcher
) : CommandHandler
{
    public override string Name { get; } = "退出";

    public override string[] Description { get; } = ["停止所有服务并退出Serein.Console"];

    public override string RootCommand { get; } = "exit";

    public override Dictionary<string, string> SubCommands { get; } = [];

    public override bool AllowToExecuteRootCommand { get; } = true;

    public override void Invoke(string subCommand, IReadOnlyList<string> args)
    {
        if (!serverManager.AnyRunning)
        {
            eventDispatcher.Dispatch(Event.SereinClosed);

            host.StopAsync().Await();
            return;
        }

        var table = new Table()
            .RoundedBorder()
            .AddColumns("[bold white]Id[/]", "[bold white]名称[/]", "[bold white]Pid[/]");
        var servers = serverManager.Servers.Where((kv) => kv.Value.Status);

        foreach (var kv in servers)
        {
            table.AddRow(
                new Text(kv.Key),
                new Text(kv.Value.Configuration.Name).Ellipsis(),
                new Text(kv.Value.Pid.ToString() ?? string.Empty)
            );
        }

        SimpleConsole.Log(
            LogLevel.Error,
            $"当前还有以下{servers.Count()}个服务器未关闭。请关闭所有服务器后再尝试退出",
            table
        );
    }
}
