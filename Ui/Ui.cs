using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text.RegularExpressions;
using System.Diagnostics;
using System.Windows.Forms;
using System.Threading;
using System.ComponentModel;

namespace Serein
{
    public partial class Ui : Form
    {
        public Ui()
        {
            InitializeComponent();
            Initialize();
            UpdateVersion();
        }

        private void SettingBotSupportedLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("https://github.com/Mrs4s/go-cqhttp");
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

        private void ShowBalloonTip(string text)
        {
            SereinIcon.BalloonTipTitle = "Serein";
            SereinIcon.BalloonTipText = text;
            SereinIcon.ShowBalloonTip(10000);
        }

        private void TaskContextMenuStrip_Add_Click(object sender, EventArgs e)
        {
            TaskList.Items.Add("test");
        }
    }
}
