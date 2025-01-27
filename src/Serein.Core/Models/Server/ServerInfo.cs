using System;
using MineStatLib;

namespace Serein.Core.Models.Server;

internal class ServerInfo : NotifyPropertyChangedModelBase, IServerInfo
{
    public string? FileName { get; internal set; }

    public string? Argument { get; internal set; }

    public DateTime? StartTime { get; internal set; }

    public DateTime? ExitTime { get; internal set; }

    public ulong OutputLines { get; internal set; }

    public ulong InputLines { get; internal set; }

    public int CpuUsage { get; internal set; }

    public MineStat? Stat { get; internal set; }
}
