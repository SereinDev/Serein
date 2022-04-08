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
    public partial class main : Form
    {
        public main()
        {
            InitializeComponent();
            if (! System.IO.File.Exists(AppDomain.CurrentDomain.BaseDirectory + "console.html"))
            {
                MessageBox.Show("文件  "+ AppDomain.CurrentDomain.BaseDirectory + "console.html  已丢失");
                System.Environment.Exit(0);
             }
            ConsoleWebBrowser.Navigate(AppDomain.CurrentDomain.BaseDirectory + "console.html");
        }

        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void groupBox3_Enter(object sender, EventArgs e)
        {

        }
    }
}
