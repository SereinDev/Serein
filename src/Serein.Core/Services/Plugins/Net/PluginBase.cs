using System.ComponentModel;
using System.Threading;
using Serein.Core.Models.Plugins;
using Serein.Core.Models.Plugins.Info;

namespace Serein.Core.Services.Plugins.Net;

public abstract partial class PluginBase : IPlugin
{
    public string FileName { get; }
    public PluginInfo Info { get; internal set; } = null!;

    public bool IsEnabled => !CancellationTokenSource.IsCancellationRequested;
    protected CancellationTokenSource CancellationTokenSource { get; }

    protected PluginBase()
    {
        CancellationTokenSource = new();
        CancellationTokenSource.Token.Register(
            () => PropertyChanged?.Invoke(this, new(nameof(IsEnabled)))
        );

#pragma warning disable IL3000 // Avoid accessing Assembly file path when publishing as a single file
        FileName = GetType().Assembly.Location;
#pragma warning restore IL3000
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    public void Disable()
    {
        CancellationTokenSource.Cancel();
        CancellationTokenSource.Dispose();
    }

    // TODO: 修改Dispose方法，确保插件在Dispose时能正确释放CancellationTokenSource
    public abstract void Dispose();

    public string Resolve(params string[] paths) => PluginManager.Resolve(this, paths);
}
