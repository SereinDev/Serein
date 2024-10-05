using System;

using Serein.Core.Models;
using Serein.Core.Models.Server;

namespace Serein.Plus.ViewModels;

public class PanelViewModel : NotifyPropertyChangedModelBase
{
    public ServerStatus Status { get; set; }

    public TimeSpan? RunTime { get; set; }

    public string? PlayerCount { get; set; }

    public string? Version { get; set; }

    public int? CPUUsage { get; set; }
}
