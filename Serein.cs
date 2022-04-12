using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Serein
{
    public partial class Serein : Form
    {
        public Serein()
        {
            InitializeComponent();
            if (! System.IO.File.Exists(AppDomain.CurrentDomain.BaseDirectory + "console.html"))
            {
                MessageBox.Show("文件  "+ AppDomain.CurrentDomain.BaseDirectory + "console.html  已丢失");
                System.Environment.Exit(0);
             }
            PanelConsoleWebBrowser.Navigate(@"file:\\\"+AppDomain.CurrentDomain.BaseDirectory + "console.html?from=panel");
            BotWebBrowser.Navigate(@"file:\\\"+AppDomain.CurrentDomain.BaseDirectory + "console.html?from=bot");
        }

        private void SettingBotSupportedLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://github.com/Mrs4s/go-cqhttp");
        }

        private void SettingServerPathSelect_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog
            {
                Filter = "支持的文件(*.exe *.bat)|*.exe;*.bat"
            };
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                SettingServerPath.Text = dialog.FileName;
            }
        }

        private void SettingBotPathSelect_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog
            {
                Filter = "go-cqhttp可执行文件|go-cqhttp.exe;go-cqhttp_windows_armv7.exe;go-cqhttp_windows_386.exe;go-cqhttp_windows_arm64.exe;go-cqhttp_windows_amd64.exe"
            };
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                SettingBotPath.Text = dialog.FileName;
            }
        }

        private void SereinIcon_MouseClick(object sender, MouseEventArgs e)
        {
            this.Show();
            this.WindowState = FormWindowState.Normal;
        }
    }
}
