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
    }
}

