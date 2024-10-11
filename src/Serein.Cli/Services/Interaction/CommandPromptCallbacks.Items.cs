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
using Serein.Core.Models.Server;
using Serein.Core.Services.Servers;

namespace Serein.Cli.Services.Interaction;

public partial class CommandPromptCallbacks
{
    private readonly Lazy<IEnumerable<CompletionItem>> _connectionSubcommnads =
        new(LoadFrom<ConnectionHandler>);

    private readonly Lazy<IEnumerable<CompletionItem>> _serverSubcommnads = new(LoadFrom<ServerHandler>);

    private readonly Lazy<IEnumerable<CompletionItem>> _pluginSubcommnads = new(LoadFrom<PluginHandler>);

    private IEnumerable<CompletionItem> GetServerCompletionItem()
    {
        return _serverManager.Servers.Select(
            (kv) =>
                new CompletionItem(kv.Key, getExtendedDescription: (token) => GetDescription(kv))
        );

        static Task<FormattedString> GetDescription(KeyValuePair<string, Server> kv)
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.AppendLine(kv.Value.Configuration.Name);
            stringBuilder.AppendLine();
            stringBuilder.AppendLine(
                "状态："
                    + kv.Value.Status switch
                    {
                        ServerStatus.Unknown => "未启动",
                        ServerStatus.Running => "运行中",
                        ServerStatus.Stopped => "已停止",
                        _ => throw new NotSupportedException(),
                    }
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

    private static IEnumerable<CompletionItem> LoadFrom<THandler>()
        where THandler : CommandHandler
    {
        return typeof(THandler)
            .GetCustomAttributes<CommandChildrenAttribute>()
            .Select(
                (attr) =>
                    new CompletionItem(
                        attr.Command,
                        getExtendedDescription: (token) =>
                            Task.FromResult(new FormattedString(attr.Description))
                    )
            );
    }
}
