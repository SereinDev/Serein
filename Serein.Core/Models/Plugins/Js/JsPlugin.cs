using System;
using System.Collections.Concurrent;
using System.Threading;

using Jint;
using Jint.Native.Function;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using Serein.Core.Models.Output;
using Serein.Core.Services.Plugins.Js;
using Serein.Core.Utils.Extensions;

namespace Serein.Core.Models.Plugins.Js;

public class JsPlugin : IPlugin
{
    public string FileName { get; }
    public PreloadConfig PreloadConfig { get; }
    public Engine Engine { get; }
    public ScriptInstance ScriptInstance { get; }
    public PluginInfo Info { get; private set; }

    public CancellationToken CancellationToken => _cancellationTokenSource.Token;
    public bool Loaded { get; internal set; }
    public ConcurrentDictionary<Event, FunctionInstance> EventHandlers;

    private readonly CancellationTokenSource _cancellationTokenSource;
    private readonly IHost _host;
    private IServiceProvider Services => _host.Services;
    private JsEngineFactory EngineFactory => Services.GetRequiredService<JsEngineFactory>();
    private IOutputHandler Logger => Services.GetRequiredService<IOutputHandler>();

    public JsPlugin(IHost host, string fileName, PreloadConfig preLoadConfig)
    {
        _cancellationTokenSource = new();
        _host = host;

        FileName = fileName;
        PreloadConfig = preLoadConfig;
        Info = new();
        EventHandlers = new();

        ScriptInstance = new(_host, this);
        Engine = EngineFactory.Create(this);
    }

    public void Execute(string text) => Engine.Execute(text);

    public void SetPluginInfo(PluginInfo? pluginInfo)
    {
        Info = pluginInfo ?? throw new ArgumentNullException(nameof(pluginInfo));
    }

    public void Dispose()
    {
        _cancellationTokenSource.Cancel();
        EventHandlers.Clear();
        Engine.Dispose();
        GC.SuppressFinalize(this);
    }

    public bool Invoke(Event @event, CancellationToken cancellationToken, params object[] args)
    {
        try
        {
            if (
                CancellationToken.IsCancellationRequested
                || !EventHandlers.TryGetValue(@event, out FunctionInstance? func)
                || func is null
            )
                return false;

            if (!Monitor.TryEnter(Engine, 1000))
                throw new TimeoutException("等待引擎超时");

            if (cancellationToken.IsCancellationRequested)
                return false;

            var result = Engine.Invoke(func, args);
            return result.IsBoolean() && result.AsBoolean();
        }
        catch (Exception e)
        {
            Logger.LogPluginError(FileName, $"触发事件{@event}时出现异常：\n{e.GetDetailString()}");
            return false;
        }
        finally
        {
            Monitor.Exit(Engine);
        }
    }
}
