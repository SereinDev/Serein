using System.Windows.Forms;

namespace Serein.Lite.Ui;

public partial class MainForm : Form
{
    public MainForm()
    {
        InitializeComponent();
        DialogFactory.ShowWelcomeDialog();
    }
}
