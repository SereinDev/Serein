using Serein.Core.Server;
using System;
using System.Windows.Forms;

namespace Serein.Ui
{
    public partial class Ui : Form
    {
        private delegate void ServerPanelConsoleWebBrowser_Delegate(object[] objects);
        private void ServerPanelConsoleWebBrowser_AppendText(object[] objects) => ServerPanelConsoleWebBrowser.Document.InvokeScript("AppendText", objects);

        public void ServerPanelConsoleWebBrowser_Invoke(string str)
        {
            object[] objects1 = { str };
            object[] objects2 = { objects1 };
            Invoke((ServerPanelConsoleWebBrowser_Delegate)ServerPanelConsoleWebBrowser_AppendText, objects2);
        }

        private void ServerPanelControlStart_Click(object sender, EventArgs e)
        {
            ServerManager.Start();
            UpdateInfo();
        }

        private void ServerPanelControlStop_Click(object sender, EventArgs e) => ServerManager.Stop();

        private void ServerPanelControlRestart_Click(object sender, EventArgs e) => ServerManager.RestartRequest();

        private void ServerPanelControlKill_Click(object sender, EventArgs e)
        {
            ServerManager.Kill();
            UpdateInfo();
        }

        private void ServerPanelConsoleEnter_Click(object sender, EventArgs e)
        {
            ServerManager.InputCommand(ServerPanelConsoleInput.Text);
            ServerPanelConsoleInput.Clear();
            ServerPanelConsoleInput.Focus();
        }

        private void ServerPanelConsoleInput_KeyDown(object sender, KeyEventArgs e)
        {
            e.Handled = true;
            if (e.KeyCode == Keys.Enter)
            {
                ServerManager.InputCommand(ServerPanelConsoleInput.Text);
                ServerPanelConsoleInput.Clear();
            }
            else if (e.KeyCode == Keys.Up || e.KeyCode == Keys.PageUp)
            {
                if (ServerManager.CommandHistoryIndex > 0)
                {
                    ServerManager.CommandHistoryIndex--;
                }
                if (ServerManager.CommandHistoryIndex >= 0 && ServerManager.CommandHistoryIndex < ServerManager.CommandHistory.Count)
                {
                    ServerPanelConsoleInput_Update(ServerManager.CommandHistory[ServerManager.CommandHistoryIndex]);
                }
            }
            else if (e.KeyCode == Keys.Down || e.KeyCode == Keys.PageDown)
            {
                if (ServerManager.CommandHistoryIndex < ServerManager.CommandHistory.Count)
                {
                    ServerManager.CommandHistoryIndex++;
                }
                if (ServerManager.CommandHistoryIndex >= 0 && ServerManager.CommandHistoryIndex < ServerManager.CommandHistory.Count)
                {
                    ServerPanelConsoleInput_Update(ServerManager.CommandHistory[ServerManager.CommandHistoryIndex]);
                }
                else if (ServerManager.CommandHistoryIndex == ServerManager.CommandHistory.Count && ServerManager.CommandHistory.Count != 0)
                {
                    ServerPanelConsoleInput_Update();
                }
            }
            else
            {
                e.Handled = false;
            }
        }

        private void ServerPanelConsoleInput_Update(string Text = "")
        {
            ServerPanelConsoleInput.Text = Text;
            ServerPanelConsoleInput.Focus();
            ServerPanelConsoleInput.Select(ServerPanelConsoleInput.TextLength, 0);
            ServerPanelConsoleInput.ScrollToCaret();
        }

        private void ServerPanelConsoleInput_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == Convert.ToChar(13))
            {
                e.Handled = true;
            }
        }
    }
}
