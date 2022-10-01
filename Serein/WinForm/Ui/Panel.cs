using Serein.Server;
using System;
using System.Windows.Forms;

namespace Serein.Ui
{
    public partial class Ui : Form
    {
        private delegate void PanelConsoleWebBrowser_Delegate(object[] objects);
        private void PanelConsoleWebBrowser_AppendText(object[] objects) => PanelConsoleWebBrowser.Document.InvokeScript("AppendText", objects);

        public void PanelConsoleWebBrowser_Invoke(string str)
        {
            object[] objects1 = { str };
            object[] objects2 = { objects1 };
            Invoke((PanelConsoleWebBrowser_Delegate)PanelConsoleWebBrowser_AppendText, objects2);
        }

        private void PanelControlStart_Click(object sender, EventArgs e) => ServerManager.Start();
        private void PanelControlStop_Click(object sender, EventArgs e) => ServerManager.Stop();
        private void PanelControlRestart_Click(object sender, EventArgs e) => ServerManager.RestartRequest();
        private void PanelControlKill_Click(object sender, EventArgs e) => ServerManager.Kill();

        private void PanelConsoleEnter_Click(object sender, EventArgs e)
        {
            ServerManager.InputCommand(PanelConsoleInput.Text);
            PanelConsoleInput.Clear();
            PanelConsoleInput.Focus();
        }

        private void PanelConsoleInput_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.Handled = true;
                ServerManager.InputCommand(PanelConsoleInput.Text);
                PanelConsoleInput.Clear();
            }
            else if (e.KeyCode == Keys.Up || e.KeyCode == Keys.PageUp)
            {
                if (ServerManager.CommandListIndex > 0)
                {
                    ServerManager.CommandListIndex--;
                }
                if (ServerManager.CommandListIndex >= 0 && ServerManager.CommandListIndex < ServerManager.CommandList.Count)
                {
                    PanelConsoleInput_Update(ServerManager.CommandList[ServerManager.CommandListIndex]);
                }
            }
            else if (e.KeyCode == Keys.Down || e.KeyCode == Keys.PageDown)
            {
                if (ServerManager.CommandListIndex < ServerManager.CommandList.Count)
                {
                    ServerManager.CommandListIndex++;
                }
                if (ServerManager.CommandListIndex >= 0 && ServerManager.CommandListIndex < ServerManager.CommandList.Count)
                {
                    PanelConsoleInput_Update(ServerManager.CommandList[ServerManager.CommandListIndex]);
                }
                else if (ServerManager.CommandListIndex == ServerManager.CommandList.Count && ServerManager.CommandList.Count != 0)
                {
                    PanelConsoleInput_Update();
                }
            }
        }

        private void PanelConsoleInput_Update(string Text = "")
        {
            PanelConsoleInput.Text = Text;
            PanelConsoleInput.Focus();
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
