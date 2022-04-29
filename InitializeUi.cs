using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Linq;
using System.Windows.Forms;
using System.Threading;

namespace Serein
{
    partial class Init
    {
        public static void CheckFiles()
        {
            if (!File.Exists(AppDomain.CurrentDomain.BaseDirectory + "console.html"))
            {
                MessageBox.Show($"文件  {AppDomain.CurrentDomain.BaseDirectory}console.html  已丢失");
                Environment.Exit(0);
            }
        }
    }
    public partial class Ui : Form
    {
        public void UpdateVersion()
        {
            SettingSereinVersion.Text = $"Version:{Global.VERSION}";
            ShowBalloonTip($"Hello!\n这是测试版（{Global.VERSION}），不建议用于生产环境哦qwq");
        }
        public void InitWebBrowser()
        {
            PanelConsoleWebBrowser.Navigate(@"file:\\\" + AppDomain.CurrentDomain.BaseDirectory + "console.html?from=panel");
            BotWebBrowser.Navigate(@"file:\\\" + AppDomain.CurrentDomain.BaseDirectory + "console.html?from=bot");
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
            Thread UpdateInfoThread = new Thread(UpdateInfo);
            UpdateInfoThread.Start();
            SetWindowTheme(RegexList.Handle, "Explorer", null);
        }
    }
}
