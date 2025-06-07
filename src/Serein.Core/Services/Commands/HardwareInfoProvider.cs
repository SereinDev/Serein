using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
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
    public HardwareInfo? Info
    {
        get
        {
            Update();
            return _info;
        }
    }

    private readonly object _lock;
    private readonly ILogger _logger;
    private bool _isLoading;
    private HardwareInfo? _info;
    private DateTime _dateTime;

    public HardwareInfoProvider(ILogger<HardwareInfoProvider> logger)
    {
        Task.Run(Update);
        _lock = new();
        _logger = logger;
    }

    /// <summary>
    /// 更新信息
    /// </summary>
    public void Update()
    {
        if (_isLoading || (DateTime.Now - _dateTime).TotalSeconds < 3)
        {
            return;
        }

        lock (_lock)
        {
            try
            {
                _isLoading = true;

                if (_info is null)
                {
                    _info = new();
                }
                else
                {
                    Try(_info.RefreshBatteryList);
                    Try(_info.RefreshBIOSList);
                    Try(_info.RefreshComputerSystemList);
                    Try(() => _info.RefreshCPUList());
                    Try(_info.RefreshDriveList);
                    Try(_info.RefreshKeyboardList);
                    Try(_info.RefreshMemoryList);
                    Try(_info.RefreshMemoryStatus);
                    Try(_info.RefreshMonitorList);
                    Try(_info.RefreshMotherboardList);
                    Try(_info.RefreshMouseList);
                    Try(() => _info.RefreshNetworkAdapterList());
                    Try(_info.RefreshOperatingSystem);
                    Try(_info.RefreshPrinterList);
                    Try(_info.RefreshSoundDeviceList);
                    Try(_info.RefreshVideoControllerList);
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
                _dateTime = DateTime.Now;
            }
        }
    }

    private void Try(
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
