using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Linq;
using System.Windows.Forms;
using System.Threading;

namespace Serein
{
    public partial class Ui : Form
    {
        private void PluginContextMenuStripAdd_Click(object sender, EventArgs e)
        {
            Plugins.Add();
            LoadPlugins();
        }
        private void PluginContextMenuStripRemove_Click(object sender, EventArgs e)
        {
            Plugins.Remove(PluginList.SelectedItems);
            LoadPlugins();
        }
        private void PluginContextMenuStripRefresh_Click(object sender, EventArgs e)
        {
            LoadPlugins();
        }
        private void PluginContextMenuStrip_Opening(object sender, CancelEventArgs e)
        {
            if (PluginList.SelectedItems.Count >= 1)
            {
                bool Mixed = false;
                bool Locked = false;
                foreach (ListViewItem file in PluginList.SelectedItems)
                {
                    if (file.ForeColor == System.Drawing.Color.Gray)
                    {
                        Locked = true;
                    }
                    else if (Locked)
                    {
                        Mixed = true;
                        break;
                    }
                }
                if (Mixed)
                {
                    PluginContextMenuStripEnable.Enabled = false;
                    PluginContextMenuStripDisable.Enabled = false;
                    PluginContextMenuStripRemove.Enabled = true;
                }
                else if (Locked)
                {
                    PluginContextMenuStripEnable.Enabled = true;
                    PluginContextMenuStripDisable.Enabled = false;
                    PluginContextMenuStripRemove.Enabled = true;
                }
                else
                {
                    PluginContextMenuStripEnable.Enabled = false;
                    PluginContextMenuStripDisable.Enabled = true;
                    PluginContextMenuStripRemove.Enabled = true;
                }
            }
            else
            {
                PluginContextMenuStripEnable.Enabled = false;
                PluginContextMenuStripDisable.Enabled = false;
                PluginContextMenuStripRemove.Enabled = false;
            }
        }
        private void PluginContextMenuStripEnable_Click(object sender, EventArgs e)
        {
            Plugins.Enable(PluginList.SelectedItems);
            LoadPlugins();
        }
        private void PluginContextMenuStripDisable_Click(object sender, EventArgs e)
        {
            Plugins.Disable(PluginList.SelectedItems);
            LoadPlugins();
        }
        public void LoadPlugins()
        {
            if (Plugins.Get() != null)
            {
                PluginList.BeginUpdate();
                PluginList.Clear();
                string[] Files = Plugins.Get();
                ListViewGroup PluginGroupJs = new ListViewGroup("Js", HorizontalAlignment.Left);
                ListViewGroup PluginGroupDll = new ListViewGroup("Dll", HorizontalAlignment.Left);
                ListViewGroup PluginGroupJar = new ListViewGroup("Jar", HorizontalAlignment.Left);
                ListViewGroup PluginGroupPy = new ListViewGroup("Py", HorizontalAlignment.Left);
                ListViewGroup PluginGroupGo = new ListViewGroup("Go", HorizontalAlignment.Left);
                ListViewGroup PluginGroupDisable = new ListViewGroup("已禁用", HorizontalAlignment.Left);
                PluginList.Groups.Add(PluginGroupJs);
                PluginList.Groups.Add(PluginGroupDll);
                PluginList.Groups.Add(PluginGroupJar);
                PluginList.Groups.Add(PluginGroupPy);
                PluginList.Groups.Add(PluginGroupGo);
                PluginList.Groups.Add(PluginGroupDisable);
                foreach (string PluginFile in Files)
                {
                    string PluginName = Path.GetFileName(PluginFile);
                    ListViewItem Item = new ListViewItem();
                    PluginName = Regex.Replace(PluginName, @"\.lock$", "");
                    Item.Text = PluginName;
                    bool added = true;
                    if (PluginFile.ToUpper().EndsWith(".JS"))
                    {
                        PluginGroupJs.Items.Add(Item);
                    }
                    else if (PluginFile.ToUpper().EndsWith(".DLL"))
                    {
                        PluginGroupDll.Items.Add(Item);
                    }
                    else if (PluginFile.ToUpper().EndsWith(".JAR"))
                    {
                        PluginGroupJar.Items.Add(Item);
                    }
                    else if (PluginFile.ToUpper().EndsWith(".PY"))
                    {
                        PluginGroupPy.Items.Add(Item);
                    }
                    else if (PluginFile.ToUpper().EndsWith(".GO"))
                    {
                        PluginGroupGo.Items.Add(Item);
                    }
                    else if (PluginFile.ToUpper().EndsWith(".LOCK"))
                    {
                        Item.ForeColor = System.Drawing.Color.Gray;
                        PluginGroupDisable.Items.Add(Item);
                    }
                    else
                    {
                        added = false;
                    }
                    if (added)
                    {
                        PluginList.Items.Add(Item);
                    }
                }
                PluginList.EndUpdate();
            }
        }
    }
}
