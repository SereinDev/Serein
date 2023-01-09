#if !UNIX
using System.Diagnostics;
#endif
using System;
using System.Net.NetworkInformation;
using System.Timers;
using SystemInfoLibrary.OperatingSystem;

namespace Serein.Base
{
    internal static class SystemInfo
    {
        /// <summary>
        /// 初始化系统信息
        /// </summary>
        public static void Init()
        {
            RefreshTimer.Elapsed += (sender, e) => Info.Update();
            RefreshTimer.Elapsed += (sender, e) => UpdateNetSpeed();
            RefreshTimer.Start();
#if !UNIX
            RefreshTimer.Elapsed += (sender, e) => CPUUsage = Counter.NextValue();
            Counter.NextValue();
            Logger.Out(Items.LogType.Debug, "Loaded.");
#endif
        }

        public static string UploadSpeed, DownloadSpeed;
        private static long BytesReceived, BytesSent;

        private static void UpdateNetSpeed()
        {
            long bytesReceived = 0, bytesSent = 0;
            foreach (NetworkInterface INet in NetworkInterface.GetAllNetworkInterfaces())
            {
                if (INet == null)
                {
                    continue;
                }
                bytesReceived += INet.GetIPStatistics().BytesReceived;
                bytesSent += INet.GetIPStatistics().BytesSent;
            }
            if (BytesReceived != 0 && BytesSent != 0)
            {
                double uploadSpeed = (double)(bytesSent - BytesSent) / 1024 / 2.5, downloadSpeed = (double)(bytesReceived - BytesReceived) / 1024 / 2.5;
                if (uploadSpeed < 1024)
                {
                    UploadSpeed = uploadSpeed.ToString("N1") + "KB/s";
                }
                else if (uploadSpeed < 1024 * 1024)
                {
                    UploadSpeed = (uploadSpeed / 1024).ToString("N!") + "MB/s";
                }
                else
                {
                    UploadSpeed = (uploadSpeed / 1024 / 1024).ToString("N!") + "GB/s";
                }
                if (downloadSpeed < 1024)
                {
                    DownloadSpeed = downloadSpeed.ToString("N!") + "KB/s";
                }
                else if (downloadSpeed < 1024 * 1024)
                {
                    DownloadSpeed = (downloadSpeed / 1024).ToString("N!") + "MB/s";
                }
                else
                {
                    DownloadSpeed = (downloadSpeed / 1024 / 1024).ToString("N!") + "GB/s";
                }
                Logger.Out(Items.LogType.DetailDebug, "Upload:" + UploadSpeed, "Download:" + DownloadSpeed);
            }
            BytesReceived = bytesReceived;
            BytesSent = bytesSent;
        }

#if !UNIX
        /// <summary>
        /// CPU性能计数器
        /// </summary>
        private static readonly PerformanceCounter Counter = new("Processor", "% Processor Time", "_Total")
        {
            MachineName = "."
        };

        /// <summary>
        /// CPU使用率
        /// </summary>
        public static float CPUUsage;
#endif
        /// <summary>
        /// 刷新计时器
        /// </summary>
        private static readonly Timer RefreshTimer = new(2500)
        {
            AutoReset = true
        };

        /// <summary>
        /// 操作系统信息
        /// </summary>
        public static OperatingSystemInfo Info = OperatingSystemInfo.GetOperatingSystemInfo();

        /// <summary>
        /// CPU频率
        /// </summary>
        public static double CPUFrequency
        {
            get
            {
                try
                {
                    return Info.Hardware.CPUs.Count > 0 ? Info.Hardware.CPUs[0].Frequency : 0;
                }
                catch (Exception e)
                {
                    Logger.Out(Items.LogType.Debug, e);
                    return 0;
                }
            }
        }

        /// <summary>
        /// CPU名称
        /// </summary>
        public static string CPUName
        {
            get
            {
                try
                {
                    return Info.Hardware.CPUs.Count > 0 ? Info.Hardware.CPUs[0].Name : "未知";
                }
                catch (Exception e)
                {
                    Logger.Out(Items.LogType.Debug, e);
                    return "未知";
                }
            }
        }

        /// <summary>
        /// CPU品牌
        /// </summary>
        public static string CPUBrand
        {
            get
            {
                try
                {
                    return Info.Hardware.CPUs.Count > 0 ? Info.Hardware.CPUs[0].Brand : "未知";
                }
                catch (Exception e)
                {
                    Logger.Out(Items.LogType.Debug, e);
                    return "未知";
                }
            }
        }

        /// <summary>
        /// 系统名称
        /// </summary>
        public static string OS => Info.Name;

        /// <summary>
        /// 已用内存
        /// </summary>
        public static ulong UsedRAM => TotalRAM - Info.Hardware.RAM.Free / 1024;

        /// <summary>
        /// 总内存
        /// </summary>
        public static ulong TotalRAM = Info.Hardware.RAM.Total / 1024;

        /// <summary>
        /// 内存占用百分比
        /// </summary>
        public static double RAMUsage => (double)((double)UsedRAM / TotalRAM * 100);
    }
}
