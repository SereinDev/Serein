using System;
using System.Collections.Concurrent;
using System.IO;
using System.Linq;
using Jint;
using Jint.Native;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serein.Core.Models.Plugins.Js;
using Serein.Core.Services.Plugins;
using Serein.Core.Services.Plugins.Js;
using Serein.Core.Utils;
using Xunit;

namespace Serein.Tests.Services.JsPlugin;

[Collection(nameof(Serein))]
public sealed partial class InteropTests : IDisposable
{
    private readonly IHost _host;
    private readonly JsPluginLoader _jsPluginLoader;
    private readonly EventDispatcher _eventDispatcher;

    public InteropTests()
    {
        _host = HostFactory.BuildNew();
        _eventDispatcher = _host.Services.GetRequiredService<EventDispatcher>();
        _jsPluginLoader = _host.Services.GetRequiredService<JsPluginLoader>();

        (
            (ConcurrentDictionary<string, Core.Services.Plugins.Js.JsPlugin>)_jsPluginLoader.Plugins
        ).TryAdd(
            "test",
            new(
                _host.Services,
                new() { Id = "test" },
                Path.Join(PathConstants.PluginsDirectory, "114514.js"),
                JsPluginConfig.Default
            )
        );
        _host.Start();
    }

    public void Dispose()
    {
        _host.StopAsync();
        _host.Dispose();
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
    public void ShouldBeAbleToResolveFile()
    {
        var kv = _jsPluginLoader.Plugins.First();
        Assert.Equal(
            Path.GetFullPath(Path.Join(PathConstants.PluginsDirectory, "111.txt")),
            kv.Value.Engine.Evaluate("serein.resolve('111.txt')")
        );
        Assert.Equal(
            Path.GetFullPath(Path.Join(PathConstants.PluginsDirectory, "a", "b", "111.txt")),
            kv.Value.Engine.Evaluate("serein.resolve('a', 'b', '111.txt')")
        );
    }

    [Theory]
    [InlineData("localStorage")]
    [InlineData("sessionStorage")]
    public void ShouldBeAbleToUseStorage(string storageName)
    {
        var kv = _jsPluginLoader.Plugins.First();

        kv.Value.Engine.Execute($"{storageName}.setItem('test', 'value')");
        Assert.Equal(
            "value",
            kv.Value.Engine.Evaluate($"{storageName}.getItem('test')").AsString()
        );
        Assert.Equal("value", kv.Value.Engine.Evaluate($"{storageName}['test']").AsString());

        kv.Value.Engine.Execute($"{storageName}.setItem('test', 'newValue')");
        Assert.Equal(
            "newValue",
            kv.Value.Engine.Evaluate($"{storageName}.getItem('test')").AsString()
        );
        Assert.Equal("newValue", kv.Value.Engine.Evaluate($"{storageName}['test']").AsString());

        kv.Value.Engine.Execute($"{storageName}.removeItem('test')");
        Assert.Equal(JsValue.Null, kv.Value.Engine.Evaluate($"{storageName}.getItem('test')"));
    }
}
