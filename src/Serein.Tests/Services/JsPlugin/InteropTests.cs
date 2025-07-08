using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Jint;
using Jint.Native;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serein.Core.Models.Plugins.Js;
using Serein.Core.Services.Data;
using Serein.Core.Services.Plugins.Js;
using Serein.Core.Utils;
using Xunit;

namespace Serein.Tests.Services.JsPlugin;

[Collection(nameof(Serein))]
public sealed partial class InteropTests : IDisposable
{
    private readonly IHost _host;
    private readonly JsPluginLoader _jsPluginLoader;

    private readonly KeyValuePair<string, Core.Services.Plugins.Js.JsPlugin> _kv;

    public InteropTests()
    {
        _host = HostFactory.BuildNew();
        _jsPluginLoader = _host.Services.GetRequiredService<JsPluginLoader>();

        _host
            .Services.GetRequiredService<SettingProvider>()
            .Value.Application.PluginEventMaxWaitingTime = 2000;

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

        _kv = _jsPluginLoader.Plugins.First();
    }

    public void Dispose()
    {
        _host.StopAsync();
        _host.Dispose();
    }

    [Fact]
    public void ShouldAccessToClrType()
    {
        Assert.NotEqual(JsValue.Undefined, _kv.Value.Engine.Evaluate("System.Console"));
    }

    [Fact]
    public void ShouldAccessToClrMethod()
    {
        Assert.Equal(
            Environment.ProcessId,
            _kv.Value.Engine.Evaluate("System.Diagnostics.Process.GetCurrentProcess().Id")
        );

        _kv.Value.Engine.Evaluate("System.IO.File.WriteAllText('test.txt', '')");
        Assert.True(File.Exists("test.txt"));
    }

    [Fact]
    public void CanResolveFile()
    {
        Assert.Equal(
            Path.GetFullPath(Path.Join(PathConstants.PluginsDirectory, "111.txt")),
            _kv.Value.Engine.Evaluate("serein.resolve('111.txt')")
        );
        Assert.Equal(
            Path.GetFullPath(Path.Join(PathConstants.PluginsDirectory, "a", "b", "111.txt")),
            _kv.Value.Engine.Evaluate("serein.resolve('a', 'b', '111.txt')")
        );
    }

    [Theory]
    [InlineData("localStorage")]
    [InlineData("sessionStorage")]
    public void CanUseStorage(string storageName)
    {
        _kv.Value.Engine.Execute($"{storageName}.setItem('test', 'value')");
        Assert.Equal(
            "value",
            _kv.Value.Engine.Evaluate($"{storageName}.getItem('test')").AsString()
        );
        Assert.Equal("value", _kv.Value.Engine.Evaluate($"{storageName}['test']").AsString());

        _kv.Value.Engine.Execute($"{storageName}.setItem('test', 'newValue')");
        Assert.Equal(
            "newValue",
            _kv.Value.Engine.Evaluate($"{storageName}.getItem('test')").AsString()
        );
        Assert.Equal("newValue", _kv.Value.Engine.Evaluate($"{storageName}['test']").AsString());

        _kv.Value.Engine.Execute($"{storageName}.removeItem('test')");
        Assert.Equal(JsValue.Null, _kv.Value.Engine.Evaluate($"{storageName}.getItem('test')"));
    }
}
