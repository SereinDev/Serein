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

        public string requestAnnouncement(string Arg)
        {
            HttpWebRequest Request;
            HttpWebResponse Response;
            Stream ResponseStream;
            StreamReader Reader;
            Request = (HttpWebRequest)WebRequest.Create($"https://zaitonn.github.io/Serein/announcement/{Arg}.txt");
            Request.Method = "GET";
            Request.ContentType = "text/html;charset=UTF-8";
            Request.UserAgent = null;
            Response = (HttpWebResponse)Request.GetResponse();
            ResponseStream = Response.GetResponseStream();
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
            TaskManager.RunnerThread.Start();
            InitWebBrowser();
            Settings.ReadSettings();
            LoadSettings();
            Settings.StartSaveSettings();
            LoadPlugins();
            LoadRegex();
            LoadTask();
            Thread UpdateInfoThread = new Thread(UpdateInfo);
            UpdateInfoThread.IsBackground = true;
            UpdateInfoThread.Start();
            Thread GetAnnouncementThread = new Thread(GetAnnouncement);
            GetAnnouncementThread.IsBackground = true;
            GetAnnouncementThread.Start();
            SetWindowTheme(RegexList.Handle, "Explorer", null);
            SetWindowTheme(TaskList.Handle, "Explorer", null);
        }
        public void GetAnnouncement()
        {
            string AnnouncementId = "", OldAnnouncementId = "";
            while (true)
            {
                if (Global.Settings_serein.EnableGetAnnouncement)
                {
                    try
                    {
                        AnnouncementId = requestAnnouncement("latest");
                        if (OldAnnouncementId != AnnouncementId)
                        {
                            OldAnnouncementId = AnnouncementId;
                            ShowBalloonTip(requestAnnouncement(AnnouncementId).Replace("\\n", "\n"));
                        }
                    }
                    catch
                    {
                        ShowBalloonTip("公告获取异常");
                    }
                }
                else
                {
                    AnnouncementId = "";
                    OldAnnouncementId = "";
                }
                Thread.Sleep(60000);
            }
        }
    }
}
