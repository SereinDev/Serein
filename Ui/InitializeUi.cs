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

        public string requestAnnouncement(string arg)
        {
            HttpWebRequest request;
            HttpWebResponse response;
            Stream ResponseStream;
            StreamReader Reader;
            request = (HttpWebRequest)WebRequest.Create($"https://zaitonn.github.io/Serein/api/announcement/{arg}.txt");
            request.Method = "GET";
            request.ContentType = "text/html;charset=UTF-8";
            request.UserAgent = null;
            response = (HttpWebResponse)request.GetResponse();
            ResponseStream = response.GetResponseStream();
            Reader = new StreamReader(ResponseStream, Encoding.GetEncoding("utf-8"));
            string Text = Reader.ReadToEnd();
            Reader.Close();
            ResponseStream.Close();
            return Text;
        }
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
            InitWebBrowser();
            Settings.ReadSettings();
            LoadSettings();
            Settings.StartSaveSettings();
            LoadPlugins();
            LoadRegex();
            Thread UpdateInfoThread = new Thread(UpdateInfo);
            UpdateInfoThread.IsBackground = true;
            UpdateInfoThread.Start();
            Thread GetAnnouncementThread = new Thread(GetAnnouncement);
            GetAnnouncementThread.IsBackground = true;
            GetAnnouncementThread.Start();
            SetWindowTheme(RegexList.Handle, "Explorer", null);
            SetWindowTheme(TaskList.Handle, "Explorer", null);
            SetWindowTheme(PluginList.Handle, "Explorer", null);

        }
        public void GetAnnouncement()
        {
            string announcementId = "", oldAnnouncementId = "";
            while (true)
            {

                if (Global.Settings_serein.EnableGetAnnouncement)
                {
                    try
                    {
                        announcementId = requestAnnouncement("latest");
                        if (oldAnnouncementId != announcementId)
                        {
                            oldAnnouncementId = announcementId;
                            ShowBalloonTip(requestAnnouncement(announcementId).Replace("\\n", "\n"));
                        }
                    }
                    catch
                    {
                        ShowBalloonTip("公告获取异常");
                    }
                }
                Thread.Sleep(60000);
            }
        }
    }
}
