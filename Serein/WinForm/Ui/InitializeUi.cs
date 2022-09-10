using Serein.Base;
using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Serein.Ui
{
    public partial class Ui : Form
    {
        [DllImport("uxtheme.dll", ExactSpelling = true, CharSet = CharSet.Unicode)]
        private static extern int SetWindowTheme(IntPtr hwnd, string pszSubAppName, string pszSubIdList);

        [DllImport("user32.dll")]
        private static extern IntPtr SendMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);

        private void InitWebBrowser()
        {
            PanelConsoleWebBrowser.Navigate(@"file:\\\" + AppDomain.CurrentDomain.BaseDirectory + "console\\console.html?type=panel");
            BotWebBrowser.Navigate(@"file:\\\" + AppDomain.CurrentDomain.BaseDirectory + "console\\console.html?type=bot");
            SereinPluginsWebBrowser.Navigate(@"file:\\\" + AppDomain.CurrentDomain.BaseDirectory + "console\\console.html?type=bot");
        }
        private void Initialize()
        {
            InitWebBrowser();
            LoadSettings();
            LoadPlugins();
            LoadRegex();
            LoadTask();
            Task UpdateInfoThread = new Task(UpdateInfo);
            UpdateInfoThread.Start();
            Loader.StartSavingAndUpdating();
            TaskRunner.Start();
            GetInfo.GetAnnouncementThread.Start();
            GetInfo.GetVersionThread.Start();
            SetWindowTheme(RegexList.Handle, "Explorer", null);
            SetWindowTheme(TaskList.Handle, "Explorer", null);
            SetWindowTheme(MemberList.Handle, "Explorer", null);
            SetWindowTheme(SereinPluginsList.Handle, "Explorer", null);
            SetWindowTheme(SettingEventList.Handle, "Explorer", null);
            SendMessage(RegexList.Handle, 4158, IntPtr.Zero, Cursors.Arrow.Handle);
            SendMessage(TaskList.Handle, 4158, IntPtr.Zero, Cursors.Arrow.Handle);
            SendMessage(MemberList.Handle, 4158, IntPtr.Zero, Cursors.Arrow.Handle);
            SendMessage(SereinPluginsList.Handle, 4158, IntPtr.Zero, Cursors.Arrow.Handle);
            SendMessage(SettingEventList.Handle, 4158, IntPtr.Zero, Cursors.Arrow.Handle);
            new Task(() => Logger.Out(999, "[Serein] Welcome. ", SystemInfo.CPUPercentage.Replace('.', 'w'))).Start();
            TaskList.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
            MemberList.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
        }

        private static void MultiOpenCheck()
        {
            Process[] Processes = Process.GetProcessesByName("Serein");
            string CurrentName = Process.GetCurrentProcess().MainModule.FileName;
            foreach (Process process in Processes)
            {
                try
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
                catch
                {
                }
            }
        }
    }
}
