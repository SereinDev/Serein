using System;
using System.IO;
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

        private void Ui_KeyPress(object sender, KeyPressEventArgs e)
        {
            MessageBox.Show($"e.KeyChar{e.KeyChar}");
        }

        private void Ui_KeyDown(object sender, KeyEventArgs e)
        {
            MessageBox.Show($"1");
        }

        private void Ui_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {

            MessageBox.Show($"3");
        }
    }
}

