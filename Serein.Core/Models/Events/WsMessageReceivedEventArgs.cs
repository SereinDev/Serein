using System;
using System.Text.Json.Nodes;

namespace Serein.Core.Models.Events;

public class WsMessageReceivedEventArgs : EventArgs
{
    public JsonNode? JsonData { get; set; }

    public string RawString { get; set; } = string.Empty;
}
