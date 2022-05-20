using System;
using System.Windows.Forms;

namespace Serein
{
    public partial class Debug : Form
    {
        public Debug()
        {
            InitializeComponent();
        }
        public void Append(string NewText)
        {
            if (DebugTextBox .InvokeRequired)
            {
                Action<string> actionDelegate = (Text) => { DebugTextBox.Text += Text+"\n"; };
                DebugTextBox.Invoke(actionDelegate, NewText);
            }
            else
            {
                DebugTextBox.Text += NewText + "\n";
            }
        }
    }
}
