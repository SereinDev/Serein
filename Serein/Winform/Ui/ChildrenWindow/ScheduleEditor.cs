using NCrontab;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Windows.Forms;

namespace Serein.Ui.ChildrenWindow
{
    public partial class ScheduleEditor : Form
    {
        public bool CancelFlag { get; private set; } = true;
        public ScheduleEditor()
        {
            InitializeComponent();
        }

        private void Cancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void Confirm_Click(object sender, EventArgs e)
        {
            if (!(string.IsNullOrEmpty(Cron.Text) || string.IsNullOrWhiteSpace(Cron.Text) ||
                string.IsNullOrEmpty(Command.Text) || string.IsNullOrWhiteSpace(Command.Text)))
            {
                if (Core.Common.Command.GetType(Command.Text) == Base.CommandType.Invalid)
                {
                    MessageBox.Show("执行命令无效", "Serein", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                CrontabSchedule crontabSchedule;
                if ((crontabSchedule = CrontabSchedule.TryParse(Cron.Text)) != null)
                {
                    CronNextTime.Text = "下一次执行时间:" + crontabSchedule.GetNextOccurrences(DateTime.Now, DateTime.Now.AddYears(1)).ToList()[0].ToString();
                    CancelFlag = false;
                    Close();
                }
                else
                {
                    CronNextTime.Text = "Cron表达式无效";
                    MessageBox.Show("Cron表达式无效", "Serein", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            else
            {
                MessageBox.Show("内容为空", "Serein", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void Cron_TextChanged(object sender, EventArgs e)
        {
            try
            {
                List<DateTime> occurrences = CrontabSchedule.Parse(Cron.Text).GetNextOccurrences(DateTime.Now, DateTime.Now.AddYears(1)).ToList();
                if (occurrences.Count > 20)
                {
                    occurrences.RemoveRange(20, occurrences.Count - 20);
                }
                _dateTimes = string.Join("\n", occurrences.Select((dateTime) => dateTime.ToString("g")));
                CronNextTime.Text = "预计执行时间：" + occurrences[0].ToString("g");
            }
            catch
            {
                CronNextTime.Text = "Cron表达式无效或超过时间限制";
                _dateTimes = string.Empty;
            }
        }
        public void Update(string cron, string remark, string command)
        {
            Cron.Text = cron;
            Command.Text = command;
            Remark.Text = remark;
        }

        private void Cron_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                Command.Focus();
            }
        }

        private void Remark_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                Confirm.Focus();
                Confirm_Click(this, EventArgs.Empty);
                e.Handled = true;
            }
        }

        private void Command_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                Remark.Focus();
            }
        }

        private void Cron_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == Convert.ToChar(13))
            {
                e.Handled = true;
            }
        }

        private void Command_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == Convert.ToChar(13))
            {
                e.Handled = true;
            }
        }

        private void Remark_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == Convert.ToChar(13))
            {
                e.Handled = true;
            }
        }

        private void CronNextTime_MouseHover(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(_dateTimes))
            {
                return;
            }
            ToolTip toolTip = new();
            toolTip.SetToolTip((Control)sender, $"最近20次执行执行时间：\n{_dateTimes}");
        }


        private void ScheduleEditer_HelpButtonClicked(object sender, CancelEventArgs e)
        {
            Process.Start(new ProcessStartInfo("https://serein.cc/docs/guide/schedule") { UseShellExecute = true });
        }

        private string _dateTimes = string.Empty;
    }
}
