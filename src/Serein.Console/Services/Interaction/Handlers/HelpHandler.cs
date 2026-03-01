using System;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using Spectre.Console;

namespace Serein.Console.Services.Interaction.Handlers;

public sealed class HelpHandler(IServiceProvider serviceProvider) : CommandHandler
{
    private readonly Lazy<CommandProvider> _commandProvider = new(
        serviceProvider.GetRequiredService<CommandProvider>
    );

    public override string Name { get; } = "帮助";

    public override string[] Description { get; } = ["显示帮助页面"];

    public override string RootCommand { get; } = "help";

    public override string? Alias { get; } = "?";

    public override Dictionary<string, string> SubCommands { get; } = [];

    public override bool AllowToExecuteRootCommand { get; } = true;

    public override void Invoke(string subCommand, IReadOnlyList<string> args)
    {
        AnsiConsole.Write(_commandProvider.Value.HelpTable);
    }
}
