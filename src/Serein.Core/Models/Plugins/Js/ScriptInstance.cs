using System;
using System.Collections.Generic;

using Jint;
using Jint.Native;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

using Serein.Core.Models.Commands;
using Serein.Core.Models.Output;
using Serein.Core.Models.Plugins.Js.Modules;
using Serein.Core.Models.Settings;
using Serein.Core.Services;
using Serein.Core.Services.Data;
using Serein.Core.Services.Networks;
using Serein.Core.Services.Plugins.Js;
using Serein.Core.Services.Servers;
using Serein.Core.Utils.Extensions;

namespace Serein.Core.Models.Plugins.Js;

public partial class ScriptInstance
{
    private readonly IHost _host;
    private readonly JsPlugin _jsPlugin;

    private IServiceProvider Services => _host.Services;
    private ISereinLogger Logger => Services.GetRequiredService<ISereinLogger>();
    private SettingProvider SettingProvider => Services.GetRequiredService<SettingProvider>();
    private CommandRunner CommandRunner => Services.GetRequiredService<CommandRunner>();
    private JsManager JsManager => Services.GetRequiredService<JsManager>();

    public ServerModule Server { get; }
    public WsModule Ws { get; }
    public Console Console => _jsPlugin.Console;
    public Setting Setting => SettingProvider.Value;
    public static string Version => SereinApp.Version;
    public static string? FullVersion => SereinApp.FullVersion;
    public static AppType Type => SereinApp.Type;
    public string Namespace => _jsPlugin.Name;

    public ScriptInstance(IHost host, JsPlugin jsPlugin)
    {
        _host = host;
        _jsPlugin = jsPlugin;

        Server = new(Services.GetRequiredService<ServerDictionary>());
        Ws = new(Services.GetRequiredService<WsNetwork>());
    }

    public void RunCommand(string? command)
    {
        CommandRunner.RunAsync(CommandParser.Parse(CommandOrigin.Plugin, command)).Await();
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
        Logger.LogPlugin(LogLevel.Information, _jsPlugin.Name, str);
    }

    public bool Exports(string? name, JsValue jsValue)
    {
        if (string.IsNullOrEmpty(name))
            return false;

        try
        {
            if (jsValue.IsNull() || jsValue.IsUndefined())
                return JsManager.ExportedVariables.Remove(name, out _);

            JsManager.ExportedVariables[name] = jsValue.ToObject();
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
            || !JsManager.ExportedVariables.TryGetValue(name, out object? o)
            ? JsValue.Undefined
            : JsValue.FromObject(_jsPlugin.Engine, o);
    }
}
