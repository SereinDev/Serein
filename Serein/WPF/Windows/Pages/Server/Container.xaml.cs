using System.Windows;
using Wpf.Ui.Controls;

namespace Serein.Windows.Pages.Server
{
    public partial class Container : UiPage
    {
        public Container()
        {
            InitializeComponent();
            PanelNavigationItem.Visibility = Global.Settings.Serein.PagesDisplayed.ServerPanel
                ? Visibility.Visible
                : Visibility.Hidden;
            PluginManagerNavigationItem.Visibility = Global
                .Settings
                .Serein
                .PagesDisplayed
                .ServerPluginManager
                ? Visibility.Visible
                : Visibility.Hidden;
            if (Global.Settings.Serein.PagesDisplayed.ServerPanel)
            {
                Navigation.Navigate(0);
            }
            else if (Global.Settings.Serein.PagesDisplayed.ServerPluginManager)
            {
                Navigation.Navigate(1);
            }
            else
            {
                Navigation.Frame = null;
            }
            Catalog.Server.Container = this;
        }
    }
}
