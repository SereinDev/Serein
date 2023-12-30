using System;

using iNKORE.UI.WPF.Modern.Controls;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using Serein.Core.Services.Data;

namespace Serein.Plus.Ui.Pages.Function;

public partial class PluginPage : Page
{
    private readonly IHost _host;
    private IServiceProvider Services => _host.Services;

    public PluginPage(IHost host)
    {
        InitializeComponent();
        _host = host;
    }
}
