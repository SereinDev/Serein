using System;

using Serein.Cli.Models;

namespace Serein.Cli.Services.Interaction.Handlers;

[CommandDescription("cls", "清屏", Priority = -1)]
public class ClearScreenHandler() : CommandHandler
{
    public override void Invoke(string[] args)
    {
        Console.Clear();
    }
}
