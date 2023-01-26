using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Serein.Extensions;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Timers;

namespace Serein.Base
{
    internal static class Net
    {
        /// <summary>
        /// 检查更新计时器
        /// </summary>
        private static readonly Timer CheckTimer = new(200000) { AutoReset = true };

        /// <summary>
        /// 在线统计计时器
        /// </summary>
        private static readonly Timer HeartbeatTimer = new(20000) { AutoReset = true };

        /// <summary>
        /// 开始网络计时器
        /// </summary>
        public static void Init()
        {
            CheckTimer.Elapsed += (_, _) => CheckVersion();
            CheckTimer.Start();
            Task.Run(() =>
            {
                10000.ToSleepFor();
                CheckVersion();
            });
            HeartbeatTimer.Elapsed += (_, _) => Get("http://count.ongsat.com/api/online/heartbeat?uri=127469ef347447698dd74c449881b877").GetAwaiter().GetResult();
            HeartbeatTimer.Start();
        }

        /// <summary>
        /// 异步Get
        /// </summary>
        /// <param name="url">链接</param>
        /// <param name="accept">Header - Accept</param>
        /// <param name="userAgent">Header - UserAgent</param>
        /// <returns>正文</returns>
        public static async Task<string> Get(string url, string accept = null, string userAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/102.0.5005.63 Safari/537.36 Edg/102.0.1245.33")
        {
            HttpClient httpClient = new HttpClient();
            if (accept != null)
            {
                httpClient.DefaultRequestHeaders.Accept.Clear();
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(accept));
            }
            httpClient.DefaultRequestHeaders.AcceptCharset.Clear();
            httpClient.DefaultRequestHeaders.AcceptCharset.Add(new StringWithQualityHeaderValue("UTF-8"));
            httpClient.DefaultRequestHeaders.Add("user-agent", userAgent);
            HttpResponseMessage response = await httpClient.GetAsync(url);
            Logger.Output(Items.LogType.DetailDebug, "Response Headers\n", response.Headers.ToString());
            Logger.Output(Items.LogType.DetailDebug, "Content\n", await response.Content.ReadAsStringAsync());
            httpClient.Dispose();
            return await response.Content.ReadAsStringAsync();
        }

        /// <summary>
        /// 上一个获取到的版本
        /// </summary>
        private static string LastVersion;

        /// <summary>
        /// 检查更新
        /// </summary>
        public static void CheckVersion()
        {
            if (Global.Settings.Serein.EnableGetUpdate)
            {
                try
                {
                    string JSON = Get("https://api.github.com/repos/Zaitonn/Serein/releases/latest", "application/vnd.github.v3+json", "Serein").GetAwaiter().GetResult();
                    string version = ((JObject)JsonConvert.DeserializeObject(JSON))["tag_name"].ToString();
                    if (LastVersion != version)
                    {
                        if (version != Global.VERSION)
                        {
                            Logger.Output(Items.LogType.Version_New, version);
                        }
                        else
                        {
                            Logger.Output(Items.LogType.Version_Latest, version);
                        }
                        LastVersion = version;
                    }
                }
                catch (Exception e)
                {
                    Logger.Output(Items.LogType.Version_Failure, e.Message);
                    Logger.Output(Items.LogType.Debug, e);
                }
            }
        }

    }
}
