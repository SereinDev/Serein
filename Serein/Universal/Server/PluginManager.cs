using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows;
using Microsoft.Win32;

namespace Serein.Server
{
    internal partial class PluginManager
    {
        public static string PluginPath = string.Empty;
        public static bool Available = false;

        /// <summary>
        /// 获取插件列表
        /// </summary>
        /// <returns>插件列表</returns>
        public static string[] Get()
        {
            if (File.Exists(Global.Settings.Server.Path))
            {
                Available = true;
                if (Directory.Exists(Path.GetDirectoryName(Global.Settings.Server.Path) + "\\plugin"))
                {
                    PluginPath = Path.GetDirectoryName(Global.Settings.Server.Path) + "\\plugin";
                }
                else if (Directory.Exists(Path.GetDirectoryName(Global.Settings.Server.Path) + "\\plugins"))
                {
                    PluginPath = Path.GetDirectoryName(Global.Settings.Server.Path) + "\\plugins";
                }
                else if (Directory.Exists(Path.GetDirectoryName(Global.Settings.Server.Path) + "\\mod"))
                {
                    PluginPath = Path.GetDirectoryName(Global.Settings.Server.Path) + "\\mod";
                }
                else if (Directory.Exists(Path.GetDirectoryName(Global.Settings.Server.Path) + "\\mods"))
                {
                    PluginPath = Path.GetDirectoryName(Global.Settings.Server.Path) + "\\mods";
                }
                else
                {
                    PluginPath = Global.Path;
                    Available = false;
                    return null;
                }
                if (!string.IsNullOrWhiteSpace(PluginPath))
                {
                    string[] Files = Directory.GetFiles(PluginPath, "*", SearchOption.TopDirectoryOnly);
                    return Files;
                }
            }
            else
            {
                Available = false;
            }
            return null;
        }

        /// <summary>
        /// 导入插件
        /// </summary>
        public static void Add()
        {
            OpenFileDialog Dialog = new OpenFileDialog()
            {
                Filter = "所有文件|*.*",
                Multiselect = true
            };
            if (Dialog.ShowDialog() == true)
            {
                foreach (string FileName in Dialog.FileNames)
                {
                    try
                    {
                        File.Copy(FileName, PluginPath + "\\" + Path.GetFileName(FileName));
                    }
                    catch (Exception Exp)
                    {
                        MessageBox.Show(
                            $"文件\"{FileName}\"复制失败\n" +
                            $"详细原因：\n" +
                            $"{Exp.Message}", "Serein",
                            MessageBoxButton.OK, MessageBoxImage.Warning
                            );
                    }
                }
            }
        }

        /// <summary>
        /// 导入插件
        /// </summary>
        /// <param name="Files">文件列表</param>
        public static void Add(List<string> Files)
        {
            foreach (string FileName in Files)
            {
                try
                {
                    File.Copy(FileName, PluginPath + "\\" + Path.GetFileName(FileName));
                }
                catch (Exception Exp)
                {
                    MessageBox.Show(
                        $"文件\"{FileName}\"复制失败\n" +
                        $"详细原因：\n" +
                        $"{Exp.Message}", "Serein",
                        MessageBoxButton.OK, MessageBoxImage.Warning
                        );
                }
            }
        }
        
        /// <summary>
        /// 服务器状态检测
        /// </summary>
        /// <returns>服务器状态</returns>
        public static bool Check()
        {
            if (ServerManager.Status)
            {
                MessageBox.Show("服务器仍在运行中", "Serein", MessageBoxButton.OK, MessageBoxImage.Warning);
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
