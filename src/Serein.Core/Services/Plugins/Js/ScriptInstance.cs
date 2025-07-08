using System;
using System.Collections.Generic;
using System.IO;
using Hardware.Info;
using Jint;
using Jint.Native;
using Jint.Native.Function;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serein.Core.Models.Abstractions;
using Serein.Core.Models.Plugins;
using Serein.Core.Models.Plugins.Info;
using Serein.Core.Services.Bindings;
using Serein.Core.Services.Commands;
using Serein.Core.Services.Data;
using Serein.Core.Services.Network.Connection;
using Serein.Core.Services.Network.Web;
using Serein.Core.Services.Permissions;
using Serein.Core.Services.Plugins.Js.Properties;
using Console = Serein.Core.Services.Plugins.Js.Properties.Console;

namespace Serein.Core.Services.Plugins.Js;

public sealed partial class ScriptInstance
{
    private readonly IServiceProvider _serviceProvider;
    private readonly JsPlugin _jsPlugin;
    private readonly PluginManager _pluginManager;
    private readonly HardwareInfoProvider _hardwareInfoProvider;
    private readonly PluginLoggerBase _pluginLogger;

    public PermissionProperty Permissions { get; }
    public ServerProperty Servers { get; }
    public ConnectionManager Connection { get; }
    public CommandProperty Command { get; }
    public WebServer WebServer { get; }
    public Console Console => _jsPlugin.Console;
    public SettingProvider Settings { get; }
    public ScheduleProvider Schedules { get; }
    public MatchProvider Matches { get; }
    public BindingManager Bindings { get; }
    public SereinApp App { get; }
    public HardwareInfo? HardwareInfo => _hardwareInfoProvider.Info;
    public PluginInfo Info => _jsPlugin.Info;
    public string Path { get; } = Directory.GetCurrentDirectory();
    public string Id => _jsPlugin.Info.Id;

    internal ScriptInstance(IServiceProvider serviceProvider, JsPlugin jsPlugin)
    {
        _serviceProvider = serviceProvider;
        _jsPlugin = jsPlugin;
        _pluginLogger = _serviceProvider.GetRequiredService<PluginLoggerBase>();
        _pluginManager = _serviceProvider.GetRequiredService<PluginManager>();
        _hardwareInfoProvider = _serviceProvider.GetRequiredService<HardwareInfoProvider>();

        App = _serviceProvider.GetRequiredService<SereinApp>();
        WebServer = _serviceProvider.GetRequiredService<WebServer>();
        Settings = _serviceProvider.GetRequiredService<SettingProvider>();
        Schedules = _serviceProvider.GetRequiredService<ScheduleProvider>();
        Matches = _serviceProvider.GetRequiredService<MatchProvider>();
        Bindings = _serviceProvider.GetRequiredService<BindingManager>();
        Connection = _serviceProvider.GetRequiredService<ConnectionManager>();

        var propertyBuilder = _serviceProvider.GetRequiredService<PropertyFactory>();
        Command = propertyBuilder.CommandProperty;
        Servers = propertyBuilder.ServerProperty;

        Permissions = new(
            Id,
            _serviceProvider.GetRequiredService<PermissionManager>(),
            _serviceProvider.GetRequiredService<GroupManager>()
        );
    }

    public void Log(params JsValue[] jsValues)
    {
        var str = string.Join<JsValue>('\x20', jsValues);
        _pluginLogger.Log(LogLevel.Information, _jsPlugin.Info.Name, str);
    }

    public void Log(JsValue jsValue)
    {
        _pluginLogger.Log(LogLevel.Information, _jsPlugin.Info.Name, jsValue.ToString());
    }

    public bool Exports(string? name, JsValue jsValue)
    {
        if (string.IsNullOrEmpty(name))
        {
            return false;
        }

        try
        {
            if (jsValue.IsNull() || jsValue.IsUndefined())
            {
                return _pluginManager.ExportedVariables.Remove(name, out _);
            }

            _pluginManager.ExportedVariables[name] = jsValue.ToObject();
            return true;
        }
        catch
        {
            return false;
        }
    }

    public JsValue Imports(string? name)
    {
        return
            string.IsNullOrEmpty(name)
            || !_pluginManager.ExportedVariables.TryGetValue(name, out object? o)
            ? JsValue.Undefined
            : JsValue.FromObject(_jsPlugin.Engine, o);
    }

    public object GetService(string type)
    {
        var t = Type.GetType(type);
        return t?.IsPublic == true
            ? _serviceProvider.GetRequiredService(t)
            : throw new ArgumentException("无法获取指定的类型", nameof(type));
    }

    public void SetListener(string eventName, JsValue jsValue)
    {
        ArgumentException.ThrowIfNullOrEmpty(eventName);

        if (!Enum.TryParse<Event>(eventName, true, out var @event))
        {
            throw new ArgumentException("无效的事件名称", nameof(eventName));
        }

        _jsPlugin.SetListener(@event, jsValue as Function);
    }

    public string Resolve(params string[] paths) => PluginManager.Resolve(_jsPlugin, paths);
}
