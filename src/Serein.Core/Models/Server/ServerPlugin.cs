using System.IO;
using Serein.Core.Services.Servers;

namespace Serein.Core.Models.Server;

public class ServerPlugin
{
    public string Path { get; }

    public string FriendlyName { get; }

    public PluginType Type { get; }

    public bool IsEnabled { get; }

    public FileInfo FileInfo { get; }

    public ServerPlugin(string path, PluginType type)
    {
        Path = path;
        Type = type;
        IsEnabled = !Path.EndsWith(ServerPluginManager.DisabledPluginExtension);
        FileInfo = new(path);

        FriendlyName = IsEnabled
            ? System.IO.Path.GetFileName(Path)
            : System.IO.Path.GetFileNameWithoutExtension(Path);
    }
}
