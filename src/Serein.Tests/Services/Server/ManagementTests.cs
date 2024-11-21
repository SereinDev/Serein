using System;
using System.IO;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using Serein.Core.Services.Servers;
using Serein.Core.Utils;

using Xunit;

namespace Serein.Tests.Services.Server;

[Collection(nameof(Serein))]
public sealed class ManagementTests : IDisposable
{
    private readonly IHost _app;
    private readonly ServerManager _serverManager;

    public ManagementTests()
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

    [Theory]
    [InlineData("")]
    [InlineData("1")]
    [InlineData("a")]
    [InlineData("abc;")]
    [InlineData("abc-")]
    [InlineData("abc-1")]
    [InlineData("con")]
    [InlineData("nul")]
    [InlineData("aux")]
    [InlineData("prn")]
    [InlineData("com0")]
    public void ShouldNotAddServerWithInvalidId(string id)
    {
        _app.Start();
        Assert.Throws<ArgumentException>(() => _serverManager.Add(id, new()));
    }

    [Fact]
    public void ShouldAddServer()
    {
        _app.Start();
        _serverManager.Add("test", new());

        Assert.True(_serverManager.Servers.ContainsKey("test"));
        Assert.True(File.Exists(string.Format(PathConstants.ServerConfigFile, "test")));
    }

    [Fact]
    public void ShouldNotRemoveRunningServer()
    {
        _app.Start();
        var server = _serverManager.Add("test", new() { FileName = "cmd" });
        server.Start();

        Assert.Throws<InvalidOperationException>(() => _serverManager.Remove("test"));
    }

    [Fact]
    public void ShouldRemoveServer()
    {
        _app.Start();
        _serverManager.Add("test", new());
        _serverManager.Remove("test");

        Assert.False(_serverManager.Servers.ContainsKey("test"));
        Assert.False(File.Exists(string.Format(PathConstants.ServerConfigFile, "test")));
    }
}