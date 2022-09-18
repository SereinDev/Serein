using System.IO;
using System.Net;
using System.Text;
using System.Timers;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;

namespace Serein.Windows
{
    internal static class Checker
    {
        private static Timer VersionTimer = new Timer(200000) { AutoReset = true };
        private static string OldVersion;

        public static void Start()
        {
            VersionTimer.Elapsed += (sender, e) => CheckVersion();
            VersionTimer.Start();
            CheckVersion();
        }

        private static void CheckVersion()
        {
            if (Global.Settings.Serein.EnableGetUpdate)
            {
                try
                {
                    JObject JsonObject = (JObject)JsonConvert.DeserializeObject(RequestInfo("https://api.github.com/repos/Zaitonn/Serein/releases/latest", true));
                    string Version = JsonObject["tag_name"].ToString();
                    if (!(string.IsNullOrEmpty(Version) && string.IsNullOrWhiteSpace(Version)) &&
                        Version != Global.VERSION && OldVersion != Version &&
                        Global.VERSION.Contains(Version))
                    {
                        Window.Notification.Show("Serein", "发现新版本:\n" + Version);
                        Window.Settings.Serein?.UpdateVersion($"（发现新版本:{Version}，你可以点击下方链接获取最新版）");
                        OldVersion = Version;
                    }
                    else if (OldVersion != Version)
                    {
                        Window.Notification.Show(
                            "Serein",
                            "获取更新成功\n" +
                            "当前已是最新版:)");
                        OldVersion = Version;
                        Window.Settings.Serein?.UpdateVersion("（已是最新版qwq）");
                    }
                }
                catch (Exception e)
                {
                    Window.Notification.Show("Serein", "更新获取异常：\n" + e.Message);
                }
            }
        }

        public static string RequestInfo(string Url, bool isApi = false)
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            Stream ResponseStream;
            StreamReader Reader;
            HttpWebRequest Request = (HttpWebRequest)WebRequest.Create(Url);
            Request.KeepAlive = false;
            Request.ProtocolVersion = HttpVersion.Version10;
            Request.Method = "GET";
            Request.ContentType = "text/html;charset=UTF-8";
            Request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/102.0.5005.63 Safari/537.36 Edg/102.0.1245.33";
            Request.Accept = isApi ? Request.Accept : "application/vnd.github.v3+json";
            HttpWebResponse Response = (HttpWebResponse)Request.GetResponse();
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
