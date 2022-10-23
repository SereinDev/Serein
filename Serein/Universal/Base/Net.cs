using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Serein.Base
{
    internal static class Net
    {
        public static async Task<string> Get(string Url, string Accept = null, string UserAgent = null)
        {
            HttpClient Client = new HttpClient();
            if (Accept != null)
            {
                Client.DefaultRequestHeaders.Accept.Clear();
                Client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(Accept));
            }
            Client.DefaultRequestHeaders.AcceptCharset.Clear();
            Client.DefaultRequestHeaders.AcceptCharset.Add(new StringWithQualityHeaderValue("UTF-8"));
            Client.DefaultRequestHeaders.UserAgent.Clear();
            Client.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue(
                UserAgent ??
                "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/102.0.5005.63 Safari/537.36 Edg/102.0.1245.33"
                ));
            return await Client.GetStringAsync(Url);
        }

        private static string OldVersion;

        private static void CheckVersion()
        {
            if (Global.Settings.Serein.EnableGetUpdate)
            {
                try
                {
                    string JSON = Get("https://api.github.com/repos/Zaitonn/Serein/releases/latest", "application/vnd.github.v3+json").GetAwaiter().GetResult();
                    JObject JsonObject = (JObject)JsonConvert.DeserializeObject(JSON);
                    string Version = JsonObject["tag_name"].ToString();
                    if (!(string.IsNullOrEmpty(Version) && string.IsNullOrWhiteSpace(Version)) &&
                        Version != Global.VERSION && OldVersion != Version)
                    {
                        Logger.Out(Items.LogType.Version_New, Version);
                    }
                    else if (OldVersion != Version)
                    {
                        Logger.Out(Items.LogType.Version_Latest, Version);
                    }
                    OldVersion = Version;
                }
                catch (Exception e)
                {
                    Logger.Out(Items.LogType.Version_Failure, e.Message);
                }
            }
        }

    }
}