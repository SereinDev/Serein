using System;
using System.Collections.Generic;
using System.Text;
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
        public string[] areas = { "禁用", "控制台", "消息（群聊）", "消息（私聊）" };
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
            if (RegexList.Items.Count <= 0)
            {
                RegexContextMenuStripClear.Enabled = false;
            }
            else
            {
                RegexContextMenuStripClear.Enabled = true;
            }
        }
        private void RegexContextMenuStripAdd_Click(object sender, EventArgs e)
        {
            RegexEditer regexEditer = new RegexEditer();
            regexEditer.ShowDialog(this);
            SaveRegex();
        }
        private void RegexContextMenuStripEdit_Click(object sender, EventArgs e)
        {
            if (RegexList.SelectedItems.Count <= 0)
            {
                return;
            }
            RegexEditer regexEditer = new RegexEditer(true);
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
            string isAdminText;
            if (regexEditer.Area.SelectedIndex <= 1)
            {
                isAdminText = "-";
            }
            else if (regexEditer.IsAdmin.Checked)
            {
                isAdminText = "是";
            }
            else
            {
                isAdminText = "否";
            }
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
                    "他将会永远失去！（真的很久！）", "Serein",
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
                    "他将会永远失去！（真的很久！）", "Serein",
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
            string isAdminText = "";
            ListViewItem Item = new ListViewItem(regex);
            if (areaIndex <= 1)
            {
                isAdminText = "-";
            }
            else if (isAdmin)
            {
                isAdminText = "是";
            }
            else
            {
                isAdminText = "否";
            }
            Item.SubItems.Add(areas[areaIndex]);
            Item.SubItems.Add(isAdminText);
            Item.SubItems.Add(remark);
            Item.SubItems.Add(command);
            if (RegexList.InvokeRequired)
            {
                Action<ListViewItem> actionDelegate = (x) => { RegexList.Items.Add(Item); };
                PanelInfoPort2.Invoke(actionDelegate, Item);
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
                FileStream TsvFile = new FileStream($"{ Global.Path }\\data\\regex.tsv", FileMode.Open);
                StreamReader Reader = new StreamReader(TsvFile, Encoding.UTF8);
                string Line;
                List<RegexItem> regexItems = new List<RegexItem>();
                while ((Line = Reader.ReadLine()) != null)
                {
                    RegexItem Item = new RegexItem();
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
            List<RegexItem> regexItems = new List<RegexItem>();
            if (!Directory.Exists(Global.Path + "\\data"))
            {
                Directory.CreateDirectory(Global.Path + "\\data");
            }
            StreamWriter RegexWriter = new StreamWriter(
                File.Open(
                    $"{Global.Path}\\data\\regex.tsv",
                    FileMode.Create,
                    FileAccess.Write
                    ),
                Encoding.UTF8
                );
            foreach (ListViewItem item in RegexList.Items)
            {
                RegexItem regexItem = new RegexItem();
                regexItem.Regex = item.Text;
                regexItem.Area = Array.IndexOf(areas, item.SubItems[1].Text);
                regexItem.IsAdmin = item.SubItems[2].Text == "是";
                regexItem.Remark = item.SubItems[3].Text;
                regexItem.Command = item.SubItems[4].Text;
                RegexWriter.WriteLine(regexItem.ConvertToStr());
                regexItems.Add(regexItem);
            }
            Global.RegexItems = regexItems;
            RegexWriter.Flush();
            RegexWriter.Close();
        }
    }

}
