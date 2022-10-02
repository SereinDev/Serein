using Wpf.Ui.Controls;

namespace Serein.Windows.Pages.Server
{
    public partial class Container : UiPage
    {
        public Container()
        {
            InitializeComponent();
            Catalog.Server.Container= this;
        }
    }
}
