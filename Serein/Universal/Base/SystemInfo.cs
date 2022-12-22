#if !LINUX
using Microsoft.VisualBasic.Devices;
#endif
using System;
using System.Diagnostics;
using System.Management;

namespace Serein.Base
{
    internal static class SystemInfo
    {
        /// <summary>
        /// CPU性能计数器
        /// </summary>
        private static readonly PerformanceCounter Counter = new PerformanceCounter("Processor", "% Processor Time", "_Total")
        {
            MachineName = "."
        };

        /// <summary>
        /// CPU使用率
        /// </summary>
        public static string CPUPercentage => Counter.NextValue().ToString("N1");

        /// <summary>
        /// CPU名称
        /// </summary>
        public static string CPUName
        {
            get
            {
                try
                {
                    foreach (ManagementObject m in new ManagementClass("Win32_Processor").GetInstances())
                    {
                        return m["Name"].ToString();
                    }
                    return "Unknown";
                }
                catch
                {
                    return "Unknown";
                }
            }
        }

        /// <summary>
        /// NET版本号
        /// </summary>
        public static string NET = Environment.Version.ToString();

#if LINUX
        public static string OS = string.Empty, RAMPercentage = string.Empty;

        public static ulong TotalRAM = 0, UsedRAM = 0;
#else

        /// <summary>
        /// 设备信息实例
        /// </summary>
        private static readonly ComputerInfo Info = new ComputerInfo();

        /// <summary>
        /// 系统名称
        /// </summary>
        public static string OS = Info.OSFullName;

        /// <summary>
        /// 已用内存
        /// </summary>
        public static ulong UsedRAM => TotalRAM - Info.AvailablePhysicalMemory / 1024 / 1024;

        /// <summary>
        /// 总内存
        /// </summary>
        public static ulong TotalRAM = Info.TotalPhysicalMemory / 1024 / 1024;

        /// <summary>
        /// 内存占用百分比
        /// </summary>
        public static string RAMPercentage => ((double)((double)UsedRAM / TotalRAM * 100)).ToString("N1");
#endif

    }
}
