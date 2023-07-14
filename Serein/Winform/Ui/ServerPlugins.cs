using Serein.Core.Server;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace Serein.Ui
{
    public partial class Ui : Form
    {
        private void PluginContextMenuStripAdd_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new()
            {
                Filter = "所有文件|*.*",
                InitialDirectory = !string.IsNullOrEmpty(PluginManager.BasePath) && Directory.Exists(PluginManager.BasePath) ? PluginManager.BasePath : Global.PATH,
                Multiselect = true
            };
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                PluginManager.Add(dialog.FileNames);
            }
            LoadPlugins();
        }

        private void PluginContextMenuStripRemove_Click(object sender, EventArgs e)
        {
            PluginManager.Remove(GetSelectedPlugins());
            LoadPlugins();
        }

        private void PluginContextMenuStripRefresh_Click(object sender, EventArgs e)
            => LoadPlugins();

        private void PluginContextMenuStrip_Opening(object sender, CancelEventArgs e)
        {
            bool Available = !string.IsNullOrEmpty(PluginManager.BasePath);
            PluginContextMenuStripAdd.Enabled = Available;
            PluginContextMenuStripShow.Enabled = Available;
            PluginContextMenuStripEnable.Enabled = Available && PluginList.SelectedItems.Count > 0;
            PluginContextMenuStripDisable.Enabled = Available && PluginList.SelectedItems.Count > 0;
            PluginContextMenuStripRemove.Enabled = Available && PluginList.SelectedItems.Count > 0;
        }

        private void PluginContextMenuStripEnable_Click(object sender, EventArgs e)
        {
            PluginManager.Enable(GetSelectedPlugins());
            LoadPlugins();
        }

        private void PluginContextMenuStripDisable_Click(object sender, EventArgs e)
        {
            PluginManager.Disable(GetSelectedPlugins());
            LoadPlugins();
        }

        private void PluginContextMenuStripShow_Click(object sender, EventArgs e)
            => PluginManager.OpenFolder(PluginList.SelectedItems.Count > 0 ? GetSelectedPlugins()[0] : null);

        /// <summary>
        /// 加载插件列表
        /// </summary>
        public void LoadPlugins()
        {
            if (PluginManager.Get() != null)
            {
                PluginList.BeginUpdate();
                PluginList.Clear();
                string[] files = PluginManager.Get();
                ListViewGroup
                    pluginGroupJs = new("Js", HorizontalAlignment.Left),
                    pluginGroupDll = new("Dll", HorizontalAlignment.Left),
                    pluginGroupJar = new("Jar", HorizontalAlignment.Left),
                    pluginGroupPy = new("Py", HorizontalAlignment.Left),
                    pluginGroupLua = new("Lua", HorizontalAlignment.Left),
                    pluginGroupTs = new("Ts", HorizontalAlignment.Left),
                    pluginGroupDisable = new("已禁用", HorizontalAlignment.Left);

                PluginList.Groups.Add(pluginGroupJs);
                PluginList.Groups.Add(pluginGroupDll);
                PluginList.Groups.Add(pluginGroupJar);
                PluginList.Groups.Add(pluginGroupPy);
                PluginList.Groups.Add(pluginGroupLua);
                PluginList.Groups.Add(pluginGroupTs);
                PluginList.Groups.Add(pluginGroupDisable);
                foreach (string file in files)
                {
                    ListViewItem listViewItem = new(Regex.Replace(Path.GetFileName(file), @"\.lock$", string.Empty));
                    switch (Path.GetExtension(file.ToLowerInvariant()))
                    {
                        case ".js":
                            pluginGroupJs.Items.Add(listViewItem);
                            break;

                        case ".dll":
                            pluginGroupDll.Items.Add(listViewItem);
                            break;

                        case ".jar":
                            pluginGroupJar.Items.Add(listViewItem);
                            break;

                        case ".py":
                            pluginGroupPy.Items.Add(listViewItem);
                            break;

                        case ".lua":
                            pluginGroupLua.Items.Add(listViewItem);
                            break;

                        case ".ts":
                            pluginGroupTs.Items.Add(listViewItem);
                            break;

                        case ".lock":
                            listViewItem.ForeColor = Color.Gray;
                            pluginGroupDisable.Items.Add(listViewItem);
                            break;

                        default:
                            continue;
                    }
                    PluginList.Items.Add(listViewItem);
                }
                PluginList.EndUpdate();
            }
        }

        /// <summary>
        /// 获取选择的插件路径列表
        /// </summary>
        /// <returns>插件路径列表</returns>
        private List<string> GetSelectedPlugins()
        {
            List<string> files = new();
            foreach (ListViewItem listViewItem in PluginList.SelectedItems)
            {
                files.Add(PluginManager.BasePath + "\\" + listViewItem.Text + (listViewItem.ForeColor == Color.Gray ? ".lock" : string.Empty));
            }
            return files;
        }
    }
}
