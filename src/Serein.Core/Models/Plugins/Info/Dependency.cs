using System;

namespace Serein.Core.Models.Plugins.Info;

public sealed class Dependency
{
    public string Id { get; set; } = string.Empty;

    public Version[] Version { get; set; } = [];
}
