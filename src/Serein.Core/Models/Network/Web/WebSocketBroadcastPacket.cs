namespace Serein.Core.Models.Network.Web;

public class WebSocketBroadcastPacket(WebSocketBroadcastType type, string? data = null)
{
    public string? Data { get; init; } = data;

    public string Type { get; init; } = type.ToString().ToLowerInvariant();
}
