using System.Collections.Generic;

namespace Serein.Core.Models.Network.Web;

public readonly record struct PluginManagerStatus
{
    public required bool IsLoading { get; init; }

    public required bool IsReloading { get; init; }

    public required IReadOnlyDictionary<string, string> CommandVariables { get; init; }
}
