using System;
using System.Linq;
using System.Threading.Tasks;
using Jint.Runtime;
using Microsoft.Extensions.DependencyInjection;
using Serein.Core.Services.Servers;
using Xunit;

namespace Serein.Tests.Services.JsPlugin;

public sealed partial class InteropTests
{
    [Theory]
    [InlineData("serein.setListener('ServerStarting', () => false);")]
    [InlineData("serein.setListener('ServerStarting', () => { return false; });")]
    [InlineData("serein.setListener('ServerStarting', async () => { return false; });")]
    [InlineData(
        "serein.setListener('ServerStarting', async () => { await System.Threading.Tasks.Task.Delay(100); return false; });"
    )]
    public async Task ShouldBeAbleToSetListener(string code)
    {
        _kv.Value.Engine.Execute(code);

        var server = _host.Services.GetRequiredService<ServerManager>().Servers.First().Value;
        server.Configuration.FileName = "test.js";
        server.Start();

        await Task.Delay(1000);

        Assert.False(server.Status);
    }

    [Fact]
    public void ShouldThrowWithInvalidListenerName()
    {
        Assert.Throws<JavaScriptException>(
            () => _kv.Value.Engine.Execute("serein.setListener('InvalidEvent', () => {});")
        );
    }
}
