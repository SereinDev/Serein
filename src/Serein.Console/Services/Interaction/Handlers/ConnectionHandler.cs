using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using Serein.Console.Models;
using Serein.Core.Services.Network.Connection;

namespace Serein.Console.Services.Interaction.Handlers;

public sealed class ConnectionHandler(
    ILogger<ConnectionHandler> logger,
    ConnectionManager connectionManager
) : CommandHandler
{
    public override string Name { get; } = "连接";

    public override string[] Description { get; } = ["连接或断开WebSocket", "查看连接信息"];

    public override string RootCommand { get; } = "connection";

    public override Dictionary<string, string> SubCommands { get; } =
        new()
        {
            ["info"] = "查看连接信息",
            ["open"] = "连接WebSocket",
            ["close"] = "断开WebSocket",
        };

    public override void Invoke(string subCommand, IReadOnlyList<string> args)
    {
        switch (subCommand)
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
        }
    }
}
