using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Serein.Extensions;
using System;
using System.IO;
using System.IO.Compression;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Timers;

namespace Serein.Base
{
    internal static class Net
    {
        /// <summary>
        /// Http客户端
        /// </summary>
        private static HttpClient HttpClient = new HttpClient();

        /// <summary>
        /// 检查更新计时器
        /// </summary>
        private static readonly Timer CheckTimer = new(200000) { AutoReset = true };

        /// <summary>
        /// 在线统计计时器
        /// </summary>
        private static readonly Timer HeartbeatTimer = new(20000) { AutoReset = true };

        /// <summary>
        /// 更新准备
        /// </summary>
        public static bool IsReadyToUpdate { get; private set; }

        /// <summary>
        /// 开始网络计时器
        /// </summary>
        public static void Init()
        {
            Task.Run(() =>
            {
                10000.ToSleepFor();
                CheckVersion();
            });
            CheckTimer.Elapsed += (_, _) => CheckVersion();
            CheckTimer.Start();
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
        public static async Task<HttpResponseMessage> Get(string url, string accept = null, string userAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/102.0.5005.63 Safari/537.36 Edg/102.0.1245.33")
        {
            if (accept != null)
            {
                HttpClient.DefaultRequestHeaders.Accept.Clear();
                HttpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(accept));
            }
            HttpClient.DefaultRequestHeaders.AcceptCharset.Clear();
            HttpClient.DefaultRequestHeaders.AcceptCharset.Add(new StringWithQualityHeaderValue("UTF-8"));
            HttpClient.DefaultRequestHeaders.Add("user-agent", userAgent);
            HttpResponseMessage response = await HttpClient.GetAsync(url);
            Logger.Output(Items.LogType.DetailDebug, "Response Headers\n", response.Headers.ToString());
            Logger.Output(Items.LogType.DetailDebug, "Content\n", await response.Content.ReadAsStringAsync());
            return response;
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
                    JObject jsonObject = ((JObject)JsonConvert.DeserializeObject(Get("https://api.github.com/repos/Zaitonn/Serein/releases/latest", "application/vnd.github.v3+json", "Serein").GetAwaiter().GetResult().Content.ReadAsStringAsync().GetAwaiter().GetResult()));
                    string version = jsonObject.TryGetString("tag_name");
                    if (LastVersion != version)
                    {
                        if (version != Global.VERSION)
                        {
                            Logger.Output(Items.LogType.Version_New, version);
                            DownloadNewVersion(jsonObject);
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

        /// <summary>
        /// 下载新版本
        /// </summary>
        private static void DownloadNewVersion(JObject jobject)
        {
            IO.CreateDirectory("cache");
            foreach (string file in Directory.GetFiles("cache", "*.*", SearchOption.AllDirectories))
            {
                if (Path.GetExtension(file.ToLowerInvariant()) != ".zip")
                {
                    File.Delete(file);
                }
            }
            string filename;
            foreach (JToken asset in jobject["assets"])
            {
                filename = asset.TryGetString("name");
                if (IsFileMatched(filename.ToLowerInvariant()) && !string.IsNullOrEmpty(asset.TryGetString("browser_download_url")))
                {
                    try
                    {
                        if (File.Exists($"cache/{asset.TryGetString("name")}"))
                        {
                            Logger.Output(Items.LogType.Debug, "文件已存在，自动跳过下载");
                        }
                        else if (IsReadyToUpdate)
                        {
                            break;
                        }
                        else
                        {
                            IsReadyToUpdate = false;
                            Logger.Output(Items.LogType.Debug, $"正在从[{asset.TryGetString("browser_download_url")}]下载[{asset.TryGetString("name")}]");
                            using (Stream stream = Get(asset.TryGetString("browser_download_url")).GetAwaiter().GetResult().Content.ReadAsStreamAsync().GetAwaiter().GetResult())
                            using (FileStream fileStream = new($"cache/{asset.TryGetString("name")}", FileMode.Create))
                            {
                                byte[] bytes = new byte[stream.Length];
                                stream.Read(bytes, 0, bytes.Length);
                                fileStream.Write(bytes, 0, bytes.Length);
                            }
                            Logger.Output(Items.LogType.Debug, "下载成功");
                        }
                        ZipFile.ExtractToDirectory($"cache/{asset.TryGetString("name")}", "cache");
                        Logger.Output(Items.LogType.Debug, "解压成功");
                        IsReadyToUpdate = true;
                    }
                    catch (Exception e)
                    {
                        Logger.Output(Items.LogType.Debug, e);
                    }
                    break;
                }
            }
        }

        /// <summary>
        /// 是否为当前匹配的版本
        /// </summary>
        private static bool IsFileMatched(string name)
        {
#if CONSOLE
            if (name.Contains("console"))
#elif WINFORM
            if (name.Contains("winform"))
#elif WPF
            if (name.Contains("wpf"))
#else
            if (false)
#endif
            {
                string netVer = Environment.Version.Major.ToString();
                return !(Environment.OSVersion.Platform == PlatformID.Unix ^ name.Contains("unix")) &&
                    !(netVer == "4" ^ name.Contains("dotnetframework472")) &&
                    !(netVer == "6" ^ name.Contains("dotnet6"));

            }
            return false;
        }
    }
}
