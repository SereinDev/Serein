using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Jint;
using Jint.Native;
using Jint.Runtime;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serein.Core.Services.Plugins.Js;
using Serein.Core.Utils;
using Xunit;

namespace Serein.Tests.Services.JsPlugin;

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
        File.WriteAllText(Path.Join(PathConstants.PluginsDirectory, "114514.js"), "");
        _host.Start();
        Task.Delay(1000).Wait();
    }

    public void Dispose()
    {
        _host.StopAsync();
        _host.Dispose();
    }

    [Fact]
    public void ShouldAccessToBuiltInProperties()
    {
        var kv = _jsPluginLoader.Plugins.First();

        Assert.Equal(Types.Object, kv.Value.Engine.Evaluate("serein").Type);
        Assert.Equal(Types.Object, kv.Value.Engine.Evaluate("console").Type);
        Assert.Equal(Types.Object, kv.Value.Engine.Evaluate("localStorage").Type);
        Assert.Equal(Types.Object, kv.Value.Engine.Evaluate("sessionStorage").Type);
    }

    [Fact]
    public void ShouldAccessToClrType()
    {
        var kv = _jsPluginLoader.Plugins.First();

        Assert.NotEqual(JsValue.Undefined, kv.Value.Engine.Evaluate("System.Console"));
    }

    [Fact]
    public void ShouldAccessToClrMethod()
    {
        var kv = _jsPluginLoader.Plugins.First();

        Assert.Equal(
            Environment.ProcessId,
            kv.Value.Engine.Evaluate("System.Diagnostics.Process.GetCurrentProcess().Id")
        );

        kv.Value.Engine.Evaluate("System.IO.File.WriteAllText('test.txt', '')");
        Assert.True(File.Exists("test.txt"));
    }

    [Fact]
    public void ShouldAccessToBuiltInModuleProcess()
    {
        var kv = _jsPluginLoader.Plugins.First();

        Assert.Equal(Environment.ProcessId, kv.Value.Engine.Evaluate("process.pid"));
        Assert.Equal(Environment.ExitCode, kv.Value.Engine.Evaluate("process.exitCode"));
        Assert.Equal(Environment.Version.ToString(), kv.Value.Engine.Evaluate("process.version"));
        Assert.Equal(Environment.CurrentDirectory, kv.Value.Engine.Evaluate("process.cwd()"));
    }

    [Fact]
    public void ShouldAccessToBuiltInModuleFs()
    {
        var kv = _jsPluginLoader.Plugins.First();

        kv.Value.Engine.Evaluate("fs.writeFileSync('test.txt', 'test')");
        Assert.True(File.Exists("test.txt"));
        Assert.Equal(
            File.ReadAllText("test.txt"),
            kv.Value.Engine.Evaluate("fs.readFileSync('test.txt')")
        );
    }

    [Fact]
    public void ShouldBeAbleToOutput()
    {
        var kv = _jsPluginLoader.Plugins.First();
        kv.Value.Engine.Evaluate("serein.console.log('test')");
        kv.Value.Engine.Evaluate("console.log('test')");
    }
}
