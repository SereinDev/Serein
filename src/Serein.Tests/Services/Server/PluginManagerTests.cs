using System;
using System.IO;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serein.Core.Models.Server;
using Serein.Core.Services.Servers;
using Xunit;

namespace Serein.Tests.Services.Server;

[Collection(nameof(Serein))]
public class PluginManagerTests : IDisposable
{
    private readonly IHost _app;
    private readonly ServerManager _serverManager;

    public PluginManagerTests()
    {
        _app = HostFactory.BuildNew();
        _serverManager = _app.Services.GetRequiredService<ServerManager>();
    }

    public void Dispose()
    {
        _app.StopAsync();
    }

    [Fact]
    public void ShouldBeEmptyIfPluginDirectoryDoesNotExist()
    {
        var server = _serverManager.Add("test", new());
        Assert.True(string.IsNullOrEmpty(server.PluginManager.CurrentPluginsDirectory));
        Assert.Empty(server.PluginManager.Plugins);
    }

    [Fact]
    public void ShouldGetFullInfoIfPluginDirectoryExists()
    {
        Directory.CreateDirectory("plugins");

        File.WriteAllText("start.bat", "");
        File.WriteAllText("plugins/1.dll", "");
        File.WriteAllText("plugins/2.js.disabled", "");

        var server = _serverManager.Add("12345", new() { FileName = "start.bat" });

        Assert.False(string.IsNullOrEmpty(server.PluginManager.CurrentPluginsDirectory));
        Assert.NotEmpty(server.PluginManager.Plugins);

        var plugin = server.PluginManager.Plugins[0];
        Assert.True(plugin.IsEnabled);
        Assert.Equal(0, plugin.FileInfo.Length);
        Assert.Equal("1.dll", plugin.FriendlyName);
        Assert.Equal(PluginType.Library, plugin.Type);

        plugin = server.PluginManager.Plugins[1];
        Assert.False(plugin.IsEnabled);
        Assert.Equal(0, plugin.FileInfo.Length);
        Assert.Equal("2.js", plugin.FriendlyName);
        Assert.Equal(PluginType.JavaScript, plugin.Type);
    }
}
