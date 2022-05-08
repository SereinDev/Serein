using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;

namespace Serein
{
    public partial class Ui : Form
    {
        [DllImport("uxtheme.dll", ExactSpelling = true, CharSet = CharSet.Unicode)]
        private static extern int SetWindowTheme(IntPtr hwnd, string pszSubAppName, string pszSubIdList);
        public void UpdateVersion()
        {
            SettingSereinVersion.Text = $"Version:{Global.VERSION}";
        }
        public void InitWebBrowser()
        {
            if (!File.Exists(Global.Path + "console\\console.html"))
            {
                PanelConsoleWebBrowser.Navigate("https://zaitonn.github.io/Serein/console/console.html");
                BotWebBrowser.Navigate("https://zaitonn.github.io/Serein/console/console.html");
            }
            else
            {
                PanelConsoleWebBrowser.Navigate(@"file:\\\" + AppDomain.CurrentDomain.BaseDirectory + "console\\console.html?type=panel");
                BotWebBrowser.Navigate(@"file:\\\" + AppDomain.CurrentDomain.BaseDirectory + "console\\console.html?type=bot");
            }
            Global.PanelConsoleWebBrowser = PanelConsoleWebBrowser;
            Global.BotWebBrowser = BotWebBrowser;
        }
        public void Initialize()
        {
            TaskManager.RunnerThread.Start();
            GetInfo.GetAnnouncementThread.Start();
            GetInfo.GetVersionThread.Start();
            InitWebBrowser();
            Settings.ReadSettings();
            LoadSettings();
            Settings.StartSaveSettings();
            LoadPlugins();
            LoadRegex();
            LoadTask();
            Thread UpdateInfoThread = new Thread(UpdateInfo)
            {
                IsBackground = true
            };
            UpdateInfoThread.Start();
            SetWindowTheme(RegexList.Handle, "Explorer", null);
            SetWindowTheme(TaskList.Handle, "Explorer", null);
        }
    }
}
