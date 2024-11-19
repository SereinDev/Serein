using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Extensions.Logging;

using Serein.Cli.Models;
using Serein.Core.Services.Data;
using Serein.Core.Services.Servers;

namespace Serein.Cli.Services.Interaction.Handlers;

[CommandName("server", "服务器")]
[CommandDescription(["管理服务器配置", "控制服务器", "查看服务器信息"])]
[CommandChildren("info", "显示信息")]
[CommandChildren("start", "启动")]
[CommandChildren("stop", "关闭")]
[CommandChildren("terminate", "强制结束")]
[CommandChildren("switch", "选择并控制")]
[CommandChildren("list", "列出所有服务器")]
public sealed class ServerHandler(
    ILogger<ServerHandler> logger,
    SettingProvider settingProvider,
    ServerManager serverManager,
    ServerSwitcher serverSwitcher
) : CommandHandler
{
    private readonly ILogger<ServerHandler> _logger = logger;
    private readonly SettingProvider _settingProvider = settingProvider;
    private readonly ServerManager _serverManager = serverManager;
    private readonly ServerSwitcher _serverSwitcher = serverSwitcher;

    public override void Invoke(IReadOnlyList<string> args)
    {
        if (args.Count == 1)
        {
            throw new InvalidArgumentException(
                "缺少参数。可用值：\"info\"、\"start\"、\"stop\"、\"terminate\"和\"switch\""
            );
        }

        if (args[1].Equals("list", StringComparison.InvariantCultureIgnoreCase))
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.AppendLine($"当前共{_serverManager.Servers.Count}个服务器配置");
            foreach (var kv in _serverManager.Servers)
            {
                stringBuilder.AppendLine($"▢ {kv.Value.Configuration.Name}");
                stringBuilder.AppendLine($"  ▫ Id： {kv.Key}");
                stringBuilder.AppendLine($"  ▫ 状态: " + (kv.Value.Status ? "运行中" : "已停止"));
                stringBuilder.AppendLine(
                    "  ▫ 启动命令行："
                        + kv.Value.Configuration.FileName
                        + " "
                        + kv.Value.Configuration.Argument
                );
            }

            _logger.LogInformation(stringBuilder.ToString());
            return;
        }

        if (
            args[1].Equals("switch", StringComparison.InvariantCultureIgnoreCase)
            && args.Count == 2
        )
        {
            throw new InvalidArgumentException("缺少服务器Id。");
        }

        var id = args.Count == 3 ? args[2] : _serverSwitcher.CurrentId;

        if (string.IsNullOrEmpty(id))
        {
            throw new InvalidArgumentException(
                "缺少服务器Id。"
                    + "你可以在命令末尾添加服务器Id或使用\"server switch <id>\"选择你要控制的服务器"
            );
        }

        if (!_serverManager.Servers.TryGetValue(id, out Server? server))
        {
            throw new InvalidArgumentException("指定的服务器不存在");
        }

        switch (args[1].ToLowerInvariant())
        {
            case "info" when args.Count <= 3:
                var info = server.Info;

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

            case "start" when args.Count <= 3:
                try
                {
                    server.Start();

                    if (string.IsNullOrEmpty(_settingProvider.Value.Application.CliCommandHeader))
                    {
                        _settingProvider.Value.Application.CliCommandHeader = "//";
                    }

                    _logger.LogWarning(
                        "服务器已启动，输入的命令将转发至服务器。若要执行Serein的命令，你需要在命令前加上\"{}\"",
                        _settingProvider.Value.Application.CliCommandHeader
                    );
                }
                catch (Exception e)
                {
                    _logger.LogError("启动失败：{}", e.Message);
                }
                break;

            case "stop" when args.Count <= 3:
                try
                {
                    server.Stop();
                }
                catch (Exception e)
                {
                    _logger.LogError("关闭失败：{}", e.Message);
                }
                break;

            case "switch" when args.Count == 3:
                try
                {
                    _serverSwitcher.SwitchTo(id);
                }
                catch (Exception e)
                {
                    _logger.LogError("选择服务器失败：{}", e.Message);
                }
                break;

            case "terminate" when args.Count <= 3:
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
                    "未知的参数。可用值：\"info\"、\"start\"、\"stop\"、\"terminate\"和\"switch\""
                );
        }
    }
}
