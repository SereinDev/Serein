using System.Threading.Tasks;
using Serein.Core.Models.Commands;
using Serein.Core.Services.Commands;

namespace Serein.Core.Services.Plugins.Js.Properties;

public sealed class CommandProperty
{
    private readonly PluginManager _pluginManager;
    private readonly CommandRunner _commandRunner;
    private readonly CommandParser _commandParser;

    internal CommandProperty(
        PluginManager pluginManager,
        CommandRunner commandRunner,
        CommandParser commandParser
    )
    {
        _pluginManager = pluginManager;
        _commandRunner = commandRunner;
        _commandParser = commandParser;
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

    public string ApplyVariables(
        string input,
        CommandContext? commandContext = null,
        bool removeInvalidVariablePatten = false
    )
    {
        return _commandParser.ApplyVariables(input, commandContext, removeInvalidVariablePatten);
    }

    public void SetVariable(string key, string? value)
    {
        if (value == null)
        {
            _pluginManager.CommandVariables.TryRemove(key, out _);
        }
        else
        {
            _pluginManager.CommandVariables.AddOrUpdate(key, value, (_, _) => value);
        }
    }
}
