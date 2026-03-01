using System;
using System.Collections.Frozen;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using PrettyPrompt.Completion;
using PrettyPrompt.Highlighting;
using Serein.Console.Services.Interaction.Handlers;
using Serein.Core;
using Spectre.Console;

namespace Serein.Console.Services.Interaction;

public sealed class CommandProvider
{
    private readonly List<CompletionItem> _completionItems = [];

    public IReadOnlyList<CompletionItem> CompletionItems => _completionItems;

    public IReadOnlyDictionary<string, CommandHandler> Handlers { get; }

    public Table HelpTable { get; }

    public CommandProvider(IServiceProvider serviceProvider, SereinApp sereinApp)
    {
        HelpTable = new Table()
            .RoundedBorder()
            .AddColumn("命令")
            .AddColumn("描述")
            .AddColumn("子命令")
            .ShowRowSeparators()
            .Title($"[bold DarkSeaGreen3]Serein.Console {sereinApp.Version}[/]");

        CommandHandler[] commandHandlers =
        [
            serviceProvider.GetRequiredService<ServerHandler>(),
            serviceProvider.GetRequiredService<ConnectionHandler>(),
            serviceProvider.GetRequiredService<PluginHandler>(),
            serviceProvider.GetRequiredService<WebServerHandler>(),
            serviceProvider.GetRequiredService<ClearScreenHandler>(),
            serviceProvider.GetRequiredService<VersionHandler>(),
            serviceProvider.GetRequiredService<ExitHandler>(),
            serviceProvider.GetRequiredService<HelpHandler>(),
        ];

        var dict = new Dictionary<string, CommandHandler>();

        foreach (var commandHandler in commandHandlers)
        {
            dict[commandHandler.RootCommand] = commandHandler;

            if (!string.IsNullOrEmpty(commandHandler.Alias))
            {
                dict[commandHandler.Alias] = commandHandler;
            }

            GenerateHelpPage(commandHandler);
            AddCompletionItem(commandHandler);
        }

        Handlers = dict.ToFrozenDictionary();
    }

    private void GenerateHelpPage(CommandHandler commandHandler)
    {
        HelpTable.AddRow(
            new Markup($"[bold white]{commandHandler.RootCommand}[/]"),
            new Markup(
                string.Join(
                    '\n',
                    commandHandler.Description.Length > 1
                        ? commandHandler.Description.Select(line => "▫ " + line)
                        : commandHandler.Description
                )
            ),
            new Markup(
                commandHandler.SubCommands.Count > 0
                    ? string.Join(
                        '\n',
                        commandHandler.SubCommands.Select(kv => $"[white]{kv.Key}[/] - {kv.Value}")
                    )
                    : "[grey italic]无[/]"
            )
        );
    }

    private void AddCompletionItem(CommandHandler commandHandler)
    {
        var completionItem = new CompletionItem(
            commandHandler.RootCommand,
            getExtendedDescription: (_) =>
            {
                var stringBuilder = new StringBuilder();
                stringBuilder.AppendLine(commandHandler.Name);
                stringBuilder.AppendLine();

                if (commandHandler.Description.Length > 0)
                {
                    stringBuilder.AppendLine("描述");
                    foreach (var line in commandHandler.Description)
                    {
                        stringBuilder.AppendLine($"▫ {line}");
                    }
                }

                return Task.FromResult<FormattedString>(
                    new(
                        stringBuilder.ToString(),
                        new FormatSpan(0, commandHandler.Name.Length, AnsiColor.BrightWhite)
                    )
                );
            }
        );

        _completionItems.Add(completionItem);
    }
}
