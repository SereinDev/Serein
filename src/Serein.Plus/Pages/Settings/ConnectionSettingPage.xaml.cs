using System;
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
    }

    private void OnPropertyChanged(object? sender, EventArgs e)
    {
        if (IsLoaded)
        {
            _settingProvider.SaveAsyncWithDebounce();
        }
    }
}
