using System.Runtime.InteropServices;
using System;
using System.Text;
using System.Collections.Generic;
using System.Threading;

namespace WebConsole
{
    static class Program
    {
        const int STD_INPUT_HANDLE = -10;
        const int STD_OUTPUT_HANDLE = -11;
        const uint ENABLE_VIRTUAL_TERMINAL_PROCESSING = 0x0004;
        const uint ENABLE_QUICK_EDIT_MODE = 0x0040;
        const uint ENABLE_INSERT_MODE = 0x0020;

        public static IList<string> Args = new string[] { };
        private static Thread WebSocketThreading = new Thread(WebSocket.Start);

        [DllImport("user32.dll", EntryPoint = "FindWindow")]
        extern static IntPtr FindWindow(string lpClassName, string lpWindowName);
        [DllImport("user32.dll", EntryPoint = "GetSystemMenu")]
        extern static IntPtr GetSystemMenu(IntPtr hWnd, IntPtr bRevert);
        [DllImport("user32.dll", EntryPoint = "RemoveMenu")]
        extern static IntPtr RemoveMenu(IntPtr hMenu, uint uPosition, uint uFlags);
        [DllImport("kernel32.dll", SetLastError = true)]
        static extern IntPtr GetStdHandle(int nStdHandle);
        [DllImport("kernel32.dll")]
        static extern bool GetConsoleMode(IntPtr hConsoleHandle, out uint lpMode);
        [DllImport("kernel32.dll")]
        static extern bool SetConsoleMode(IntPtr hConsoleHandle, uint dwMode);


        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        /// <param name="args">启动参数</param>
        [STAThread]
        private static void Main(string[] args)
        {
            Init();
            Args = args;
            WebSocket.Start();
            ReadKey();
        }

        private static void Init()
        {
            var Handle = GetStdHandle(STD_OUTPUT_HANDLE);
            GetConsoleMode(Handle, out uint OutputMode);
            OutputMode |= ENABLE_VIRTUAL_TERMINAL_PROCESSING;
            SetConsoleMode(Handle, OutputMode);
            Handle = GetStdHandle(STD_INPUT_HANDLE);
            GetConsoleMode(Handle, out uint InputMode);
            InputMode &= ~ENABLE_QUICK_EDIT_MODE;
            InputMode &= ~ENABLE_INSERT_MODE;
            SetConsoleMode(Handle, InputMode);
            IntPtr windowHandle = FindWindow(null, Console.Title);
            IntPtr closeMenu = GetSystemMenu(windowHandle, IntPtr.Zero);
            uint SC_CLOSE = 0xF060;
            RemoveMenu(closeMenu, SC_CLOSE, 0x0);
            Console.OutputEncoding = Encoding.UTF8;
        }
        private static void ReadKey()
        {
            while (true)
            {
                Console.ReadKey(true);
            }
        }
    }
}