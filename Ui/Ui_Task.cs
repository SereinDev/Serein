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
    partial class Ui : Form
    {
        private void TaskContextMenuStrip_Command_Click(object sender, EventArgs e)
        {
            Process.Start("https://zaitonn.github.io/Serein/Command.html");
        }
        private void TaskContextMenuStrip_Variables_Click(object sender, EventArgs e)
        {
            Process.Start("https://zaitonn.github.io/Serein/Variables.html");
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
            if (TaskList.Items.Count <= 0)
            {
                TaskContextMenuStrip_Clear.Enabled = false;
            }
            else
            {
                TaskContextMenuStrip_Clear.Enabled = true;
            }
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
            TaskList.Items.Add(Item);
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
                    "他将会永远失去！（真的很久！）", "Serein",
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
                    "他将会永远失去！（真的很久！）", "Serein",
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
                Directory.CreateDirectory(Global.Path + "\\data");
            }
            StreamWriter TaskWriter = new StreamWriter(
                File.Open(
                    $"{Global.Path}\\data\\task.tsv",
                    FileMode.Create,
                    FileAccess.Write
                    ),
                Encoding.UTF8
                );
            List<TaskItem> TaskItems = new List<TaskItem>();
            DateTime Now = DateTime.Now;
            foreach (ListViewItem Item in TaskList.Items)
            {
                TaskItem TI = new TaskItem();
                TI.Cron = Item.Text;
                TI.Remark = Item.SubItems[1].Text;
                TI.Command = Item.SubItems[2].Text;
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
                FileStream TsvFile = new FileStream($"{ Global.Path }\\data\\task.tsv", FileMode.Open);
                StreamReader Reader = new StreamReader(TsvFile, Encoding.UTF8);
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
                TsvFile.Close();
                Reader.Close();
                Global.TaskItems = TaskItems;
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
        public void LoadTask(string FileName)
        {
            TaskList.BeginUpdate();
            TaskList.Items.Clear();
            if (File.Exists(FileName))
            {
                FileStream TsvFile = new FileStream(FileName, FileMode.Open);
                StreamReader Reader = new StreamReader(TsvFile, Encoding.UTF8);
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
                TsvFile.Close();
                Reader.Close();
                Global.TaskItems = TaskItems;
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
    }
}
