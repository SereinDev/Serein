using System;
using System.Linq;
using Jint;
using Microsoft.Extensions.DependencyInjection;
using Serein.Core.Models.Commands;
using Serein.Core.Services.Plugins;
using Xunit;

namespace Serein.Tests.Services.JsPlugin;

public sealed partial class InteropTests : IDisposable
{
    [Fact]
    public void ShouldBeAbleToSetVariable()
    {
        var kv = _jsPluginLoader.Plugins.First();
        var commandVariables = _host.Services.GetRequiredService<PluginManager>().CommandVariables;

        kv.Value.Engine.Evaluate("serein.command.setVariable('test', 'value')");

        Assert.Equal("value", commandVariables["test"]);

        kv.Value.Engine.Evaluate("serein.command.setVariable('test', 'newValue')");

        Assert.Equal("newValue", commandVariables["test"]);
    }

    [Fact]
    public void ShouldBeAbleToParseCommand()
    {
        var kv = _jsPluginLoader.Plugins.First();

        var command = (Command?)
            kv.Value.Engine.Evaluate("serein.command.parse('[g]command')").ToObject();

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
        var kv = _jsPluginLoader.Plugins.First();

        kv.Value.Engine.Evaluate($"serein.console.{functionName}('test')");
        kv.Value.Engine.Evaluate($"console.{functionName}('test')");

        kv.Value.Engine.Evaluate($"serein.console.{functionName}(1, 'test')");
        kv.Value.Engine.Evaluate($"console.{functionName}(2, 'test')");
    }

    [Fact]
    public void ShouldBeAbleToManageServers()
    {
        var kv = _jsPluginLoader.Plugins.First();

        kv.Value.Execute("serein.servers.add('test', {  })");

        Assert.NotNull(kv.Value.Engine.Evaluate("serein.servers['test']"));
        Assert.True(kv.Value.Engine.Evaluate("serein.servers.remove('test')").AsBoolean());
    }
}
