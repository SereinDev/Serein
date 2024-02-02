using System;
using System.Text.RegularExpressions;
using System.Windows;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using Serein.Core.Services.Data;

using Page = iNKORE.UI.WPF.Modern.Controls.Page;

namespace Serein.Plus.Ui.Pages.Settings;

public partial class NetworkSettingPage : Page
{
    private readonly IHost _host;

    private IServiceProvider Services => _host.Services;
    private SettingProvider SettingProvider => Services.GetRequiredService<SettingProvider>();

    public NetworkSettingPage(IHost host)
    {
        _host = host;
        InitializeComponent();
        DataContext = SettingProvider;
        AccessToken.Password = SettingProvider.Value.Network.AccessToken;
    }

    private void OnPasswordChanged(object? sender, EventArgs e)
    {
        SettingProvider.Value.Network.AccessToken = AccessToken.Password;
        OnPropertyChanged(sender, e);
    }

    private void OnPropertyChanged(object? sender, EventArgs e)
    {
        if (IsLoaded)
            SettingProvider.SaveAsyncWithDebounce();
    }

    private void UseReverseWebSocket_Click(object sender, RoutedEventArgs e)
    {
        SettingProvider.Value.Network.Uri = SettingProvider.Value.Network.UseReverseWebSocket
            ? Regex.Replace(SettingProvider.Value.Network.Uri, @"^ws://", "http://")
            : Regex.Replace(SettingProvider.Value.Network.Uri, @"^http://", "ws://");

        OnPropertyChanged(sender, e);
    }
}
