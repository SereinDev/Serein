using Wpf.Ui.Controls;

namespace Serein.Windows.Pages.Settings
{
    public partial class Event : UiPage
    {
        public Event()
        {
            InitializeComponent();
            Window.Settings.Event = this;
        }
    }
}
