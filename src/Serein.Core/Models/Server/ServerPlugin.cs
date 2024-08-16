using System;
using System.IO;
using System.Text.Json.Serialization;

using Serein.Core.Services.Servers;

namespace Serein.Core.Models.Server;

public class ServerPlugin
{
    public string Path { get; }

    public string FriendlyName { get; }

    public PluginType Type { get; }

    public bool IsEnable { get; }

    public FileInfo FileInfo => _fileInfo.Value;

    private readonly Lazy<FileInfo> _fileInfo;

    public ServerPlugin(string path, PluginType type)
    {
        Path = path;
        Type = type;
        IsEnable = !Path.EndsWith(ServerPluginManager.DisabledPluginExtension);
        _fileInfo = new(() => new(Path));

        FriendlyName = IsEnable
            ? System.IO.Path.GetFileName(Path)
            : System.IO.Path.GetFileNameWithoutExtension(Path);
    }
}
