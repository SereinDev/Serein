using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text.RegularExpressions;
using System.Diagnostics;
using System.Windows.Forms;
using System.Threading;
using System.ComponentModel;

namespace Serein
{
    partial class Ui:Form
    {
        private void TaskContextMenuStrip_Add_Click(object sender, EventArgs e)
        {
            new TaskEditer().ShowDialog();
        }
        private void TaskContextMenuStrip_Delete_Click(object sender, EventArgs e)
        {

        }
        private void TaskContextMenuStrip_Clear_Click(object sender, EventArgs e)
        {

        }
        private void TaskContextMenuStrip_Refresh_Click(object sender, EventArgs e)
        {

        }
        public void AddTask(string Cron,string Command,string Remark)
        {
            ListViewItem Item = new ListViewItem(Cron);
            Item.SubItems.Add(Command);
            Item.SubItems.Add(Remark);
            TaskList.Items.Add(Item);
        }
    }
}
