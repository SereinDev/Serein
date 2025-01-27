using System;

namespace Serein.Core.Models.Network.Web;

public class ApiPacket<T>
{
    public int Code { get; init; } = 200;

    public string? ErrorMsg { get; init; }

    public T? Data { get; init; }

    public DateTime Time { get; } = DateTime.Now;
}

public class ApiPacket : ApiPacket<object> { }
