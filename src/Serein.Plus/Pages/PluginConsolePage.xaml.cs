using iNKORE.UI.WPF.Modern.Controls;

using Microsoft.Extensions.Hosting;

namespace Serein.Plus.Pages;

public partial class PluginConsolePage : Page
{
    private readonly IHost _host;

    public PluginConsolePage(IHost host)
    {
        _host = host;
        InitializeComponent();
    }
}
