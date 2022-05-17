using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;

namespace Serein
{
    class GetInfo
    {
        public static Thread GetAnnouncementThread = new Thread(GetAnnouncement)
        {
            IsBackground = true
        };
        public static Thread GetVersionThread = new Thread(GetVersion)
        {
            IsBackground = true
        };
        public static void GetAnnouncement()
        {
            Thread.Sleep(100);
            string OldAnnouncementId = "";
            while (true)
            {
                if (Global.Settings_serein.EnableGetAnnouncement)
                {
                    try
                    {
                        string AnnouncementId = RequestInfo("https://zaitonn.github.io/Serein/announcement/latest.txt");
                        if (OldAnnouncementId != AnnouncementId)
                        {
                            OldAnnouncementId = AnnouncementId;
                            Global.Ui.ShowBalloonTip(RequestInfo($"https://zaitonn.github.io/Serein/announcement/{AnnouncementId}.txt").Replace("\\n", "\n"));
                        }
                    }
                    catch (Exception e)
                    {
                        Global.Ui.ShowBalloonTip("公告获取异常：\n" + e.Message);
                    }
                }
                else
                {
                    OldAnnouncementId = "";
                }
                Thread.Sleep(120000);
            }
        }
        public static void GetVersion()
        {
            string OldPreVersion = "";
            Thread.Sleep(10000);
            while (true)
            {
                if (Global.Settings_serein.UpdateInfoType == 2)
                {
                    try
                    {
                        string PreVersion = RequestInfo("https://zaitonn.github.io/Serein/version/latest.txt");
                        if (PreVersion != Global.VERSION && OldPreVersion != PreVersion)
                        {
                            Global.Ui.ShowBalloonTip("发现新版本（测试版）:\n" + PreVersion);
                            OldPreVersion = PreVersion;
                        }
                    }
                    catch (Exception e)
                    {
                        Global.Ui.ShowBalloonTip("更新获取异常：\n" + e.Message);
                    }
                }
                else if (Global.Settings_serein.UpdateInfoType == 1)
                {
                    try
                    {
                        JObject JsonObject = (JObject)JsonConvert.DeserializeObject(RequestInfo("https://api.github.com/repos/Zaitonn/Serein/releases/latest", true));
                        string Version = JsonObject["tag_name"].ToString();
                        if (!(string.IsNullOrEmpty(Version) && string.IsNullOrWhiteSpace(Version)) &&
                            Version != Global.VERSION && OldPreVersion != Version)
                        {
                            Global.Ui.ShowBalloonTip("发现新版本:\n" + Version);
                            OldPreVersion = Version;
                        }
                    }
                    catch (Exception e)
                    {
                        Global.Ui.ShowBalloonTip("更新获取异常：\n" + e.Message);
                    }
                }
                else
                {
                    OldPreVersion = "";
                }
                Thread.Sleep(120000);
            }
        }
        public static string RequestInfo(string Url, bool isApi = false)
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            HttpWebRequest Request;
            HttpWebResponse Response;
            Stream ResponseStream;
            StreamReader Reader;
            Request = (HttpWebRequest)WebRequest.Create(Url);
            Request.KeepAlive = false;
            Request.ProtocolVersion = HttpVersion.Version10;
            Request.Method = "GET";
            Request.ContentType = "text/html;charset=UTF-8";
            Request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/101.0.4951.41 Safari/537.36 Edg/101.0.1210.32";
            Request.Accept = isApi ? Request.Accept : "application/vnd.github.v3+json";
            Response = (HttpWebResponse)Request.GetResponse();
            ResponseStream = Response.GetResponseStream();
            Reader = new StreamReader(ResponseStream, Encoding.GetEncoding("utf-8"));
            string Text = Reader.ReadToEnd();
            Reader.Close();
            ResponseStream.Close();
            Request.Abort();
            return Text;
        }
    }
}
