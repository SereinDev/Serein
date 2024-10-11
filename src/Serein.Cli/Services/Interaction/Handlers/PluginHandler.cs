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
[CommandDescription(["重新加载插件", "管理插件"])]
[CommandChildren("reload", "重新加载插件")]
[CommandChildren("list", "显示插件列表")]
public class PluginHandler(
    ILogger<PluginHandler> logger,
    PluginManager pluginManager,
    JsPluginLoader jsPluginLoader,
    NetPluginLoader netPluginLoader
) : CommandHandler
{
    private readonly ILogger<PluginHandler> _logger = logger;
    private readonly PluginManager _pluginManager = pluginManager;
    private readonly JsPluginLoader _jsPluginLoader = jsPluginLoader;
    private readonly NetPluginLoader _netPluginLoader = netPluginLoader;

    public override void Invoke(IReadOnlyList<string> args)
    {
        if (args.Count == 1)
            throw new InvalidArgumentException("缺少参数。可用值：\"reload\"和\"list\"");

        switch (args[1].ToLowerInvariant())
        {
            case "reload":
                Task.Run(_pluginManager.Reload);
                break;

            case "list":
                var stringBuilder = new StringBuilder();

                stringBuilder.AppendLine(
                    $"当前共有{_jsPluginLoader.Plugins.Count + _netPluginLoader.Plugins.Count}个插件"
                );
                foreach (var plugin in _jsPluginLoader.Plugins)
                {
                    stringBuilder.AppendLine($"[JS] {plugin.Key}");
                    AppendPluginInfo(stringBuilder, plugin.Value);
                }
                foreach (var plugin in _netPluginLoader.Plugins)
                {
                    stringBuilder.AppendLine($"[NET] {plugin.Key}");
                    AppendPluginInfo(stringBuilder, plugin.Value);
                }

                _logger.LogInformation("{}", stringBuilder);
                break;

            default:
                throw new InvalidArgumentException("未知的参数。可用值：\"reload\"和\"list\"");
        }

        static void AppendPluginInfo(StringBuilder stringBuilder, IPlugin plugin)
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
}
