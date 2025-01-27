using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Serein.Cli.Models;
using Serein.Core.Models.Plugins;
using Serein.Core.Services.Plugins;
using Serein.Core.Services.Plugins.Js;
using Serein.Core.Services.Plugins.Net;

namespace Serein.Cli.Services.Interaction.Handlers;

[CommandName("plugin", "插件")]
[CommandDescription(["重新加载插件", "管理插件", "禁用插件"])]
[CommandChildren("reload", "重新加载插件")]
[CommandChildren("list", "显示插件列表")]
[CommandChildren("disable", "禁用插件")]
public sealed class PluginHandler(
    ILogger<PluginHandler> logger,
    PluginManager pluginManager,
    JsPluginLoader jsPluginLoader,
    NetPluginLoader netPluginLoader
) : CommandHandler
{
    public override void Invoke(IReadOnlyList<string> args)
    {
        if (args.Count == 1)
        {
            throw new InvalidArgumentException(
                "缺少参数。可用值：\"reload\"、\"list\"和\"disable\""
            );
        }

        switch (args[1].ToLowerInvariant())
        {
            case "reload" when args.Count == 2:
                Task.Run(pluginManager.Reload);
                break;

            case "list" when args.Count == 2:
                var stringBuilder = new StringBuilder();

                stringBuilder.AppendLine(
                    $"当前共有{jsPluginLoader.Plugins.Count + netPluginLoader.Plugins.Count}个插件"
                );
                foreach (var plugin in jsPluginLoader.Plugins)
                {
                    stringBuilder.AppendLine($"[JS] {plugin.Key}");
                    AppendPluginInfo(stringBuilder, plugin.Value);
                }
                foreach (var plugin in netPluginLoader.Plugins)
                {
                    stringBuilder.AppendLine($"[NET] {plugin.Key}");
                    AppendPluginInfo(stringBuilder, plugin.Value);
                }

                logger.LogInformation("{}", stringBuilder);
                break;

            case "disable" when args.Count == 3:
                if (args.Count == 2)
                {
                    throw new InvalidArgumentException("缺少插件Id");
                }

                if (jsPluginLoader.Plugins.TryGetValue(args[2], out var jsPlugin))
                {
                    if (jsPlugin.IsEnabled)
                    {
                        jsPlugin.Disable();
                    }
                    else
                    {
                        throw new InvalidOperationException("插件已经被禁用");
                    }
                }

                if (netPluginLoader.Plugins.TryGetValue(args[2], out var netPlugin))
                {
                    if (netPlugin.IsEnabled)
                    {
                        netPlugin.Disable();
                    }
                    else
                    {
                        throw new InvalidOperationException("插件已经被禁用");
                    }
                }
                break;

            default:
                throw new InvalidArgumentException(
                    "未知的参数。可用值：\"reload\"、\"list\"和\"disable\""
                );
        }
    }

    private static void AppendPluginInfo(StringBuilder stringBuilder, IPlugin plugin)
    {
        stringBuilder.AppendLine($" ▫ 名称 {plugin.Info.Name}");
        stringBuilder.AppendLine($" ▫ 描述 {plugin.Info.Description}");
        stringBuilder.AppendLine($" ▫ 版本 {plugin.Info.Version}");
        stringBuilder.AppendLine(
            $" ▫ 作者 {string.Join(',', plugin.Info.Authors.Select(author => author.ToString()))}"
        );
        stringBuilder.AppendLine($" ▫ 状态 {(plugin.IsEnabled ? "已启用" : "已禁用")}");
    }
}
