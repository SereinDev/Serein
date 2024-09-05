using System.Reflection;
using System.Windows.Navigation;

using Serein.Core;
using Serein.Core.Utils.Extensions;

using Page = iNKORE.UI.WPF.Modern.Controls.Page;

namespace Serein.Plus.Pages.Settings;

public partial class AboutPage : Page
{
    public AboutPage()
    {
        InitializeComponent();
        DataContext = this;
    }

    public string Version { get; } = SereinApp.Version;
    public string AssemblyVersion { get; } = Assembly.GetExecutingAssembly().ToString();
    public string? AssemblyInformationalVersion { get; } = SereinApp.FullVersion;

    private void OnRequestNavigate(object sender, RequestNavigateEventArgs e)
    {
        var uri = e.Uri;
        if (uri.IsAbsoluteUri && uri.Scheme.Contains("http"))
        {
            uri.OpenInBrowser();
            e.Handled = true;
        }
    }
}
