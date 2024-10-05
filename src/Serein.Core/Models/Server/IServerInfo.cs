using System;
using System.ComponentModel;

using MineStatLib;

namespace Serein.Core.Models.Server;

public interface IServerInfo : INotifyPropertyChanged
{
    public string? FileName { get; }

    public string? Argument { get; }

    public DateTime? StartTime { get; }

    public DateTime? ExitTime { get; }

    public ulong OutputLines { get; }

    public ulong InputLines { get; }

    public int CPUUsage { get; }

    public MineStat? Stat { get; }
}
