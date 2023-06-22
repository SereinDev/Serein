using Serein.Base;
using Serein.Core.JSPlugin;
using Serein.Core.Server;
using Serein.Extensions;
using System;
using System.Runtime.InteropServices;
using System.Text;

namespace Serein.Utils.Console
{
    internal static class Base
    {
        private const int STD_INPUT_HANDLE = -10;
        private const int STD_OUTPUT_HANDLE = -11;
        private const uint ENABLE_VIRTUAL_TERMINAL_PROCESSING = 0x0004;
        private const uint ENABLE_QUICK_EDIT_MODE = 0x0040;
        private const uint ENABLE_INSERT_MODE = 0x0020;

        [DllImport("user32.dll", EntryPoint = "FindWindow", CharSet = CharSet.Unicode)]
        private extern static IntPtr FindWindow(string? lpClassName, string lpWindowName);

        [DllImport("user32.dll", EntryPoint = "GetSystemMenu")]
        private extern static IntPtr GetSystemMenu(IntPtr hWnd, IntPtr bRevert);

        [DllImport("user32.dll", EntryPoint = "RemoveMenu")]
        private extern static IntPtr RemoveMenu(IntPtr hMenu, uint uPosition, uint uFlags);

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern IntPtr GetStdHandle(int nStdHandle);

        [DllImport("kernel32.dll")]
        private static extern bool GetConsoleMode(IntPtr hConsoleHandle, out uint lpMode);

        [DllImport("kernel32.dll")]
        private static extern bool SetConsoleMode(IntPtr hConsoleHandle, uint dwMode);

        /// <summary>
        /// 初始化控制台
        /// </summary>
        public static void Init()
        {
            System.Console.ForegroundColor = ConsoleColor.Gray;
            System.Console.OutputEncoding = Encoding.UTF8;
            if (Environment.OSVersion.Platform == PlatformID.Win32NT)
            {
                IntPtr handle = GetStdHandle(STD_OUTPUT_HANDLE);
                GetConsoleMode(handle, out uint outputMode);
                outputMode |= ENABLE_VIRTUAL_TERMINAL_PROCESSING;
                SetConsoleMode(handle, outputMode);
                handle = GetStdHandle(STD_INPUT_HANDLE);
                GetConsoleMode(handle, out uint inputMode);
                inputMode &= ~ENABLE_QUICK_EDIT_MODE;
                inputMode &= ~ENABLE_INSERT_MODE;
                SetConsoleMode(handle, inputMode);
                IntPtr closeMenu = GetSystemMenu(FindWindow(null, System.Console.Title), IntPtr.Zero);
                RemoveMenu(closeMenu, 0xF060, 0x0);
            }
            Logger.Output(LogType.Info, Global.LOGO);
#if UNIX
            if (Environment.OSVersion.Platform != PlatformID.Unix)
            {
                Logger.Output(LogType.Warn, "此版本为Unix专供。为获得更好的使用体验，请尝试使用`Console`类型的Serein，下载链接：https://github.com/Zaitonn/Serein/releases/latest");
            }
#else
            if (Environment.OSVersion.Platform != PlatformID.Win32NT)
            {
                Logger.Output(LogType.Warn, "此版本为Windows专供。为获得更好的使用体验，请尝试使用`Console For Unix`类型的Serein，下载链接：https://github.com/Zaitonn/Serein/releases/latest");
            }
#endif
        }

        /// <summary>
        /// 启动输入循环
        /// </summary>
        public static void Start()
        {
            Runtime.Start();
            Logger.Output(LogType.Info, "启动成功");
            Logger.Output(LogType.Info, "你可以输入“help”获取更多信息");
            System.Console.Title = "Serein " + Global.VERSION;
            System.Console.CancelKeyPress += (_, e) =>
            {
                if (ServerManager.Status)
                {
                    e.Cancel = true;
                    ServerManager.Kill();
                }
                else
                {
                    Logger.Output(LogType.Info, "正在关闭...");
                    JSFunc.Trigger(EventType.SereinClose);
                    500.ToSleep();
                    Environment.Exit(0);
                }
            };
            while (true)
            {
                Input.Process(System.Console.ReadLine()?.Trim());
            }
        }
    }
}
