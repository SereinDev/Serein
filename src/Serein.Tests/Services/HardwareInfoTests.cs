using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serein.Core.Services.Commands;
using Xunit;

namespace Serein.Tests.Services;

[Collection(nameof(Serein))]
public class HardwareInfoTests : IDisposable
{
    private readonly IHost _app;
    private readonly HardwareInfoProvider _hardwareInfoProvider;

    public HardwareInfoTests()
    {
        _app = HostFactory.BuildNew(true);
        _app.Start();

        _hardwareInfoProvider = _app.Services.GetRequiredService<HardwareInfoProvider>();
    }

    public void Dispose()
    {
        _app.StopAsync();
        _app.Dispose();
    }

    [Fact]
    public async Task ShouldNotBeNull()
    {
        await Task.Delay(2000);
        Assert.NotNull(_hardwareInfoProvider.Info);
    }

    [Fact]
    public async Task ShouldBeAbleToUpdate()
    {
        _hardwareInfoProvider.Update();
        await Task.Delay(2000);
        Assert.NotNull(_hardwareInfoProvider.Info);
    }
}
