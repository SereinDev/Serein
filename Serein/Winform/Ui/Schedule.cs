using Serein.Base;
using Serein.Utils;
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
        private void ScheduleList_MouseUp(object sender, MouseEventArgs e)
        {
            IsDragging = false;
            if ((ScheduleList.SelectedItems.Count != 0) && (ItemDraged != null))
            {
                if (ItemDraged.Index != ScheduleList.SelectedItems[0].Index && ItemDraged.Index >= 0)
                {
                    ScheduleList.Items.RemoveAt(ItemDraged.Index);
                    ScheduleList.Items.Insert(ScheduleList.SelectedItems[0].Index, ItemDraged);
                    ItemDraged = null;
                    SaveSchedule();
                }
            }
        }

        private void ScheduleList_ItemDrag(object sender, ItemDragEventArgs e)
        {
            ItemDraged = (ListViewItem)e.Item;
            IsDragging = true;
        }

        private void ScheduleList_ItemMouseHover(object sender, ListViewItemMouseHoverEventArgs e)
        {
            e.Item.Selected = IsDragging;
        }

        private void ScheduleContextMenuStrip_Opening(object sender, CancelEventArgs e)
        {
            ScheduleContextMenuStrip_Edit.Enabled = ScheduleList.SelectedItems.Count > 0;
            ScheduleContextMenuStrip_Delete.Enabled = ScheduleList.SelectedItems.Count > 0;
            ScheduleContextMenuStrip_Enable.Enabled = ScheduleList.SelectedItems.Count > 0 && ScheduleList.SelectedItems[0].ForeColor == Color.Gray;
            ScheduleContextMenuStrip_Disable.Enabled = ScheduleList.SelectedItems.Count > 0 && ScheduleList.SelectedItems[0].ForeColor != Color.Gray;
            ScheduleContextMenuStrip_Clear.Enabled = ScheduleList.Items.Count > 0;
        }

        private void ScheduleContextMenuStrip_Enable_Click(object sender, EventArgs e)
        {
            if (ScheduleList.SelectedItems.Count >= 1)
            {
                ScheduleList.Items[ScheduleList.SelectedIndices[0]].ForeColor = Color.Black;
                ScheduleList.Items[ScheduleList.SelectedIndices[0]].SubItems[1].ForeColor = Color.Black;
                ScheduleList.Items[ScheduleList.SelectedIndices[0]].SubItems[2].ForeColor = Color.Black;
                SaveSchedule();
            }
        }

        private void ScheduleContextMenuStrip_Disable_Click(object sender, EventArgs e)
        {
            if (ScheduleList.SelectedItems.Count >= 1)
            {
                ScheduleList.Items[ScheduleList.SelectedIndices[0]].ForeColor = Color.Gray;
                ScheduleList.Items[ScheduleList.SelectedIndices[0]].SubItems[1].ForeColor = Color.Gray;
                ScheduleList.Items[ScheduleList.SelectedIndices[0]].SubItems[2].ForeColor = Color.Gray;
                SaveSchedule();
            }
        }

        private void ScheduleContextMenuStrip_Add_Click(object sender, EventArgs e)
        {
            ScheduleEditor Editor = new();
            Editor.ShowDialog(this);
            if (Editor.CancelFlag)
            {
                return;
            }
            ListViewItem Item = new(Editor.Cron.Text);
            Item.SubItems.Add(Editor.Remark.Text);
            Item.SubItems.Add(Editor.Command.Text);
            if (ScheduleList.SelectedItems.Count > 0)
            {
                ScheduleList.Items.Insert(ScheduleList.SelectedItems[0].Index + 1, Item);
            }
            else
            {
                ScheduleList.Items.Add(Item);
            }
            SaveSchedule();
        }

        private void ScheduleContextMenuStrip_Edit_Click(object sender, EventArgs e)
        {
            if (ScheduleList.SelectedItems.Count >= 1)
            {
                ScheduleEditor Editor = new();
                Editor.Update(
                    ScheduleList.SelectedItems[0].Text,
                    ScheduleList.SelectedItems[0].SubItems[1].Text,
                    ScheduleList.SelectedItems[0].SubItems[2].Text
                    );
                Editor.ShowDialog(this);
                if (Editor.CancelFlag)
                {
                    return;
                }
                ScheduleList.SelectedItems[0].Text = Editor.Cron.Text;
                ScheduleList.SelectedItems[0].SubItems[1].Text = Editor.Remark.Text;
                ScheduleList.SelectedItems[0].SubItems[2].Text = Editor.Command.Text;
            }
            SaveSchedule();
        }

        private void ScheduleContextMenuStrip_Delete_Click(object sender, EventArgs e)
        {
            if (ScheduleList.SelectedItems.Count > 0)
            {
                int result = (int)MessageBox.Show(
                    "确定删除该任务？\n" +
                    "它将会永远失去！（真的很久！）", "Serein",
                    MessageBoxButtons.OKCancel, MessageBoxIcon.Information
                    );
                if (result == 1)
                {
                    ScheduleList.Items.RemoveAt(ScheduleList.SelectedItems[0].Index);
                    SaveSchedule();
                }
            }
        }

        private void ScheduleContextMenuStrip_Clear_Click(object sender, EventArgs e)
        {
            if (ScheduleList.Items.Count > 0)
            {
                int result = (int)MessageBox.Show(
                    "确定删除所有任务？\n" +
                    "它将会永远失去！（真的很久！）", "Serein",
                    MessageBoxButtons.OKCancel, MessageBoxIcon.Information
                    );
                if (result == 1)
                {
                    ScheduleList.Items.Clear();
                    SaveSchedule();
                }
            }
        }

        private void ScheduleContextMenuStrip_Refresh_Click(object sender, EventArgs e)
        {
            IO.ReadSchedule();
            LoadSchedule();
            SaveSchedule();
        }

        private void SaveSchedule()
        {
            List<Schedule> list = new();
            foreach (ListViewItem listViewItem in ScheduleList.Items)
            {
                list.Add(new()
                {
                    Cron = listViewItem.Text,
                    Remark = listViewItem.SubItems[1].Text,
                    Command = listViewItem.SubItems[2].Text,
                    Enable = listViewItem.ForeColor != Color.Gray
                });
            }
            Global.Schedules = list;
            IO.SaveSchedule();
        }

        public void LoadSchedule()
        {
            ScheduleList.BeginUpdate();
            ScheduleList.Items.Clear();
            foreach (Schedule schedule in Global.Schedules)
            {
                ListViewItem listViewItem = new(schedule.Cron);
                listViewItem.SubItems.Add(schedule.Remark);
                listViewItem.SubItems.Add(schedule.Command);
                if (!schedule.Enable)
                {
                    listViewItem.ForeColor = Color.Gray;
                    listViewItem.SubItems[1].ForeColor = Color.Gray;
                    listViewItem.SubItems[2].ForeColor = Color.Gray;
                }
                ScheduleList.Items.Add(listViewItem);
            }
            ScheduleList.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
            ScheduleList.EndUpdate();
        }

        private void ScheduleContextMenuStrip_Command_Click(object sender, EventArgs e) => Process.Start(new ProcessStartInfo("https://serein.cc/docs/guide/command") { UseShellExecute = true });
        private void ScheduleContextMenuStrip_Variables_Click(object sender, EventArgs e) => Process.Start(new ProcessStartInfo("https://serein.cc/docs/guide/variables") { UseShellExecute = true });
    }
}
