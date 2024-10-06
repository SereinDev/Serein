using System;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

using Serein.Cli.Models;
using Serein.Core.Models.Server;
using Serein.Core.Services.Servers;

using Spectre.Console;

namespace Serein.Cli.Services.Interaction.Handlers;

[CommandDescription("server", "管理服务器", Priority = 999)]
[CommandUsage("server <id> info", "显示服务器信息")]
[CommandUsage("server <id> start", "启动服务器")]
[CommandUsage("server <id> stop", "关闭服务器")]
[CommandUsage("server <id> terminate", "强制结束服务器")]
public class ServerHandler(IHost host) : CommandHandler
{
    private readonly ServerManager _serverManager = host.Services.GetRequiredService<ServerManager>();
    private readonly ILogger _logger =  host.Services.GetRequiredService<ILogger<ServerHandler>>();

    public override void Invoke(string[] args)
    {
        if (args.Length == 1)
            throw new InvalidArgumentException(
                "缺少参数。可用值：\"info\"、\"start\"、\"stop\"和\"terminate\""
            );

        if (args.Length == 2)
            throw new InvalidArgumentException("缺少服务器ID。");

        if (!_serverManager.Servers.TryGetValue(args[1], out Server? server))
            throw new InvalidArgumentException("指定的服务器不存在。");

        switch (args[2].ToLowerInvariant())
        {
            case "info":
                var info = server.ServerInfo;

                var table = new Table();

                table
                    .RoundedBorder()
                    .AddColumns(
                        new TableColumn("服务器状态") { Alignment = Justify.Center },
                        new(
                            server.Status switch
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
                    server.Start();
                }
                catch (Exception e)
                {
                    _logger.LogWarning("启动失败：{}", e.Message);
                }
                break;

            case "stop":
                try
                {
                    server.Stop();
                }
                catch (Exception e)
                {
                    _logger.LogWarning("关闭失败：{}", e.Message);
                }
                break;

            case "terminate":
                try
                {
                    server.Terminate();
                }
                catch (Exception e)
                {
                    _logger.LogWarning("强制结束失败：{}", e.Message);
                }
                break;

            default:
                throw new InvalidArgumentException(
                    "未知的参数。可用值：\"info\"、\"start\"、\"stop\"和\"terminate\""
                );
        }
    }
}
