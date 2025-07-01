using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serein.Core.Models.Server;
using Serein.Core.Services.Servers;
using Xunit;

namespace Serein.Tests.Services.Server;

[Collection(nameof(Serein))]
public sealed class ProcessTests : IDisposable
{
    private readonly IHost _app;
    private readonly ServerManager _serverManager;

    public ProcessTests()
    {
        _app = HostFactory.BuildNew();
        _serverManager = _app.Services.GetRequiredService<ServerManager>();
    }

    public void Dispose()
    {
        foreach (var (_, server) in _serverManager.Servers)
        {
            if (server.Status)
            {
                server.Terminate();
            }
        }

        _app.StopAsync();
    }

    [Fact]
    public async Task ShouldRestartServer()
    {
        _app.Start();

        var server = _serverManager.Add(
            "test",
            new()
            {
                FileName = Environment.OSVersion.Platform == PlatformID.Win32NT ? "cmd" : "sh",
                AutoRestart = true,
            }
        );
        server.Start();
        server.Input("exit 1");

        await Task.Delay(1000);

        Assert.False(server.Status);
        Assert.Equal(RestartStatus.Preparing, server.RestartStatus);

        server.Stop();
        await Task.Delay(1000);

        Assert.Equal(RestartStatus.None, server.RestartStatus);

        // ----------------------------------------------

        server.Start();
        server.Input("exit 1");

        await Task.Delay(10000);

        Assert.Equal(RestartStatus.None, server.RestartStatus);
        Assert.True(server.Status);
    }

    [Fact]
    public void ShouldGetPid()
    {
        _app.Start();

        var server = _serverManager.Add(
            "test",
            new()
            {
                FileName = Environment.OSVersion.Platform == PlatformID.Win32NT ? "cmd" : "sh",
                AutoRestart = true,
            }
        );

        Assert.Null(server.Pid);
        server.Start();

        Assert.NotNull(server.Pid);
    }

    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public async Task ShouldStartServer(bool usePty)
    {
        _app.Start();

        var server = _serverManager.Add(
            "test",
            new()
            {
                FileName = Environment.OSVersion.Platform == PlatformID.Win32NT ? "cmd" : "sh",
                Pty = { IsEnabled = usePty },
            }
        );

        Assert.Null(server.Pid);
        server.Start();

        await Task.Delay(1000);
        Assert.NotNull(server.Pid);

        server.Input("echo \"1\"");
        server.Input("exit");

        await Task.Delay(1000);
        Assert.Null(server.Pid);
        Assert.False(server.Status);
    }
}
