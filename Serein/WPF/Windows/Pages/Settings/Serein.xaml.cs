using System.Windows;
using System.Windows.Controls;
using Wpf.Ui.Appearance;
using Wpf.Ui.Controls;

namespace Serein.Windows.Pages.Settings
{
    public partial class Serein : UiPage
    {
        private readonly new bool Loaded;

        public Serein()
        {
            InitializeComponent();
            Load();
            Loaded = true;
            Catalog.Settings.Serein = this;
        }

        private void Load()
        {
            UseDarkTheme.IsChecked = Global.Settings.Serein.UseDarkTheme;
            AutoUpdate.IsChecked = Global.Settings.Serein.AutoUpdate;
            EnableGetUpdate.IsChecked = Global.Settings.Serein.EnableGetUpdate;
            ThemeFollowSystem.IsChecked = Global.Settings.Serein.ThemeFollowSystem;
            MaxCacheLines.Value = Global.Settings.Serein.MaxCacheLines;
            Version.Text = "当前版本：" + Global.VERSION;
            BuildInfo.Text = Global.BuildInfo.ToString();
        }

        private void EnableGetUpdate_Click(object sender, RoutedEventArgs e)
        {
            Global.Settings.Serein.EnableGetUpdate = EnableGetUpdate.IsChecked ?? false;
            AutoUpdate.IsEnabled = Global.Settings.Serein.EnableGetUpdate;
            AutoUpdate.IsChecked = (AutoUpdate.IsChecked ?? false) ? Global.Settings.Serein.EnableGetUpdate : false;
        }

        private void AutoUpdate_Click(object sender, RoutedEventArgs e)
            => Global.Settings.Serein.AutoUpdate = AutoUpdate.IsChecked ?? false;

        private void ThemeFollowSystem_Click(object sender, RoutedEventArgs e)
        {
            UseDarkTheme.IsChecked = (UseDarkTheme.IsChecked ?? false) && (ThemeFollowSystem.IsChecked ?? false) ? false : UseDarkTheme.IsChecked;
            Global.Settings.Serein.UseDarkTheme = UseDarkTheme.IsChecked ?? false;
            Theme.Apply(Global.Settings.Serein.UseDarkTheme ? ThemeType.Dark : ThemeType.Light);
        }

        private void UseDarkTheme_Click(object sender, RoutedEventArgs e)
        {
            ThemeFollowSystem.IsChecked = (ThemeFollowSystem.IsChecked ?? false) && (UseDarkTheme.IsChecked ?? false) ? false : ThemeFollowSystem.IsChecked;
            Global.Settings.Serein.UseDarkTheme = UseDarkTheme.IsChecked ?? false;
            Theme.Apply(Global.Settings.Serein.UseDarkTheme ? ThemeType.Dark : ThemeType.Light);
        }

        public void UpdateVersion(string text)
            => Dispatcher.Invoke(() => { Version.Text = "当前版本：" + Global.VERSION + text; });

        private void MaxCacheLines_TextChanged(object sender, TextChangedEventArgs e)
            => Global.Settings.Serein.MaxCacheLines = Loaded ? (int)MaxCacheLines.Value : Global.Settings.Serein.MaxCacheLines;
    }
}
