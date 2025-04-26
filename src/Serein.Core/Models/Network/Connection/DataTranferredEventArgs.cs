using System;
using System.Text.Json.Serialization;

namespace Serein.Core.Models.Network.Connection;

public class DataTranferredEventArgs : EventArgs
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public required DataTranferredType Type { get; init; }

    public required string Data { get; init; }
}
