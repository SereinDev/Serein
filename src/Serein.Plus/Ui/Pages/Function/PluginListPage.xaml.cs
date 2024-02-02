using iNKORE.UI.WPF.Modern.Controls;

using Microsoft.Extensions.Hosting;

namespace Serein.Plus.Ui.Pages.Function;

public partial class PluginListPage : Page
{
    private readonly IHost _host;

    public PluginListPage(IHost host)
    {
        _host = host;
        InitializeComponent();
    }
}
