using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serein.Core.Models.Network.Connection;
using Serein.Core.Services.Data;
using Serein.Core.Services.Network.Connection;
using Xunit;

namespace Serein.Tests.Services.Connection;

[Collection(nameof(Serein))]
public class SatoriAdapterTests : IDisposable
{
    private readonly IHost _app;
    private readonly SettingProvider _settingProvider;
    private readonly ConnectionManager _connectionManager;

    public SatoriAdapterTests()
    {
        _app = HostFactory.BuildNew();
        _settingProvider = _app.Services.GetRequiredService<SettingProvider>();
        _connectionManager = _app.Services.GetRequiredService<ConnectionManager>();
        _app.Start();

        _settingProvider.Value.Connection.Adapter = AdapterType.Satori;
        _settingProvider.Value.Connection.OneBot.Uri = "http://127.0.0.1:8080";
    }

    public void Dispose()
    {
        _app.StopAsync();
        _app.Dispose();
    }

    [Fact]
    public async Task ShouldThrowWhenInvokeSendData()
    {
        _connectionManager.Start();

        await Assert.ThrowsAsync<NotSupportedException>(
            async () => await _connectionManager.SendDataAsync("12345")
        );
    }

    [Fact]
    public void ShouldStartSatoriAdapter()
    {
        _connectionManager.Start();
        Assert.True(_connectionManager.IsActive);
    }
}
