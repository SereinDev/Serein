using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Net;
using System.Windows.Forms;
using System.Threading;
using System.Runtime.InteropServices;

namespace Serein
{
    public partial class Ui : Form
    {
        [DllImport("uxtheme.dll", ExactSpelling = true, CharSet = CharSet.Unicode)]
        private static extern int SetWindowTheme(IntPtr hwnd, string pszSubAppName, string pszSubIdList);

        public string GetAnnouncement()
        {
            HttpWebRequest request;
            HttpWebResponse response;
            Stream ResponseStream;
            StreamReader Reader;
            request = (HttpWebRequest)WebRequest.Create("https://zaitonn.github.io/Serein/api/announcement/latest.txt");
            request.Method = "GET";
            request.ContentType = "text/html;charset=UTF-8";
            request.UserAgent = null;
            request.Timeout = 5;
            response = (HttpWebResponse)request.GetResponse();
            ResponseStream = response.GetResponseStream();
            Reader = new StreamReader(ResponseStream, Encoding.GetEncoding("utf-8"));
            Reader.Close();
            ResponseStream.Close();
            request = (HttpWebRequest)WebRequest.Create($"https://zaitonn.github.io/Serein/api/announcement/{Reader.ReadToEnd()}.txt");
            request.Method = "GET";
            request.ContentType = "text/html;charset=UTF-8";
            request.UserAgent = null;
            request.Timeout = 5;
            response = (HttpWebResponse)request.GetResponse();
            ResponseStream = response.GetResponseStream();
            Reader = new StreamReader(ResponseStream, Encoding.GetEncoding("utf-8"));
            string announcement = Reader.ReadToEnd();
            Reader.Close();
            ResponseStream.Close();
            return announcement;
        }
        public void UpdateVersion()
        {
            SettingSereinVersion.Text = $"Version:{Global.VERSION}";
        }
        public void InitWebBrowser()
        {
            if(!File.Exists(Global.Path + "console\\console.html"))
            {
                PanelConsoleWebBrowser.Navigate("https://zaitonn.github.io/Serein/Console/console.html");
                BotWebBrowser.Navigate("https://zaitonn.github.io/Serein/Console/console.html");
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
            InitWebBrowser();
            Settings.ReadSettings();
            LoadSettings();
            Settings.StartSaveSettings();
            LoadPlugins();
            LoadRegex();
            Thread UpdateInfoThread = new Thread(UpdateInfo);
            UpdateInfoThread.Start();
            SetWindowTheme(RegexList.Handle, "Explorer", null);
            if (Global.Settings_serein.EnableGetAnnouncement)
            {
                Task task = new Task(() =>
                {
                    ShowBalloonTip(GetAnnouncement());
                });
                task.Start();

            }
        }
    }
}
