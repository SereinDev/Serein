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
    public partial class MemberInfoEditer : Form
    {
        public bool CancelFlag = true;
        public MemberInfoEditer(ListViewItem Item)
        {
            InitializeComponent();
            ID.Text += Item.Text;
            NickName.Text += Item.SubItems[2].Text;
            GameIDBox.Text = Item.SubItems[4].Text;
        }

        private void Cancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void Confirm_Click(object sender, EventArgs e)
        {
            if (!Regex.IsMatch(GameIDBox.Text, @"^[a-zA-Z0-9_\s-]{4,16}$"))
            {
                MessageBox.Show(
                    "游戏ID不合法",
                    "Serein",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                    );
            }
            else
            {
                CancelFlag = false;
                Close();
            }
        }
    }
}
