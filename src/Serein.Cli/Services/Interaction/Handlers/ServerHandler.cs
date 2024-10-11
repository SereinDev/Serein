using System;
using System.Collections.Generic;

using Microsoft.Extensions.Logging;

using Serein.Cli.Models;
using Serein.Core.Services.Servers;

namespace Serein.Cli.Services.Interaction.Handlers;

[CommandName("server", "服务器")]
[CommandDescription(["管理服务器配置", "控制服务器", "查看服务器信息"])]
[CommandUsage("server <id> info", "显示服务器信息")]
[CommandUsage("server <id> start", "启动服务器")]
[CommandUsage("server <id> stop", "关闭服务器")]
[CommandUsage("server <id> terminate", "强制结束服务器")]
[CommandChildren("info", "显示信息")]
[CommandChildren("start", "启动")]
[CommandChildren("stop", "关闭")]
[CommandChildren("terminate", "强制结束")]

public class ServerHandler(ILogger<ServerHandler> logger, ServerManager serverManager) : CommandHandler
{
    private readonly ILogger<ServerHandler> _logger = logger;
    private readonly ServerManager _serverManager = serverManager;

    public override void Invoke(IReadOnlyList<string> args)
    {
        if (args.Count == 1)
            throw new InvalidArgumentException("缺少服务器ID。");

        if (args.Count == 2)
            throw new InvalidArgumentException(
                "缺少参数。可用值：\"info\"、\"start\"、\"stop\"和\"terminate\""
            );

        if (!_serverManager.Servers.TryGetValue(args[1], out Server? server))
            throw new InvalidArgumentException("指定的服务器不存在");

        switch (args[2].ToLowerInvariant())
        {
            case "info":
                var info = server.ServerInfo;

                // var table = new Table();

                // table
                //     .RoundedBorder()
                //     .AddColumns(
                //         new TableColumn("服务器状态") { Alignment = Justify.Center },
                //         new(
                //             server.Status switch
                //             {
                //                 ServerStatus.Running => "[green3]●[/] 运行中",
                //                 ServerStatus.Stopped => "[gray]●[/] 已关闭",
                //                 ServerStatus.Unknown => "[gray]●[/] 未启动",
                //                 _ => throw new NotSupportedException()
                //             }
                //         )
                //         {
                //             Alignment = Justify.Center
                //         }
                //     )
                //     .AddRow(
                //         "运行时长",
                //         info?.StartTime is null
                //             ? "-"
                //             : (DateTime.Now - info.StartTime).ToString() ?? "-"
                //     )
                //     .AddRow("CPU占用", (info?.CPUUsage ?? 0).ToString("N1") + "%")
                //     .AddRow("输入行数", info?.InputLines.ToString() ?? "-")
                //     .AddRow("输出行数", info?.OutputLines.ToString() ?? "-");

                // AnsiConsole.Write(table);

                break;
            case "start":
                try
                {
                    server.Start();
                }
                catch (Exception e)
                {
                    _logger.LogError("启动失败：{}", e.Message);
                }
                break;

            case "stop":
                try
                {
                    server.Stop();
                }
                catch (Exception e)
                {
                    _logger.LogError("关闭失败：{}", e.Message);
                }
                break;

            case "terminate":
                try
                {
                    server.Terminate();
                }
                catch (Exception e)
                {
                    _logger.LogError("强制结束失败：{}", e.Message);
                }
                break;

            default:
                throw new InvalidArgumentException(
                    "未知的参数。可用值：\"info\"、\"start\"、\"stop\"和\"terminate\""
                );
        }
    }
}
