using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using Serein.Cli.Models;
using Serein.Core.Services.Network.Connection;

namespace Serein.Cli.Services.Interaction.Handlers;

[CommandName("connection", "连接")]
[CommandDescription(["连接或断开WebSocket", "查看连接信息"])]
[SubCommand("info", "查看WebSocket连接状态")]
[SubCommand("open", "打开WebSocket连接")]
[SubCommand("close", "关闭WebSocket连接")]
public sealed class ConnectionHandler(
    ILogger<ConnectionHandler> logger,
    ConnectionManager connectionManager
) : CommandHandler
{
    public override void Invoke(IReadOnlyList<string> args)
    {
        if (args.Count == 1)
        {
            throw new InvalidArgumentException("缺少参数。可用值：\"info\"、\"open\"和\"close\"");
        }

        switch (args[1].ToLowerInvariant())
        {
            case "info":
                logger.LogInformation(
                    "连接状态：{}",
                    connectionManager.IsActive ? "已连接" : "未连接"
                );

                break;
            case "open":
                try
                {
                    connectionManager.Start();
                }
                catch (Exception e)
                {
                    logger.LogError("连接失败：{}", e.Message);
                }
                break;

            case "close":
                try
                {
                    connectionManager.Stop();
                }
                catch (Exception e)
                {
                    logger.LogError("断开失败：{}", e.Message);
                }
                break;

            default:
                throw new InvalidArgumentException(
                    "未知的参数。可用值：\"info\"、\"open\"和\"close\""
                );
        }
    }
}
