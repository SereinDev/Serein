using System.Windows;
using Wpf.Ui.Appearance;
using Wpf.Ui.Controls;

namespace Serein.Windows.Pages.Settings
{
    public partial class Serein : UiPage
    {
        public Serein()
        {
            InitializeComponent();
            Load();
            Window.Settings.Serein = this;
        }

        private void Load()
        {
            UseDarkTheme.IsChecked = Global.Settings.Serein.UseDarkTheme;
            EnableGetAnnouncement.IsChecked = Global.Settings.Serein.EnableGetAnnouncement;
            EnableGetUpdate.IsChecked = Global.Settings.Serein.EnableGetUpdate;
        }

        private void EnableGetAnnouncement_Click(object sender, RoutedEventArgs e) => Global.Settings.Serein.EnableGetAnnouncement = EnableGetAnnouncement.IsChecked ?? false;
        private void EnableGetUpdate_Click(object sender, RoutedEventArgs e) => Global.Settings.Serein.EnableGetUpdate = EnableGetUpdate.IsChecked ?? false;

        private void ThemeFollowSystem_Click(object sender, RoutedEventArgs e)
        {
            UseDarkTheme.IsChecked = UseDarkTheme.IsChecked ?? false && (ThemeFollowSystem.IsChecked ?? false) ? false : UseDarkTheme.IsChecked;
            Global.Settings.Serein.UseDarkTheme = UseDarkTheme.IsChecked ?? false;
            Theme.Apply(Global.Settings.Serein.UseDarkTheme ? ThemeType.Dark : ThemeType.Light);
        }

        private void UseDarkTheme_Click(object sender, RoutedEventArgs e)
        {
            ThemeFollowSystem.IsChecked = ThemeFollowSystem.IsChecked ?? false && (UseDarkTheme.IsChecked ?? false) ? false : ThemeFollowSystem.IsChecked;
            Global.Settings.Serein.UseDarkTheme = UseDarkTheme.IsChecked ?? false;
            Theme.Apply(Global.Settings.Serein.UseDarkTheme ? ThemeType.Dark : ThemeType.Light);
        }
    }
}
