using System;

namespace Serein.Core.Models.Server;

public interface IServerInfo
{
    public string? FileName { get; }

    public string? Argument { get; }

    public DateTime? StartTime { get; }

    public DateTime? ExitTime { get; }

    public ulong OutputLines { get; }

    public ulong InputLines { get; }

    public double CPUUsage { get; }

    public Motd.Motd? Motd { get; }
}
