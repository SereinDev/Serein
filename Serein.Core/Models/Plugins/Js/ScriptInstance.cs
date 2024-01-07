using System;
using System.Threading;

using Jint.Native;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using Serein.Core.Models.Commands;
using Serein.Core.Models.Settings;
using Serein.Core.Services;
using Serein.Core.Services.Data;
using Serein.Core.Services.Networks;
using Serein.Core.Services.Server;
using Serein.Core.Utils.Extensions;

namespace Serein.Core.Models.Plugins.Js;

public partial class ScriptInstance
{
    private readonly IHost _host;
    private readonly JsPlugin _jsPlugin;

    private IServiceProvider Services => _host.Services;
    private IOutputHandler Logger => Services.GetRequiredService<IOutputHandler>();
    private SettingProvider SettingProvider => Services.GetRequiredService<SettingProvider>();
    private CommandRunner CommandRunner => Services.GetRequiredService<CommandRunner>();
    private readonly CancellationToken _token;

    public ServerModule Server { get; }
    public WsModule Ws { get; }

    public ScriptInstance(IHost host, JsPlugin jsPlugin)
    {
        _host = host;
        _jsPlugin = jsPlugin;
        _token = jsPlugin.CancellationToken;

        Server = new(Services.GetRequiredService<ServerManager>(), _token);
        Ws = new(Services.GetRequiredService<WsNetwork>(), _token);
    }

    public static string Version => SereinApp.Version;
    public string Namespace => _jsPlugin.FileName;
    public Setting Setting => SettingProvider.Value;

    public void RunCommand(string? command)
    {
        _token.ThrowIfCancellationRequested();
        CommandRunner.RunAsync(CommandParser.Parse(CommandOrigin.Plugin, command)).Await();
    }

    public Command ParseCommand(string? command)
    {
        _token.ThrowIfCancellationRequested();
        return CommandParser.Parse(CommandOrigin.Plugin, command);
    }

    public void Log(params JsValue[] jsValues)
    {
        _token.ThrowIfCancellationRequested();

        var str = string.Join<JsValue>('\x20', jsValues);
        Logger.LogPluginInfomation(_jsPlugin.FileName, str);
    }
}
