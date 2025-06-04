using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text.Json.Serialization;
using System.Threading;
using Jint;
using Jint.Native.Function;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serein.Core.Models.Abstractions;
using Serein.Core.Models.Plugins;
using Serein.Core.Models.Plugins.Info;
using Serein.Core.Models.Plugins.Js;
using Serein.Core.Utils.Extensions;
using JsConsole = Serein.Core.Services.Plugins.Js.Properties.Console;

namespace Serein.Core.Services.Plugins.Js;

public class JsPlugin : IPlugin
{
    public string FileName { get; }
    public PluginInfo Info { get; }
    public JsPluginConfig Config { get; }

    [JsonIgnore]
    public Engine Engine { get; }

    [JsonIgnore]
    public JsConsole Console { get; }

    [JsonIgnore]
    public TimerFactory TimerFactory { get; }

    [JsonIgnore]
    public ScriptInstance ScriptInstance { get; }

    [JsonIgnore]
    public CancellationToken CancellationToken => _cancellationTokenSource.Token;

    internal IReadOnlyDictionary<Event, Function> EventHandlers => _eventHandlers;
    public bool IsEnabled => !_cancellationTokenSource.IsCancellationRequested;

    private readonly CancellationTokenSource _cancellationTokenSource;
    private readonly IPluginLogger _pluginLogger;
    private readonly ConcurrentDictionary<Event, Function> _eventHandlers;

    public event PropertyChangedEventHandler? PropertyChanged;

    internal JsPlugin(
        IServiceProvider serviceProvider,
        PluginInfo pluginInfo,
        string fileName,
        JsPluginConfig config
    )
    {
        _cancellationTokenSource = new();
        _pluginLogger = serviceProvider.GetRequiredService<IPluginLogger>();

        _eventHandlers = new();
        Info = pluginInfo;
        FileName = fileName;
        Config = config;
        TimerFactory = new(Info.Name, _pluginLogger, _cancellationTokenSource.Token);
        Console = new(_pluginLogger, Info.Name);
        ScriptInstance = new(serviceProvider, this);
        Engine = serviceProvider.GetRequiredService<JsEngineFactory>().Create(this);
    }

    public void Execute(string text) => Engine.Execute(text);

    public void Dispose()
    {
        Disable();

        _eventHandlers.Clear();
        Engine.Dispose();
        _cancellationTokenSource.Dispose();
        GC.SuppressFinalize(this);
    }

    internal void SetListener(Event @event, Function? func)
    {
        lock (_eventHandlers)
        {
            if (func is null)
            {
                _eventHandlers.TryRemove(@event, out _);
            }
            else
            {
                _eventHandlers[@event] = func;
            }
        }
    }

    internal bool Invoke(Event @event, CancellationToken cancellationToken, params object[] args)
    {
        var entered = false;
        try
        {
            if (
                CancellationToken.IsCancellationRequested
                || !EventHandlers.TryGetValue(@event, out Function? func)
                || func is null
            )
            {
                return true;
            }

            if (!Monitor.TryEnter(Engine, 1000))
            {
                throw new TimeoutException("等待引擎超时");
            }

            entered = true;

            if (cancellationToken.IsCancellationRequested)
            {
                return true;
            }

            var result = Engine.Invoke(func, args);
            {
                return !result.IsBoolean() || result.AsBoolean();
            }
        }
        catch (Exception e)
        {
            _pluginLogger.Log(
                LogLevel.Error,
                Info.Name,
                $"触发事件{@event}时出现异常：\n{e.GetDetailString()}"
            );
            return false;
        }
        finally
        {
            if (entered)
            {
                Monitor.Exit(Engine);
            }
        }
    }

    public void Disable()
    {
        if (!_cancellationTokenSource.IsCancellationRequested)
        {
            _cancellationTokenSource.Cancel();
        }

        PropertyChanged?.Invoke(this, new(nameof(IsEnabled)));
    }
}
