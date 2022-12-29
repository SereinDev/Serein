using Serein.Base;
using Serein.JSPlugin;
using System;
using System.Runtime.InteropServices;
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
            IO.StartSavingAndUpdating();
            TaskRunner.Start();
            Net.Init();
            System.Threading.Tasks.Task.Run(UpdateInfo);
            System.Threading.Tasks.Task.Run(JSPluginManager.Load);
            System.Threading.Tasks.Task.Run(SystemInfo.Init);
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
        }
    }
}
