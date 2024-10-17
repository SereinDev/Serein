using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Extensions.DependencyInjection;

using PrettyPrompt.Completion;
using PrettyPrompt.Highlighting;

using Serein.Cli.Models;
using Serein.Cli.Services.Interaction.Handlers;
using Serein.Core;

namespace Serein.Cli.Services.Interaction;

public class CommandProvider
{
    public IReadOnlyList<CompletionItem> RootCommandItems { get; }

    public IReadOnlyDictionary<string, CommandHandler> Handlers { get; }

    public string HelpPage { get; }

    public CommandProvider(IServiceProvider serviceProvider)
    {
        CommandHandler[] commands =
        [
            serviceProvider.GetRequiredService<ServerHandler>(),
            serviceProvider.GetRequiredService<ConnectionHandler>(),
            serviceProvider.GetRequiredService<PluginHandler>(),
            serviceProvider.GetRequiredService<ClearScreenHandler>(),
            serviceProvider.GetRequiredService<VersionHandler>(),
            serviceProvider.GetRequiredService<ExitHandler>(),
            serviceProvider.GetRequiredService<HelpHandler>(),
        ];

        var stringBuilder = new StringBuilder();
        var dict = new Dictionary<string, CommandHandler>();
        var list = new List<CompletionItem>();

        stringBuilder.AppendLine($"Serein.Cli {SereinApp.Version}");

        foreach (var command in commands)
        {
            var type = command.GetType();
            var attribute = type.GetCustomAttribute<CommandNameAttribute>();
            if (attribute is null)
                continue;

            dict[attribute.RootCommand] = command;
            if (attribute.RootCommand == "help")
                dict["?"] = command;

            GenerateHelpPage(list, stringBuilder, type);
        }

        HelpPage = stringBuilder.ToString();
        Handlers = dict;
        RootCommandItems = list;
    }

    private static void GenerateHelpPage(
        List<CompletionItem> completionItems,
        StringBuilder stringBuilder,
        Type type
    )
    {
        var nameAttribute = type.GetCustomAttribute<CommandNameAttribute>();
        var descriptionAttribute = type.GetCustomAttribute<CommandDescriptionAttribute>();
        if (nameAttribute is null || descriptionAttribute is null)
            return;

        stringBuilder.AppendLine($"■ {nameAttribute.RootCommand}  {nameAttribute.Name}");

        stringBuilder.AppendLine(" ▢ 描述");
        foreach (var line in descriptionAttribute.Lines)
            stringBuilder.AppendLine($"  ▫ {line}");


        var childrenAttributes = type.GetCustomAttributes<CommandChildrenAttribute>();
        if (childrenAttributes.Any())
        {
            stringBuilder.AppendLine(" ▢ 用法");
            foreach (var child in childrenAttributes)
                stringBuilder.AppendLine(
                    $"  ▫ {nameAttribute.RootCommand} {child.Command}  {child.Description}"
                );
        }

        stringBuilder.AppendLine();
        completionItems.Add(CreateCompletionItem(nameAttribute, descriptionAttribute));
    }

    private static CompletionItem CreateCompletionItem(
        CommandNameAttribute nameAttribute,
        CommandDescriptionAttribute descriptionAttribute
    )
    {
        return new(
            nameAttribute.RootCommand,
            getExtendedDescription: (cancellationToken) =>
            {
                var stringBuilder = new StringBuilder();
                stringBuilder.AppendLine(nameAttribute.Name);
                stringBuilder.AppendLine();

                if (descriptionAttribute is not null)
                {
                    stringBuilder.AppendLine("描述");
                    foreach (var line in descriptionAttribute.Lines)
                        stringBuilder.AppendLine($"▫ {line}");
                }

                return Task.FromResult<FormattedString>(
                    new(
                        stringBuilder.ToString(),
                        new FormatSpan(0, nameAttribute.Name.Length, AnsiColor.BrightWhite)
                    )
                );
            }
        );
    }
}
