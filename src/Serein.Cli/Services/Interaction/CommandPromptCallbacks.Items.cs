using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

using PrettyPrompt.Completion;
using PrettyPrompt.Highlighting;

using Serein.Cli.Models;
using Serein.Cli.Services.Interaction.Handlers;
using Serein.Core.Models.Plugins;
using Serein.Core.Models.Plugins.Js;
using Serein.Core.Models.Server;
using Serein.Core.Services.Servers;

namespace Serein.Cli.Services.Interaction;

public partial class CommandPromptCallbacks
{
    private readonly Lazy<IEnumerable<CompletionItem>> _connectionSubcommnads =
        new(LoadFrom<ConnectionHandler>);

    private readonly Lazy<IEnumerable<CompletionItem>> _serverSubcommnads =
        new(LoadFrom<ServerHandler>);

    private readonly Lazy<IEnumerable<CompletionItem>> _pluginSubcommnads =
        new(LoadFrom<PluginHandler>);

    private readonly Task<IReadOnlyList<CompletionItem>> _emptyTask = Task.FromResult<
        IReadOnlyList<CompletionItem>
    >([]);

    private IEnumerable<CompletionItem> GetServerCompletionItem()
    {
        return _serverManager.Servers.Select(
            (kv) =>
                new CompletionItem(kv.Key, getExtendedDescription: (_) => GetDescription(kv))
        );

        static Task<FormattedString> GetDescription(KeyValuePair<string, Server> kv)
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.AppendLine(kv.Value.Configuration.Name);
            stringBuilder.AppendLine();
            stringBuilder.AppendLine(
                "状态：" + (kv.Value.Status ? "运行中" : "已停止")
            );
            stringBuilder.AppendLine(
                "启动命令行："
                    + kv.Value.Configuration.FileName
                    + " "
                    + kv.Value.Configuration.Argument
            );
            return Task.FromResult(
                new FormattedString(
                    stringBuilder.ToString(),
                    new FormatSpan(0, kv.Value.Configuration.Name.Length, AnsiColor.BrightWhite)
                )
            );
        }
    }

    private IEnumerable<CompletionItem> GetPluginIdCompletionItem()
    {
        var dictionary = new Dictionary<string, IPlugin>();

        foreach (var kv in _jsPluginLoader.Plugins)
        {
            dictionary.TryAdd(kv.Key, kv.Value);
        }

        foreach (var kv in _netPluginLoader.Plugins)
        {
            dictionary.TryAdd(kv.Key, kv.Value);
        }

        return dictionary.Select(
            (kv) =>
                new CompletionItem(kv.Key, getExtendedDescription: (_) => GetDescription(kv))
        );

        static Task<FormattedString> GetDescription(KeyValuePair<string, IPlugin> kv)
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.AppendLine(kv.Value.Info.Name);
            stringBuilder.AppendLine();
            stringBuilder.AppendLine("类型：" + (kv.Value is JsPlugin ? "Js" : "Net"));
            stringBuilder.AppendLine($"版本：{kv.Value.Info.Version}");
            stringBuilder.AppendLine($"描述：{kv.Value.Info.Description}");
            stringBuilder.AppendLine(
                $" 作者：{string.Join(',', kv.Value.Info.Authors.Select(author => author.ToString()))}"
                );
            return Task.FromResult(
                new FormattedString(
                    stringBuilder.ToString(),
                    new FormatSpan(0, kv.Value.Info.Name.Length, AnsiColor.BrightWhite)
                )
            );
        }
    }

    private static IEnumerable<CompletionItem> LoadFrom<THandler>()
        where THandler : CommandHandler
    {
        return typeof(THandler)
            .GetCustomAttributes<CommandChildrenAttribute>()
            .Select(
                (attr) =>
                    new CompletionItem(
                        attr.Command,
                        getExtendedDescription: (_) =>
                            Task.FromResult(new FormattedString(attr.Description))
                    )
            );
    }
}
