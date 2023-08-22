using Serein.Core.JSPlugin;
using System;
using System.IO;
using System.Timers;

#if WPF
using Serein.Windows;
#endif

namespace Serein.Utils.IO
{
    internal static class FileSaver
    {
#if !CONSOLE
        /// <summary>
        /// 保存更新设置计时器
        /// </summary>
        public static readonly Timer Timer = new(2000) { AutoReset = true };
#endif

        /// <summary>
        /// 懒惰计时器
        /// </summary>
        public static readonly Timer LazyTimer = new(60000) { AutoReset = true };

        /// <summary>
        /// 启动保存和更新设置定时器
        /// </summary>
        public static void StartSaving()
        {
#if !CONSOLE
            Timer.Elapsed += (_, _) => Setting.SaveSettings();
            Timer.Start();
#endif
            LazyTimer.Elapsed += (_, _) => Data.SaveMember();
            LazyTimer.Elapsed += (_, _) => Data.SaveGroupCache();
            LazyTimer.Elapsed += (_, _) => Data.SavePermissionGroups();
            LazyTimer.Start();
        }

        /// <summary>
        /// 读取所有文件
        /// </summary>
        public static void ReadAll()
        {
            Data.ReadRegex();
            Data.ReadMember();
            if (File.Exists(Path.Combine("data", "task.json")) && !File.Exists(Path.Combine("data", "schedule.json")))
            {
                Data.ReadSchedule(Path.Combine("data", "task.json"));
                File.Delete(Path.Combine("data", "task.json"));
            }
            else
            {
                Data.ReadSchedule();
            }
            Data.ReadGroupCache();
            Data.ReadPermissionGroups();
            Setting.ReadSettings();
            Setting.SaveSettings();
        }


        /// <summary>
        /// 热重载
        /// </summary>
        /// <param name="type">类型</param>
        /// <param name="allowToReloadPlugin">允许重新加载插件</param>
        public static void Reload(string type, bool allowToReloadPlugin = false)
        {
            switch (type?.ToLowerInvariant())
            {
                case null:
                case "all":
                    ReadAll();
                    if (allowToReloadPlugin)
                    {
                        JSPluginManager.Reload();
                    }
#if WINFORM
                    Program.Ui?.Invoke(Program.Ui.LoadRegex);
                    Program.Ui?.Invoke(Program.Ui.LoadMember);
                    Program.Ui?.Invoke(Program.Ui.LoadSchedule);
                    Program.Ui?.Invoke(Program.Ui.LoadSettings);
                    if (allowToReloadPlugin)
                    {
                        Program.Ui?.Invoke(Program.Ui.LoadJSPluginPublicly);
                    }
#elif WPF
                    Catalog.Function.Regex?.Dispatcher.Invoke(Catalog.Function.Regex.Load);
                    Catalog.Function.Member?.Dispatcher.Invoke(Catalog.Function.Member.Load);
                    Catalog.Function.Schedule?.Dispatcher.Invoke(Catalog.Function.Schedule.Load);
                    if (allowToReloadPlugin)
                    {
                        Catalog.Function.JSPlugin?.LoadPublicly();
                    }
#endif
                    break;
                case "regex":
                    Data.ReadRegex();
#if WINFORM
                    Program.Ui?.Invoke(Program.Ui.LoadRegex);
#elif WPF
                    Catalog.Function.Regex?.Dispatcher.Invoke(Catalog.Function.Regex.Load);
#endif
                    break;
                case "member":
                    Data.ReadMember();
#if WINFORM
                    Program.Ui?.Invoke(Program.Ui.LoadMember);
#elif WPF
                    Catalog.Function.Member?.Dispatcher.Invoke(Catalog.Function.Member.Load);
#endif
                    break;
                case "schedule":
                    Data.ReadSchedule();
#if WINFORM
                    Program.Ui?.Invoke(Program.Ui.LoadSchedule);
#elif WPF
                    Catalog.Function.Schedule?.Dispatcher.Invoke(Catalog.Function.Schedule.Load);
#endif
                    break;
                case "groupcache":
                    Data.ReadGroupCache();
                    break;
                case "settings":
                    Setting.ReadSettings();
#if WINFORM
                    Program.Ui?.Invoke(Program.Ui.LoadSettings);
#endif
                    break;
                case "permissiongroup":
                    Data.ReadPermissionGroups();
                    break;
                case "plugin":
                case "plugins":
                    if (allowToReloadPlugin)
                    {
                        JSPluginManager.Reload();
#if WINFORM
                        Program.Ui?.Invoke(Program.Ui.LoadJSPluginPublicly);
#elif WPF
                        Catalog.Function.JSPlugin?.LoadPublicly();
#endif
                        break;
                    }
                    throw new InvalidOperationException("权限不足");
                default:
                    throw new ArgumentException("重新加载类型未知");
            }
        }

    }
}