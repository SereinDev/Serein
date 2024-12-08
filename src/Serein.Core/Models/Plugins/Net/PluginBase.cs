using System.ComponentModel;
using System.Threading;
using Serein.Core.Models.Plugins.Info;
using Serein.Core.Services.Plugins;

namespace Serein.Core.Models.Plugins.Net;

public abstract partial class PluginBase : IPlugin
{
    public string FileName { get; internal set; } = null!;
    public PluginInfo Info { get; internal set; } = null!;

    public bool IsEnabled => !CancellationTokenSource.IsCancellationRequested;
    protected CancellationTokenSource CancellationTokenSource { get; }

    protected PluginBase()
    {
        CancellationTokenSource = new();
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    public void Disable()
    {
        CancellationTokenSource.Cancel();
        PropertyChanged?.Invoke(this, new(nameof(IsEnabled)));
    }

    public abstract void Dispose();

    public string Resolve(params string[] paths) => PluginManager.Resolve(this, paths);
}
