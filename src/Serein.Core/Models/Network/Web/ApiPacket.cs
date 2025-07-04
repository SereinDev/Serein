using System;

namespace Serein.Core.Models.Network.Web;

public class ApiPacket
{
    public object? Data { get; init; }

    public string? ErrorMsg { get; init; }

    public string[] Details { get; init; } = [];

    public DateTime Time { get; } = DateTime.Now;
}
