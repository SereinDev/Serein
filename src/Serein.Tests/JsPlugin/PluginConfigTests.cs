using System;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

using Jint.Runtime;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using Serein.Core.Models.Plugins.Info;
using Serein.Core.Models.Plugins.Js;
using Serein.Core.Services.Plugins.Js;
using Serein.Core.Utils;
using Serein.Core.Utils.Json;

using Xunit;

namespace Serein.Tests.JsPlugin;

[Collection(nameof(Serein))]
public sealed class PluginConfigTests : IDisposable
{
    private readonly IHost _host;
    private readonly JsPluginLoader _jsPluginLoader;

    public PluginConfigTests()
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
    public async Task ShouldLoadJsPluginWithoutSpecifyingAssemblies()
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
                JsonSerializerOptionsFactory.CamelCase
            )
        );

        var config = new JsPluginConfig
        {
            NetAssemblies = ["System.Text.Json"],
            AllowGetType = true,
            AllowStringCompilation = false,
            Strict = true,
        };

        File.WriteAllText(
            Path.Join(PathConstants.PluginsDirectory, "test", PathConstants.JsPluginConfigFileName),
            JsonSerializer.Serialize(config, JsonSerializerOptionsFactory.CamelCase)
        );
        File.WriteAllText(Path.Join(PathConstants.PluginsDirectory, "test", "index.js"), "");

        await _host.StartAsync();
        await Task.Delay(500);

        var kv = _jsPluginLoader.Plugins.First();

        Assert.Equal(kv.Value.Config.AllowGetType, config.AllowGetType);
        kv.Value.Engine.Evaluate("const a = new System.Guid(); a.GetType();");

        Assert.Equal(kv.Value.Config.NetAssemblies, config.NetAssemblies);
        kv.Value.Engine.Evaluate("System.Text.Json.JsonSerializer.Serialize('{}');");

        Assert.Equal(kv.Value.Config.Strict, config.Strict);
        Assert.Throws<JavaScriptException>(() => kv.Value.Engine.Evaluate("c = 1;"));
    }
}