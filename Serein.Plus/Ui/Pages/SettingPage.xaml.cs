using System;
using System.ComponentModel;
using System.Windows.Controls;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using Serein.Core.Models.Settings;
using Serein.Core.Services.Data;

using Page = iNKORE.UI.WPF.Modern.Controls.Page;

namespace Serein.Plus.Ui.Pages;

public partial class SettingPage : Page
{
    private readonly IHost _host;
    private IServiceProvider Services => _host.Services;
    private SettingProvider SettingProvider => Services.GetRequiredService<SettingProvider>();
    public ApplicationSetting ApplicationSetting => SettingProvider.Value.Application;
    public AutoRunSetting AutoRunSetting => SettingProvider.Value.AutoRun;
    public ConnectionSetting ConnectionSetting => SettingProvider.Value.Connection;
    public FunctionSetting FunctionSetting => SettingProvider.Value.Function;
    public PagesSetting PagesSetting => SettingProvider.Value.Pages;
    public ReactionSetting ReactionSetting => SettingProvider.Value.Reaction;
    public ServerSetting ServerSetting => SettingProvider.Value.Server;

    public SettingPage(IHost host)
    {
        InitializeComponent();
        _host = host;
    }

    private void OnPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        SettingProvider.Save();
    }
}
