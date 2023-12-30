using System;

using Jint.Native;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using Serein.Core.Models.Commands;
using Serein.Core.Services;
using Serein.Core.Services.Server;
using Serein.Core.Utils.Extensions;

namespace Serein.Core.Models.Plugins.Js;

public partial class ScriptInstance
{
    private readonly IHost _host;
    private readonly JsPlugin _jsPlugin;

    private IServiceProvider Services => _host.Services;
    private IOutputHandler Logger => Services.GetRequiredService<IOutputHandler>();
    private CommandRunner CommandRunner => Services.GetRequiredService<CommandRunner>();
    private CommandParser CommandParser => Services.GetRequiredService<CommandParser>();

    public ServerModule Server { get; }

    public ScriptInstance(IHost host, JsPlugin jsPlugin)
    {
        _host = host;
        _jsPlugin = jsPlugin;
        Server = new(Services.GetRequiredService<ServerManager>());
    }

    public string Version => App.Version;
    public string Namespace => _jsPlugin.Namespace;

    public void RunCommand(string? command)
    {
        CommandRunner.RunAsync(CommandParser.Parse(CommandOrigin.Plugin, command)).Await();
    }

    public Command ParseCommand(string? command)
    {
        return CommandParser.Parse(CommandOrigin.Plugin, command);
    }

    public void Log(params JsValue[] jsValues)
    {
        var str = string.Join<JsValue>('\x20', jsValues);
        Logger.LogPluginInfomation(_jsPlugin.Namespace, str);
    }
}
