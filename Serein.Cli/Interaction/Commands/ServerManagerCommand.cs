using System;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

using Serein.Cli.Models;
using Serein.Core.Models;
using Serein.Core.Models.Commands;
using Serein.Core.Models.Server;
using Serein.Core.Services.Server;

using Spectre.Console;

namespace Serein.Cli.Interaction.Commands;

[CommandDescription("server", "管理服务器", Priority = 999)]
[CommandUsage("server info", "显示服务器信息")]
[CommandUsage("server start", "启动服务器")]
[CommandUsage("server stop", "关闭服务器")]
[CommandUsage("server terminate", "强制结束服务器")]
public class ServerManagerCommand : Command
{
    public ServerManagerCommand(IHost host)
        : base(host) { }

    private ServerManager ServerManager => Services.GetRequiredService<ServerManager>();
    private IOutputHandler Logger => Services.GetRequiredService<IOutputHandler>();

    public override void Parse(string[] args)
    {
        if (args.Length == 1)
            throw new InvalidArgumentException(
                "缺少参数。可用值：\"info\"、\"start\"、\"stop\"和\"terminate\""
            );

        switch (args[1].ToLowerInvariant())
        {
            case "info":
                var info = ServerManager.ServerInfo;

                var table = new Table();

                table
                    .RoundedBorder()
                    .AddColumns(
                        new TableColumn("服务器状态") { Alignment = Justify.Center },
                        new(
                            ServerManager.Status switch
                            {
                                ServerStatus.Running => "[green3]●[/] 运行中",
                                ServerStatus.Stopped => "[gray]●[/] 已关闭",
                                ServerStatus.Unknown => "[gray]●[/] 未启动",
                                _ => throw new NotSupportedException()
                            }
                        )
                        {
                            Alignment = Justify.Center
                        }
                    )
                    .AddRow(
                        "运行时长",
                        info?.StartTime is null
                            ? "-"
                            : (DateTime.Now - info.StartTime).ToString() ?? "-"
                    )
                    .AddRow("CPU占用", (info?.CPUUsage ?? 0).ToString("N1") + "%")
                    .AddRow("输入行数", info?.InputLines.ToString() ?? "-")
                    .AddRow("输出行数", info?.OutputLines.ToString() ?? "-");

                AnsiConsole.Write(table);

                break;
            case "start":
                try
                {
                    ServerManager.Start();
                }
                catch (Exception e)
                {
                    Logger.LogWarning("启动失败：{}", e.Message);
                }
                break;

            case "stop":
                try
                {
                    ServerManager.Stop(CallerType.User);
                }
                catch (Exception e)
                {
                    Logger.LogWarning("关闭失败：{}", e.Message);
                }
                break;

            case "terminate":
                try
                {
                    ServerManager.Terminate();
                }
                catch (Exception e)
                {
                    Logger.LogWarning("强制结束失败：{}", e.Message);
                }
                break;

            default:
                throw new InvalidArgumentException(
                    "未知的参数。可用值：\"info\"、\"start\"、\"stop\"和\"terminate\""
                );
        }
    }
}
