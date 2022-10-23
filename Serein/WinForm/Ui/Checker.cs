using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace Serein.Ui
{
    internal static class Checker
    {
        private static string OldPreVersion = string.Empty;
        private static Timer _Timer = new Timer(100000) { AutoReset = true };

        public static void Start()
        {
            _Timer.Elapsed += (sender, e) => GetVersion();
            _Timer.Start();
            Task.Run(GetVersion);
        }

        private static void GetVersion()
        {
            if (Program.Ui != null && Program.Ui.Visible && Global.Settings.Serein.EnableGetUpdate)
            {
                try
                {
                    JObject JsonObject = (JObject)JsonConvert.DeserializeObject(RequestInfo("https://api.github.com/repos/Zaitonn/Serein/releases/latest", true));
                    string Version = JsonObject["tag_name"].ToString();
                    if (!(string.IsNullOrEmpty(Version) && string.IsNullOrWhiteSpace(Version)) &&
                        Version != Global.VERSION && OldPreVersion != Version &&
                        Global.VERSION.Contains(Version))
                    {
                        Program.Ui.ShowBalloonTip("发现新版本:\n" + Version);
                        Program.Ui.SettingSereinVersion_Update($"当前版本：{Global.VERSION} （发现新版本:{Version}，你可以点击下方链接获取最新版）");
                        OldPreVersion = Version;
                    }
                    else if (OldPreVersion != Version)
                    {
                        Program.Ui.ShowBalloonTip(
                            "获取更新成功\n" +
                            "当前已是最新版:)");
                        OldPreVersion = Version;
                        Program.Ui.SettingSereinVersion_Update($"当前版本：{Global.VERSION} （已是最新版qwq）");
                    }
                }
                catch (Exception e)
                {
                    Program.Ui.ShowBalloonTip("更新获取异常：\n" + e.Message);
                }
            }
            else
            {
                OldPreVersion = string.Empty;
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
            Request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/102.0.5005.63 Safari/537.36 Edg/102.0.1245.33";
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
