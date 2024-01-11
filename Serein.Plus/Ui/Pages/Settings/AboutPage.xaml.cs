using System;
using System.Diagnostics;
using System.Windows.Navigation;

using Microsoft.Extensions.Hosting;

using Page = iNKORE.UI.WPF.Modern.Controls.Page;

namespace Serein.Plus.Ui.Pages.Settings;

public partial class AboutPage : Page
{
    private readonly IHost _host;

    private IServiceProvider Services => _host.Services;

    public AboutPage(IHost host)
    {
        _host = host;
        InitializeComponent();
    }

    private void OnRequestNavigate(object sender, RequestNavigateEventArgs e)
    {
        Uri uri = e.Uri;
        if (uri.IsAbsoluteUri && uri.Scheme.Contains("http", StringComparison.OrdinalIgnoreCase))
        {
            Process.Start(new ProcessStartInfo(uri.ToString()) { UseShellExecute = true });
            e.Handled = true;
        }
    }
}
