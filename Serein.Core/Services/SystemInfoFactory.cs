using System.Diagnostics;
using System.Net.NetworkInformation;
using System.Threading.Tasks;
using System.Timers;

using SystemInfoLibrary.OperatingSystem;

namespace Serein.Core.Services;

// warning CA1416: 可在所有平台上访问此调用站点。"PerformanceCounter" 仅在 'windows' 上受支持。
#pragma warning disable CA1416

public class SystemInfoFactory
{
    private readonly Timer _timer;

#if WINDOWS
    private PerformanceCounter? _counter;
#endif

    public OperatingSystemInfo Info { get; private set; }

    public SystemInfoFactory()
    {
        Info = OperatingSystemInfo.GetOperatingSystemInfo();
        _timer = new(2500);
        _timer.Elapsed += UpdateInfo;
        _timer.Start();

#if WINDOWS
        // Initialize `_counter` asynchronously
        Task.Run(
            () => _counter = new("Processor", "% Processor Time", "_Total") { MachineName = "." }
        );
#endif
    }

    private void UpdateInfo(object? sender, ElapsedEventArgs e)
    {
        Info.Update();
        UpdateNetSpeed();
#if WINDOWS
        Task.Run(() => CPUUsage = _counter?.NextValue() ?? 0);
#endif
    }

    public double CPUUsage { get; private set; }

    public string? UploadSpeed { get; private set; }

    public string? DownloadSpeed { get; private set; }

    private decimal _bytesReceived,
        _bytesSent;

    private void UpdateNetSpeed()
    {
        decimal bytesReceived = 0,
            bytesSent = 0;
        foreach (NetworkInterface @interface in NetworkInterface.GetAllNetworkInterfaces())
        {
            if (@interface == null)
                continue;
            bytesReceived += @interface.GetIPStatistics().BytesReceived;
            bytesSent += @interface.GetIPStatistics().BytesSent;
        }
        if (_bytesReceived != 0 && _bytesSent != 0)
        {
            double uploadSpeed = (double)(bytesSent - _bytesSent) / 1024 / 2.5,
                downloadSpeed = (double)(bytesReceived - _bytesReceived) / 1024 / 2.5;

            UploadSpeed =
                uploadSpeed < 1024
                    ? uploadSpeed.ToString("N1") + "KB/s"
                    : uploadSpeed < 1024 * 1024
                        ? (uploadSpeed / 1024).ToString("N1") + "MB/s"
                        : (uploadSpeed / 1024 / 1024).ToString("N1") + "GB/s";
            DownloadSpeed =
                downloadSpeed < 1024
                    ? downloadSpeed.ToString("N1") + "KB/s"
                    : downloadSpeed < 1024 * 1024
                        ? (downloadSpeed / 1024).ToString("N1") + "MB/s"
                        : (downloadSpeed / 1024 / 1024).ToString("N1") + "GB/s";
        }
        _bytesReceived = bytesReceived;
        _bytesSent = bytesSent;
    }
}
