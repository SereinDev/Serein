using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace Serein.Ui.ChildrenWindow
{
    public partial class MemberInfoEditor : Form
    {
        public bool CancelFlag { get; private set; } = true;

        public MemberInfoEditor(ListViewItem listViewItem)
        {
            InitializeComponent();
            ID.Text = listViewItem.Text;
            NickName.Text = listViewItem.SubItems[2].Text;
            GameIDBox.Text = listViewItem.SubItems[4].Text;
        }

        private void Cancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void Confirm_Click(object sender, EventArgs e)
        {
            if (
                !Regex.IsMatch(
                    GameIDBox.Text,
                    Global.Settings.Serein.Function.RegexForCheckingGameID
                )
            )
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
            Process.Start(
                new ProcessStartInfo("https://serein.cc/docs/guide/member")
                {
                    UseShellExecute = true
                }
            );
        }
    }
}
