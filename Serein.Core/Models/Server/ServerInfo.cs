using System;

namespace Serein.Core.Models.Server;

public class ServerInfo : IServerInfo
{
    public string? FileName { get; internal set; }

    public string? Argument { get; internal set; }

    public DateTime? StartTime { get; internal set; }

    public DateTime? ExitTime { get; internal set; }

    public ulong OutputLines { get; internal set; }

    public ulong InputLines { get; internal set; }

    public double CPUUsage { get; internal set; }

    public Motd.Motd? Motd { get; internal set; }
}
