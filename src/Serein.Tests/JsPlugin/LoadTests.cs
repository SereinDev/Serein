using System;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

using Jint.Native;
using Jint.Runtime;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using Serein.Core.Services.Plugins.Js;
using Serein.Core.Utils;
using Serein.Core.Utils.Json;

using Xunit;

namespace Serein.Tests.JsPlugin;

[Collection(nameof(Serein))]
public sealed class LoadTests : IDisposable
{
    private readonly IHost _host;
    private readonly JsPluginLoader _jsPluginLoader;

    public LoadTests()
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
    public async Task ShouldLoadSingleJsPlugin()
    {
        File.WriteAllText(Path.Join(PathConstants.PluginsDirectory, "1.js"), "");

        await _host.StartAsync();
        await Task.Delay(500);

        Assert.Single(_jsPluginLoader.Plugins);

        var kv = _jsPluginLoader.Plugins.First();
        Assert.Equal(kv.Key, kv.Value.Info.Id);
        Assert.Equal("1", kv.Value.Info.Name);
        Assert.Equal("1.js", Path.GetFileName(kv.Value.Engine.Evaluate("__filename").ToString()));
    }

    [Fact]
    public async Task ShouldSkipJsModuleFile()
    {
        File.WriteAllText(Path.Join(PathConstants.PluginsDirectory, "1.module.js"), "");

        await _host.StartAsync();
        await Task.Delay(500);

        Assert.Empty(_jsPluginLoader.Plugins);
    }
}