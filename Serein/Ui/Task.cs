using NCrontab;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Serein.Items;
using Serein.Ui.ChildrenWindow;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Serein.Ui
{
    public partial class Ui : Form
    {
        private void TaskList_MouseUp(object sender, MouseEventArgs e)
        {
            isdrag = false;
            if ((TaskList.SelectedItems.Count != 0) && (itemDraged != null))
            {
                if (itemDraged.Index != TaskList.SelectedItems[0].Index && itemDraged.Index >= 0)
                {
                    TaskList.Items.RemoveAt(itemDraged.Index);
                    TaskList.Items.Insert(TaskList.SelectedItems[0].Index, itemDraged);
                    itemDraged = null;
                    SaveTask();
                }
            }
        }
        private void TaskList_ItemDrag(object sender, ItemDragEventArgs e)
        {
            itemDraged = (ListViewItem)e.Item;
            isdrag = true;
        }

        private void TaskList_ItemMouseHover(object sender, ListViewItemMouseHoverEventArgs e)
        {
            e.Item.Selected = isdrag;
        }
        private void TaskContextMenuStrip_Opening(object sender, CancelEventArgs e)
        {
            if (TaskList.SelectedItems.Count <= 0)
            {
                TaskContextMenuStrip_Edit.Enabled = false;
                TaskContextMenuStrip_Delete.Enabled = false;
                TaskContextMenuStrip_Enable.Enabled = false;
                TaskContextMenuStrip_Disable.Enabled = false;
            }
            else
            {
                TaskContextMenuStrip_Edit.Enabled = true;
                TaskContextMenuStrip_Delete.Enabled = true;
                if (TaskList.SelectedItems[0].ForeColor == Color.Gray)
                {
                    TaskContextMenuStrip_Disable.Enabled = false;
                    TaskContextMenuStrip_Enable.Enabled = true;
                }
                else
                {
                    TaskContextMenuStrip_Disable.Enabled = true;
                    TaskContextMenuStrip_Enable.Enabled = false;
                }
            }
            TaskContextMenuStrip_Clear.Enabled = TaskList.Items.Count > 0;
        }
        private void TaskContextMenuStrip_Enable_Click(object sender, EventArgs e)
        {
            if (TaskList.SelectedItems.Count >= 1)
            {
                TaskList.Items[TaskList.SelectedIndices[0]].ForeColor = Color.Black;
                TaskList.Items[TaskList.SelectedIndices[0]].SubItems[1].ForeColor = Color.Black;
                TaskList.Items[TaskList.SelectedIndices[0]].SubItems[2].ForeColor = Color.Black;
                SaveTask();
            }
        }
        private void TaskContextMenuStrip_Disable_Click(object sender, EventArgs e)
        {
            if (TaskList.SelectedItems.Count >= 1)
            {
                TaskList.Items[TaskList.SelectedIndices[0]].ForeColor = Color.Gray;
                TaskList.Items[TaskList.SelectedIndices[0]].SubItems[1].ForeColor = Color.Gray;
                TaskList.Items[TaskList.SelectedIndices[0]].SubItems[2].ForeColor = Color.Gray;
                SaveTask();
            }
        }
        private void TaskContextMenuStrip_Add_Click(object sender, EventArgs e)
        {
            TaskEditer TE = new TaskEditer();
            TE.ShowDialog();
            if (TE.CancelFlag)
            {
                return;
            }
            ListViewItem Item = new ListViewItem(TE.Cron.Text);
            Item.SubItems.Add(TE.Remark.Text);
            Item.SubItems.Add(TE.Command.Text);
            if (TaskList.SelectedItems.Count > 0)
            {
                TaskList.Items.Insert(TaskList.SelectedItems[0].Index + 1, Item);
            }
            else
            {
                TaskList.Items.Add(Item);
            }
            SaveTask();
        }
        private void TaskContextMenuStrip_Edit_Click(object sender, EventArgs e)
        {
            if (TaskList.SelectedItems.Count >= 1)
            {
                TaskEditer TE = new TaskEditer();
                TE.Update(
                    TaskList.SelectedItems[0].Text,
                    TaskList.SelectedItems[0].SubItems[1].Text,
                    TaskList.SelectedItems[0].SubItems[2].Text
                    );
                TE.ShowDialog();
                if (TE.CancelFlag)
                {
                    return;
                }
                TaskList.SelectedItems[0].Text = TE.Cron.Text;
                TaskList.SelectedItems[0].SubItems[1].Text = TE.Remark.Text;
                TaskList.SelectedItems[0].SubItems[2].Text = TE.Command.Text;
            }
            SaveTask();
        }
        private void TaskContextMenuStrip_Delete_Click(object sender, EventArgs e)
        {
            if (TaskList.SelectedItems.Count > 0)
            {
                int result = (int)MessageBox.Show(
                    "确定删除该任务？\n" +
                    "它将会永远失去！（真的很久！）", "Serein",
                    MessageBoxButtons.OKCancel, MessageBoxIcon.Information
                    );
                if (result == 1)
                {
                    TaskList.Items.RemoveAt(TaskList.SelectedItems[0].Index);
                    SaveTask();
                }
            }
        }
        private void TaskContextMenuStrip_Clear_Click(object sender, EventArgs e)
        {
            if (TaskList.Items.Count > 0)
            {
                int result = (int)MessageBox.Show(
                    "确定删除所有任务？\n" +
                    "它将会永远失去！（真的很久！）", "Serein",
                    MessageBoxButtons.OKCancel, MessageBoxIcon.Information
                    );
                if (result == 1)
                {
                    TaskList.Items.Clear();
                    SaveTask();
                }
            }
        }
        private void TaskContextMenuStrip_Refresh_Click(object sender, EventArgs e)
        {
            LoadTask();
            SaveTask();
        }
        private void SaveTask()
        {
            if (!Directory.Exists(Global.Path + "\\data"))
            {
                Directory.CreateDirectory(Global.Path + "\\data");
            }
            StreamWriter TaskWriter = new StreamWriter(
                File.Open(
                    $"{Global.Path}\\data\\task.json",
                    FileMode.Create,
                    FileAccess.Write
                    ),
                Encoding.UTF8
                );
            JObject ListJObject = new JObject();
            JArray ListJArray = new JArray();
            List<TaskItem> TaskItems = new List<TaskItem>();
            DateTime Now = DateTime.Now;
            foreach (ListViewItem Item in TaskList.Items)
            {
                TaskItem TI = new TaskItem()
                {
                    Cron = Item.Text,
                    Remark = Item.SubItems[1].Text,
                    Command = Item.SubItems[2].Text,
                    Enable = Item.ForeColor != Color.Gray
                };
                try
                {
                    List<DateTime> Occurrences = CrontabSchedule.Parse(TI.Cron).GetNextOccurrences(Now, Now.AddYears(1)).ToList();
                    TI.NextTime = Occurrences[0];
                }
                catch
                {
                    continue;
                }
                TI.Enable = Item.ForeColor != Color.Gray;
                TaskItems.Add(TI);
                JObject ListItemJObject = JObject.FromObject(TI);
                ListJArray.Add(ListItemJObject);
            }
            ListJObject.Add("type", "TASK");
            ListJObject.Add("comment", "非必要请不要直接修改文件，语法错误可能导致数据丢失");
            ListJObject.Add("data", ListJArray);
            TaskWriter.Write(ListJObject.ToString());
            TaskWriter.Flush();
            TaskWriter.Close();
            Global.UpdateTaskItems(TaskItems);
        }
        private void LoadTask()
        {
            TaskList.BeginUpdate();
            TaskList.Items.Clear();
            if (File.Exists($"{Global.Path}\\data\\task.json"))
            {
                StreamReader Reader = new StreamReader(
                    File.Open(
                        $"{Global.Path}\\data\\task.json",
                        FileMode.Open
                    ),
                    Encoding.UTF8);
                string Text = Reader.ReadToEnd();
                if (!string.IsNullOrEmpty(Text))
                {
                    try
                    {
                        JObject JsonObject = (JObject)JsonConvert.DeserializeObject(Text);
                        if (JsonObject["type"].ToString().ToUpper() != "TASK")
                        {
                            return;
                        }
                        Global.UpdateTaskItems(((JArray)JsonObject["data"]).ToObject<List<TaskItem>>());
                    }
                    catch { }
                }
                Reader.Close();
            }
            foreach (TaskItem Item in Global.TaskItems)
            {
                ListViewItem listViewItem = new ListViewItem(Item.Cron);
                listViewItem.SubItems.Add(Item.Remark);
                listViewItem.SubItems.Add(Item.Command);
                if (!Item.Enable)
                {
                    listViewItem.ForeColor = Color.Gray;
                    listViewItem.SubItems[1].ForeColor = Color.Gray;
                    listViewItem.SubItems[2].ForeColor = Color.Gray;
                }
                TaskList.Items.Add(listViewItem);
            }
            TaskList.EndUpdate();
        }
        private void LoadTask(string FileName)
        {
            TaskList.BeginUpdate();
            TaskList.Items.Clear();
            if (File.Exists(FileName))
            {
                StreamReader Reader = new StreamReader(
                    File.Open(
                        FileName,
                        FileMode.Open
                    ),
                    Encoding.UTF8);
                if (FileName.ToUpper().EndsWith(".TSV"))
                {
                    string Line;
                    List<TaskItem> TaskItems = new List<TaskItem>();
                    while ((Line = Reader.ReadLine()) != null)
                    {
                        TaskItem Item = new TaskItem();
                        Item.ConvertToItem(Line);
                        if (!Item.CheckItem())
                        {
                            continue;
                        }
                        TaskItems.Add(Item);
                    }
                    Global.TaskItems = TaskItems;
                }
                else if (FileName.ToUpper().EndsWith(".JSON"))
                {
                    string Text = Reader.ReadToEnd();
                    if (string.IsNullOrEmpty(Text)) { return; }
                    try
                    {
                        JObject JsonObject = (JObject)JsonConvert.DeserializeObject(Text);
                        if (JsonObject["type"].ToString().ToUpper() != "TASK")
                        {
                            return;
                        }
                        Global.UpdateTaskItems(((JArray)JsonObject["data"]).ToObject<List<TaskItem>>());
                    }
                    catch { }
                }
                Reader.Close();
                foreach (TaskItem Item in Global.TaskItems)
                {
                    ListViewItem listViewItem = new ListViewItem(Item.Cron);
                    listViewItem.SubItems.Add(Item.Remark);
                    listViewItem.SubItems.Add(Item.Command);
                    if (!Item.Enable)
                    {
                        listViewItem.ForeColor = Color.Gray;
                        listViewItem.SubItems[1].ForeColor = Color.Gray;
                        listViewItem.SubItems[2].ForeColor = Color.Gray;
                    }
                    TaskList.Items.Add(listViewItem);
                }
            }
            TaskList.EndUpdate();
        }

        private void TaskContextMenuStrip_Command_Click(object sender, EventArgs e)
        {
            Process.Start(new ProcessStartInfo("https://zaitonn.github.io/Serein/Command.html") { UseShellExecute = true });
        }
        private void TaskContextMenuStrip_Variables_Click(object sender, EventArgs e)
        {
            Process.Start(new ProcessStartInfo("https://zaitonn.github.io/Serein/Variables.html") { UseShellExecute = true });
        }
    }
}
