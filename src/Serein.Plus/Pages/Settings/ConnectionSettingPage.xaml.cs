using System;
using System.Text.RegularExpressions;
using System.Windows;

using Serein.Core.Services.Data;

using Page = iNKORE.UI.WPF.Modern.Controls.Page;

namespace Serein.Plus.Pages.Settings;

public partial class ConnectionSettingPage : Page
{
    private readonly SettingProvider _settingProvider;

    public ConnectionSettingPage(SettingProvider settingProvider)
    {
        _settingProvider = settingProvider;
        DataContext = _settingProvider;

        InitializeComponent();
        AccessToken.Password = _settingProvider.Value.Connection.AccessToken;
    }

    private void OnPasswordChanged(object? sender, EventArgs e)
    {
        _settingProvider.Value.Connection.AccessToken = AccessToken.Password;
        OnPropertyChanged(sender, e);
    }

    private void OnPropertyChanged(object? sender, EventArgs e)
    {
        if (IsLoaded)
            _settingProvider.SaveAsyncWithDebounce();
    }

    private void UseReverseWebSocket_Click(object sender, RoutedEventArgs e)
    {
        _settingProvider.Value.Connection.Uri = _settingProvider
            .Value
            .Connection
            .UseReverseWebSocket
            ? Regex.Replace(_settingProvider.Value.Connection.Uri, @"^ws://", "http://")
            : Regex.Replace(_settingProvider.Value.Connection.Uri, @"^http://", "ws://");

        OnPropertyChanged(sender, e);
    }
}
