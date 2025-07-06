using System;
using System.Text.Json.Serialization;
using Serein.Core.Models.Abstractions;

namespace Serein.Core.Models.Plugins.Info;

public sealed class PluginInfo : NotifyPropertyChangedModelBase
{
    [JsonRequired]
    public string Name { get; set; } = string.Empty;

    [JsonRequired]
    public string Id { get; set; } = string.Empty;

    [JsonRequired]
    public Version Version { get; set; } = new(0, 0, 0);

    [JsonRequired]
    public PluginType Type { get; set; }

    public Author[] Authors { get; set; } = [];

    public string? Description { get; set; }

    public PluginTag[] Tags { get; set; } = [];

    public Dependency[] Dependencies { get; set; } = [];

    public string? EntryFile { get; set; }

    public PluginTargets? Targets { get; set; } = new();
}
