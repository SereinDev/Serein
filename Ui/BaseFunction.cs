using System;
using System.Windows.Forms;

namespace Serein
{
    public partial class Ui : Form
    {
        private void SereinIcon_BalloonTipClicked(object sender, EventArgs e)
        {
            Visible = true;
            ShowInTaskbar = true;
            WindowState = FormWindowState.Normal;
            Activate();
        }

        private void SereinIcon_MouseClick(object sender, MouseEventArgs e)
        {
            Visible = true;
            ShowInTaskbar = true;
            WindowState = FormWindowState.Normal;
            Activate();
        }

        private void Serein_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (Server.Status)
            {
                e.Cancel = true;
                Visible = false;
                ShowInTaskbar = false;
                ShowBalloonTip("服务器进程仍在运行中\n(已自动最小化至托盘，点击托盘图标即可复原窗口)");
            }
        }
        public void ShowBalloonTip(string text)
        {
            SereinIcon.BalloonTipTitle = "Serein";
            SereinIcon.BalloonTipText = text;
            SereinIcon.ShowBalloonTip(10000);
        }
        private void Debug_Append(string Text)
        {
            if (Global.Settings_Serein.Debug)
            {
                if (DebugTextBox.InvokeRequired)
                {
                    Action<string> actionDelegate = (_Text) => { DebugTextBox.Text = DebugTextBox.Text + "\n" + _Text; };
                    PanelInfoTime2.Invoke(actionDelegate, Text);
                }
                else
                {
                    DebugTextBox.Text = DebugTextBox.Text + "\n" + Text;
                }

            }
        }
        private void UpdateVersion()
        {
            SettingSereinVersion.Text = $"当前版本：{Global.VERSION}";
            Debug_Append(Global.VERSION);
        }
    }
}
