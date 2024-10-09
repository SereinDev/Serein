using System.Collections.Generic;

namespace Serein.Cli.Services.Interaction.Handlers;

public abstract class CommandHandler()
{
    public abstract void Invoke(IReadOnlyList<string> args);
}
