using System;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serein.Core.Models.Plugins.Info;
using Serein.Core.Services.Plugins.Net;
using Serein.Core.Utils;
using Serein.Core.Utils.Json;
using Serein.Tests.Utils;
using Xunit;

namespace Serein.Tests.Services.NetPlugin;

[Collection(nameof(Serein))]
public sealed class LoadTests : IDisposable
{
    private readonly IHost _host;
    private readonly NetPluginLoader _netPluginLoader;

    public LoadTests()
    {
        _host = HostFactory.BuildNew();
        _netPluginLoader = _host.Services.GetRequiredService<NetPluginLoader>();

        Directory.CreateDirectory(Path.Join(PathConstants.PluginsDirectory, "test"));
    }

    public void Dispose()
    {
        _host.StopAsync();
        _host.Dispose();
    }

    [Fact]
    public async Task ShouldNotLoadNetPluginWithoutValidPluginClass()
    {
        CSharpCompilationHelper.Compile(
            """
            namespace EmptyPlugin;
            public class EmptyClass{}
            """,
            nameof(ShouldNotLoadNetPluginWithoutValidPluginClass),
            Path.Join(
                PathConstants.PluginsDirectory,
                "test",
                nameof(ShouldNotLoadNetPluginWithoutValidPluginClass) + ".dll"
            )
        );

        File.WriteAllText(
            Path.Join(PathConstants.PluginsDirectory, "test", PathConstants.PluginInfoFileName),
            JsonSerializer.Serialize(
                new PluginInfo
                {
                    Id = nameof(PluginBase),
                    Name = "test",
                    EntryFile = nameof(ShouldNotLoadNetPluginWithoutValidPluginClass) + ".dll",
                    Type = PluginType.Net,
                },
                JsonSerializerOptionsFactory.Common
            )
        );

        await _host.StartAsync();
        await Task.Delay(100);

        Assert.Empty(_netPluginLoader.Plugins);
    }

    [Fact]
    public async Task ShouldNotLoadNetPluginWithTwoPluginClasses()
    {
        CSharpCompilationHelper.Compile(
            """
            using Serein.Core.Models.Plugins.Net;

            namespace MyPlugin;

            public class Plugin1: PluginBase
            {
                public override void Dispose() { }
            }

            public class Plugin2: PluginBase
            {
                public override void Dispose() { }
            }
            """,
            nameof(ShouldNotLoadNetPluginWithTwoPluginClasses),
            Path.Join(
                PathConstants.PluginsDirectory,
                "test",
                nameof(ShouldNotLoadNetPluginWithTwoPluginClasses) + ".dll"
            )
        );

        File.WriteAllText(
            Path.Join(PathConstants.PluginsDirectory, "test", PathConstants.PluginInfoFileName),
            JsonSerializer.Serialize(
                new PluginInfo
                {
                    Id = nameof(PluginBase),
                    Name = "test",
                    EntryFile = nameof(ShouldNotLoadNetPluginWithTwoPluginClasses) + ".dll",
                    Type = PluginType.Net,
                },
                JsonSerializerOptionsFactory.Common
            )
        );

        await _host.StartAsync();
        await Task.Delay(100);

        Assert.Empty(_netPluginLoader.Plugins);
    }

    [Fact]
    public async Task ShouldLoadNetPluginWithValidAssembly()
    {
        CSharpCompilationHelper.Compile(
            """
            using Serein.Core.Models.Plugins.Net;

            namespace MyPlugin;
            public class Plugin: PluginBase
            {
                public override void Dispose() { }
            }
            """,
            nameof(ShouldLoadNetPluginWithValidAssembly),
            Path.Join(
                PathConstants.PluginsDirectory,
                "test",
                nameof(ShouldLoadNetPluginWithValidAssembly) + ".dll"
            )
        );

        File.WriteAllText(
            Path.Join(PathConstants.PluginsDirectory, "test", PathConstants.PluginInfoFileName),
            JsonSerializer.Serialize(
                new PluginInfo
                {
                    Id = nameof(PluginBase),
                    Name = "test",
                    EntryFile = nameof(ShouldLoadNetPluginWithValidAssembly) + ".dll",
                    Type = PluginType.Net,
                },
                JsonSerializerOptionsFactory.Common
            )
        );

        await _host.StartAsync();
        await Task.Delay(100);

        Assert.Single(_netPluginLoader.Plugins);
        Assert.Equal(nameof(PluginBase), _netPluginLoader.Plugins.First().Key);
    }

    [Fact]
    public async Task ShouldLoadNetPluginWithoutSpecifyingEntryFile()
    {
        CSharpCompilationHelper.Compile(
            """
            using System;
            using Serein.Core.Models.Plugins.Net;

            namespace MyPlugin;

            public class Plugin: PluginBase
            {
                public override void Dispose() { }
            }
            """,
            nameof(ShouldLoadNetPluginWithoutSpecifyingEntryFile),
            Path.Join(PathConstants.PluginsDirectory, "test", nameof(PluginBase) + ".dll")
        );

        File.WriteAllText(
            Path.Join(PathConstants.PluginsDirectory, "test", PathConstants.PluginInfoFileName),
            JsonSerializer.Serialize(
                new PluginInfo
                {
                    Id = nameof(PluginBase),
                    Name = "test",
                    Type = PluginType.Net,
                },
                JsonSerializerOptionsFactory.Common
            )
        );

        await _host.StartAsync();
        await Task.Delay(100);

        Assert.Single(_netPluginLoader.Plugins);
    }
}
