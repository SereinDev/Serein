using System.Collections.Generic;
using Spectre.Console;

namespace Serein.Console.Services.Interaction.Handlers;

public sealed class ClearScreenHandler : CommandHandler
{
    public override Dictionary<string, string> SubCommands { get; } = [];

    public override string Name { get; } = "清屏";

    public override string[] Description { get; } = ["清除控制台所有输出"];

    public override string RootCommand { get; } = "clear";

    public override bool AllowToExecuteRootCommand { get; } = true;

    public override void Invoke(string subCommand, IReadOnlyList<string> args)
    {
        AnsiConsole.Clear();
    }
}
