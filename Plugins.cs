using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace Serein
{
    partial class Plugins
    {
        public static string PluginPath = "";
        public static string[] Get()
        {
            if (File.Exists(Global.Settings_server.Path))
            {
                if (Directory.Exists(Path.GetDirectoryName(Global.Settings_server.Path) + "\\plugin"))
                {
                    PluginPath = Path.GetDirectoryName(Global.Settings_server.Path) + "\\plugin";
                }
                else if (Directory.Exists(Path.GetDirectoryName(Global.Settings_server.Path) + "\\plugins"))
                {
                    PluginPath = Path.GetDirectoryName(Global.Settings_server.Path) + "\\plugins";
                }
                else
                {
                    PluginPath = "";
                }
                if (PluginPath != "")
                {
                    string[] Files = Directory.GetFiles(PluginPath, "*", SearchOption.TopDirectoryOnly);

                    return Files;
                }
            }
            return null;
        }
        public static void Add()
        {
            OpenFileDialog Dialog = new OpenFileDialog
            {
                Filter = "所有文件|*.*",
                Multiselect = true
            };
            if (Dialog.ShowDialog() == DialogResult.OK)
            {
                foreach (string AddedFile in Dialog.FileNames)
                {
                    try
                    {
                        File.Copy(AddedFile, PluginPath + "\\" + Path.GetFileName(AddedFile));
                    }
                    catch (Exception Exp)
                    {
                        MessageBox.Show(
                            $"文件\"{AddedFile}\"复制失败\n" +
                            $"详细原因：\n" +
                            $"{Exp.Message}", "Serein",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning
                            );
                    }
                }
            }
        }
        public static void Remove(ListView.SelectedListViewItemCollection Items)
        {
            if (Items.Count == 1 && !Check())
            {
                if ((int)MessageBox.Show(
                    $"确定删除\"{Items[0].Text}\"？\n" +
                    $"他将会永远失去！（真的很久！）", "Serein",
                    MessageBoxButtons.OKCancel, MessageBoxIcon.Information
                    ) == 1
                    && !Check())
                {
                    try
                    {
                        if (Items[0].ForeColor == Color.Gray)
                        {
                            File.Delete(PluginPath + "\\" + Items[0].Text + ".lock");

                        }
                        else
                        {
                            File.Delete(PluginPath + "\\" + Items[0].Text);
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
            else if (Items.Count > 1 && !Check())
            {
                if ((int)MessageBox.Show(
                   $"确定删除\"{Items[0].Text}\"等{Items.Count}个文件？\n" +
                   $"他将会永远失去！（真的很久！）", "Serein",
                   MessageBoxButtons.OKCancel, MessageBoxIcon.Information
                   ) == 1
                   && !Check())
                {
                    foreach (ListViewItem Item in Items)
                    {
                        try
                        {
                            if (Item.ForeColor == Color.Gray)
                            {
                                File.Delete(PluginPath + "\\" + Item + ".lock");

                            }
                            else
                            {
                                File.Delete(PluginPath + "\\" + Item);
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
        }
        public static void Disable(ListView.SelectedListViewItemCollection Items)
        {
            if (!Check())
            {
                foreach (ListViewItem Item in Items)
                {
                    try
                    {
                        FileInfo RenamedFile = new FileInfo(PluginPath + "\\" + Item.Text);
                        RenamedFile.MoveTo(PluginPath + "\\" + Item.Text + ".lock");
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
        }
        public static void Enable(ListView.SelectedListViewItemCollection Items)
        {
            foreach (ListViewItem Item in Items)
            {
                try
                {
                    FileInfo RenamedFile = new FileInfo(PluginPath + "\\" + Item.Text + ".lock");
                    RenamedFile.MoveTo(PluginPath + "\\" + Item.Text);
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
        public static bool Check()
        {
            if (Server.Status)
            {
                MessageBox.Show("服务器仍在运行中", "Serein", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return true;
            }
            else
            {
                return false;
            }
        }
    }

}
