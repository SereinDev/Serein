using Microsoft.VisualBasic.Devices;
using System;
using System.Diagnostics;
using System.Management;

namespace Serein.Base
{
    internal static class SystemInfo
    {
        private static readonly PerformanceCounter pcCpuLoad = new PerformanceCounter("Processor", "% Processor Time", "_Total")
        {
            MachineName = "."
        };
        public static string OS = new ComputerInfo().OSFullName;
        public static string NET = Environment.Version.ToString();
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
        public static string UsedRAM => ((ulong.TryParse(TotalRAM, out ulong i) ? i : 0) - new ComputerInfo().AvailablePhysicalMemory / 1024 / 1024).ToString();
        public static string TotalRAM = (new ComputerInfo().TotalPhysicalMemory / 1024 / 1024).ToString();
        public static string RAMPercentage => (
                    (double)(ulong.TryParse(UsedRAM, out ulong i) ? i : 1) /
                    (ulong.TryParse(TotalRAM, out ulong j) ? j : 1) * 100
                ).ToString("N1");
        public static string CPUPercentage => pcCpuLoad.NextValue().ToString("N1");
    }
}
