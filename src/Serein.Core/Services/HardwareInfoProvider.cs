using System;
using System.Threading.Tasks;
using System.Timers;

using Hardware.Info;

namespace Serein.Core.Services;

public class HardwareInfoProvider
{
    public HardwareInfo? Info { get; private set; }
    public readonly Timer _timer;

    public HardwareInfoProvider()
    {
        Task.Run(() => Info = new());
        _timer = new(2000);
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
        catch (Exception) { }
    }
}
