using System;
using System.Reflection;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

using Serein.Core.Models.Output;
using Serein.Core.Services;
using Serein.Core.Services.Data;
using Serein.Core.Utils.Json;

namespace Serein.Core;

public sealed class SereinApp : IHost
{
    public static readonly string Version =
        Assembly.GetCallingAssembly().GetName().Version?.ToString() ?? "?";

    private readonly CancellationTokenSource _cancellationTokenSource = new();
    private SettingProvider SettingProvider => Services.GetRequiredService<SettingProvider>();
    private ScheduleRunner ScheduleRunner => Services.GetRequiredService<ScheduleRunner>();
    private IOutputHandler Logger => Services.GetRequiredService<IOutputHandler>();

    public Action? OnStarted { get; set; }
    public CancellationToken CancellationToken => _cancellationTokenSource.Token;

    private readonly IHost _host;

    public SereinApp(IHost host)
    {
        _host = host;

        Logger.LogInformation("Serein {}", Version);
        Logger.LogInformation("仓库: https://github.com/Zaitonn/Serein");
        Logger.LogInformation("Copyright (C) 2022 Zaitonn. All rights reserved.");

        Logger.LogDebug(
            "设置：{}",
            JsonSerializer.Serialize(
                SettingProvider.Value,
                options: new(JsonSerializerOptionsFactory.CamelCase) { WriteIndented = true }
            )
        );
    }

    public IServiceProvider Services => _host.Services;

    public void Dispose()
    {
        _cancellationTokenSource.Dispose();
        GC.SuppressFinalize(this);
    }

    public Task StartAsync(CancellationToken cancellationToken = default)
    {
        OnStarted?.Invoke();
        ScheduleRunner.Start();
        Logger.LogInformation("Serein启动成功");

        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken = default)
    {
        _cancellationTokenSource.Cancel();
        return Task.CompletedTask;
    }
}
