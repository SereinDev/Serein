using NCrontab;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Serein
{
    public partial class Ui : Form
    {
        private void TaskContextMenuStrip_Command_Click(object sender, EventArgs e)
        {
            _ = Process.Start("https://zaitonn.github.io/Serein/Command.html");
        }
        private void TaskContextMenuStrip_Variables_Click(object sender, EventArgs e)
        {
            _ = Process.Start("https://zaitonn.github.io/Serein/Variables.html");
        }
        private void TaskContextMenuStrip_Opening(object sender, CancelEventArgs e)
        {
            SaveRegex();
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
            TaskEditer TE = new();
            _ = TE.ShowDialog();
            if (TE.CancelFlag)
            {
                return;
            }
            ListViewItem Item = new(TE.Cron.Text);
            _ = Item.SubItems.Add(TE.Remark.Text);
            _ = Item.SubItems.Add(TE.Command.Text);
            _ = TaskList.Items.Add(Item);
            SaveTask();
        }
        private void TaskContextMenuStrip_Edit_Click(object sender, EventArgs e)
        {
            if (TaskList.SelectedItems.Count >= 1)
            {
                TaskEditer TE = new();
                TE.Update(
                    TaskList.SelectedItems[0].Text,
                    TaskList.SelectedItems[0].SubItems[1].Text,
                    TaskList.SelectedItems[0].SubItems[2].Text
                    );
                _ = TE.ShowDialog();
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
            SaveTask();
            LoadTask();
        }
        public void SaveTask()
        {
            if (!Directory.Exists(Global.Path + "\\data"))
            {
                _ = Directory.CreateDirectory(Global.Path + "\\data");
            }
            StreamWriter TaskWriter = new(
                File.Open(
                    $"{Global.Path}\\data\\task.tsv",
                    FileMode.Create,
                    FileAccess.Write
                    ),
                Encoding.UTF8
                );
            List<TaskItem> TaskItems = new();
            DateTime Now = DateTime.Now;
            foreach (ListViewItem Item in TaskList.Items)
            {
                TaskItem TI = new()
                {
                    Cron = Item.Text,
                    Remark = Item.SubItems[1].Text,
                    Command = Item.SubItems[2].Text
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
                TaskWriter.WriteLine(TI.ConvertToStr());
            }
            TaskWriter.Flush();
            TaskWriter.Close();
            Global.TaskItems = TaskItems;
        }
        public void LoadTask()
        {
            TaskList.BeginUpdate();
            TaskList.Items.Clear();
            if (File.Exists($"{Global.Path}\\data\\task.tsv"))
            {
                FileStream TsvFile = new($"{Global.Path}\\data\\task.tsv", FileMode.Open);
                StreamReader Reader = new(TsvFile, Encoding.UTF8);
                string Line;
                List<TaskItem> TaskItems = new();
                while ((Line = Reader.ReadLine()) != null)
                {
                    TaskItem Item = new();
                    Item.ConvertToItem(Line);
                    if (!Item.CheckItem())
                    {
                        continue;
                    }
                    TaskItems.Add(Item);
                }
                TsvFile.Close();
                Reader.Close();
                Global.TaskItems = TaskItems;
            }
            foreach (TaskItem Item in Global.TaskItems)
            {
                ListViewItem listViewItem = new(Item.Cron);
                _ = listViewItem.SubItems.Add(Item.Remark);
                _ = listViewItem.SubItems.Add(Item.Command);
                if (!Item.Enable)
                {
                    listViewItem.ForeColor = Color.Gray;
                    listViewItem.SubItems[1].ForeColor = Color.Gray;
                    listViewItem.SubItems[2].ForeColor = Color.Gray;
                }
                _ = TaskList.Items.Add(listViewItem);
            }
            TaskList.EndUpdate();
        }
        public void LoadTask(string FileName)
        {
            TaskList.BeginUpdate();
            TaskList.Items.Clear();
            if (File.Exists(FileName))
            {
                FileStream TsvFile = new(FileName, FileMode.Open);
                StreamReader Reader = new(TsvFile, Encoding.UTF8);
                string Line;
                List<TaskItem> TaskItems = new();
                while ((Line = Reader.ReadLine()) != null)
                {
                    TaskItem Item = new();
                    Item.ConvertToItem(Line);
                    if (!Item.CheckItem())
                    {
                        continue;
                    }
                    TaskItems.Add(Item);
                }
                TsvFile.Close();
                Reader.Close();
                Global.TaskItems = TaskItems;
            }
            foreach (TaskItem Item in Global.TaskItems)
            {
                ListViewItem listViewItem = new(Item.Cron);
                _ = listViewItem.SubItems.Add(Item.Remark);
                _ = listViewItem.SubItems.Add(Item.Command);
                if (!Item.Enable)
                {
                    listViewItem.ForeColor = Color.Gray;
                    listViewItem.SubItems[1].ForeColor = Color.Gray;
                    listViewItem.SubItems[2].ForeColor = Color.Gray;
                }
                _ = TaskList.Items.Add(listViewItem);
            }
            TaskList.EndUpdate();
        }
    }
}
