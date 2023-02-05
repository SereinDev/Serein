#if !CONSOLE
using Serein.Base;
using Serein.Utils;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;

namespace Serein.Core.Server
{
    internal static class PluginManager
    {
        /// <summary>
        /// 根路径
        /// </summary>
        public static string BasePath = string.Empty;

        /// <summary>
        /// 操作可用性
        /// </summary>
        public static bool Available
        {
            get
            {
                if (ServerManager.Status)
                {
                    Logger.MsgBox("服务器仍在运行中", "Serein", 0, 48);
                    return false;
                }
                return !(string.IsNullOrEmpty(BasePath) && !Directory.Exists(BasePath));
            }
        }

        private static string Check(string subPath)
            => Directory.Exists(Path.Combine(Path.GetDirectoryName(Global.Settings.Server.Path), subPath)) ?
            Path.Combine(Path.GetDirectoryName(Global.Settings.Server.Path), subPath) : null;

        /// <summary>
        /// 获取插件列表
        /// </summary>
        /// <returns>插件列表</returns>
        public static string[] Get()
        {
            if (File.Exists(Global.Settings.Server.Path))
            {
                BasePath = Check("plugin") ?? Check("plugins") ?? Check("mod") ?? Check("mods") ?? string.Empty;
                Logger.Output(LogType.Debug, BasePath);
                if (!string.IsNullOrWhiteSpace(BasePath))
                {
                    return Directory.GetFiles(BasePath, "*", SearchOption.TopDirectoryOnly);
                }
            }
            return null;
        }

        /// <summary>
        /// 导入插件
        /// </summary>
        /// <param name="files">文件列表</param>
        public static void Add(IList<string> files)
        {
            foreach (string filename in files)
            {
                try
                {
                    File.Copy(filename, Path.Combine(BasePath, Path.GetFileName(filename)));
                }
                catch (Exception e)
                {
                    Logger.MsgBox($"文件\"{filename}\" 导入失败\n{e.Message}", "Serein", 0, 48);
                    Logger.Output(LogType.Debug, e);
                    break;
                }
            }
        }

        /// <summary>
        /// 删除文件
        /// </summary>
        /// <param name="files">文件列表</param>
        public static void Remove(List<string> files)
        {
            if (Available)
            {
                if (files.Count <= 0)
                {
                    Logger.Output(LogType.Debug, "数据不合法");
                }
                else if (Logger.MsgBox($"确定删除\"{files[0]}\"{(files.Count > 1 ? $"等{files.Count}个文件" : string.Empty)}？\n它将会永远失去！（真的很久！）", "Serein", 1, 48))
                {
                    foreach (string file in files)
                    {
                        try
                        {
                            File.Delete(file);
                        }
                        catch (Exception e)
                        {
                            Logger.Output(LogType.Debug, e);
                            Logger.MsgBox(
                                $"文件\"{file}\"删除失败\n{e.Message}", "Serein",
                                0, 48
                            );
                            break;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 禁用插件
        /// </summary>
        /// <param name="Files">文件列表</param>
        public static void Disable(List<string> Files)
        {
            foreach (string file in Files)
            {
                try
                {
                    if (file.ToLowerInvariant().EndsWith(".lock"))
                    {
                        continue;
                    }
                    File.Move(file, file + ".lock");
                }
                catch (Exception e)
                {
                    Logger.Output(LogType.Debug, e);
                    Logger.MsgBox(
                        $"文件\"{file}\"禁用失败\n" +
                        $"{e.Message}", "Serein",
                        0, 48
                        );
                    break;
                }
            }
        }

        /// <summary>
        /// 启用插件
        /// </summary>
        /// <param name="files">文件列表</param>
        public static void Enable(List<string> files)
        {
            foreach (string file in files)
            {
                try
                {
                    File.Move(file, System.Text.RegularExpressions.Regex.Replace(file, @"\.lock$", string.Empty, System.Text.RegularExpressions.RegexOptions.IgnoreCase));
                }
                catch (Exception e)
                {
                    Logger.Output(LogType.Debug, e);
                    Logger.MsgBox(
                        $"文件\"{file}\"启用失败\n" +
                        $"{e.Message}", "Serein",
                        0, 48
                        );
                    break;
                }
            }
        }

        /// <summary>
        /// 获取相对路径
        /// </summary>
        /// <param name="file">文件路径</param>
        /// <returns>相对路径</returns>
        public static string GetRelativeUri(string file)
        {
            return WebUtility.UrlDecode(new Uri(BasePath).MakeRelativeUri(new Uri(file)).OriginalString);
        }

        /// <summary>
        /// 获取绝对路径
        /// </summary>
        /// <param name="file">文件路径</param>
        /// <returns>绝对路径</returns>
        public static string GetAbsoluteUri(string file)
        {
            return Path.Combine(Directory.GetParent(BasePath).FullName, file).Replace('/', '\\').TrimStart('\u202a');
        }

        /// <summary>
        /// 打开文件夹
        /// </summary>
        /// <param name="Path">路径</param>
        public static void OpenFolder(string Path = null)
        {
            if (Environment.OSVersion.Platform == PlatformID.Win32NT)
                Process.Start(new ProcessStartInfo("Explorer.exe")
                {
                    Arguments = !string.IsNullOrEmpty(Path)
                    ? $"/e,/select,\"{Path}\""
                    : $"/e,\"{BasePath}\""
                });
        }

        public static readonly List<string> AcceptableList = new() { ".py", ".dll", ".js", ".jar", ".lua", ".ts" };

        /// <summary>
        /// 尝试导入
        /// </summary>
        /// <param name="files">文件列表</param>
        /// <returns>导入结果</returns>
        public static bool TryImport(Array files)
        {
            List<string> fileList = new();
            List<string> filenameList = new();
            string filename;
            foreach (object file in files)
            {
                filename = file.ToString();
                if (string.IsNullOrEmpty(filename) && AcceptableList.Contains(Path.GetExtension(filename.ToLowerInvariant())))
                {
                    fileList.Add(filename);
                    filenameList.Add(Path.GetFileName(filename));
                }
            }
            if (fileList.Count > 0 && Logger.MsgBox($"是否将以下文件复制到插件文件夹内？\n{string.Join("\n", filenameList)}", "Serein", 1, 48))
            {
                Add(fileList);
                return true;
            }
            else if (fileList.Count == 0 && files.Length > 0)
            {
                Logger.MsgBox("无法识别所选文件", "Serein", 0, 48);
            }
            return false;
        }
    }
}
#endif
