using Serein.Base;
using Serein.Items;
using Serein.Ui.ChildrenWindow;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
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
            TaskContextMenuStrip_Edit.Enabled = TaskList.SelectedItems.Count > 0;
            TaskContextMenuStrip_Delete.Enabled = TaskList.SelectedItems.Count > 0;
            TaskContextMenuStrip_Enable.Enabled = TaskList.SelectedItems.Count > 0 && TaskList.SelectedItems[0].ForeColor != Color.Gray;
            TaskContextMenuStrip_Disable.Enabled = TaskList.SelectedItems.Count > 0 && TaskList.SelectedItems[0].ForeColor == Color.Gray;
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
            TaskEditor Editor = new TaskEditor();
            Editor.ShowDialog();
            if (Editor.CancelFlag)
            {
                return;
            }
            ListViewItem Item = new ListViewItem(Editor.Cron.Text);
            Item.SubItems.Add(Editor.Remark.Text);
            Item.SubItems.Add(Editor.Command.Text);
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
                TaskEditor Editor = new TaskEditor();
                Editor.Update(
                    TaskList.SelectedItems[0].Text,
                    TaskList.SelectedItems[0].SubItems[1].Text,
                    TaskList.SelectedItems[0].SubItems[2].Text
                    );
                Editor.ShowDialog();
                if (Editor.CancelFlag)
                {
                    return;
                }
                TaskList.SelectedItems[0].Text = Editor.Cron.Text;
                TaskList.SelectedItems[0].SubItems[1].Text = Editor.Remark.Text;
                TaskList.SelectedItems[0].SubItems[2].Text = Editor.Command.Text;
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
            List<Task> TaskItems = new List<Task>();
            foreach (ListViewItem Item in TaskList.Items)
            {
                Task TI = new Task()
                {
                    Cron = Item.Text,
                    Remark = Item.SubItems[1].Text,
                    Command = Item.SubItems[2].Text,
                    Enable = Item.ForeColor != Color.Gray
                };
                TaskItems.Add(TI);
            }
            Global.UpdateTaskItems(TaskItems);
            Loader.SaveTask();
        }

        private void LoadTask(string FileName = null)
        {
            TaskList.BeginUpdate();
            TaskList.Items.Clear();
            Loader.ReadTask();
            foreach (Task Item in Global.TaskItems)
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

        private void TaskContextMenuStrip_Command_Click(object sender, EventArgs e) => Process.Start(new ProcessStartInfo("https://serein.cc/Command.html") { UseShellExecute = true });
        private void TaskContextMenuStrip_Variables_Click(object sender, EventArgs e) => Process.Start(new ProcessStartInfo("https://serein.cc/Variables.html") { UseShellExecute = true });
    }
}
