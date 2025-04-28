using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serein.Core.Models.Plugins;
using Serein.Core.Models.Plugins.Info;
using Serein.Core.Services.Plugins.Net;
using Serein.Core.Services.Servers;
using Serein.Core.Utils;
using Serein.Core.Utils.Json;
using Serein.Tests.Utils;
using Xunit;

namespace Serein.Tests.Services.NetPlugin;

[Collection(nameof(Serein))]
public sealed class EventTests : IDisposable
{
    private readonly IHost _host;
    private readonly NetPluginLoader _netPluginLoader;

    public EventTests()
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
    public async Task ShouldInvokePluginLoadedEvent()
    {
        CSharpCompilationHelper.Compile(
            """
            using System;
            using System.Threading.Tasks;
            using Serein.Core.Services.Plugins.Net;

            namespace MyPlugin;

            public class Plugin: PluginBase
            {
                public bool IsInvoked { get; private set; }
                public override void Dispose() { }

                protected override Task OnPluginsLoaded()
                {
                    IsInvoked = true;
                    return Task.CompletedTask;
                }
            }
            """,
            nameof(ShouldInvokePluginLoadedEvent),
            Path.Join(
                PathConstants.PluginsDirectory,
                "test",
                nameof(ShouldInvokePluginLoadedEvent) + ".dll"
            )
        );

        File.WriteAllText(
            Path.Join(PathConstants.PluginsDirectory, "test", PathConstants.PluginInfoFileName),
            JsonSerializer.Serialize(
                new PluginInfo
                {
                    Id = nameof(Event.PluginsLoaded),
                    Name = "test",
                    Type = PluginType.Net,
                    EntryFile = nameof(ShouldInvokePluginLoadedEvent) + ".dll",
                },
                JsonSerializerOptionsFactory.Common
            )
        );
        await _host.StartAsync();

        await Task.Delay(1000);

        dynamic d = _netPluginLoader.Plugins.First().Value;
        Assert.True(d.IsInvoked);
    }

    [Fact]
    public async Task ShouldInvokeServerEvent()
    {
        CSharpCompilationHelper.Compile(
            """
            using System;
            using System.Collections.Generic;
            using System.Threading.Tasks;
            using Serein.Core.Services.Plugins.Net;
            using Serein.Core.Services.Servers;

            namespace MyPlugin;

            public class Plugin: PluginBase
            {
                public List<string> Events { get; } = [];
                public override void Dispose() { }

                protected override Task<bool> OnServerStarting(Server server)
                {
                    Events.Add(nameof(OnServerStarting));
                    return Task.FromResult(true);
                }

                protected override Task OnServerStarted(Server server)
                {
                    Events.Add(nameof(OnServerStarted));
                    return Task.CompletedTask;
                }

                protected override Task<bool> OnServerStopping(Server server)
                {
                    Events.Add(nameof(OnServerStopping));
                    return Task.FromResult(true);
                }

                protected override Task OnServerExited(Server server, int exitcode, DateTime exitTime)
                {
                    Events.Add(nameof(OnServerExited));
                    return Task.CompletedTask;
                }

                protected override Task OnServerInput(Server server, string line)
                {
                    Events.Add(nameof(OnServerInput));
                    return Task.CompletedTask;
                }
            }
            """,
            nameof(ShouldInvokeServerEvent),
            Path.Join(
                PathConstants.PluginsDirectory,
                "test",
                nameof(ShouldInvokeServerEvent) + ".dll"
            )
        );

        File.WriteAllText(
            Path.Join(PathConstants.PluginsDirectory, "test", PathConstants.PluginInfoFileName),
            JsonSerializer.Serialize(
                new PluginInfo
                {
                    Id = nameof(Event.ServerStarting),
                    Name = "test",
                    Type = PluginType.Net,
                    EntryFile = nameof(ShouldInvokeServerEvent) + ".dll",
                },
                JsonSerializerOptionsFactory.Common
            )
        );
        await _host.StartAsync();

        var serverManager = _host.Services.GetRequiredService<ServerManager>();
        var server = serverManager.Add(
            "abcdefg",
            new() { FileName = "cmd", OutputEncoding = EncodingMap.EncodingType.GBK }
        );
        server.Start();
        server.Input("HELP");
        server.Stop();
        server.Terminate();
        await Task.Delay(1000);

        dynamic d = _netPluginLoader.Plugins.First().Value;
        var list = (List<string>)d.Events;
        Assert.Contains("OnServerStarting", list);
        Assert.Contains("OnServerStarted", list);
        Assert.Contains("OnServerStopping", list);
        Assert.Contains("OnServerExited", list);
        Assert.Contains("OnServerInput", list);
    }
}
