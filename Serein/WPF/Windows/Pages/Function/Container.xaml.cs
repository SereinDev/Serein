using System.Windows;
using Wpf.Ui.Controls;

namespace Serein.Windows.Pages.Function
{
    public partial class Container : UiPage
    {
        public Container()
        {
            InitializeComponent();
            BotNavigationItem.Visibility = Global.Settings.Serein.Pages.Bot ? Visibility.Visible : Visibility.Hidden;
            MemberNavigationItem.Visibility = Global.Settings.Serein.Pages.Member ? Visibility.Visible : Visibility.Hidden;
            RegexNavigationItem.Visibility = Global.Settings.Serein.Pages.RegexList ? Visibility.Visible : Visibility.Hidden;
            TaskNavigationItem.Visibility = Global.Settings.Serein.Pages.Task ? Visibility.Visible : Visibility.Hidden;
            JSPluginNavigationItem.Visibility = Global.Settings.Serein.Pages.JSPlugin ? Visibility.Visible : Visibility.Hidden;
            if (Global.Settings.Serein.Pages.Bot)
            {
                Navigation.Navigate(0);
            }
            else if (Global.Settings.Serein.Pages.Member)
            {
                Navigation.Navigate(1);
            }
            else if (Global.Settings.Serein.Pages.RegexList)
            {
                Navigation.Navigate(3);
            }
            else if (Global.Settings.Serein.Pages.Task)
            {
                Navigation.Navigate(4);
            }
            else if (Global.Settings.Serein.Pages.JSPlugin)
            {
                Navigation.Navigate(5);
            }
            else
            {
                Navigation.Frame = null;
            }
            Catalog.Function.Container = this;
        }
    }
}
