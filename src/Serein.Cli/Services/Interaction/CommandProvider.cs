using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Extensions.Hosting;

using PrettyPrompt.Completion;
using PrettyPrompt.Highlighting;

using Serein.Cli.Models;
using Serein.Cli.Services.Interaction.Handlers;
using Serein.Core;

namespace Serein.Cli.Services.Interaction;

public class CommandProvider
{
    private readonly IHost _host;

    public IReadOnlyList<CompletionItem> RootCommandItems { get; }

    public IReadOnlyDictionary<string, CommandHandler> Handlers { get; }

    public string HelpPage { get; }

    public CommandProvider(IHost host)
    {
        _host = host;
        CommandHandler[] commands =
        [
            new ServerHandler(_host),
            new ConnectionHandler(_host),
            new ClearScreenHandler(),
            new VersionHandler(_host),
            new ExitHandler(_host),
            new HelpHandler(this, _host),
        ];

        var stringBuilder = new StringBuilder();
        var attributes =
            new List<(
                CommandNameAttribute,
                CommandDescriptionAttribute?,
                IEnumerable<CommandUsageAttribute>
            )>();
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

        var usageAttributes = type.GetCustomAttributes<CommandUsageAttribute>();
        if (usageAttributes.Any())
        {
            stringBuilder.AppendLine(" ▢ 用法");
            foreach (var usage in usageAttributes)
                stringBuilder.AppendLine($"  ▫ {usage.Example}  {usage.Description}");
        }
        else
        {
            var childrenAttributes = type.GetCustomAttributes<CommandChildrenAttribute>();
            if (childrenAttributes.Any())
            {
                stringBuilder.AppendLine(" ▢ 用法");
                foreach (var child in childrenAttributes)
                    stringBuilder.AppendLine(
                        $"  ▫ {nameAttribute.RootCommand} {child.Command}  {child.Description}"
                    );
            }
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
