using Serein.Core.Services.Commands;
using Serein.Core.Services.Servers;

namespace Serein.Core.Services.Plugins.Js.Properties;

internal class PropertyFactory(
    ServerManager serverManager,
    PluginManager pluginManager,
    CommandRunner commandRunner,
    CommandParser commandParser
)
{
    public ServerProperty ServerProperty { get; } = new(serverManager);

    public CommandProperty CommandProperty { get; } =
        new(pluginManager, commandRunner, commandParser);
}
