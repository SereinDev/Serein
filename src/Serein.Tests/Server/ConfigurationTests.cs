using System;
using System.IO;
using System.Threading.Tasks;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using Serein.Core.Services.Servers;
using Serein.Core.Utils;

using Xunit;

namespace Serein.Tests.Server;

[Collection(nameof(Serein))]
public sealed class ConfigurationTests : IDisposable
{
    private readonly IHost _app;
    private readonly ServerManager _serverManager;

    public ConfigurationTests()
    {
        _app = HostFactory.BuildNew();
        _serverManager = _app.Services.GetRequiredService<ServerManager>();
    }

    public void Dispose()
    {
        foreach (var (_, server) in _serverManager.Servers)
            if (server.Status)
                server.Terminate();

        _app.StopAsync();
    }
    
    [Fact]
    public async Task ShouldStartServerWithStartWhenSettingUpTrue()
    {
        var server = _serverManager.Add("test", new()
        {
            FileName = "cmd",
            StartWhenSettingUp = true
        });

        await _app.StartAsync();
        await Task.Delay(500);

        Assert.True(server.Status);
    }

    [Fact]
    public async Task ShouldOutputToFile()
    {
        var server = _serverManager.Add("test", new()
        {
            FileName = "cmd",
            SaveLog = true
        });

        await _app.StartAsync();
        _app.Start();

        server.Start();
        await Task.Delay(1000);

        Assert.True(Directory.Exists(string.Format(PathConstants.ServerLogDirectory, server.Id)));
        Assert.NotEmpty(Directory.GetFiles(string.Format(PathConstants.ServerLogDirectory, server.Id)));
    }
}