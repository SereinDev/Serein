using System;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

using Serein.Cli.Models;
using Serein.Core.Services.Network.Connection;

using Spectre.Console;

namespace Serein.Cli.Interaction.Commands;

[CommandDescription("cn", "管理连接", Priority = 998)]
[CommandUsage("cn info", "显示连接信息")]
[CommandUsage("cn open", "连接WebSocket")]
[CommandUsage("cn close", "断开WebSocket")]
public class ConnectionCommand(IHost host) : Command(host)
{
    private WsConnectionManager WsNetwork => Services.GetRequiredService<WsConnectionManager>();
    private ILogger Logger => Services.GetRequiredService<ILogger>();

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
