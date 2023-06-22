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
        private static readonly Timer _checkTimer = new(200000) { AutoReset = true };

        /// <summary>
        /// 更新初始化
        /// </summary>
        public static void Init()
        {
            Task.Run(CheckVersion);
            _checkTimer.Elapsed += (_, _) => CheckVersion();
            _checkTimer.Start();
            AppDomain.CurrentDomain.ProcessExit += (_, _) => StartUpdater();
        }

        /// <summary>
        /// 更新准备
        /// </summary>
        public static bool IsReadyToUpdate { get; private set; }

        /// <summary>
        /// 上一个获取到的版本
        /// </summary>
        public static string? LastVersion { get; private set; }

        /// <summary>
        /// 检查更新
        /// </summary>
        public static void CheckVersion()
        {
            if (!Global.Settings.Serein.EnableGetUpdate)
            {
                return;
            }
            try
            {
                JObject jsonObject = ((JObject)JsonConvert.DeserializeObject(Net.Get("https://api.github.com/repos/Zaitonn/Serein/releases/latest", "application/vnd.github.v3+json", "Serein").Await().Content.ReadAsStringAsync().Await())!);
                string? version = jsonObject["tag_name"]?.ToString();
                if (LastVersion != version && !string.IsNullOrEmpty(version))
                {
                    LastVersion = version;
                    if (version != Global.VERSION)
                    {
                        Logger.Output(Base.LogType.Version_New, version);
                        if (Global.Settings.Serein.AutoUpdate)
                        {
                            DownloadNewVersion(jsonObject);
                        }
                    }
                    else
                    {
                        Logger.Output(Base.LogType.Version_Latest, version);
                    }
                }
            }
            catch (Exception e)
            {
                Logger.Output(Base.LogType.Version_Failure, e.Message);
                Logger.Output(Base.LogType.Debug, e);
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
            foreach (JToken asset in jobject["assets"]!)
            {
                string? filename = asset["name"]?.ToString();
                string? url = asset["browser_download_url"]?.ToString();

                if (string.IsNullOrEmpty(filename) ||
                    !IdentifyFile(filename?.ToLowerInvariant()) ||
                    string.IsNullOrEmpty(url))
                {
                    continue;
                }

                try
                {
                    if (File.Exists($"update/{filename}"))
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
                        Logger.Output(Base.LogType.Version_Downloading, url);
                        Logger.Output(Base.LogType.Debug, $"正在从[{url}]下载[{asset["name"]}]");
                        using (Stream stream = Net.Get(url!).Await().Content.ReadAsStreamAsync().Await())
                        using (FileStream fileStream = new($"update/{filename}", FileMode.Create))
                        {
                            byte[] bytes = new byte[stream.Length];
                            _ = stream.Read(bytes, 0, bytes.Length);
                            fileStream.Write(bytes, 0, bytes.Length);
                        }
                        Logger.Output(Base.LogType.Debug, "下载成功");
                    }

                    ZipFile.ExtractToDirectory($"update/{filename}", "update");
                    Logger.Output(Base.LogType.Debug, "解压成功");
                    IsReadyToUpdate = true;
                    Logger.Output(Base.LogType.Version_Ready, "新版本已下载完毕\n" + (Environment.OSVersion.Platform == PlatformID.Win32NT ? "重启即可自动更新" : "你可以自行打开“update”文件夹复制替换"));
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
        /// 识别文件
        /// </summary>
        private static bool IdentifyFile(string? name)
        {
            if (name?.Contains(Global.TYPE) ?? false)
            {
                string netVer = Environment.Version.Major.ToString();
                return
                    !(Environment.OSVersion.Platform == PlatformID.Unix ^ name.Contains("unix")) &&
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