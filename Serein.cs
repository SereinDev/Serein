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

        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
        }

        private void SettingBotSupportedLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://github.com/Mrs4s/go-cqhttp");
        }
    }
}
