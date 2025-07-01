using System.Threading.Tasks;
using EmbedIO;
using Jint;
using Jint.Native;
using Jint.Native.Function;
using Jint.Runtime;
using Microsoft.Extensions.DependencyInjection;
using Serein.Core.Models.Commands;
using Serein.Core.Services.Data;
using Serein.Core.Services.Plugins;
using Xunit;

namespace Serein.Tests.Services.JsPlugin;

public sealed partial class InteropTests
{
    [Fact]
    public void ShouldBeAbleToSetVariable()
    {
        var commandVariables = _host.Services.GetRequiredService<PluginManager>().CommandVariables;

        _kv.Value.Engine.Evaluate("serein.command.setVariable('test', 'value')");

        Assert.Equal("value", commandVariables["test"]);

        _kv.Value.Engine.Evaluate("serein.command.setVariable('test', 'newValue')");

        Assert.Equal("newValue", commandVariables["test"]);
    }

    [Fact]
    public void ShouldBeAbleToParseCommand()
    {
        var command = (Command?)
            _kv.Value.Engine.Evaluate("serein.command.parse('[g]command')").ToObject();

        Assert.NotNull(command);
        Assert.Equal(CommandOrigin.Plugin, command.Origin);
        Assert.Equal(CommandType.SendGroupMsg, command.Type);
    }

    [Theory]
    [InlineData("log")]
    [InlineData("info")]
    [InlineData("warn")]
    [InlineData("error")]
    public void ShouldBeAbleToOutput(string functionName)
    {
        _kv.Value.Engine.Evaluate($"serein.console.{functionName}('test')");
        _kv.Value.Engine.Evaluate($"console.{functionName}('test')");

        _kv.Value.Engine.Evaluate($"serein.console.{functionName}(1, 'test')");
        _kv.Value.Engine.Evaluate($"console.{functionName}(2, 'test')");
    }

    [Fact]
    public void ShouldBeAbleToManageServers()
    {
        _kv.Value.Execute("serein.servers.add('test', {  })");

        Assert.NotNull(_kv.Value.Engine.Evaluate("serein.servers['test']"));
        Assert.True(_kv.Value.Engine.Evaluate("serein.servers.remove('test')").AsBoolean());
    }

    [Fact]
    public void ShouldAccessToSettingProvider()
    {
        Assert.Equal(Types.Object, _kv.Value.Engine.Evaluate("serein.settings").Type);
        Assert.True(_kv.Value.Engine.Evaluate("serein.settings.save") is Function);
        Assert.Equal(0, _kv.Value.Engine.Evaluate("serein.settings.value.connection.adapter"));
    }

    [Fact]
    public void ShouldAccessToMatchProvider()
    {
        Assert.Equal(Types.Object, _kv.Value.Engine.Evaluate("serein.matches").Type);
        Assert.True(_kv.Value.Engine.Evaluate("serein.matches.save") is Function);

        _host.Services.GetRequiredService<MatchProvider>().Value.Add(new());
        Assert.Equal(1, _kv.Value.Engine.Evaluate("serein.matches.value.length"));

        _kv.Value.Engine.Execute("serein.matches.save()");
    }

    [Fact]
    public void ShouldAccessToScheduleProvider()
    {
        Assert.Equal(Types.Object, _kv.Value.Engine.Evaluate("serein.schedules").Type);
        Assert.True(_kv.Value.Engine.Evaluate("serein.schedules.save") is Function);

        _host.Services.GetRequiredService<ScheduleProvider>().Value.Add(new());
        Assert.Equal(1, _kv.Value.Engine.Evaluate("serein.schedules.value.length"));

        _kv.Value.Engine.Execute("serein.schedules.save()");
    }

