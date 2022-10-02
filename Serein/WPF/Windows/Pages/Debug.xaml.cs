using System.Windows.Controls;

namespace Serein.Windows.Pages
{
    /// <summary>
    /// Debug.xaml 的交互逻辑
    /// </summary>
    public partial class Debug : Page
    {
        public Debug()
        {
            InitializeComponent();
            Catalog.Debug = this;
        }

        public void AppendText(string Line)
        {
            Dispatcher.Invoke(() =>
            {
                if (DebugTextBox.Text.Split('\n').Length > 50000)
                {
                    DebugTextBox.Text = string.Empty;
                }
                DebugTextBox.Text += Line + "\r\n";
            });
        }
    }
}
