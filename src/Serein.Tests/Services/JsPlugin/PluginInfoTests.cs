using System;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serein.Core.Models.Plugins.Info;
using Serein.Core.Services.Plugins.Js;
using Serein.Core.Utils;
using Serein.Core.Utils.Json;
using Xunit;

namespace Serein.Tests.Services.JsPlugin;

[Collection(nameof(Serein))]
public sealed class PluginInfoTests : IDisposable
{
    private readonly IHost _host;
    private readonly JsPluginLoader _jsPluginLoader;

    public PluginInfoTests()
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
    public async Task ShouldNotLoadJsPluginWithoutPluginInfo()
    {
        Directory.CreateDirectory(Path.Join(PathConstants.PluginsDirectory, "test"));
        File.WriteAllText(Path.Join(PathConstants.PluginsDirectory, "test", "1.js"), "");

        await _host.StartAsync();
        await Task.Delay(500);

        Assert.Empty(_jsPluginLoader.Plugins);
    }

    [Fact]
    public async Task ShouldNotLoadJsPluginWithInvalidPluginInfo()
    {
        Directory.CreateDirectory(Path.Join(PathConstants.PluginsDirectory, "test"));
        File.WriteAllText(
            Path.Join(PathConstants.PluginsDirectory, "test", PathConstants.PluginInfoFileName),
            "{}"
        );
        File.WriteAllText(Path.Join(PathConstants.PluginsDirectory, "test", "1.js"), "");

        await _host.StartAsync();
        await Task.Delay(500);

        Assert.Empty(_jsPluginLoader.Plugins);
    }

    [Fact]
    public async Task ShouldLoadJsPluginWithValidPluginInfo()
    {
        Directory.CreateDirectory(Path.Join(PathConstants.PluginsDirectory, "test"));
        File.WriteAllText(
            Path.Join(PathConstants.PluginsDirectory, "test", PathConstants.PluginInfoFileName),
            JsonSerializer.Serialize(
                new PluginInfo
                {
                    Id = "test1",
                    Name = "test",
                    EntryFile = "1.js",
                    Type = PluginType.Js,
                },
                JsonSerializerOptionsFactory.Common
            )
        );
        File.WriteAllText(Path.Join(PathConstants.PluginsDirectory, "test", "1.js"), "");

        await _host.StartAsync();
        await Task.Delay(1000);

        var kv = _jsPluginLoader.Plugins.First();

        Assert.Equal("test1", kv.Key);
        Assert.Equal("test", kv.Value.Info.Name);
        Assert.Equal("1.js", Path.GetFileName(kv.Value.Engine.Evaluate("__filename").ToString()));
        Assert.Equal("1.js", Path.GetFileName(kv.Value.FileName));
    }

    [Fact]
    public async Task ShouldLoadJsPluginWithoutSpecifyingEntryFile()
    {
        Directory.CreateDirectory(Path.Join(PathConstants.PluginsDirectory, "test"));
        File.WriteAllText(
            Path.Join(PathConstants.PluginsDirectory, "test", PathConstants.PluginInfoFileName),
            JsonSerializer.Serialize(
                new PluginInfo
                {
                    Id = "test1",
                    Name = "test",
                    Type = PluginType.Js,
                },
                JsonSerializerOptionsFactory.Common
            )
        );
        File.WriteAllText(Path.Join(PathConstants.PluginsDirectory, "test", "index.js"), "");

        await _host.StartAsync();
        await Task.Delay(500);

        Assert.Equal("test1", _jsPluginLoader.Plugins.First().Key);
    }
}
