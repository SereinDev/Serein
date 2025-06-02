using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Serein.Core.Utils;
using Xunit;

namespace Serein.Tests.Services;

[Collection(nameof(Serein))]
public class FileLoggerTests : IDisposable
{
    private readonly IHost _app;

    public FileLoggerTests()
    {
        _app = HostFactory.BuildNew(true);
        _app.Start();
    }

    public void Dispose()
    {
        _app.StopAsync();
        _app.Dispose();
    }

    [Fact]
    public async Task ShouldLogToFile()
    {
        await Task.Delay(5000);
        var path = Path.Combine(PathConstants.LogDirectory, "app");
        Assert.True(Directory.Exists(path));
        Assert.NotEmpty(Directory.GetFiles(path));
    }
}
