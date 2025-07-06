namespace Serein.Core.Models.Network.Web;

public class BroadcastPacket(string type, string? origin, object? data)
{
    public string Type { get; init; } = type.ToLowerInvariant();

    public string? Origin { get; init; } = origin;

    public object? Data { get; init; } = data;
}
