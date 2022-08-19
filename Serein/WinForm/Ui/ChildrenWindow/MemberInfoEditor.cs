using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace Serein.Ui.ChildrenWindow
{
    public partial class MemberInfoEditor : Form
    {
        public bool CancelFlag = true;
        public MemberInfoEditor(ListViewItem Item)
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

        private void MemberInfoEditer_HelpButtonClicked(object sender, CancelEventArgs e)
        {
            Process.Start(new ProcessStartInfo("https://serein.cc/Tutorial.html#%E6%88%90%E5%91%98%E7%AE%A1%E7%90%86") { UseShellExecute = true });
        }
    }
}
