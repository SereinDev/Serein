﻿using System;
using Serein.Core.Models.Abstractions;

namespace Serein.Plus.ViewModels;

public class PanelViewModel : NotifyPropertyChangedModelBase
{
    public bool Status { get; set; }

    public TimeSpan? RunTime { get; set; }

    public string? PlayerCount { get; set; }

    public string? Version { get; set; }

    public int? CpuUsage { get; set; }
}
