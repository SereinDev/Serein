using System;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

using Serein.Core.Services.Data;
using Serein.Core.Utils.Json;

namespace Serein.Core;

public sealed partial class SereinApp : IHost
{
    public SereinApp(IHost host)
    {
        Current = this;
        _host = host;

        Logger.LogInformation("Serein.{} {}", Type, FullVersion);
        Logger.LogInformation("仓库: https://github.com/SereinDev/Serein");
        Logger.LogInformation("Copyright (C) 2022 Zaitonn. All rights reserved.");

        Logger.LogDebug(
            "设置：{}",
            JsonSerializer.Serialize(
                SettingProvider.Value,
                options: new(JsonSerializerOptionsFactory.CamelCase) { WriteIndented = true }
            )
        );
    }

    private readonly IHost _host;
    private SettingProvider SettingProvider => Services.GetRequiredService<SettingProvider>();
    private ILogger Logger => Services.GetRequiredService<ILogger>();
    public IServiceProvider Services => _host.Services;

    public void Dispose()
    {
        GC.SuppressFinalize(this);
    }

    public async Task StartAsync(CancellationToken cancellationToken = default)
    {
        await _host.StartAsync(cancellationToken);
        Logger.LogInformation("Serein启动成功");
    }

    public Task StopAsync(CancellationToken cancellationToken = default)
    {
        return _host.StopAsync(cancellationToken);
    }
}
