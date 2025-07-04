using System;

namespace Serein.Core.Models.Plugins.Info;

public sealed record PluginTargets(Version? Min, Version? Max);
