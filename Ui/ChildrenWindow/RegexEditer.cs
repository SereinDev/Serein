using System;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace Serein
{
    public partial class RegexEditer : Form
    {
        public bool CancelFlag = true;
        public RegexEditer()
        {
            InitializeComponent();
            Area.SelectedIndex = 0;
        }

        private void Confirm_Click(object sender, EventArgs e)
        {
            if (
                !(string.IsNullOrWhiteSpace(RegexTextBox.Text) || string.IsNullOrEmpty(RegexTextBox.Text) ||
                string.IsNullOrWhiteSpace(CommandTextBox.Text) || string.IsNullOrEmpty(CommandTextBox.Text)
                ))
            {
                if (Command.GetType(CommandTextBox.Text) == -1)
                {
                    MessageBox.Show("执行命令无效", "Serein", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                try
                {
                    Regex m = new Regex(RegexTextBox.Text);
                    m.Match("");
                    CancelFlag = false;
                    Close();
                }
                catch
                {
                    MessageBox.Show("正则表达式无效", "Serein", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            else
            {
                MessageBox.Show("内容为空", "Serein", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        private void Cancel_Click(object sender, EventArgs e)
        {
            Close();
        }
        private void Area_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Area.SelectedIndex <= 1)
            {
                IsAdmin.Enabled = false;
                IsAdmin.Checked = false;
            }
            else
            {
                IsAdmin.Enabled = true;
            }
        }
        public void UpdateInfo(int areaIndex, string regex, bool isAdmin, string remark, string command)
        {
            Area.SelectedIndex = areaIndex;
            RegexTextBox.Text = regex;
            IsAdmin.Checked = isAdmin;
            RemarkTextBox.Text = remark;
            CommandTextBox.Text = command;
            if (Area.SelectedIndex <= 1)
            {
                IsAdmin.Enabled = false;
                IsAdmin.Checked = false;
            }
            else
            {
                IsAdmin.Enabled = true;
            }
        }

        private void Regex_TextChanged(object sender, EventArgs e)
        {
            if (RegexTextBox.Text.Contains("\t"))
            {
                RegexTextBox.Text = RegexTextBox.Text.Replace("\t", "");
            }
        }

        private void Command_TextChanged(object sender, EventArgs e)
        {
            if (CommandTextBox.Text.Contains("\t"))
            {
                CommandTextBox.Text = CommandTextBox.Text.Replace("\t", "");
            }
        }

        private void Remark_TextChanged(object sender, EventArgs e)
        {
            if (RemarkTextBox.Text.Contains("\t"))
            {
                RemarkTextBox.Text = RemarkTextBox.Text.Replace("\t", "");
            }
        }
    }
}
