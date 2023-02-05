using Serein.Core;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace Serein.Ui.ChildrenWindow
{
    public partial class RegexEditor : Form
    {
        public bool CancelFlag { get; private set; } = true;

        public RegexEditor()
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
                if (Command.GetType(CommandTextBox.Text) == Base.CommandType.Invalid)
                {
                    MessageBox.Show("执行命令无效", "Serein", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                try
                {
                    new Regex(RegexTextBox.Text).Match(string.Empty);
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
            if (Area.SelectedIndex <= 1 || Area.SelectedIndex == 4)
            {
                IsAdmin.Enabled = false;
                IsAdmin.Checked = false;
            }
            else
            {
                IsAdmin.Enabled = true;
            }
            if (Area.SelectedIndex == 4)
            {
                MessageBox.Show(
                    "保存前请务必检查这条正则触发的命令是否会导致再次被所触发内容触发，" +
                    "配置错误可能导致机器人刷屏甚至被封号",
                    "Serein",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                    );
            }
        }
        public void UpdateInfo(int areaIndex, string regex, bool isAdmin, string remark, string command)
        {
            Area.SelectedIndex = areaIndex;
            RegexTextBox.Text = regex;
            IsAdmin.Checked = isAdmin;
            RemarkTextBox.Text = remark;
            CommandTextBox.Text = command;
            if (Area.SelectedIndex <= 1 || Area.SelectedIndex == 4)
            {
                IsAdmin.Enabled = false;
                IsAdmin.Checked = false;
            }
            else
            {
                IsAdmin.Enabled = true;
            }
        }

        private void RegexTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                CommandTextBox.Focus();
            }
        }

        private void CommandTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                RemarkTextBox.Focus();
            }
        }
        private void RemarkTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                Confirm.Focus();
                Confirm_Click(this, EventArgs.Empty);
            }
        }

        private void RegexTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == Convert.ToChar(13))
            {
                e.Handled = true;
            }
        }

        private void CommandTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == Convert.ToChar(13))
            {
                e.Handled = true;
            }
        }


        private void RemarkTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == Convert.ToChar(13))
            {
                e.Handled = true;
            }
        }

        private void RegexEditer_HelpButtonClicked(object sender, CancelEventArgs e)
        {
            Process.Start(new ProcessStartInfo("https://serein.cc/#/Function/Regex") { UseShellExecute = true });
        }
    }
}
