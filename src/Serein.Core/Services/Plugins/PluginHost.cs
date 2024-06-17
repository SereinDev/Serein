using System;
using System.Collections.Concurrent;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using Serein.Core.Models.Plugins;
using Serein.Core.Services.Plugins.Js;

namespace Serein.Core.Services.Plugins;

public class PluginHost(IHost host)
{
    private readonly IHost _host = host;
    private IServiceProvider Services => _host.Services;
    private JsManager JsManager => Services.GetRequiredService<JsManager>();
    private EventDispatcher EventDispatcher => Services.GetRequiredService<EventDispatcher>();
    public ConcurrentDictionary<string, string> Variables { get; } = new();
    public event EventHandler? PluginsReloading;

    public void SetVariable(string key, object? value)
    {
        var str = value?.ToString() ?? string.Empty;
        Variables.AddOrUpdate(key, str, (_, _) => str);
    }

    public void Load()
    {
        JsManager.Load();
    }

    public void Reload()
    {
        EventDispatcher.Dispatch(Event.PluginsUnloading);

        JsManager.Unload();

        PluginsReloading?.Invoke(null, EventArgs.Empty);
        Load();
    }
}
