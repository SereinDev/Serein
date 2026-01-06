using System.Collections.Generic;

namespace Serein.Console.Services.Interaction.Handlers;

public abstract class CommandHandler
{
    public abstract void Invoke(IReadOnlyList<string> args);
}
