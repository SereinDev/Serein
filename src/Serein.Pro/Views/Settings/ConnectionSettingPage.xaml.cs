using System;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml.Controls;

using Serein.Core;
using Serein.Core.Services.Data;


namespace Serein.Pro.Views.Settings;

public sealed partial class ConnectionSettingPage : Page
{
    private readonly IServiceProvider _services;

    private SettingProvider SettingProvider { get; }

    public ConnectionSettingPage()
    {
        _services = SereinApp.Current!.Services;
        SettingProvider = _services.GetRequiredService<SettingProvider>();

        InitializeComponent();
    }
}
