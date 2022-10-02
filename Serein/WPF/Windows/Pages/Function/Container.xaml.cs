using Wpf.Ui.Controls;

namespace Serein.Windows.Pages.Function
{
    public partial class Container : UiPage
    {
        public Container()
        {
            InitializeComponent();
            Catalog.Function.Container = this;
        }
    }
}
