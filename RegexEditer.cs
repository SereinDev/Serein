using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Serein
{
    public partial class RegexEditer : Form
    {
        public RegexEditer()
        {
            InitializeComponent();
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
                    Global.ui.AddRegex(
                        Area.SelectedIndex,
                        Regex.Text,
                        IsAdmin.Checked,
                        Remark.Text,
                        Command.Text
                        );
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
            }
            else
            {
                IsAdmin.Enabled = true;
            }
        }
    }
}
