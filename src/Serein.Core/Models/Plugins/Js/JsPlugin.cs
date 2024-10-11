using System;
using System.Collections.Concurrent;
using System.ComponentModel;
using System.Threading;

using Jint;
using Jint.Native.Function;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

using Serein.Core.Models.Output;
using Serein.Core.Models.Plugins.Info;
using Serein.Core.Services.Plugins.Js;
using Serein.Core.Utils.Extensions;

using JsConsole = Serein.Core.Services.Plugins.Js.Console;

namespace Serein.Core.Models.Plugins.Js;

public class JsPlugin : IPlugin
{
    public PluginInfo Info { get; }
    public string FileName { get; }
    public JsPluginConfig Config { get; }
    public Engine Engine { get; }
    public ScriptInstance ScriptInstance { get; }
    public JsConsole Console { get; }

    public CancellationToken CancellationToken => _cancellationTokenSource.Token;
    public ConcurrentDictionary<Event, Function> EventHandlers;
    public bool IsEnabled => !_cancellationTokenSource.IsCancellationRequested;

    private readonly CancellationTokenSource _cancellationTokenSource;
    private readonly IHost _host;

    public event PropertyChangedEventHandler? PropertyChanged;

    private IServiceProvider Services => _host.Services;
    private JsEngineFactory EngineFactory => Services.GetRequiredService<JsEngineFactory>();
    private IPluginLogger Logger => Services.GetRequiredService<IPluginLogger>();

    public JsPlugin(IHost host, PluginInfo pluginInfo, string fileName, JsPluginConfig config)
    {
        _cancellationTokenSource = new();
        _host = host;
        Info = pluginInfo;
        FileName = fileName;
        Config = config;
        EventHandlers = new();
        Console = new(Logger, Info.Name);
        ScriptInstance = new(_host, this);
        Engine = EngineFactory.Create(this);
    }

    public void Execute(string text) => Engine.Execute(text);

    public void Dispose()
    {
        Disable();
        EventHandlers.Clear();
        Engine.Dispose();
        GC.SuppressFinalize(this);
    }

    public bool Invoke(Event @event, CancellationToken cancellationToken, params object[] args)
    {
        var entered = false;
        try
        {
            if (
                CancellationToken.IsCancellationRequested
                || !EventHandlers.TryGetValue(@event, out Function? func)
                || func is null
            )
                return false;

            if (!Monitor.TryEnter(Engine, 1000))
                throw new TimeoutException("等待引擎超时");

            entered = true;

            if (cancellationToken.IsCancellationRequested)
                return false;

            var result = Engine.Invoke(func, args);
            return result.IsBoolean() && result.AsBoolean();
        }
        catch (Exception e)
        {
            Logger.Log(LogLevel.Error, Info.Name, $"触发事件{@event}时出现异常：\n{e.GetDetailString()}");
            return false;
        }
        finally
        {
            if (entered)
                Monitor.Exit(Engine);
        }
    }

    public void Disable()
    {
        if (!_cancellationTokenSource.IsCancellationRequested)
            _cancellationTokenSource.Cancel();

        PropertyChanged?.Invoke(this, new(nameof(IsEnabled)));
    }
}
