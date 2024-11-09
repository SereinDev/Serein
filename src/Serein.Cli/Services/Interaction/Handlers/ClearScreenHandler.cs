using System;
using System.Collections.Generic;

using Serein.Cli.Models;

namespace Serein.Cli.Services.Interaction.Handlers;

[CommandName("clear", "清屏")]
[CommandDescription(["清除控制台所有输出"])]
public sealed class ClearScreenHandler() : CommandHandler
{
    public override void Invoke(IReadOnlyList<string> args)
    {
        Console.Clear();
    }
}
