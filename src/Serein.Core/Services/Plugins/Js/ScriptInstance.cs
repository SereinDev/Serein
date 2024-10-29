using System;
using System.Collections.Generic;

using Jint;
using Jint.Native;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using Serein.Core.Models.Output;
using Serein.Core.Models.Plugins.Js;
using Serein.Core.Models.Settings;
using Serein.Core.Services.Data;
using Serein.Core.Services.Commands;
using Serein.Core.Services.Servers;
using Serein.Core.Services.Network.Connection;
using Serein.Core.Services.Permissions;
using Serein.Core.Services.Plugins.Js.Properties;

using Console = Serein.Core.Services.Plugins.Js.Properties.Console;

namespace Serein.Core.Services.Plugins.Js;

public partial class ScriptInstance
{
    private readonly IServiceProvider _serviceProvider;
    private readonly JsPlugin _jsPlugin;
    private readonly SettingProvider _settingProvider;
    private readonly PluginManager _pluginManager;
    private readonly IPluginLogger _pluginLogger;

    public PermissionProperty Permissions { get; }
    public ServerProperty Servers { get; }
    public ConnectionProperty Connection { get; }
    public MetadataProperty Metadata { get; }
    public CommandProperty Command { get; }
    public Console Console => _jsPlugin.Console;
    public Setting Setting => _settingProvider.Value;
    public string Id => _jsPlugin.Info.Id;

    internal ScriptInstance(IServiceProvider serviceProvider, JsPlugin jsPlugin)
    {
        _serviceProvider = serviceProvider;
        _jsPlugin = jsPlugin;
        _settingProvider = _serviceProvider.GetRequiredService<SettingProvider>();
        _pluginManager = _serviceProvider.GetRequiredService<PluginManager>();
        _pluginLogger = _serviceProvider.GetRequiredService<IPluginLogger>();

        Metadata = new();
        Command = new(_pluginManager, _serviceProvider.GetRequiredService<CommandRunner>());
        Servers = new(_serviceProvider.GetRequiredService<ServerManager>());
        Connection = new(_serviceProvider.GetRequiredService<WsConnectionManager>());
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

    public bool Exports(string? name, JsValue jsValue)
    {
        if (string.IsNullOrEmpty(name))
            return false;

        try
        {
            if (jsValue.IsNull() || jsValue.IsUndefined())
                return _pluginManager.ExportedVariables.Remove(name, out _);

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
            : throw new InvalidOperationException("无法获取指定的类型");
    }

    public string Resolve(params string[] paths) => PluginManager.Resolve(_jsPlugin, paths);
}
