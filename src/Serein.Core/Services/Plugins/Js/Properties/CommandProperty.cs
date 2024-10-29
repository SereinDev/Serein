using Serein.Core.Models.Commands;
using Serein.Core.Services.Commands;
using Serein.Core.Utils.Extensions;

namespace Serein.Core.Services.Plugins.Js.Properties;

public class CommandProperty
{
    private readonly PluginManager _pluginManager;
    private readonly CommandRunner _commandRunner;

    internal CommandProperty(PluginManager pluginManager, CommandRunner commandRunner)
    {
        _pluginManager = pluginManager;
        _commandRunner = commandRunner;
    }

    public void Run(string? command)
    {
        _commandRunner.RunAsync(Parse(command)).Await();
    }

#pragma warning disable CA1822
    public Command Parse(string? command)
    {
        return CommandParser.Parse(CommandOrigin.Plugin, command);
    }
#pragma warning restore CA1822

    public void SetVariable(string key, object? value)
    {
        _pluginManager.SetCommandVariable(key, value);
    }
}