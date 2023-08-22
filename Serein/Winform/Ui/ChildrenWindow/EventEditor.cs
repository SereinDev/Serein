using Serein.Core.Common;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace Serein.Ui.ChildrenWindow
{
    public partial class EventEditor : Form
    {
        public bool CancelFlag { get; private set; }

        public EventEditor()
           => InitializeComponent();

        public EventEditor(string Command)
        {
            InitializeComponent();
            CommandTextBox.Text = Regex.Replace(Command, @"(\n|\r|\\n|\\r)+", "\r\n");
        }

        private void EventEditor_HelpButtonClicked(object sender, CancelEventArgs e)
        {
            Process.Start(new ProcessStartInfo("https://serein.cc/docs/guide/event") { UseShellExecute = true });
        }

        private void Confirm_Click(object sender, EventArgs e)
        {
            if (Regex.IsMatch(CommandTextBox.Text, @"^[\s\n\r]+?$"))
            {
                MessageBox.Show("命令内容为空", "Serein", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else if (Command.GetType(CommandTextBox.Text) == Base.CommandType.Invalid)
            {
                MessageBox.Show("执行命令无效", "Serein", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                CancelFlag = false;
                Close();
            }
        }

        private void Cancel_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
