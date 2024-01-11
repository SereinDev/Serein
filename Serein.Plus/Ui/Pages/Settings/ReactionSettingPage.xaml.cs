using System;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using Serein.Core.Models.Settings;
using Serein.Core.Services.Data;

using Page = iNKORE.UI.WPF.Modern.Controls.Page;

namespace Serein.Plus.Ui.Pages.Settings;

public partial class ReactionSettingPage : Page
{
    private readonly IHost _host;

    private IServiceProvider Services => _host.Services;
    private SettingProvider SettingProvider => Services.GetRequiredService<SettingProvider>();
    private ReactionSetting ReactionSetting => SettingProvider.Value.Reaction;

    public ReactionSettingPage(IHost host)
    {
        _host = host;
        InitializeComponent();
        DataContext = SettingProvider;
    }

    private void OnPropertyChanged(object? sender, EventArgs e)
    {
        if (IsLoaded)
            SettingProvider.SaveAsyncWithDebounce();
    }
}
