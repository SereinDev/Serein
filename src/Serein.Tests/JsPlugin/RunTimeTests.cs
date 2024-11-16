using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

using Jint.Native;
using Jint.Runtime;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using Serein.Core.Services.Plugins.Js;
using Serein.Core.Utils;

using Xunit;

namespace Serein.Tests.JsPlugin;

[Collection(nameof(Serein))]
public sealed class RunTimeTests : IDisposable
{
    private readonly IHost _host;
    private readonly JsPluginLoader _jsPluginLoader;

    public RunTimeTests()
    {
        _host = HostFactory.BuildNew();
        _jsPluginLoader = _host.Services.GetRequiredService<JsPluginLoader>();

        Directory.CreateDirectory(PathConstants.PluginsDirectory);
    }

    public void Dispose()
    {
        _host.StopAsync();
        _host.Dispose();
    }

    [Fact]
    public async Task ShouldAccessToBuiltInProperties()
    {
        File.WriteAllText(Path.Join(PathConstants.PluginsDirectory, "2.js"), "");

        await _host.StartAsync();
        await Task.Delay(500);

        var kv = _jsPluginLoader.Plugins.First();

        Assert.Equal(Types.Object, kv.Value.Engine.Evaluate("serein").Type);
        Assert.Equal(Types.Object, kv.Value.Engine.Evaluate("console").Type);
        Assert.Equal(Types.Object, kv.Value.Engine.Evaluate("localStorage").Type);
        Assert.Equal(Types.Object, kv.Value.Engine.Evaluate("sessionStorage").Type);
    }

    [Fact]
    public async Task ShouldAccessToClrType()
    {
        File.WriteAllText(Path.Join(PathConstants.PluginsDirectory, "3.js"), "");

        await _host.StartAsync();
        await Task.Delay(500);

        var kv = _jsPluginLoader.Plugins.First();

        Assert.NotEqual(JsValue.Undefined, kv.Value.Engine.Evaluate("System.Console"));
    }

    [Fact]
    public async Task ShouldAccessToClrMethod()
    {
        File.WriteAllText(Path.Join(PathConstants.PluginsDirectory, "4.js"), "");

        await _host.StartAsync();
        await Task.Delay(500);

        var kv = _jsPluginLoader.Plugins.First();

        Assert.Equal(
            Environment.ProcessId,
            kv.Value.Engine.Evaluate("System.Diagnostics.Process.GetCurrentProcess().Id")
        );

        kv.Value.Engine.Evaluate("System.IO.File.WriteAllText('test.txt', '')");
        Assert.True(File.Exists("test.txt"));
    }
}
