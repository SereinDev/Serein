using System;

using Jint;

using Microsoft.Extensions.Hosting;

using Serein.Core.Services.Plugins.Js;

namespace Serein.Core.Models.Plugins.Js;

public class JsPlugin : IDisposable
{
    public PreLoadConfig PreLoadConfig => _preLoadConfig;
    public Engine Engine => _engine;
    public string Namespace { get; }
    public PluginInfo PluginInfo => _pluginInfo ?? new();

    private PluginInfo? _pluginInfo;
    private readonly PreLoadConfig _preLoadConfig;
    private readonly Engine _engine;
    private readonly ScriptInstance _scriptInstance;
    private readonly IHost _host;

    public JsPlugin(IHost host, string @namespace, PreLoadConfig preLoadConfig)
    {
        _host = host;
        Namespace = @namespace;
        _preLoadConfig = preLoadConfig;

        _scriptInstance = new(_host, this);
        _engine = EngineFactory.Create(Namespace, _scriptInstance, preLoadConfig);
    }

    public void SetPluginInfo(PluginInfo pluginInfo) { }

    public void Dispose() { }
}
