using System;

using Microsoft.Extensions.Hosting;

namespace Serein.Cli.Interaction.Commands;

[CommandDescription("cls", "清屏", Priority = -1)]
public class ClearScreenCommand(IHost host) : Command(host)
{
    public override void Parse(string[] args)
    {
        Console.Clear();
    }
}
