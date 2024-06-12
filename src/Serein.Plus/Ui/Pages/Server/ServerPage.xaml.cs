using System;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using Page = iNKORE.UI.WPF.Modern.Controls.Page;

namespace Serein.Plus.Ui.Pages.Server;

public partial class ServerPage : Page
{
    private readonly IHost _host;
    private Core.Services.Servers.Server ServerManager =>
        _host.Services.GetRequiredService<Core.Services.Servers.Server>();

    public ServerPage(IHost host)
    {
        _host = host;
        InitializeComponent();
    }
}
