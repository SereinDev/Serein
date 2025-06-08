using System.Threading.Tasks;
using Serein.Core.Models.Commands;
using Serein.Core.Services.Commands;

namespace Serein.Core.Services.Plugins.Js.Properties;

public sealed class CommandProperty
{
    private readonly PluginManager _pluginManager;
    private readonly CommandRunner _commandRunner;

    internal CommandProperty(PluginManager pluginManager, CommandRunner commandRunner)
    {
        _pluginManager = pluginManager;
        _commandRunner = commandRunner;
    }

    public async Task RunAsync(string? command)
    {
        await _commandRunner.RunAsync(Parse(command));
    }

#pragma warning disable CA1822
    public Command Parse(string? command)
    {
        return CommandParser.Parse(CommandOrigin.Plugin, command);
    }
#pragma warning restore CA1822

    public void SetVariable(string key, string? value)
    {
        _pluginManager.SetCommandVariable(key, value);
    }
}
