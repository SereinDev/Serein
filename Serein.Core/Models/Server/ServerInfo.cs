using System;

namespace Serein.Core.Models.Server;

public class ServerInfo : IServerInfo
{
    public string? FileName { get; set; }

    public string? Argument { get; set; }

    public DateTime? StartTime { get; set; }

    public DateTime? ExitTime { get; set; }

    public ulong OutputLines { get; set; }

    public ulong InputLines { get; set; }

    public double CPUUsage { get; set; }

    public Motd.Motd? Motd { get; set; }
}