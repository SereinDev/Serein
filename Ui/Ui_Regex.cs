using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace Serein
{
    public partial class Ui : Form
    {
        public string[] areas = { "禁用", "控制台", "消息（群聊）", "消息（私聊）" };
        private void RegexContextMenuStripVariables_Click(object sender, EventArgs e)
        {
            Process.Start("https://zaitonn.github.io/Serein/Variables.html");
        }

        private void RegexContextMenuStripCommand_Click(object sender, EventArgs e)
        {
            Process.Start("https://zaitonn.github.io/Serein/Command.html");
        }
        private void RegexContextMenuStrip_Opening(object sender, CancelEventArgs e)
        {
            SaveRegex();
            if (RegexList.SelectedItems.Count <= 0)
            {
                RegexContextMenuStripEdit.Enabled = false;
                RegexContextMenuStripDelete.Enabled = false;
            }
            else
            {
                RegexContextMenuStripEdit.Enabled = true;
                RegexContextMenuStripDelete.Enabled = true;
            }
            RegexContextMenuStripClear.Enabled = RegexList.Items.Count > 0;
        }
        private void RegexContextMenuStripAdd_Click(object sender, EventArgs e)
        {
            RegexEditer regexEditer = new();
            regexEditer.ShowDialog(this);
            if (regexEditer.CancelFlag)
            {
                return;
            }
            AddRegex(
                    regexEditer.Area.SelectedIndex,
                    regexEditer.RegexTextBox.Text,
                    regexEditer.IsAdmin.Checked,
                    regexEditer.RemarkTextBox.Text,
                    regexEditer.CommandTextBox.Text
                    );
            SaveRegex();
        }
        private void RegexContextMenuStripEdit_Click(object sender, EventArgs e)
        {
            if (RegexList.SelectedItems.Count <= 0)
            {
                return;
            }
            RegexEditer regexEditer = new();
            int index = Array.IndexOf(areas, RegexList.SelectedItems[0].SubItems[1].Text);
            regexEditer.UpdateInfo(
                index,
                RegexList.SelectedItems[0].Text,
                RegexList.SelectedItems[0].SubItems[2].Text == "是",
                RegexList.SelectedItems[0].SubItems[3].Text,
                RegexList.SelectedItems[0].SubItems[4].Text
                );
            regexEditer.ShowDialog(this);
            if (regexEditer.CancelFlag)
            {
                return;
            }
            string isAdminText = regexEditer.Area.SelectedIndex <= 1 ? "-" : regexEditer.IsAdmin.Checked ? "是" : "否";
            RegexList.SelectedItems[0].Text = regexEditer.RegexTextBox.Text;
            RegexList.SelectedItems[0].SubItems[1].Text = areas[regexEditer.Area.SelectedIndex];
            RegexList.SelectedItems[0].SubItems[2].Text = isAdminText;
            RegexList.SelectedItems[0].SubItems[3].Text = regexEditer.RemarkTextBox.Text;
            RegexList.SelectedItems[0].SubItems[4].Text = regexEditer.CommandTextBox.Text;
            SaveRegex();
        }
        private void RegexContextMenuStripClear_Click(object sender, EventArgs e)
        {
            if (RegexList.Items.Count > 0)
            {
                int result = (int)MessageBox.Show(
                    "确定删除所有记录？\n" +
                    "它将会永远失去！（真的很久！）", "Serein",
                    MessageBoxButtons.OKCancel, MessageBoxIcon.Information
                    );
                if (result == 1)
                {
                    RegexList.Items.Clear();
                    SaveRegex();
                }
            }
        }
        private void RegexContextMenuStripDelete_Click(object sender, EventArgs e)
        {
            if (RegexList.SelectedItems.Count > 0)
            {
                int result = (int)MessageBox.Show(
                    "确定删除此行记录？\n" +
                    "它将会永远失去！（真的很久！）", "Serein",
                    MessageBoxButtons.OKCancel, MessageBoxIcon.Information
                    );
                if (result == 1)
                {
                    RegexList.Items.RemoveAt(RegexList.SelectedItems[0].Index);
                    SaveRegex();
                }
            }
        }
        public void AddRegex(int areaIndex, string regex, bool isAdmin, string remark, string command)
        {
            if (
              string.IsNullOrWhiteSpace(regex) || string.IsNullOrEmpty(regex) ||
              string.IsNullOrWhiteSpace(command) || string.IsNullOrEmpty(command)
              )
            {
                return;
            }
            string isAdminText = string.Empty;
            ListViewItem Item = new(regex);
            isAdminText = areaIndex <= 1 ? "-" : isAdmin ? "是" : "否";
            Item.SubItems.Add(areas[areaIndex]);
            Item.SubItems.Add(isAdminText);
            Item.SubItems.Add(remark);
            Item.SubItems.Add(command);
            if (RegexList.InvokeRequired)
            {
                Action<ListViewItem> actionDelegate = (x) => { RegexList.Items.Add(Item); };
                PanelInfoLevel2.Invoke(actionDelegate, Item);
            }
            else
            {
                RegexList.Items.Add(Item);
            }
        }
        private void RegexContextMenuStripRefresh_Click(object sender, EventArgs e)
        {
            RegexList.BeginUpdate();
            LoadRegex();
            SaveRegex();
            RegexList.EndUpdate();
        }
        public void LoadRegex()
        {
            RegexList.Items.Clear();
            if (File.Exists($"{Global.Path}\\data\\regex.tsv"))
            {
                FileStream TsvFile = new($"{Global.Path}\\data\\regex.tsv", FileMode.Open);
                StreamReader Reader = new(TsvFile, Encoding.UTF8);
                string Line;
                List<RegexItem> regexItems = new();
                while ((Line = Reader.ReadLine()) != null)
                {
                    RegexItem Item = new();
                    Item.ConvertToItem(Line);
                    if (!Item.CheckItem())
                    {
                        continue;
                    }
                    AddRegex(Item.Area, Item.Regex, Item.IsAdmin, Item.Remark, Item.Command);
                    regexItems.Add(Item);
                }
                TsvFile.Close();
                Reader.Close();
                Global.RegexItems = regexItems;
            }
        }
        public void LoadRegex(string FileName)
        {
            RegexList.Items.Clear();
            if (File.Exists(FileName))
            {
                FileStream TsvFile = new(FileName, FileMode.Open);
                StreamReader Reader = new(TsvFile, Encoding.UTF8);
                string Line;
                List<RegexItem> regexItems = new();
                while ((Line = Reader.ReadLine()) != null)
                {
                    RegexItem Item = new();
                    Item.ConvertToItem(Line);
                    if (!Item.CheckItem())
                    {
                        continue;
                    }
                    AddRegex(Item.Area, Item.Regex, Item.IsAdmin, Item.Remark, Item.Command);
                    regexItems.Add(Item);
                }
                TsvFile.Close();
                Reader.Close();
                Global.RegexItems = regexItems;
            }
        }
        public void SaveRegex()
        {
            List<RegexItem> regexItems = new();
            if (!Directory.Exists(Global.Path + "\\data"))
            {
                Directory.CreateDirectory(Global.Path + "\\data");
            }
            StreamWriter RegexWriter = new(
                File.Open(
                    $"{Global.Path}\\data\\regex.tsv",
                    FileMode.Create,
                    FileAccess.Write
                    ),
                Encoding.UTF8
                );
            foreach (ListViewItem item in RegexList.Items)
            {
                RegexItem regexItem = new()
                {
                    Regex = item.Text,
                    Area = Array.IndexOf(areas, item.SubItems[1].Text),
                    IsAdmin = item.SubItems[2].Text == "是",
                    Remark = item.SubItems[3].Text,
                    Command = item.SubItems[4].Text
                };
                RegexWriter.WriteLine(regexItem.ConvertToStr());
                regexItems.Add(regexItem);
            }
            Global.RegexItems = regexItems;
            RegexWriter.Flush();
            RegexWriter.Close();
        }
    }

}
