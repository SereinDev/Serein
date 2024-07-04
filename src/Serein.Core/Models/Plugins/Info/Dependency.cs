using System;

namespace Serein.Core.Models.Plugins.Info;

public record Dependency(string Id, Version[] Version);
