using System.Windows;
using System.Windows.Controls;
using Wpf.Ui.Appearance;
using Wpf.Ui.Controls;

namespace Serein.Windows.Pages.Settings
{
    public partial class Serein : UiPage
    {
        private new bool Loaded = false;
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
            EnableGetUpdate.IsChecked = Global.Settings.Serein.EnableGetUpdate;
            MaxCacheLines.Value = Global.Settings.Serein.MaxCacheLines;
            Version.Text = "当前版本：" + Global.VERSION;
            BuildInfo.Text =
                $"编译类型：{Global.BuildInfo.Type}\r\n" +
                $"编译时间：{Global.BuildInfo.Time}\r\n" +
                $"编译路径：{Global.BuildInfo.Dir}\r\n" +
                $"系统类型：{Global.BuildInfo.OS}\r\n" +
                $"详细信息：{Global.BuildInfo.Detail}";
        }

        private void EnableGetUpdate_Click(object sender, RoutedEventArgs e)
            => Global.Settings.Serein.EnableGetUpdate = EnableGetUpdate.IsChecked ?? false;

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

        public void UpdateVersion(string Text)
            => Dispatcher.Invoke(() => { Version.Text = "当前版本：" + Global.VERSION + Text; });

        private void MaxCacheLines_TextChanged(object sender, TextChangedEventArgs e)
            => Global.Settings.Serein.MaxCacheLines = Loaded ? (int)MaxCacheLines.Value : Global.Settings.Serein.MaxCacheLines;
    }
}
