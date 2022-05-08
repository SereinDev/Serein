using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
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
        public static string RequestInfo(string Url)
        {
            HttpWebRequest Request;
            HttpWebResponse Response;
            Stream ResponseStream;
            StreamReader Reader;
            Request = (HttpWebRequest)WebRequest.Create(Url);
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
        public static void GetAnnouncement()
        {
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
                    catch
                    {
                        Global.Ui.ShowBalloonTip("公告获取异常");
                    }
                }
                else
                {
                    OldAnnouncementId = "";
                }
                Thread.Sleep(60000);
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
                    catch
                    {
                        Global.Ui.ShowBalloonTip("更新获取异常");
                    }
                }
                else if (Global.Settings_serein.UpdateInfoType == 1)
                {
                    try
                    {
                        JObject JsonObject = (JObject)JsonConvert.DeserializeObject(RequestInfo("https://api.github.com/repos/Zaitonn/Serein/releases/latest"));
                        string Version = JsonObject["tag_name"].ToString();
                        if (!(string.IsNullOrEmpty(Version) && string.IsNullOrWhiteSpace(Version)) &&
                            Version != Global.VERSION && OldPreVersion != Version)
                        {
                            Global.Ui.ShowBalloonTip("发现新版本:\n" + Version);
                            OldPreVersion = Version;
                        }
                    }
                    catch
                    {
                        Global.Ui.ShowBalloonTip("更新获取异常");
                    }
                }
                else
                {
                    OldPreVersion = "";
                }
                Thread.Sleep(60000);
            }
        }
    }
}
