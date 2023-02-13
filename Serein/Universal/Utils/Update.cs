using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Serein.Extensions;
using Serein.Properties;
using System;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Threading.Tasks;
using System.Timers;

namespace Serein.Utils
{
    internal static class Update
    {
        /// <summary>
        /// 检查更新计时器
        /// </summary>
        private static readonly Timer CheckTimer = new(200000) { AutoReset = true };

        /// <summary>
        /// 更新初始化
        /// </summary>
        public static void Init()
        {
            Task.Run(CheckVersion);
            CheckTimer.Elapsed += (_, _) => CheckVersion();
            CheckTimer.Start();
            AppDomain.CurrentDomain.ProcessExit += (_, _) => StartUpdater();
        }

        /// <summary>
        /// 更新准备
        /// </summary>
        public static bool IsReadyToUpdate { get; private set; }

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
                    JObject jsonObject = ((JObject)JsonConvert.DeserializeObject(Net.Get("https://api.github.com/repos/Zaitonn/Serein/releases/latest", "application/vnd.github.v3+json", "Serein").GetAwaiter().GetResult().Content.ReadAsStringAsync().GetAwaiter().GetResult()));
                    string version = jsonObject.TryGetString("tag_name");
                    if (LastVersion != version && !string.IsNullOrEmpty(version))
                    {
                        if (version != Global.VERSION)
                        {
                            Logger.Output(Base.LogType.Version_New, version);
                            DownloadNewVersion(jsonObject);
                        }
                        else
                        {
                            Logger.Output(Base.LogType.Version_Latest, version);
                        }
                        LastVersion = version;
                    }
                }
                catch (Exception e)
                {
                    Logger.Output(Base.LogType.Version_Failure, e.Message);
                    Logger.Output(Base.LogType.Debug, e);
                }
            }
        }

        /// <summary>
        /// 下载新版本
        /// </summary>
        private static void DownloadNewVersion(JObject jobject)
        {
            IO.CreateDirectory("update");
            foreach (string file in Directory.GetFiles("update", "*.*", SearchOption.AllDirectories))
            {
                if (Path.GetExtension(file.ToLowerInvariant()) != ".zip")
                {
                    File.Delete(file);
                }
            }
            foreach (JToken asset in jobject["assets"])
            {
                string filename = asset.TryGetString("name");
                if (!IsFileMatched(filename.ToLowerInvariant()) || string.IsNullOrEmpty(asset.TryGetString("browser_download_url"))) { continue; }
                try
                {
                    if (File.Exists($"update/{asset.TryGetString("name")}"))
                    {
                        IsReadyToUpdate = false;
                        Logger.Output(Base.LogType.Debug, "文件已存在，自动跳过下载");
                    }
                    else if (IsReadyToUpdate)
                    {
                        break;
                    }
                    else
                    {
                        IsReadyToUpdate = false;
                        Logger.Output(Base.LogType.Version_Downloading, asset.TryGetString("browser_download_url"));
                        Logger.Output(Base.LogType.Debug, $"正在从[{asset.TryGetString("browser_download_url")}]下载[{asset.TryGetString("name")}]");
                        using (Stream stream = Net.Get(asset.TryGetString("browser_download_url")).GetAwaiter().GetResult().Content.ReadAsStreamAsync().GetAwaiter().GetResult())
                        using (FileStream fileStream = new($"update/{asset.TryGetString("name")}", FileMode.Create))
                        {
                            byte[] bytes = new byte[stream.Length];
                            _ = stream.Read(bytes, 0, bytes.Length);
                            fileStream.Write(bytes, 0, bytes.Length);
                        }
                        Logger.Output(Base.LogType.Debug, "下载成功");
                    }
                    ZipFile.ExtractToDirectory($"update/{asset.TryGetString("name")}", "update");
                    Logger.Output(Base.LogType.Debug, "解压成功");
                    IsReadyToUpdate = true;
                    Logger.Output(Base.LogType.Version_Ready, "新版本已下载完毕\n" + (Global.Settings.Serein.AutoUpdate && Environment.OSVersion.Platform == PlatformID.Win32NT ? "重启即可自动更新" : "你可以自行打开“update”文件夹复制替换"));
                }
                catch (Exception e)
                {
                    Logger.Output(Base.LogType.Version_DownloadFailed, e.Message);
                    Logger.Output(Base.LogType.Debug, e);
                }
                break;
            }
        }

        /// <summary>
        /// 是否为当前匹配的版本
        /// </summary>
        private static bool IsFileMatched(string name)
        {
            if (name.Contains(Global.TYPE))
            {
                string netVer = Environment.Version.Major.ToString();
                return !(Environment.OSVersion.Platform == PlatformID.Unix ^ name.Contains("unix")) &&
                    !(netVer == "4" ^ name.Contains("dotnetframework472")) &&
                    !(netVer == "6" ^ name.Contains("dotnet6"));
            }
            return false;
        }

        /// <summary>
        /// 启动 Updater.exe
        /// </summary>
        public static void StartUpdater()
        {
            if (!Global.Settings.Serein.AutoUpdate || !IsReadyToUpdate || Environment.OSVersion.Platform != PlatformID.Win32NT)
            {
                return;
            }
            if (!File.Exists("Updater.exe"))
            {
                using (FileStream fileStream = new("Updater.exe", FileMode.Create))
                {
                    fileStream.Write(Resources.Updater, 0, Resources.Updater.Length);
                }
            }
            Process.Start(new ProcessStartInfo("Updater.exe")
            {
                WorkingDirectory = Global.PATH,
                UseShellExecute = false,
            });
        }
    }
}