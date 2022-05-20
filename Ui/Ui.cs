using System;
using System.Windows.Forms;

namespace Serein
{
    public partial class Ui : Form
    {
        public Ui()
        {
            MultiOpenCheck();
            InitializeComponent();
            Initialize();
            UpdateVersion();
        }
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

        private void PanelTableLayout_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (!PanelConsoleInput.Focused)
            {
                PanelConsoleInput.Focus();
            }
        }
    }
}

