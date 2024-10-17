using System;
using System.Collections.Generic;

using Jint;
using Jint.Native;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

using Serein.Core.Models.Commands;
using Serein.Core.Models.Output;
using Serein.Core.Models.Plugins.Js;
using Serein.Core.Models.Settings;
using Serein.Core.Services.Commands;
using Serein.Core.Services.Data;
using Serein.Core.Services.Network.Connection;
using Serein.Core.Services.Permissions;
using Serein.Core.Services.Plugins.Js.Modules;
using Serein.Core.Services.Servers;
using Serein.Core.Utils.Extensions;

namespace Serein.Core.Services.Plugins.Js;

public partial class ScriptInstance
{
    private readonly IHost _host;
    private readonly JsPlugin _jsPlugin;

    private readonly SettingProvider _settingProvider;
    private readonly CommandRunner _commandRunner;
    private readonly PluginManager _pluginManager;
    private readonly IPluginLogger _pluginLogger;

    public PermissionModule Permissions { get; }
    public ServerModule Servers { get; }
    public WsModule Ws { get; }
    public Console Console => _jsPlugin.Console;
    public Setting Setting => _settingProvider.Value;
    public static string Version => SereinApp.Version;
    public static string? FullVersion => SereinApp.FullVersion;
    public static AppType Type => SereinApp.Type;
    public string Id => _jsPlugin.Info.Id;
    public IServiceProvider Services => _host.Services;

    public ScriptInstance(IHost host, JsPlugin jsPlugin)
    {
        _host = host;
        _jsPlugin = jsPlugin;

        _settingProvider = Services.GetRequiredService<SettingProvider>();
        _commandRunner = Services.GetRequiredService<CommandRunner>();
        _pluginManager = Services.GetRequiredService<PluginManager>();
        _pluginLogger = Services.GetRequiredService<IPluginLogger>();

        Servers = new(Services.GetRequiredService<ServerManager>());
        Ws = new(Services.GetRequiredService<WsConnectionManager>());
        Permissions = new(
            Id,
            Services.GetRequiredService<PermissionManager>(),
            Services.GetRequiredService<GroupManager>()
        );

    }

    public void RunCommand(string? command)
    {
        _commandRunner.RunAsync(CommandParser.Parse(CommandOrigin.Plugin, command)).Await();
    }

#pragma warning disable CA1822
    public Command ParseCommand(string? command)
    {
        return CommandParser.Parse(CommandOrigin.Plugin, command);
    }
#pragma warning restore CA1822

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

    public string Resolve(params string[] paths) => PluginManager.Resolve(_jsPlugin, paths);
}
