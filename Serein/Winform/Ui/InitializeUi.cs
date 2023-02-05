using Serein.Base;
using Serein.Core.JSPlugin;
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
            ServerPanelConsoleWebBrowser.Navigate(@"file:\\\" + Global.PATH + "console\\console.html?type=ServerPanel");
            BotWebBrowser.Navigate(@"file:\\\" + Global.PATH + "console\\console.html?type=bot");
            JSPluginWebBrowser.Navigate(@"file:\\\" + Global.PATH + "console\\console.html?type=bot");
        }

        private void Initialize()
        {
            InitWebBrowser();
            LoadSettings();
            LoadPlugins();
            LoadRegex();
            LoadTask();
            UpdateInfoTimer.Start();
            UpdateInfoTimer.Elapsed += (_, _) =>
            {
                if (!Disposing)
                {
                    Invoke(UpdateInfo);
                }
            };
            System.Threading.Tasks.Task.Run(UpdateInfo);
            SetWindowTheme(RegexList.Handle, "Explorer", null);
            SetWindowTheme(TaskList.Handle, "Explorer", null);
            SetWindowTheme(MemberList.Handle, "Explorer", null);
            SetWindowTheme(JSPluginList.Handle, "Explorer", null);
            SetWindowTheme(SettingEventList.Handle, "Explorer", null);
            SendMessage(RegexList.Handle, 4158, IntPtr.Zero, Cursors.Arrow.Handle);
            SendMessage(TaskList.Handle, 4158, IntPtr.Zero, Cursors.Arrow.Handle);
            SendMessage(MemberList.Handle, 4158, IntPtr.Zero, Cursors.Arrow.Handle);
            SendMessage(JSPluginList.Handle, 4158, IntPtr.Zero, Cursors.Arrow.Handle);
            SendMessage(SettingEventList.Handle, 4158, IntPtr.Zero, Cursors.Arrow.Handle);
        }
    }
}
