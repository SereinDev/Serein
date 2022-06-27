using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
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
        public string[] areas = { "禁用", "控制台", "消息（群聊）", "消息（私聊）" , "消息（自身发送）" };

        private void RegexList_ItemDrag(object sender, ItemDragEventArgs e)
        {
            itemDraged = (ListViewItem)e.Item;
            isdrag = true;
        }

        private void RegexList_MouseUp(object sender, MouseEventArgs e)
        {
            isdrag = false;
            if ((RegexList.SelectedItems.Count != 0) && (itemDraged != null))
            {
                if (itemDraged.Index != RegexList.SelectedItems[0].Index)
                {
                    RegexList.Items.RemoveAt(itemDraged.Index);
                    RegexList.Items.Insert(RegexList.SelectedItems[0].Index, itemDraged);
                    SaveRegex();
                    itemDraged = null;
                }
            }
        }

        private void RegexList_ItemMouseHover(object sender, ListViewItemMouseHoverEventArgs e)
        {
            e.Item.Selected = isdrag;
        }
        private void RegexContextMenuStrip_Opening(object sender, CancelEventArgs e)
        {
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
            RegexEditer regexEditer = new RegexEditer();
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
            RegexEditer regexEditer = new RegexEditer();
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
            string isAdminText = regexEditer.Area.SelectedIndex <= 1 || regexEditer.Area.SelectedIndex == 4 ? "-" : regexEditer.IsAdmin.Checked ? "是" : "否";
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
            ListViewItem Item = new ListViewItem(regex);
            isAdminText = areaIndex==4 || areaIndex <= 1 ? "-" : isAdmin ? "是" : "否";
            Item.SubItems.Add(areas[areaIndex]);
            Item.SubItems.Add(isAdminText);
            Item.SubItems.Add(remark);
            Item.SubItems.Add(command);
            if (RegexList.InvokeRequired)
            {
                Action<ListViewItem> actionDelegate = (x) =>
                {
                    RegexList.Items.Add(Item);
                };
                PanelInfoLevel2.Invoke(actionDelegate, Item);
            }
            else
            {
                if (RegexList.SelectedItems.Count > 0)
                {
                    RegexList.Items.Insert(RegexList.SelectedItems[0].Index + 1, Item);
                }
                else
                {
                    RegexList.Items.Add(Item);
                }
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
            RegexList.BeginUpdate();
            RegexList.Items.Clear();
            if (!Directory.Exists(Global.Path + "\\data"))
            {
                Directory.CreateDirectory(Global.Path + "\\data");
            }
            if (File.Exists($"{Global.Path}\\data\\regex.json"))
            {
                StreamReader Reader = new StreamReader(
                    File.Open(
                        $"{Global.Path}\\data\\regex.json",
                        FileMode.Open
                    ),
                    Encoding.UTF8);
                string Text = Reader.ReadToEnd();
                if (!string.IsNullOrEmpty(Text))
                {
                    try
                    {
                        JObject JsonObject = (JObject)JsonConvert.DeserializeObject(Text);
                        if (JsonObject["type"].ToString().ToUpper() != "REGEX")
                        {
                            return;
                        }
                        Global.RegexItems = ((JArray)JsonObject["data"]).ToObject<List<RegexItem>>();
                        foreach (RegexItem Item in Global.RegexItems)
                        {
                            if (Item.CheckItem())
                            {
                                AddRegex(Item.Area, Item.Regex, Item.IsAdmin, Item.Remark, Item.Command);
                            }
                        }
                    }
                    catch { }
                }
                Reader.Close();
                RegexList.EndUpdate();
            }
        }

        public void LoadRegex(string FileName)
        {
            RegexList.BeginUpdate();
            RegexList.Items.Clear();
            if (File.Exists(FileName))
            {
                StreamReader Reader = new StreamReader(
                    File.Open(
                        FileName,
                        FileMode.Open
                    ),
                    Encoding.UTF8
                    );
                if (FileName.ToUpper().EndsWith(".TSV"))
                {
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
                    Global.RegexItems = regexItems;
                }
                else if (FileName.ToUpper().EndsWith(".JSON"))
                {
                    string Text = Reader.ReadToEnd();
                    if (string.IsNullOrEmpty(Text)) { return; }
                    try
                    {
                        JObject JsonObject = (JObject)JsonConvert.DeserializeObject(Text);
                        if (JsonObject["type"].ToString().ToUpper() != "REGEX")
                        {
                            return;
                        }
                        Global.RegexItems = ((JArray)JsonObject["data"]).ToObject<List<RegexItem>>();
                        foreach (RegexItem Item in Global.RegexItems)
                        {
                            if (Item.CheckItem())
                            {
                                AddRegex(Item.Area, Item.Regex, Item.IsAdmin, Item.Remark, Item.Command);
                            }
                        }
                    }
                    catch { }
                }
                Reader.Close();
                RegexList.EndUpdate();
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
                    $"{Global.Path}\\data\\regex.json",
                    FileMode.Create,
                    FileAccess.Write
                    ),
                Encoding.UTF8
                );
            JObject ListJObject = new JObject();
            JArray ListJArray = new JArray();
            foreach (ListViewItem item in RegexList.Items)
            {
                RegexItem regexItem = new RegexItem()
                {
                    Regex = item.Text,
                    Area = Array.IndexOf(areas, item.SubItems[1].Text),
                    IsAdmin = item.SubItems[2].Text == "是",
                    Remark = item.SubItems[3].Text,
                    Command = item.SubItems[4].Text
                };
                regexItems.Add(regexItem);
                JObject ListItemJObject = JObject.FromObject(regexItem);
                ListJArray.Add(ListItemJObject);
            }
            ListJObject.Add("type", "REGEX");
            ListJObject.Add("comment", "非必要请不要直接修改文件，语法错误可能导致数据丢失");
            ListJObject.Add("data", ListJArray);
            RegexWriter.Write(ListJObject.ToString());
            Global.RegexItems = regexItems;
            RegexWriter.Flush();
            RegexWriter.Close();
        }
        private void RegexContextMenuStripVariables_Click(object sender, EventArgs e)
        {
            Process.Start(new ProcessStartInfo("https://zaitonn.github.io/Serein/Variables.html") { UseShellExecute = true });
        }

        private void RegexContextMenuStripCommand_Click(object sender, EventArgs e)
        {
            Process.Start(new ProcessStartInfo("https://zaitonn.github.io/Serein/Command.html") { UseShellExecute = true });
        }
    }
}
