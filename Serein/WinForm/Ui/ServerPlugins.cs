using Serein.Server;
using System;
using System.ComponentModel;
using System.Diagnostics;
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
            PluginManager.Add();
            LoadPlugins();
        }
        private void PluginContextMenuStripRemove_Click(object sender, EventArgs e)
        {
            var Items = PluginList.SelectedItems;
            if (Items.Count == 1 && !PluginManager.Check())
            {
                if ((int)MessageBox.Show(
                    $"确定删除\"{Items[0].Text}\"？\n" +
                    $"它将会永远失去！（真的很久！）", "Serein",
                    MessageBoxButtons.OKCancel, MessageBoxIcon.Information
                    ) == 1
                    && !PluginManager.Check())
                {
                    try
                    {
                        if (Items[0].ForeColor == Color.Gray)
                        {
                            File.Delete(PluginManager.PluginPath + "\\" + Items[0].Text + ".lock");
                        }
                        else
                        {
                            File.Delete(PluginManager.PluginPath + "\\" + Items[0].Text);
                        }
                    }
                    catch (Exception Exp)
                    {
                        MessageBox.Show(
                                $"文件\"{Items[0].Text}\"删除失败\n" +
                                $"详细原因：\n" +
                                $"{Exp.Message}", "Serein",
                                MessageBoxButtons.OK, MessageBoxIcon.Warning
                                );
                    }
                }
            }
            else if (Items.Count > 1 && !PluginManager.Check())
            {
                if ((int)MessageBox.Show(
                   $"确定删除\"{Items[0].Text}\"等{Items.Count}个文件？\n" +
                   $"它将会永远失去！（真的很久！）", "Serein",
                   MessageBoxButtons.OKCancel, MessageBoxIcon.Information
                   ) == 1
                   && !PluginManager.Check())
                {
                    foreach (ListViewItem Item in Items)
                    {
                        try
                        {
                            if (Item.ForeColor == Color.Gray)
                            {
                                File.Delete(PluginManager.PluginPath + "\\" + Item.Text + ".lock");
                            }
                            else
                            {
                                File.Delete(PluginManager.PluginPath + "\\" + Item.Text);
                            }
                        }
                        catch (Exception Exp)
                        {
                            MessageBox.Show(
                                    $"文件\"{Item.Text}\"删除失败\n" +
                                    $"详细原因：\n" +
                                    $"{Exp.Message}", "Serein",
                                    MessageBoxButtons.OK, MessageBoxIcon.Warning
                                    );
                        }
                    }
                }
            }
            LoadPlugins();
        }

        private void PluginContextMenuStripRefresh_Click(object sender, EventArgs e)
        {
            LoadPlugins();
        }

        private void PluginContextMenuStrip_Opening(object sender, CancelEventArgs e)
        {
            if (!PluginManager.Available)
            {
                PluginContextMenuStripAdd.Enabled = false;
                PluginContextMenuStripShow.Enabled = false;
                PluginContextMenuStripEnable.Enabled = false;
                PluginContextMenuStripDisable.Enabled = false;
                PluginContextMenuStripRemove.Enabled = false;
            }
            else
            {
                PluginContextMenuStripAdd.Enabled = true;
                PluginContextMenuStripShow.Enabled = true;
            }
            if (PluginList.SelectedItems.Count >= 1)
            {
                bool Mixed = false;
                bool Locked = false;
                foreach (ListViewItem file in PluginList.SelectedItems)
                {
                    if (file.ForeColor == Color.Gray)
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

            if (!PluginManager.Check())
            {
                foreach (ListViewItem Item in PluginList.SelectedItems)
                {
                    try
                    {
                        FileInfo RenamedFile = new FileInfo(PluginManager.PluginPath + "\\" + Item.Text);
                        RenamedFile.MoveTo(PluginManager.PluginPath + "\\" + Item.Text + ".lock");
                    }
                    catch (Exception Exp)
                    {
                        MessageBox.Show(
                            $"文件\"{Item.Text}\"禁用失败\n" +
                            $"详细原因：\n" +
                            $"{Exp.Message}", "Serein",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning
                            );
                    }
                }
            }
            LoadPlugins();
        }

        private void PluginContextMenuStripDisable_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem Item in PluginList.SelectedItems)
            {
                try
                {
                    FileInfo RenamedFile = new FileInfo(PluginManager.PluginPath + "\\" + Item.Text + ".lock");
                    RenamedFile.MoveTo(PluginManager.PluginPath + "\\" + Item.Text);
                }
                catch (Exception Exp)
                {
                    MessageBox.Show(
                                    $"文件\"{Item.Text}\"禁用失败\n" +
                                    $"详细原因：\n" +
                                    $"{Exp.Message}", "Serein",
                                    MessageBoxButtons.OK, MessageBoxIcon.Warning
                                    );
                }
            }
            LoadPlugins();
        }

        private void PluginContextMenuStripShow_Click(object sender, EventArgs e)
        {
            ProcessStartInfo psi = new ProcessStartInfo("Explorer.exe")
            {
                Arguments = PluginList.SelectedItems.Count >= 1
                ? PluginList.SelectedItems[0].ForeColor == Color.Gray
                    ? $"/e,/select,\"{PluginManager.PluginPath}\\{PluginList.SelectedItems[0].Text}.lock\""
                    : $"/e,/select,\"{PluginManager.PluginPath}\\{PluginList.SelectedItems[0].Text}\""
                : $"/e,\"{PluginManager.PluginPath}\""
            };
            Process.Start(psi);
        }

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
                ListViewGroup PluginGroupGo = new ListViewGroup("Go", HorizontalAlignment.Left);
                ListViewGroup PluginGroupDisable = new ListViewGroup("已禁用", HorizontalAlignment.Left);
                PluginList.Groups.Add(PluginGroupJs);
                PluginList.Groups.Add(PluginGroupDll);
                PluginList.Groups.Add(PluginGroupJar);
                PluginList.Groups.Add(PluginGroupPy);
                PluginList.Groups.Add(PluginGroupLua);
                PluginList.Groups.Add(PluginGroupGo);
                PluginList.Groups.Add(PluginGroupDisable);
                foreach (string PluginFile in Files)
                {
                    string PluginName = Path.GetFileName(PluginFile);
                    ListViewItem Item = new ListViewItem();
                    PluginName = Regex.Replace(PluginName, @"\.lock$", string.Empty);
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
                    else if (PluginFile.ToUpper().EndsWith(".LUA"))
                    {
                        PluginGroupLua.Items.Add(Item);
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
