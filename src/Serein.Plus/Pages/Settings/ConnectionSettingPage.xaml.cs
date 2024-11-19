using System;
using System.Collections.Generic;
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
        GroupTextBox.Text = string.Join(';', _settingProvider.Value.Connection.Groups);
        AdministratorsTextBox.Text = string.Join(
            ';',
            _settingProvider.Value.Connection.Administrators
        );
    }

    private void OnPasswordChanged(object? sender, EventArgs e)
    {
        _settingProvider.Value.Connection.AccessToken = AccessToken.Password;
        OnPropertyChanged(sender, e);
    }

    private void OnPropertyChanged(object? sender, EventArgs e)
    {
        if (IsLoaded)
        {
            _settingProvider.SaveAsyncWithDebounce();
        }
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

    private void GroupTextBox_LostFocus(object sender, RoutedEventArgs e)
    {
        var list = new List<long>();
        foreach (
            var id in GroupTextBox.Text.Split(
                ';',
                StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries
            )
        )
        {
            if (long.TryParse(id, out long i))
            {
                list.Add(i);
            }
        }

        _settingProvider.Value.Connection.Groups = [.. list];
        OnPropertyChanged(sender, e);
    }

    private void AdministratorsTextBox_LostFocus(object sender, RoutedEventArgs e)
    {
        var list = new List<long>();
        foreach (
            var id in AdministratorsTextBox.Text.Split(
                ';',
                StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries
            )
        )
        {
            if (long.TryParse(id, out long i))
            {
                list.Add(i);
            }
        }

        _settingProvider.Value.Connection.Administrators = [.. list];
        OnPropertyChanged(sender, e);
    }
}
