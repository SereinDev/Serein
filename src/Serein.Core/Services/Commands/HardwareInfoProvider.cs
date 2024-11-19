using System;
using System.Threading.Tasks;
using System.Timers;

using Hardware.Info;

using Microsoft.Extensions.Logging;

namespace Serein.Core.Services.Commands;

public sealed class HardwareInfoProvider
{
    public HardwareInfo? Info { get; private set; }
    private readonly object _lock;
    private readonly Timer _timer;
    private readonly ILogger _logger;
    private bool _isLoading;

    public HardwareInfoProvider(ILogger<HardwareInfoProvider> logger)
    {
        Task.Run(Update);
        _lock = new();
        _logger = logger;
        _timer = new(5000);
        _timer.Elapsed += (_, _) => Update();
        _timer.Start();
    }

    public void Update()
    {
        if (_isLoading)
        {
            return;
        }

        lock (_lock)
            try
            {
                _isLoading = true;

                if (Info is null)
                {
                    Info = new();
                }
                else
                {
                    Info.RefreshAll();
                }
            }
            catch (Exception e)
            {
                _logger.LogError("更新信息失败：{}", e.Message);
                _logger.LogDebug(e, "更新信息失败");
            }
            finally
            {
                _isLoading = false;
            }
    }
}
