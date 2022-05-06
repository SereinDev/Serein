using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using NCrontab;

namespace Serein
{
    public partial class TaskEditer : Form
    {
        public TaskEditer()
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
                if (Serein.Command.GetType(Command.Text) == -1)
                {
                    MessageBox.Show("命令内容无效", "Serein", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                try
                {
                    List<DateTime> Occurrences = CrontabSchedule.Parse(Cron.Text).GetNextOccurrences(DateTime.Now, DateTime.Now.AddYears(1)).ToList();
                    CronNextTime.Text = "下一次执行时间:" + Occurrences[0].ToString();
                    Close();
                    return;
                }
                catch
                {
                    CronNextTime.Text = "下一次执行时间:" + "Error";
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
                List<DateTime> Occurrences = CrontabSchedule.Parse(Cron.Text).GetNextOccurrences(DateTime.Now, DateTime.Now.AddYears(1)).ToList();
                CronNextTime.Text = Occurrences[0].ToString();
            }
            catch
            {
                CronNextTime.Text = "Error";
            }
        }
        public void Update(string CronText,string RemarkText, string CommandText)
        {
            Cron.Text = CronText;
            Command.Text = CommandText;
            Remark.Text = RemarkText;
        }
    }
}
