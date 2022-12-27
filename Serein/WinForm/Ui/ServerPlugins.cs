using Serein.Server;
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
            OpenFileDialog Dialog = new OpenFileDialog
            {
                Filter = "所有文件|*.*",
                Multiselect = true
            };
            if (Dialog.ShowDialog() == DialogResult.OK)
            {
                PluginManager.Add(Dialog.FileNames);
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
        private void LoadPlugins()
        {
            if (PluginManager.Get() != null)
            {
                PluginList.BeginUpdate();
                PluginList.Clear();
                string[] Files = PluginManager.Get();
                ListViewGroup PluginGroupJs = new ListViewGroup("Js", HorizontalAlignment.Left);
                ListViewGroup PluginGroupDll = new ListViewGroup("Dll", HorizontalAlignment.Left);
                ListViewGroup PluginGroupJar = new ListViewGroup("Jar", HorizontalAlignment.Left);
                ListViewGroup PluginGroupPy = new ListViewGroup("Py", HorizontalAlignment.Left);
                ListViewGroup PluginGroupLua = new ListViewGroup("Lua", HorizontalAlignment.Left);
                ListViewGroup PluginGroupTs = new ListViewGroup("Ts", HorizontalAlignment.Left);
                ListViewGroup PluginGroupDisable = new ListViewGroup("已禁用", HorizontalAlignment.Left);
                PluginList.Groups.Add(PluginGroupJs);
                PluginList.Groups.Add(PluginGroupDll);
                PluginList.Groups.Add(PluginGroupJar);
                PluginList.Groups.Add(PluginGroupPy);
                PluginList.Groups.Add(PluginGroupLua);
                PluginList.Groups.Add(PluginGroupTs);
                PluginList.Groups.Add(PluginGroupDisable);
                foreach (string PluginFile in Files)
                {
                    ListViewItem Item = new ListViewItem
                    {
                        Text = Regex.Replace(Path.GetFileName(PluginFile), @"\.lock$", string.Empty)
                    };
                    switch (Path.GetExtension(PluginFile.ToLower()))
                    {
                        case ".js":
                            PluginGroupJs.Items.Add(Item);
                            break;
                        case ".dll":
                            PluginGroupDll.Items.Add(Item);
                            break;
                        case ".jar":
                            PluginGroupJar.Items.Add(Item);
                            break;
                        case ".py":
                            PluginGroupPy.Items.Add(Item);
                            break;
                        case ".lua":
                            PluginGroupLua.Items.Add(Item);
                            break;
                        case ".ts":
                            PluginGroupTs.Items.Add(Item);
                            break;
                        case ".lock":
                            Item.ForeColor = Color.Gray;
                            PluginGroupDisable.Items.Add(Item);
                            break;
                        default:
                            continue;
                    }
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
            List<string> Files = new List<string>();
            foreach (ListViewItem Item in PluginList.SelectedItems)
            {
                Files.Add(PluginManager.BasePath + "\\" + Item.Text + (Item.ForeColor == Color.Gray ? ".lock" : string.Empty));
            }
            return Files;
        }
    }
}
