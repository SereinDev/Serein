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

    public Author[] Authors { get; init; } = [];

    public string? Description { get; init; }

    public PluginTag[] Tags { get; init; } = [];

    public Version[] TargetingSerein { get; init; } = [];

    public Dependency[] Dependencies { get; init; } = [];

    public string? EntryFile { get; init; }

    [JsonRequired]
    public PluginType Type { get; init; }
}
