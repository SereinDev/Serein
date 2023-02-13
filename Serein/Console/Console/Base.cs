using Serein.Utils;
using Serein.Base;
using Serein.Core.JSPlugin;
using Serein.Core.Server;
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
        private extern static IntPtr FindWindow(string lpClassName, string lpWindowName);

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
                IntPtr Handle = GetStdHandle(STD_OUTPUT_HANDLE);
                GetConsoleMode(Handle, out uint OutputMode);
                OutputMode |= ENABLE_VIRTUAL_TERMINAL_PROCESSING;
                SetConsoleMode(Handle, OutputMode);
                Handle = GetStdHandle(STD_INPUT_HANDLE);
                GetConsoleMode(Handle, out uint InputMode);
                InputMode &= ~ENABLE_QUICK_EDIT_MODE;
                InputMode &= ~ENABLE_INSERT_MODE;
                SetConsoleMode(Handle, InputMode);
                IntPtr WindowHandle = FindWindow(null, System.Console.Title);
                IntPtr CloseMenu = GetSystemMenu(WindowHandle, IntPtr.Zero);
                RemoveMenu(CloseMenu, 0xF060, 0x0);
            }
            Logger.Output(LogType.Info, Global.LOGO);
#if UNIX
            if (Environment.OSVersion.Platform != PlatformID.Unix)
            {
                Logger.Output(LogType.Warn, "此版本为Unix专供。为获得更好的使用体验，请尝试使用Console类型的Serein，下载链接：https://github.com/Zaitonn/Serein/releases/latest");
            }
#else
            if (Environment.OSVersion.Platform != PlatformID.Win32NT)
            {
                Logger.Output(LogType.Warn, "此版本为Windows专供。为获得更好的使用体验，请尝试使用Console For Unix类型的Serein，下载链接：https://github.com/Zaitonn/Serein/releases/latest");
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
                    ServerManager.Kill();
                    e.Cancel = true;
                }
                else
                {
                    Logger.Output(LogType.Info, "正在关闭...");
                    JSFunc.Trigger(EventType.SereinClose);
                    Environment.Exit(0);
                }
            };
            while (true)
            {
                string line = System.Console.ReadLine();
                if (line != null)
                {
                    Input.Process(line.Trim());
                }
            }
        }
    }
}
