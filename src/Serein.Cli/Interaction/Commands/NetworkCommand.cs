using System;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

using Serein.Cli.Models;
using Serein.Core.Models.Output;
using Serein.Core.Models.Server;
using Serein.Core.Services.Networks;
using Serein.Core.Services.Servers;

using Spectre.Console;

namespace Serein.Cli.Interaction.Commands;

[CommandDescription("net", "管理连接", Priority = 998)]
[CommandUsage("net info", "显示连接信息")]
[CommandUsage("net open", "连接WebSocket")]
[CommandUsage("net close", "断开WebSocket")]
public class NetworkCommand : Command
{
    public NetworkCommand(IHost host)
        : base(host) { }

    private WsNetwork WsNetwork => Services.GetRequiredService<WsNetwork>();
    private ISereinLogger Logger => Services.GetRequiredService<ISereinLogger>();

    public override void Parse(string[] args)
    {
        if (args.Length == 1)
            throw new InvalidArgumentException("缺少参数。可用值：\"info\"、\"open\"和\"close\"");

        switch (args[1].ToLowerInvariant())
        {
            case "info":

                var table = new Table();

                table
                    .RoundedBorder()
                    .AddColumns(
                        new TableColumn("连接状态") { Alignment = Justify.Center },
                        new(WsNetwork.Active ? "[green3]●[/] 已连接" : "[gray]●[/] 未连接")
                        {
                            Alignment = Justify.Center
                        }
                    );

                AnsiConsole.Write(table);

                break;
            case "open":
                try
                {
                    WsNetwork.Start();
                }
                catch (Exception e)
                {
                    Logger.LogWarning("连接失败：{}", e.Message);
                }
                break;

            case "close":
                try
                {
                    WsNetwork.Stop();
                }
                catch (Exception e)
                {
                    Logger.LogWarning("断开失败：{}", e.Message);
                }
                break;

            default:
                throw new InvalidArgumentException(
                    "未知的参数。可用值：\"info\"、\"open\"和\"close\""
                );
        }
    }
}
