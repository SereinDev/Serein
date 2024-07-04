using System;
using System.Text.Json.Serialization;

namespace Serein.Core.Models.Plugins.Info;

public class PluginInfo
{
    [JsonRequired]
    public string Name { get; init; } = string.Empty;

    [JsonRequired]
    public string Id { get; init; } = string.Empty;

    public Version Version { get; init; } = new(0, 0, 0);

    public Author[] Authors { get; init; } = Array.Empty<Author>();

    public string? Description { get; init; }

    public PluginTag[] Tags { get; init; } = Array.Empty<PluginTag>();

    public Version[] TargetingSerein { get; init; } = Array.Empty<Version>();

    public Dependency[] Dependencies { get; init; } = Array.Empty<Dependency>();

    public string? EntryFile { get; init; }

    [JsonRequired]
    public PluginType PluginType { get; init; }
}
