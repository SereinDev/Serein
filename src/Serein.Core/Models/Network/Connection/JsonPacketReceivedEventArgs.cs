using System;
using System.Text.Json.Nodes;

namespace Serein.Core.Models.Network;

public class JsonPacketReceivedEventArgs(JsonNode packet) : EventArgs
{
    public JsonNode Packet { get; } = packet;
}
