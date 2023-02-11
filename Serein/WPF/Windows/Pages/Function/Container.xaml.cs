using System.Windows;
using Wpf.Ui.Controls;

namespace Serein.Windows.Pages.Function
{
    public partial class Container : UiPage
    {
        public Container()
        {
            InitializeComponent();
            BotNavigationItem.Visibility = Global.Settings.Serein.PagesDisplayed.Bot ? Visibility.Visible : Visibility.Hidden;
            MemberNavigationItem.Visibility = Global.Settings.Serein.PagesDisplayed.Member ? Visibility.Visible : Visibility.Hidden;
            RegexNavigationItem.Visibility = Global.Settings.Serein.PagesDisplayed.RegexList ? Visibility.Visible : Visibility.Hidden;
            ScheduleNavigationItem.Visibility = Global.Settings.Serein.PagesDisplayed.Schedule ? Visibility.Visible : Visibility.Hidden;
            JSPluginNavigationItem.Visibility = Global.Settings.Serein.PagesDisplayed.JSPlugin ? Visibility.Visible : Visibility.Hidden;
            if (Global.Settings.Serein.PagesDisplayed.Bot)
            {
                Navigation.Navigate(0);
            }
            else if (Global.Settings.Serein.PagesDisplayed.Member)
            {
                Navigation.Navigate(1);
            }
            else if (Global.Settings.Serein.PagesDisplayed.RegexList)
            {
                Navigation.Navigate(3);
            }
            else if (Global.Settings.Serein.PagesDisplayed.Schedule)
            {
                Navigation.Navigate(4);
            }
            else if (Global.Settings.Serein.PagesDisplayed.JSPlugin)
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
