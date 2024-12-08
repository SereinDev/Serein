using System;
using System.IO;
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
public sealed class InjectionTest : IDisposable
{
    private readonly IHost _host;
    private readonly NetPluginLoader _netPluginLoader;

    public InjectionTest()
    {
        _host = HostFactory.BuildNew();
        _netPluginLoader = _host.Services.GetRequiredService<NetPluginLoader>();

        Directory.CreateDirectory(PathConstants.PluginsDirectory);
        Directory.CreateDirectory(Path.Join(PathConstants.PluginsDirectory, "test"));
    }

    public void Dispose()
    {
        _host.StopAsync();
        _host.Dispose();
    }

    [Fact]
    public async Task ShouldSupportInjectionWithIServiceProvider()
    {
        CSharpCompilationHelper.Compile(
            """
            using System;
            using System.Threading.Tasks;
            using Serein.Core.Models.Plugins.Net;

            namespace MyPlugin;

            public class Plugin(IServiceProvider serviceProvider): PluginBase
            {
                public override void Dispose() { }
            }
            """,
            nameof(ShouldSupportInjectionWithIServiceProvider),
            Path.Join(
                PathConstants.PluginsDirectory,
                "test",
                nameof(ShouldSupportInjectionWithIServiceProvider) + ".dll"
            )
        );

        File.WriteAllText(
            Path.Join(PathConstants.PluginsDirectory, "test", PathConstants.PluginInfoFileName),
            JsonSerializer.Serialize(
                new PluginInfo
                {
                    Id = nameof(ShouldSupportInjectionWithIServiceProvider),
                    Name = "test",
                    Type = PluginType.Net,
                    EntryFile = nameof(ShouldSupportInjectionWithIServiceProvider) + ".dll",
                },
                JsonSerializerOptionsFactory.Common
            )
        );
        await _host.StartAsync();

        Assert.Single(_netPluginLoader.Plugins);
    }

    [Fact]
    public async Task ShouldSupportInjectionWithOneType()
    {
        CSharpCompilationHelper.Compile(
            $$"""
            using System;
            using System.Threading.Tasks;
            using Serein.Core.Models.Plugins.Net;
            using Serein.Core.Services.Servers;

            namespace MyPlugin;

            public class Plugin(ServerManager serverManager): PluginBase
            {
                public override void Dispose() { }
            }
            """,
            nameof(ShouldSupportInjectionWithOneType),
            Path.Join(
                PathConstants.PluginsDirectory,
                "test",
                $"{nameof(ShouldSupportInjectionWithOneType)}.dll"
            )
        );

        File.WriteAllText(
            Path.Join(PathConstants.PluginsDirectory, "test", PathConstants.PluginInfoFileName),
            JsonSerializer.Serialize(
                new PluginInfo
                {
                    Id = nameof(ShouldSupportInjectionWithOneType),
                    Name = "test",
                    Type = PluginType.Net,
                    EntryFile = $"{nameof(ShouldSupportInjectionWithOneType)}.dll",
                },
                JsonSerializerOptionsFactory.Common
            )
        );
        await _host.StartAsync();

        Assert.Single(_netPluginLoader.Plugins);
    }

    [Fact]
    public async Task ShouldSupportInjectionWithMultipleTypes()
    {
        CSharpCompilationHelper.Compile(
            $$"""
            using System;
            using System.Threading.Tasks;
            using Serein.Core.Models.Plugins.Net;
            using Serein.Core.Services.Commands;
            using Serein.Core.Services.Servers;

            namespace MyPlugin;

            public class Plugin(ServerManager serverManager, Matcher matcher): PluginBase
            {
                public override void Dispose() { }
            }
            """,
            nameof(ShouldSupportInjectionWithMultipleTypes),
            Path.Join(
                PathConstants.PluginsDirectory,
                "test",
                $"{nameof(ShouldSupportInjectionWithMultipleTypes)}.dll"
            )
        );

        File.WriteAllText(
            Path.Join(PathConstants.PluginsDirectory, "test", PathConstants.PluginInfoFileName),
            JsonSerializer.Serialize(
                new PluginInfo
                {
                    Id = nameof(ShouldSupportInjectionWithMultipleTypes),
                    Name = "test",
                    Type = PluginType.Net,
                    EntryFile = $"{nameof(ShouldSupportInjectionWithMultipleTypes)}.dll",
                },
                JsonSerializerOptionsFactory.Common
            )
        );
        await _host.StartAsync();

        Assert.Single(_netPluginLoader.Plugins);
    }
}
