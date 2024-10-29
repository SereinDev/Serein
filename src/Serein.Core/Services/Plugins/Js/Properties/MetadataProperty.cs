namespace Serein.Core.Services.Plugins.Js.Properties;

public class MetadataProperty
{
    internal MetadataProperty() { }

    public string Version { get; } = SereinApp.Version;

    public string? FullVersion { get; } = SereinApp.FullVersion;

    public AppType Type { get; } = SereinApp.Type;
}