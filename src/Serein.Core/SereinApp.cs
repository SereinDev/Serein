using System;
using System.IO;
using System.Reflection;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

using Serein.Core.Services;
using Serein.Core.Services.Data;
using Serein.Core.Services.Network;
using Serein.Core.Utils;
using Serein.Core.Utils.Json;

namespace Serein.Core;

public sealed class SereinApp : IHost
{
    public static readonly string Version =
        Assembly.GetCallingAssembly().GetName().Version?.ToString() ?? "¿¿¿";
    public static readonly string? FullVersion = Assembly
        .GetCallingAssembly()
        .GetCustomAttribute<AssemblyInformationalVersionAttribute>()
        ?.InformationalVersion;
    public static readonly AppType Type;
    public static readonly bool StartForTheFirstTime = !File.Exists(PathConstants.SettingFile);

    public static SereinApp? Current { get; private set; }

    static SereinApp()
    {
        Type = Assembly.GetEntryAssembly()?.GetName().Name switch
        {
            "Serein.Cli" => AppType.Cli,
            "Serein.Lite" => AppType.Lite,
            "Serein.Plus" => AppType.Plus,
            _ => AppType.Unknown,
        };
    }

    private readonly CancellationTokenSource _cancellationTokenSource = new();
    private SettingProvider SettingProvider => Services.GetRequiredService<SettingProvider>();
    private ScheduleRunner ScheduleRunner => Services.GetRequiredService<ScheduleRunner>();
    private ILogger Logger => Services.GetRequiredService<ILogger>();
    private UpdateChecker UpdateChecker => Services.GetRequiredService<UpdateChecker>();

    public event EventHandler? AppStarted;
    public CancellationToken CancellationToken => _cancellationTokenSource.Token;

    private readonly IHost _host;

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

    public IServiceProvider Services => _host.Services;

    public void Dispose()
    {
        _cancellationTokenSource.Dispose();
        GC.SuppressFinalize(this);
    }

    public Task StartAsync(CancellationToken cancellationToken = default)
    {
        AppStarted?.Invoke(this, EventArgs.Empty);
        ScheduleRunner.Start();
        UpdateChecker.Start();
        Logger.LogInformation("Serein启动成功");

        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken = default)
    {
        _cancellationTokenSource.Cancel();
        return Task.CompletedTask;
    }
}