    [Fact]
    public async Task ShouldAccessToWebServer()
    {
        Assert.Equal(
            (int)WebServerState.Stopped,
            _kv.Value.Engine.Evaluate("serein.webServer.state")
        );

        _kv.Value.Engine.Execute("serein.webServer.start()");
        await Task.Delay(1000);

        Assert.Equal(
            (int)WebServerState.Listening,
            _kv.Value.Engine.Evaluate("serein.webServer.state")
        );

        _kv.Value.Engine.Execute("serein.webServer.stop()");
        await Task.Delay(1000);

        Assert.Equal(
            (int)WebServerState.Stopped,
            _kv.Value.Engine.Evaluate("serein.webServer.state")
        );
    }

    [Fact]
    public void ShouldAccessToConnectionManager()
    {
        Assert.Equal(Types.Object, _kv.Value.Engine.Evaluate("serein.connection").Type);

        Assert.False(_kv.Value.Engine.Evaluate("serein.connection.isActive").AsBoolean());
        Assert.Equal(0, _kv.Value.Engine.Evaluate("serein.connection.sent"));
        Assert.Equal(0, _kv.Value.Engine.Evaluate("serein.connection.received"));
        Assert.Equal(JsValue.Null, _kv.Value.Engine.Evaluate("serein.connection.startedAt"));

        Assert.True(_kv.Value.Engine.Evaluate("serein.connection.start") is Function);
        Assert.True(_kv.Value.Engine.Evaluate("serein.connection.stop") is Function);
        Assert.True(_kv.Value.Engine.Evaluate("serein.connection.sendDataAsync") is Function);
        Assert.True(_kv.Value.Engine.Evaluate("serein.connection.sendMessageAsync") is Function);

        Assert.Throws<JavaScriptException>(
            () => _kv.Value.Engine.Evaluate("serein.connection.stop()")
        );
    }

    [Fact]
    public void ShouldAccessToPermissionProperty()
    {
        Assert.Equal(Types.Object, _kv.Value.Engine.Evaluate("serein.permissions").Type);
        Assert.True(_kv.Value.Engine.Evaluate("serein.permissions.register") is Function);
        Assert.True(_kv.Value.Engine.Evaluate("serein.permissions.unregister") is Function);

        _kv.Value.Engine.Execute("serein.permissions.register('node1')");
        _kv.Value.Engine.Execute("serein.permissions.register('node2', 'description')");
        Assert.Equal(JsValue.Null, _kv.Value.Engine.Evaluate("serein.permissions['test.node1']"));
        Assert.Equal("description", _kv.Value.Engine.Evaluate("serein.permissions['test.node2']"));

        _kv.Value.Engine.Execute("serein.permissions.unregister('node1')");
        _kv.Value.Engine.Execute("serein.permissions.unregister('node2')");

        Assert.Equal(
            JsValue.Undefined,
            _kv.Value.Engine.Evaluate("serein.permissions['test.node2']")
        );
        Assert.Equal(
            JsValue.Undefined,
            _kv.Value.Engine.Evaluate("serein.permissions['test.node2']")
        );

        Assert.Equal(1, _kv.Value.Engine.Evaluate("serein.permissions.groups.ids.length"));
        Assert.Equal("everyone", _kv.Value.Engine.Evaluate("serein.permissions.groups.ids[0]"));
    }

    [Fact]
    public void ShouldAccessToApp()
    {
        Assert.Equal(Types.Object, _kv.Value.Engine.Evaluate("serein.app").Type);
        Assert.NotEmpty(_kv.Value.Engine.Evaluate("serein.app.version.toString()").AsString());
        Assert.NotEmpty(_kv.Value.Engine.Evaluate("serein.app.fullVersion").AsString());
        Assert.NotEmpty(_kv.Value.Engine.Evaluate("serein.app.assemblyName").AsString());
    }

    [Fact]
    public void ShouldAccessToHardwareInfo()
    {
        _kv.Value.Engine.Evaluate("serein.hardwareInfo");
    }

    [Fact]
    public void ShouldAccessToPluginId()
    {
        Assert.Equal(_kv.Key, _kv.Value.Engine.Evaluate("serein.id"));
    }
}
