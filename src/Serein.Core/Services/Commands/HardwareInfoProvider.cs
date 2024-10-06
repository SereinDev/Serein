using System;
using System.Threading.Tasks;
using System.Timers;

using Hardware.Info;

using Microsoft.Extensions.Logging;

namespace Serein.Core.Services.Commands;

public class HardwareInfoProvider
{
    public HardwareInfo? Info { get; private set; }
    private readonly Timer _timer;
    private readonly ILogger _logger;

    public HardwareInfoProvider(ILogger<HardwareInfoProvider> logger)
    {
        Task.Run(() => Info = new());

        _logger = logger;
        _timer = new(5000);
        _timer.Elapsed += (_, _) => Update();
        _timer.Start();
    }

    public void Update()
    {
        if (Info is null)
            return;

        try
        {
            Info.RefreshAll();
        }
        catch (Exception e)
        {
            _logger.LogError("更新信息失败：{}", e.Message);
            _logger.LogDebug(e, "[{}] 更新信息失败", nameof(HardwareInfoProvider));
        }
    }
}
