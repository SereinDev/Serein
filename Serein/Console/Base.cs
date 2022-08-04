using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using Serein.Items;
using Serein.Base;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Serein.Console
{
    internal class Base
    {
        const int STD_INPUT_HANDLE = -10;
        const int STD_OUTPUT_HANDLE = -11;
        const uint ENABLE_VIRTUAL_TERMINAL_PROCESSING = 0x0004;
        const uint ENABLE_QUICK_EDIT_MODE = 0x0040;
        const uint ENABLE_INSERT_MODE = 0x0020;

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
            IntPtr windowHandle = FindWindow(null, System.Console.Title);
            IntPtr closeMenu = GetSystemMenu(windowHandle, IntPtr.Zero);
            uint SC_CLOSE = 0xF060;
            RemoveMenu(closeMenu, SC_CLOSE, 0x0);
        }
        public static void Start()
        {
            System.Console.OutputEncoding = Encoding.UTF8;
            System.Console.Title = "Serein " + Global.VERSION;
            System.Console.CancelKeyPress += (sender, e) =>
            {
                e.Cancel = true;
                Output.Logger(2, "若要退出Serein请使用\"exit\"命令");
            };
            Output.Logger(1, "Welcome.");
            while (true)
            {
                string Line = System.Console.ReadLine() ?? "";
                Input.Process(Line.Trim());
            }
        }

        public static void Load(string[] args =null)
        {
            Settings.Base.ReadSettings();
            Members.Load();
            Global.Args = args ?? Global.Args;
            Global.Settings.Serein.Debug = Global.Args.Contains("debug");
            if (!Directory.Exists(Global.Path + "\\data"))
            {
                Directory.CreateDirectory(Global.Path + "\\data");
            }
            if (File.Exists($"{Global.Path}\\data\\regex.json"))
            {
                string Text = File.ReadAllText($"{Global.Path}\\data\\regex.json",Encoding.UTF8);
                if (!string.IsNullOrEmpty(Text))
                {
                    JObject JsonObject = (JObject)JsonConvert.DeserializeObject(Text);
                    if (JsonObject["type"].ToString().ToUpper() == "REGEX")
                    {
                        Global.UpdateRegexItems(((JArray)JsonObject["data"]).ToObject<List<RegexItem>>());
                    }
                }
            }
            if (File.Exists($"{Global.Path}\\data\\task.json"))
            {
                string Text = File.ReadAllText($"{Global.Path}\\data\\task.json", Encoding.UTF8);
                if (!string.IsNullOrEmpty(Text))
                {
                    JObject JsonObject = (JObject)JsonConvert.DeserializeObject(Text);
                    if (JsonObject["type"].ToString().ToUpper() == "TASK")
                    {
                        Global.UpdateTaskItems(((JArray)JsonObject["data"]).ToObject<List<TaskItem>>());
                    }
                }
            }
        }
    }
}
