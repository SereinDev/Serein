using Wpf.Ui.Controls;

namespace Serein.Windows.Pages.Settings
{
    public partial class Serein : UiPage
    {
        public Serein()
        {
            InitializeComponent();
            Window.Settings.Serein = this;
        }
    }
}
