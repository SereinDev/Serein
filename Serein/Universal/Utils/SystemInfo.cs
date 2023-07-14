#if !UNIX
using System;
using System.Diagnostics;
#endif
using Serein.Utils.Output;
using System.Net.NetworkInformation;
using System.Threading.Tasks;
using System.Timers;
using SystemInfoLibrary.OperatingSystem;

namespace Serein.Utils
{
    internal static class SystemInfo
    {
        /// <summary>
        /// 初始化系统信息
        /// </summary>
        public static void Init()
        {
#if !UNIX
            if (Environment.OSVersion.Platform == PlatformID.Win32NT)
            {
                _counter = new("Processor", "% Processor Time", "_Total")
                {
                    MachineName = "."
                };
                Task.Run(() => _counter?.NextValue());
            }
#endif
            Info = OperatingSystemInfo.GetOperatingSystemInfo();
            OS = Info.Name;
            if (Info.Hardware.CPUs.Count > 0)
            {
                CPUName = Info.Hardware.CPUs[0].Name;
                CPUBrand = Info.Hardware.CPUs[0].Brand;
                CPUFrequency = Info.Hardware.CPUs[0].Frequency;
            }
            else
            {
                CPUName = "未知";
                CPUBrand = "未知";
            }
            _refreshTimer.Elapsed += (_, _) => Info.Update();
            _refreshTimer.Elapsed += (_, _) => UpdateNetSpeed();
            _refreshTimer.Start();
#if !UNIX
            _refreshTimer.Elapsed += (_, _) => CPUUsage = _counter?.NextValue() ?? 0;
#endif
            Logger.Output(Base.LogType.Debug, "Loaded.");
        }

        /// <summary>
        /// 上传速度
        /// </summary>
        public static string? UploadSpeed { get; private set; }

        /// <summary>
        /// 下载速度
        /// </summary>
        public static string? DownloadSpeed { get; private set; }

        private static decimal _bytesReceived, _bytesSent;

        /// <summary>
        /// 更新网速
        /// </summary>
        private static void UpdateNetSpeed()
        {
            decimal bytesReceived = 0, bytesSent = 0;
            foreach (NetworkInterface INet in NetworkInterface.GetAllNetworkInterfaces())
            {
                if (INet == null)
                {
                    continue;
                }
                bytesReceived += INet.GetIPStatistics().BytesReceived;
                bytesSent += INet.GetIPStatistics().BytesSent;
            }
            if (_bytesReceived != 0 && _bytesSent != 0)
            {
                double uploadSpeed = (double)(bytesSent - _bytesSent) / 1024 / 2.5, downloadSpeed = (double)(bytesReceived - _bytesReceived) / 1024 / 2.5;
                if (uploadSpeed < 1024)
                {
                    UploadSpeed = uploadSpeed.ToString("N1") + "KB/s";
                }
                else if (uploadSpeed < 1024 * 1024)
                {
                    UploadSpeed = (uploadSpeed / 1024).ToString("N1") + "MB/s";
                }
                else
                {
                    UploadSpeed = (uploadSpeed / 1024 / 1024).ToString("N1") + "GB/s";
                }
                if (downloadSpeed < 1024)
                {
                    DownloadSpeed = downloadSpeed.ToString("N1") + "KB/s";
                }
                else if (downloadSpeed < 1024 * 1024)
                {
                    DownloadSpeed = (downloadSpeed / 1024).ToString("N1") + "MB/s";
                }
                else
                {
                    DownloadSpeed = (downloadSpeed / 1024 / 1024).ToString("N1") + "GB/s";
                }
                Logger.Output(Base.LogType.DetailDebug, "Upload:" + UploadSpeed, "Download:" + DownloadSpeed);
            }
            _bytesReceived = bytesReceived;
            _bytesSent = bytesSent;
        }

#if !UNIX
        /// <summary>
        /// CPU性能计数器
        /// </summary>
        private static PerformanceCounter? _counter;
#endif

        /// <summary>
        /// CPU使用率
        /// </summary>
        public static float CPUUsage { get; private set; }

        /// <summary>
        /// 刷新计时器
        /// </summary>
        private static readonly Timer _refreshTimer = new(2500)
        {
            AutoReset = true
        };

        /// <summary>
        /// 操作系统信息
        /// </summary>
        public static OperatingSystemInfo? Info { get; private set; }

        /// <summary>
        /// CPU频率
        /// </summary>
        public static double CPUFrequency { get; private set; }

        /// <summary>
        /// CPU名称
        /// </summary>
        public static string CPUName { get; private set; } = string.Empty;

        /// <summary>
        /// CPU品牌
        /// </summary>
        public static string CPUBrand { get; private set; } = string.Empty;

        /// <summary>
        /// 系统名称
        /// </summary>
        public static string OS { get; private set; } = string.Empty;

        /// <summary>
        /// 已用内存
        /// </summary>
        public static ulong UsedRAM => TotalRAM - (Info?.Hardware?.RAM?.Free ?? 0) / 1024;

        /// <summary>
        /// 总内存
        /// </summary>
        public static ulong TotalRAM => (Info?.Hardware?.RAM?.Total ?? 1) / 1024;

        /// <summary>
        /// 内存占用百分比
        /// </summary>
        public static double RAMUsage => (double)((double)UsedRAM / TotalRAM * 100);
    }
}
