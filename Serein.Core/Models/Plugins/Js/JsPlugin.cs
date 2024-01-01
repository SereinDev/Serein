using System;
using System.Threading;

using Jint;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using Serein.Core.Services.Plugins.Js;

namespace Serein.Core.Models.Plugins.Js;

public class JsPlugin : IDisposable
{
    public string Namespace { get; }
    public PreloadConfig PreloadConfig { get; }
    public Engine Engine { get; }
    public ScriptInstance ScriptInstance { get; }
    public PluginInfo PluginInfo { get; private set; }

    public CancellationToken CancellationToken => _cancellationTokenSource.Token;
    public bool Loaded { get; internal set; }

    private readonly CancellationTokenSource _cancellationTokenSource;
    private readonly IHost _host;
    private IServiceProvider Services => _host.Services;
    private EngineFactory EngineFactory => Services.GetRequiredService<EngineFactory>();

    public JsPlugin(IHost host, string @namespace, PreloadConfig preLoadConfig)
    {
        _cancellationTokenSource = new();
        _host = host;

        Namespace = @namespace;
        PreloadConfig = preLoadConfig;
        PluginInfo = new();

        ScriptInstance = new(_host, this);
        Engine = EngineFactory.Create(this);
    }

    public void Execute(string text) => Engine.Execute(text);

    public void SetPluginInfo(PluginInfo? pluginInfo)
    {
        PluginInfo = pluginInfo ?? throw new ArgumentNullException(nameof(pluginInfo));
    }

    public void Dispose()
    {
        _cancellationTokenSource.Cancel();
        Engine.Dispose();
        GC.SuppressFinalize(this);
    }
}
