using System;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Serein
{
    public partial class Ui : Form
    {
        private delegate void PanelConsoleWebBrowser_Delegate(object[] objects);
        private void PanelConsoleWebBrowser_AppendText(object[] objects)
        {
            _ = PanelConsoleWebBrowser.Document.InvokeScript("AppendText", objects);
        }
        public void PanelConsoleWebBrowser_Invoke(string str)
        {
            object[] objects1 = { str };
            object[] objects2 = { objects1 };
            _ = Invoke((PanelConsoleWebBrowser_Delegate)PanelConsoleWebBrowser_AppendText, objects2);
        }

        private void PanelControlStart_Click(object sender, EventArgs e)
        {
            new Task(() =>
            {
                {
                    Server.Start();
                }
            }).Start();
        }
        private void PanelControlStop_Click(object sender, EventArgs e)
        {
            Server.Stop();
        }
        private void PanelControlRestart_Click(object sender, EventArgs e)
        {
            Server.RestartRequest();
        }
        private void PanelControlKill_Click(object sender, EventArgs e)
        {
            Server.Kill();
        }
        private void PanelConsoleEnter_Click(object sender, EventArgs e)
        {
            Server.InputCommand(PanelConsoleInput.Text);
            PanelConsoleInput.Clear();
            _ = PanelConsoleInput.Focus();
        }
        private void PanelConsoleInput_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.Handled = true;
                Server.InputCommand(PanelConsoleInput.Text);
                PanelConsoleInput.Clear();
            }
            else if (e.KeyCode is Keys.Up or Keys.PageUp)
            {
                if (Server.CommandListIndex > 0)
                {
                    Server.CommandListIndex--;
                }
                if (Server.CommandListIndex >= 0 && Server.CommandListIndex < Server.CommandList.Count)
                {
                    PanelConsoleInput_Update(Server.CommandList[Server.CommandListIndex]);
                }
            }
            else if (e.KeyCode is Keys.Down or Keys.PageDown)
            {
                if (Server.CommandListIndex < Server.CommandList.Count)
                {
                    Server.CommandListIndex++;
                }
                if (Server.CommandListIndex >= 0 && Server.CommandListIndex < Server.CommandList.Count)
                {
                    PanelConsoleInput_Update(Server.CommandList[Server.CommandListIndex]);
                }
                else if (Server.CommandListIndex == Server.CommandList.Count && Server.CommandList.Count != 0)
                {
                    PanelConsoleInput_Update();
                }
            }
        }
        private void PanelConsoleInput_Update(string Text = "")
        {
            PanelConsoleInput.Text = Text;
            _ = PanelConsoleInput.Focus();
            PanelConsoleInput.Select(PanelConsoleInput.TextLength, 0);
            PanelConsoleInput.ScrollToCaret();
        }
        private void PanelConsoleInput_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == Convert.ToChar(13))
            {
                e.Handled = true;
            }
        }
    }
}
