using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Serein.Console.Models;
using Serein.Console.Utils;
using Serein.Core.Models.Plugins;
using Serein.Core.Services.Plugins;
using Serein.Core.Services.Plugins.Js;
using Serein.Core.Services.Plugins.Net;
using Spectre.Console;

namespace Serein.Console.Services.Interaction.Handlers;

public sealed class PluginHandler(
    PluginManager pluginManager,
    JsPluginLoader jsPluginLoader,
    NetPluginLoader netPluginLoader
) : CommandHandler
{
    public override string Name { get; } = "插件";

    public override string[] Description { get; } = ["重新加载插件", "管理插件", "禁用插件"];

    public override string RootCommand { get; } = "plugin";

    public override Dictionary<string, string> SubCommands { get; } =
        new()
        {
            ["reload"] = "重新加载插件",
            ["info"] = "显示插件信息",
            ["list"] = "显示插件列表",
            ["disable"] = "禁用插件",
        };

    public override void Invoke(string subCommand, IReadOnlyList<string> args)
    {
        switch (subCommand)
        {
            case "reload":
                Task.Run(pluginManager.Reload);
                break;

            case "list":
                LogList();
                break;

            case "info":
                if (args.Count == 2)
                {
                    throw new InvalidArgumentException("缺少插件Id");
                }

                if (jsPluginLoader.Plugins.TryGetValue(args[2], out var jsPlugin1))
                {
                    LogPluginInfo(args[2], jsPlugin1);
                }
                else if (netPluginLoader.Plugins.TryGetValue(args[2], out var netPlugin1))
                {
                    LogPluginInfo(args[2], netPlugin1);
                }
                else
                {
                    throw new InvalidArgumentException("未找到插件");
                }
                break;

            case "disable":
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
        }
    }

    private void LogList()
    {
        var table = new Table()
            .AddColumn("[bold white]类型[/]")
            .AddColumn("[bold white]名称[/]")
            .AddColumn("[bold white]状态[/]")
            .AddColumn("[bold white]版本[/]")
            .AddColumn("[bold white]作者[/]")
            .AddColumn("[bold white]描述[/]")
            .RoundedBorder()
            .ShowRowSeparators();

        foreach (var plugin in jsPluginLoader.Plugins)
        {
            AppendPluginInfo(plugin.Key, plugin.Value);
        }

        foreach (var plugin in netPluginLoader.Plugins)
        {
            AppendPluginInfo(plugin.Key, plugin.Value);
        }

        SimpleConsole.Log(
            LogLevel.Information,
            $"当前共有{jsPluginLoader.Plugins.Count + netPluginLoader.Plugins.Count}个插件。",
            table
        );

        void AppendPluginInfo(string key, IPlugin plugin)
        {
            table.AddRow(
                new Markup(plugin is JsPlugin ? $"[#FFE70B]JavaScript[/]" : $"[#512BD4].NET[/]"),
                new Columns(
                    new Markup($"{plugin.Info.Name.EscapeMarkup()}"),
                    new Markup($"[gray italic]({key.EscapeMarkup()})[/]")
                ).Collapse(),
                new Markup(plugin.IsEnabled ? "已启用" : "[IndianRed]已禁用[/]"),
                new Text(plugin.Info.Version.ToString()),
                new Text(string.Join(',', plugin.Info.Authors.Select(author => author.ToString()))),
                new Text(plugin.Info.Description ?? string.Empty)
            );
        }
    }

    private void LogPluginInfo(string id, IPlugin plugin)
    {
        var table = new Table()
            .RoundedBorder()
            .ShowRowSeparators()
            .AddColumn("_", c => c.Centered())
            .AddColumn("_")
            .HideHeaders()
            .AddRow(
                new Markup("[bold white]类型[/]"),
                new Markup(plugin is JsPlugin ? $"[#FFE70B]JavaScript[/]" : $"[#512BD4].NET[/]")
            )
            .AddRow(
                new Markup("[bold white]状态[/]"),
                new Markup(plugin.IsEnabled ? "已启用" : "[IndianRed]已禁用[/]")
            )
            .AddRow(
                new Markup("[bold white]Id[/]"),
                new Markup(id.EscapeMarkup(), new(decoration: Decoration.Italic))
            )
            .AddRow(new Markup("[bold white]名称[/]"), new Text(plugin.Info.Name))
            .AddRow(new Markup("[bold white]版本[/]"), new Text(plugin.Info.Version.ToString()))
            .AddRow(
                new Markup("[bold white]作者[/]"),
                new Text(string.Join('\n', plugin.Info.Authors.Select(author => author.ToString())))
            )
            .AddRow(
                new Markup("[bold white]描述[/]"),
                new Text(plugin.Info.Description ?? string.Empty)
            )
            .AddRow(
                new Markup("[bold white]入口点[/]"),
                new TextPath(plugin.FileName)
                {
                    LeafStyle = Color.White,
                    SeparatorStyle = Color.Gray,
                }
            );

        SimpleConsole.Log(LogLevel.Information, table);
    }
}
