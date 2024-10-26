using System.Text.Json.Serialization;

namespace Serein.Core.Models.Network.WebApi;

public class WebSocketBroadcastPacket(WebSocketBroadcastType type, string? data = null)
{
    public string? Data { get; init; } = data;

    [JsonConverter(typeof(JsonStringEnumConverter<WebSocketBroadcastType>))]
    public WebSocketBroadcastType Type { get; init; } = type;
}