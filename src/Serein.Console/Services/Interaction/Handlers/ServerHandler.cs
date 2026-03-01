using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using Serein.Console.Models;
using Serein.Console.Utils;
using Serein.Core.Services.Data;
using Serein.Core.Services.Servers;
using Spectre.Console;

namespace Serein.Console.Services.Interaction.Handlers;

public sealed class ServerHandler(
    ILogger<ServerHandler> logger,
    SettingProvider settingProvider,
    ServerManager serverManager,
    ServerSwitcher serverSwitcher
) : CommandHandler
{
    public override string Name { get; } = "服务器";

    public override string[] Description { get; } =
        ["管理服务器配置", "控制服务器", "查看服务器信息"];

    public override string RootCommand { get; } = "server";

    public override Dictionary<string, string> SubCommands { get; } =
        new()
        {
            ["info"] = "显示信息",
            ["start"] = "启动",
            ["stop"] = "关闭",
            ["terminate"] = "强制结束",
            ["switch"] = "选择并控制",
            ["list"] = "列出所有服务器",
        };

    public override void Invoke(string subCommand, IReadOnlyList<string> args)
    {
        if (subCommand.Equals("list", StringComparison.OrdinalIgnoreCase))
        {
            LogList();
            return;
        }

        if (subCommand.Equals("switch", StringComparison.OrdinalIgnoreCase) && args.Count == 2)
        {
            throw new InvalidArgumentException("缺少服务器Id。");
        }

        var id = args.Count == 3 ? args[2] : serverSwitcher.CurrentId;

        if (string.IsNullOrEmpty(id))
        {
            throw new InvalidArgumentException(
                "缺少服务器Id。"
                    + "你可以在命令末尾添加服务器Id或使用\"server switch <id>\"选择你要控制的服务器"
            );
        }

        if (!serverManager.Servers.TryGetValue(id, out Server? server))
        {
            throw new InvalidArgumentException("指定的服务器不存在");
        }

        switch (subCommand.ToLowerInvariant())
        {
            case "info":
                LogServerInfo(server);
                break;

            case "start":
                try
                {
                    server.Start();

                    if (string.IsNullOrEmpty(settingProvider.Value.Application.CliCommandHeader))
                    {
                        settingProvider.Value.Application.CliCommandHeader = "//";
                    }

                    logger.LogWarning(
                        "服务器已启动，输入的命令将转发至服务器。若要执行Serein的命令，你需要在命令前加上\"{}\"",
                        settingProvider.Value.Application.CliCommandHeader
                    );
                }
                catch (Exception e)
                {
                    logger.LogError("启动失败：{}", e.Message);
                }
                break;

            case "stop":
                try
                {
                    server.Stop();
                }
                catch (Exception e)
                {
                    logger.LogError("关闭失败：{}", e.Message);
                }
                break;

            case "switch" when args.Count == 3:
                try
                {
                    serverSwitcher.SwitchTo(id);
                }
                catch (Exception e)
                {
                    logger.LogError("选择服务器失败：{}", e.Message);
                }
                break;

            case "terminate":
                try
                {
                    server.Terminate();
                }
                catch (Exception e)
                {
                    logger.LogError("强制结束失败：{}", e.Message);
                }
                break;
        }
    }

    private void LogServerInfo(Server server)
    {
        var table = new Table()
            .RoundedBorder()
            .HideHeaders()
            .AddColumn("_", c => c.Centered())
            .AddColumn("_")
            .ShowRowSeparators()
            .AddRow("[bold white]Id[/]", $"[gray italic]({server.Id.EscapeMarkup()})[/]")
            .AddRow("[bold white]名称[/]", server.Configuration.Name.EscapeMarkup())
            .AddRow(
                "[bold white]状态[/]",
                server.Status ? $"运行中 [gray italic](Pid:{server.Pid})[/]" : "未启动"
            )
            .AddRow(
                "[bold white]启动命令行[/]",
                $"{server.Configuration.FileName.EscapeMarkup()} "
                    + $"[gray]{server.Configuration.Argument.EscapeMarkup()}[/]"
            );

        if (server.Info.StartTime is not null)
        {
            table.AddRow("[bold white]启动时间[/]", server.Info.StartTime.Value.ToString("G"));
        }
        else if (server.Info.ExitTime is not null)
        {
            table.AddRow("[bold white]停止时间[/]", server.Info.ExitTime.Value.ToString("G"));
        }

        if (server.Status && server.Info.Stat is not null && server.Info.Stat.ServerUp)
        {
            table
                .AddRow("[bold white]版本[/]", server.Info.Stat.Version.EscapeMarkup())
                .AddRow(
                    new Markup("[bold white]在线人数[/]"),
                    new Text(
                        server.Info.Stat.CurrentPlayers + "/" + server.Info.Stat.MaximumPlayers
                    )
                );
        }

        SimpleConsole.Log(LogLevel.Information, table);
    }

    private void LogList()
    {
        var table = new Table()
            .RoundedBorder()
            .ShowRowSeparators()
            .AddColumn("[bold white]名称[/]")
            .AddColumn("[bold white]状态[/]")
            .AddColumn("[bold white]启动命令行[/]");

        foreach (var kv in serverManager.Servers)
        {
            table.AddRow(
                new Markup(
                    $"{kv.Value.Configuration.Name.EscapeMarkup()} [gray italic]({kv.Key.EscapeMarkup()})[/]"
                ),
                new Markup(
                    kv.Value.Status ? $"运行中 [gray italic](Pid:{kv.Value.Pid})[/]" : "未启动"
                ),
                new Markup(
                    $"{kv.Value.Configuration.FileName.EscapeMarkup()} "
                        + $"[gray]{kv.Value.Configuration.Argument.EscapeMarkup()}[/]"
                ).Ellipsis()
            );
        }

        SimpleConsole.Log(
            LogLevel.Information,
            $"当前共{serverManager.Servers.Count}个服务器。",
            table
        );
    }
}
