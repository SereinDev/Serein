using System;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

using Serein.Core.Services;

using Serein.Core.Services.Data;
using Serein.Core.Utils;
using Serein.Core.Utils.Json;

namespace Serein.Core;

public sealed partial class SereinApp : IHost
{
    public DateTime StartTime { get; }

    public SereinApp(IHost host)
    {
        Current = this;
        StartTime = DateTime.Now;
        _host = host;
        _logger = Services.GetRequiredService<ILogger>();
        _settingProvider = Services.GetRequiredService<SettingProvider>();
        Services.GetRequiredService<SentryReporter>().Init();

        _logger.LogInformation("Serein.{} {}", Type, FullVersion);
        _logger.LogInformation("仓库: {}", UrlConstants.Repository);
        _logger.LogInformation("Copyright (C) 2022 Zaitonn. All rights reserved.");

        _logger.LogDebug("[{}] 版本：{}", nameof(SereinApp), FullVersion);
        _logger.LogDebug("[{}] 程序集：{}", nameof(SereinApp), typeof(SereinApp).Assembly.GetName());
        _logger.LogDebug("[{}] 首次启动：{}", nameof(SereinApp), StartForTheFirstTime);
        _logger.LogDebug("[{}] 启动时间：{}", nameof(SereinApp), StartTime);
        _logger.LogDebug(
            "设置：{}",
            JsonSerializer.Serialize(
                _settingProvider.Value,
                options: new(JsonSerializerOptionsFactory.CamelCase) { WriteIndented = true }
            )
        );
    }

    private readonly IHost _host;
    private readonly ILogger _logger;
    private readonly SettingProvider _settingProvider;
    public IServiceProvider Services => _host.Services;

    public void Dispose()
    {
        if (Current == this)
            Current = default;

        GC.SuppressFinalize(this);
    }

    public async Task StartAsync(CancellationToken cancellationToken = default)
    {
        await _host.StartAsync(cancellationToken);

        _logger.LogDebug("[{}] 初始化用时：{} ms", nameof(SereinApp), (DateTime.Now - StartTime).TotalMilliseconds);
        _logger.LogInformation("Serein启动成功！");
    }

    public Task StopAsync(CancellationToken cancellationToken = default)
    {
        _logger.LogDebug("[{}] 正在停止", nameof(SereinApp));
        return _host.StopAsync(cancellationToken);
    }
}
