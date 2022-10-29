using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
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
        private static Timer CheckTimer = new Timer(200000) { AutoReset = true };

        /// <summary>
        /// 开始检查更新
        /// </summary>
        public static void StartChecking()
        {
            CheckTimer.Elapsed += (sender, e) => CheckVersion();
            CheckTimer.Start();
            Task.Run(async delegate
            {
                await Task.Delay(10000);
                CheckVersion();
            });
        }

        /// <summary>
        /// 异步Get
        /// </summary>
        /// <param name="Url">链接</param>
        /// <param name="Accept">Header - Accept</param>
        /// <param name="UserAgent">Header - UserAgent</param>
        /// <returns>正文</returns>
        public static async Task<string> Get(string Url, string Accept = null, string UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/102.0.5005.63 Safari/537.36 Edg/102.0.1245.33")
        {
            HttpClient Client = new HttpClient();
            if (Accept != null)
            {
                Client.DefaultRequestHeaders.Accept.Clear();
                Client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(Accept));
            }
            Client.DefaultRequestHeaders.AcceptCharset.Clear();
            Client.DefaultRequestHeaders.AcceptCharset.Add(new StringWithQualityHeaderValue("UTF-8"));
            Client.DefaultRequestHeaders.Add("user-agent", UserAgent);
            HttpResponseMessage Response = await Client.GetAsync(Url);
            Logger.Out(Items.LogType.Debug, "[Net:Get() - Headers]", Response.Headers.ToString());
            Logger.Out(Items.LogType.Debug, "[Net:Get() - Content ]", await Response.Content.ReadAsStringAsync());
            return await Response.Content.ReadAsStringAsync();
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
                    JObject JsonObject = (JObject)JsonConvert.DeserializeObject(JSON);
                    string Version = JsonObject["tag_name"].ToString();
                    if (!(string.IsNullOrEmpty(Version) && string.IsNullOrWhiteSpace(Version)) && Version != Global.VERSION && LastVersion != Version)
                        Logger.Out(Items.LogType.Version_New, Version);
                    else if (LastVersion != Version)
                        Logger.Out(Items.LogType.Version_Latest, Version);
                    LastVersion = Version;
                }
                catch (Exception e)
                {
                    Logger.Out(Items.LogType.Version_Failure, e.Message);
                    Logger.Out(Items.LogType.Debug, "[Net:CheckVersion()]", e);
                }
            }
        }

    }
}