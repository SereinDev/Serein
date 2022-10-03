using Microsoft.VisualBasic.Devices;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Management;

namespace Serein.Base
{
    internal static class SystemInfo
    {
        private static readonly PerformanceCounter Counter = new PerformanceCounter("Processor", "% Processor Time", "_Total")
        {
            MachineName = "."
        };
        private static ComputerInfo Info = new ComputerInfo();

        public static string OS = Info.OSFullName;
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
        public static ulong UsedRAM => TotalRAM - Info.AvailablePhysicalMemory / 1024 / 1024;
        public static ulong TotalRAM = Info.TotalPhysicalMemory / 1024 / 1024;
        public static string RAMPercentage =>
           ((double)((double)UsedRAM / TotalRAM * 100)).ToString("N1");
        public static string CPUPercentage => Counter.NextValue().ToString("N1");
    }
}
