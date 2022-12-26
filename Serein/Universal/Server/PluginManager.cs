using Serein.Base;
using Serein.Items;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;

namespace Serein.Server
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
                else if (string.IsNullOrEmpty(BasePath) && Directory.Exists(BasePath))
                {
                    return false;
                }
                else
                    return true;
            }
        }

        private static string Check(string SubPath)
            => Directory.Exists(Path.Combine(Path.GetDirectoryName(Global.Settings.Server.Path), SubPath)) ?
            Path.Combine(Path.GetDirectoryName(Global.Settings.Server.Path), SubPath) : null;

        /// <summary>
        /// 获取插件列表
        /// </summary>
        /// <returns>插件列表</returns>
        public static string[] Get()
        {
            if (File.Exists(Global.Settings.Server.Path))
            {
                BasePath = Check("plugin") ?? Check("plugins") ?? Check("mod") ?? Check("mods") ?? string.Empty;
                Logger.Out(LogType.Debug, BasePath);
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
        /// <param name="Files">文件列表</param>
        public static void Add(IList<string> Files)
        {
            foreach (string FileName in Files)
            {
                try
                {
                    File.Copy(FileName, Path.Combine(BasePath, Path.GetFileName(FileName)));
                }
                catch (Exception e)
                {
                    Logger.MsgBox($"文件\"{FileName}\" 导入失败\n{e.Message}", "Serein", 0, 48);
                    Logger.Out(LogType.Debug, e);
                    break;
                }
            }
        }

        /// <summary>
        /// 删除文件
        /// </summary>
        /// <param name="Files">文件列表</param>
        public static void Remove(List<string> Files)
        {
            if (Available)
            {
                if (Files.Count <= 0)
                {
                    Logger.Out(LogType.Debug, "数据不合法");
                }
                else if (Logger.MsgBox($"确定删除\"{Files[0]}\"{(Files.Count > 1 ? $"等{Files.Count}个文件" : string.Empty)}？\n它将会永远失去！（真的很久！）", "Serein", 1, 48))
                {
                    foreach (string FileName in Files)
                    {
                        try
                        {
                            File.Delete(FileName);
                        }
                        catch (Exception e)
                        {
                            Logger.Out(LogType.Debug, e);
                            Logger.MsgBox(
                                $"文件\"{FileName}\"删除失败\n{e.Message}", "Serein",
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
            foreach (string FileName in Files)
            {
                try
                {
                    if (FileName.ToLower().EndsWith(".lock"))
                    {
                        continue;
                    }
                    File.Move(FileName, FileName + ".lock");
                }
                catch (Exception e)
                {
                    Logger.Out(LogType.Debug, e);
                    Logger.MsgBox(
                        $"文件\"{FileName}\"禁用失败\n" +
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
        /// <param name="Files">文件列表</param>
        public static void Enable(List<string> Files)
        {
            foreach (string FileName in Files)
            {
                try
                {
                    File.Move(FileName, System.Text.RegularExpressions.Regex.Replace(FileName, @"\.lock$", string.Empty, System.Text.RegularExpressions.RegexOptions.IgnoreCase));
                }
                catch (Exception e)
                {
                    Logger.Out(LogType.Debug, e);
                    Logger.MsgBox(
                        $"文件\"{FileName}\"启用失败\n" +
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
        /// <param name="File">文件路径</param>
        /// <returns>相对路径</returns>
        public static string GetRelativeUri(string File)
        {
            return WebUtility.UrlDecode(new Uri(BasePath).MakeRelativeUri(new Uri(File)).OriginalString);
        }

        /// <summary>
        /// 获取绝对路径
        /// </summary>
        /// <param name="File">文件路径</param>
        /// <returns>绝对路径</returns>
        public static string GetAbsoluteUri(string File)
        {
            return Path.Combine(Directory.GetParent(BasePath).FullName, File).Replace('/', '\\').TrimStart('\u202a');
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
    }
}
