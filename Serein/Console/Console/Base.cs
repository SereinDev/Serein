using Serein.Base;
using Serein.Server;
using System;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Serein.Console
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
            var Handle = GetStdHandle(STD_OUTPUT_HANDLE);
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
            System.Console.OutputEncoding = Encoding.UTF8;
        }

        /// <summary>
        /// 启动输入循环
        /// </summary>
        public static void Start()
        {
            Logger.Out(Items.LogType.Info, "Welcome.");
            Logger.Out(Items.LogType.Info, "你可以输入\"help\"获取更多信息");
            if (Global.Args.Contains("auto_connect"))
                Task.Run(() => Websocket.Connect(false));
            if (Global.Args.Contains("auto_start"))
                Task.Run(() => ServerManager.Start(true));
            System.Console.Title = "Serein " + Global.VERSION;
            System.Console.CancelKeyPress += (sender, e) =>
            {
                e.Cancel = true;
                Logger.Out(Items.LogType.Error, "若要关闭Serein请使用\"exit\"命令");
            };
            while (true)
            {
                string Line = System.Console.ReadLine();
                if (!string.IsNullOrEmpty(Line))
                    Input.Process(Line.Trim());
            }
        }

        /// <summary>
        /// 加载配置文件
        /// </summary>
        /// <param name="args">启动参数</param>
        public static void Load(string[] args = null)
        {
            IO.ReadAll();
            IO.SaveSettings();
            Global.Args = args ?? Global.Args;
            Global.Settings.Serein.Debug = Global.Args.Contains("debug");
        }
    }
}
