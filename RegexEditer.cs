using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text.RegularExpressions;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Serein
{
    public partial class RegexEditer : Form
    {
        public bool Edit = false;
        public bool CancelFlag = true;
        public RegexEditer(bool edit=false)
        {
            InitializeComponent();
            Edit = edit;
            Area.SelectedIndex = 0;
        }

        private void Confirm_Click(object sender, EventArgs e)
        {
            if(
                !string.IsNullOrWhiteSpace(Regex.Text)||!string.IsNullOrEmpty(Regex.Text)||
                !string.IsNullOrWhiteSpace(Command.Text)||!string.IsNullOrEmpty(Command.Text)
                )
            {
                try
                {
                    Regex m = new Regex(Regex.Text);
                    m.Match("");
                    if (!Edit)
                    {
                        Global.ui.AddRegex(
                            Area.SelectedIndex,
                            Regex.Text,
                            IsAdmin.Checked,
                            Remark.Text,
                            Command.Text
                            );
                    }
                    CancelFlag = false;
                    Close();
                }
                catch
                {
                    MessageBox.Show("正则表达式不合法", "Serein", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
            if(Area.SelectedIndex <= 1)
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
            Regex.Text = regex;
            IsAdmin.Checked = isAdmin;
            Remark.Text = remark;
            Command.Text = command;
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
            if (Regex.Text.Contains("\t"))
            {
                Regex.Text = Regex.Text.Replace("\t", "");
            }
        }

        private void Command_TextChanged(object sender, EventArgs e)
        {
            if (Command.Text.Contains("\t"))
            {
                Command.Text = Command.Text.Replace("\t", "");
            }
        }

        private void Remark_TextChanged(object sender, EventArgs e)
        {
            if (Remark.Text.Contains("\t"))
            {
                Remark.Text = Remark.Text.Replace("\t", "");
            }
        }
    }
}
