using System;
using System.Collections.Generic;
using System.Threading;
using Jint.Native;
using Jint.Native.Function;
using Microsoft.Extensions.Logging;
using Serein.Core.Models.Abstractions;
using Serein.Core.Utils.Extensions;
using Timer = System.Timers.Timer;

namespace Serein.Core.Services.Plugins.Js;

public sealed class TimerFactory
{
    private long _timerId;
    private readonly Dictionary<long, Timer> _timers;
    private readonly IPluginLogger _pluginLogger;
    private readonly CancellationToken _cancellationToken;
    private readonly string _name;

    internal TimerFactory(
        string name,
        IPluginLogger pluginLogger,
        CancellationToken cancellationToken
    )
    {
        _timers = [];
        _pluginLogger = pluginLogger;
        _cancellationToken = cancellationToken;
        _name = name;

        _cancellationToken.Register(() =>
        {
            foreach (var timer in _timers.Values)
            {
                timer.Stop();
                timer.Dispose();
            }

            _timers.Clear();
        });
    }

    public long SetTimeout(JsValue jsValue, long milliseconds, params JsValue[] args)
    {
        return Add(jsValue, milliseconds, false, args);
    }

    public void ClearTimeout(long id)
    {
        Clear(id);
    }

    public long SetInterval(JsValue jsValue, long milliseconds, params JsValue[] args)
    {
        return Add(jsValue, milliseconds, true, args);
    }

    public void ClearInterval(long id)
    {
        Clear(id);
    }

    private long Add(JsValue jsValue, long milliseconds, bool autoReset, params JsValue[] args)
    {
        _cancellationToken.ThrowIfCancellationRequested();

        if (jsValue is not Function function)
        {
            throw new ArgumentException("The first argument must be a function.", nameof(jsValue));
        }

        var timer = new Timer(milliseconds) { AutoReset = autoReset };
        var id = _timerId++;
        timer.Start();
        timer.Elapsed += (_, _) => SafeCall(function, args);

        _timers.Add(id, timer);

        return id;
    }

    private void Clear(long id)
    {
        if (_timers.TryGetValue(id, out var timer))
        {
            timer.Stop();
            timer.Dispose();
            _timers.Remove(id);
        }
    }

    private void SafeCall(Function function, params JsValue[] args)
    {
        var entered = false;

        try
        {
            if (Monitor.TryEnter(function.Engine, 1000))
            {
                entered = true;
                function.Engine.Call(function, args);
            }
            else
            {
                throw new TimeoutException();
            }
        }
        catch (Exception e)
        {
            _pluginLogger.Log(
                LogLevel.Error,
                _name,
                $"An error occurred while calling the function: {e.GetDetailString()}"
            );
        }
        finally
        {
            if (entered)
            {
                Monitor.Exit(function.Engine);
            }
        }
    }
}
