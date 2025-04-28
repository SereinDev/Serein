using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Timers;
using Hardware.Info;
using Microsoft.Extensions.Logging;

namespace Serein.Core.Services.Commands;

/// <summary>
/// 硬件信息提供者
/// </summary>
public sealed class HardwareInfoProvider
{
    /// <summary>
    /// 硬件信息
    /// </summary>
    public HardwareInfo? Info { get; private set; }

    private readonly object _lock;
    private readonly ILogger _logger;
    private bool _isLoading;

    public HardwareInfoProvider(
        ILogger<HardwareInfoProvider> logger,
        CancellationTokenProvider cancellationTokenProvider
    )
    {
        Task.Run(Update);
        _lock = new();
        _logger = logger;

        var timer = new Timer(5000);
        timer.Elapsed += (_, _) => Update();
        timer.Start();

        cancellationTokenProvider.Token.Register(() =>
        {
            timer.Stop();
            timer.Dispose();
        });
    }

    /// <summary>
    /// 更新信息
    /// </summary>
    public void Update()
    {
        if (_isLoading)
        {
            return;
        }

        lock (_lock)
        {
            try
            {
                _isLoading = true;

                if (Info is null)
                {
                    Info = new();
                }
                else
                {
                    Try(Info.RefreshBatteryList);
                    Try(Info.RefreshBIOSList);
                    Try(Info.RefreshComputerSystemList);
                    Try(() => Info.RefreshCPUList());
                    Try(Info.RefreshDriveList);
                    Try(Info.RefreshKeyboardList);
                    Try(Info.RefreshMemoryList);
                    Try(Info.RefreshMemoryStatus);
                    Try(Info.RefreshMonitorList);
                    Try(Info.RefreshMotherboardList);
                    Try(Info.RefreshMouseList);
                    Try(() => Info.RefreshNetworkAdapterList());
                    Try(Info.RefreshOperatingSystem);
                    Try(Info.RefreshPrinterList);
                    Try(Info.RefreshSoundDeviceList);
                    Try(Info.RefreshVideoControllerList);
                }
            }
            catch (Exception e)
            {
                _logger.LogError("初始化失败：{}", e.Message);
                _logger.LogDebug(e, "初始化失败");
            }
            finally
            {
                _isLoading = false;
            }
        }

        void Try(
            Action action,
            [CallerArgumentExpression(nameof(action))] string? expression = null
        )
        {
            try
            {
                action();
            }
            catch (Exception e)
            {
                _logger.LogDebug(e, "更新信息失败：（{}）", expression);
            }
        }
    }
}
