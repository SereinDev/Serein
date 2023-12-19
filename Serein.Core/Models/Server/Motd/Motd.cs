using System.Net;

namespace Serein.Core.Models.Server.Motd;

public abstract class Motd
{
    public IPAddress IP { get; private set; } = IPAddress.Loopback;

    public int Port { get; protected set; } = -1;

    public long PlayerCapacity { get; protected set; }

    public long OnlinePlayers { get; protected set; }

    public string? Description { get; protected set; }

    public string? Protocol { get; protected set; }

    public string? Version { get; protected set; }

    public string? LevelName { get; protected set; }

    public string? GameMode { get; protected set; }

    public double Latency { get; protected set; }

    public string? Favicon { get; protected set; }

    public string? FaviconCQCode =>
        string.IsNullOrEmpty(Favicon)
            ? string.Empty
            : $"[CQ:image,file=base64://{Favicon![(Favicon.IndexOf(',') + 1)..]}]";

    public string? OriginText { get; protected set; }
    public string? Exception { get; protected set; }
    public bool IsSuccessful { get; protected set; }
}