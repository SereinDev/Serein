using System.Collections.Generic;

namespace Serein.Console.Services.Interaction.Handlers;

public abstract class CommandHandler
{
    public abstract string Name { get; }

    public abstract string[] Description { get; }

    public virtual bool AllowToExecuteRootCommand { get; }

    public abstract string RootCommand { get; }

    public abstract Dictionary<string, string> SubCommands { get; }

    public abstract void Invoke(string subCommand, IReadOnlyList<string> args);

    public virtual string? Alias { get; }
}
