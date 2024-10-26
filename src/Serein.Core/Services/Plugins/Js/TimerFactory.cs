using System;
using System.Collections.Generic;
using System.Threading;

using Jint.Native;
using Jint.Native.Function;

using Timer = System.Timers.Timer;

namespace Serein.Core.Services.Plugins.Js;

public class TimerFactory
{
    private long _timeoutTimerId;
    private long _intervalTimerId;
    private readonly Dictionary<long, Timer> _timeoutTimers;
    private readonly Dictionary<long, Timer> _intervalTimers;

    public TimerFactory(CancellationToken cancellationToken)
    {
        _timeoutTimers = [];
        _intervalTimers = [];

        cancellationToken.Register(() =>
        {
            foreach (var timer in _timeoutTimers.Values)
            {
                timer.Stop();
                timer.Dispose();
            }

            foreach (var timer in _intervalTimers.Values)
            {
                timer.Stop();
                timer.Dispose();
            }

            _timeoutTimers.Clear();
            _intervalTimers.Clear();
        });
    }

    public long SetTimeout(JsValue function, long milliseconds, params JsValue[] args)
    {
        var timer = new Timer(milliseconds) { AutoReset = false };
        var id = _timeoutTimerId++;
        timer.Start();
        timer.Elapsed += (_, _) => SafeCall(function, args);

        _timeoutTimers.Add(id, timer);

        return id;
    }

    public void ClearTimeout(long id)
    {
        if (_timeoutTimers.TryGetValue(id, out var timer))
        {
            timer.Stop();
            timer.Dispose();
            _timeoutTimers.Remove(id);
        }
    }

    public long SetInterval(JsValue function, long milliseconds, params JsValue[] args)
    {
        var timer = new Timer(milliseconds) { AutoReset = true };
        var id = _intervalTimerId++;
        timer.Start();
        timer.Elapsed += (_, _) => SafeCall(function, args);

        _intervalTimers.Add(id, timer);

        return id;
    }

    public void ClearInterval(long id)
    {
        if (_intervalTimers.TryGetValue(id, out var timer))
        {
            timer.Stop();
            timer.Dispose();
            _intervalTimers.Remove(id);
        }
    }

    private static void SafeCall(JsValue jsValue, params JsValue[] args)
    {
        var entered = false;

        if (jsValue is not Function function)
            return;

        try
        {
            if (Monitor.TryEnter(function.Engine, 1000))
            {
                entered = true;
                function.Engine.Call(function, args);
            }
            else
                throw new TimeoutException();
        }
        finally
        {
            if (entered)
                Monitor.Exit(function.Engine);
        }
    }
}