#if !UNIX
using System.Diagnostics;
#endif
using System;
using System.Timers;
using SystemInfoLibrary.OperatingSystem;

namespace Serein.Base
{
    internal static class SystemInfo
    {
#if !UNIX
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
        public static float CPUUsage => Counter.NextValue();
#endif
        /// <summary>
        /// 刷新计时器
        /// </summary>
        private static readonly Timer RefreshTimer = new Timer(2500)
        {
            AutoReset = true
        };

        /// <summary>
        /// 初始化系统信息
        /// </summary>
        public static void Init()
        {
            RefreshTimer.Elapsed += (_, e) => Info.Update();
            RefreshTimer.Start();
#if !UNIX
            System.Threading.Tasks.Task.Run(() => Logger.Out(Items.LogType.Debug, "Welcome. ", CPUUsage.ToString("N1").Replace('.', 'w')));
#endif
        }

        /// <summary>
        /// NET版本号
        /// </summary>
        public static string NET = Environment.Version.ToString();

        /// <summary>
        /// 操作系统信息
        /// </summary>
        public static OperatingSystemInfo Info = OperatingSystemInfo.GetOperatingSystemInfo();

        /// <summary>
        /// CPU频率
        /// </summary>
        public static double CPUFrequency => Info.Hardware.CPUs[0].Frequency;

        /// <summary>
        /// CPU名称
        /// </summary>
        public static string CPUName = Info.Hardware.CPUs[0].Name;

        /// <summary>
        /// CPU品牌
        /// </summary>
        public static string CPUBrand = Info.Hardware.CPUs[0].Brand;

        /// <summary>
        /// 系统名称
        /// </summary>
        public static string OS = Info.Name;

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
