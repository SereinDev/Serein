using System;
using System.Diagnostics;
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

            PanelConsoleWebBrowser.Navigate(@"file:\\\" + AppDomain.CurrentDomain.BaseDirectory + "console\\console.html?type=panel");
            BotWebBrowser.Navigate(@"file:\\\" + AppDomain.CurrentDomain.BaseDirectory + "console\\console.html?type=bot");
            Global.PanelConsoleWebBrowser = PanelConsoleWebBrowser;
            Global.BotWebBrowser = BotWebBrowser;
        }
        public void Initialize()
        {
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
            TaskManager.RunnerThread.Start();
            GetInfo.GetAnnouncementThread.Start();
            GetInfo.GetVersionThread.Start();
            SetWindowTheme(RegexList.Handle, "Explorer", null);
            SetWindowTheme(TaskList.Handle, "Explorer", null);
        }
        private void MultiOpenCheck()
        {
            Process[] Processes = Process.GetProcessesByName("Serein");
            string CurrentName = Process.GetCurrentProcess().MainModule.FileName;
            foreach (Process process in Processes)
            {
                if (process.MainModule.FileName == CurrentName && process.Id != Process.GetCurrentProcess().Id)
                {
                    if ((int)MessageBox.Show(
                        $"检测到位于相同目录下的进程（PID={process.Id}）\n" +
                        "同时运行多个进程可能导致数据无法保存甚至崩溃\n\n" +
                        "是否继续运行？\n\n" +
                        "Tip:你可以将Serein的整个运行目录复制多份隔离运行以解决此问题", "Serein",
                        MessageBoxButtons.OKCancel, MessageBoxIcon.Warning
                    ) != 1)
                    {
                        Environment.Exit(0);
                    }
                    break;
                }
            }
        }
    }
}
