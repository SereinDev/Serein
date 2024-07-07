using System;
using System.Threading;

using Serein.Core.Models.Plugins.Info;

namespace Serein.Core.Models.Plugins.Net;

public abstract partial class PluginBase : IPlugin
{
    public string FileName { get; internal set; } = null!;
    public PluginInfo Info { get; internal set; } = null!;
    public IServiceProvider Services { get; internal set; } = null!;
    public bool IsEnabled => !CancellationTokenSource.IsCancellationRequested;
    protected CancellationTokenSource CancellationTokenSource { get; }

    protected PluginBase()
    {
        CancellationTokenSource = new();
    }

    public void Disable()
    {
        if (!CancellationTokenSource.IsCancellationRequested)
            CancellationTokenSource.Cancel();
    }

    public abstract void Dispose();
}
