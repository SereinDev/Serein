using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Serein.Core;
using Serein.Core.Utils;
using Serein.Core.Utils.Extensions;
using Page = iNKORE.UI.WPF.Modern.Controls.Page;

namespace Serein.Plus.Pages.Settings;

public partial class AboutPage : Page
{
    public AboutPage(SereinApp sereinApp)
    {
        Version = sereinApp.Version.ToString();
        AssemblyInformationalVersion = sereinApp.FullVersion;

        InitializeComponent();
        DataContext = this;
    }

    public string Version { get; }
    public string AssemblyVersion { get; } = Assembly.GetExecutingAssembly().ToString();
    public string? AssemblyInformationalVersion { get; }

    private void OnRequestNavigate(object sender, RequestNavigateEventArgs e)
    {
        var uri = e.Uri;
        if (uri.IsAbsoluteUri && uri.Scheme.Contains("http"))
        {
            uri.OpenInBrowser();
            e.Handled = true;
        }
    }

    private void SettingsCard_Click(object sender, RoutedEventArgs e)
    {
        switch ((sender as Control)?.Tag as string)
        {
            case "Repo":
                UrlConstants.Repository.OpenInBrowser();
                break;
            case "Group":
                UrlConstants.Group.OpenInBrowser();
                break;
            case "Docs":
                UrlConstants.Docs.OpenInBrowser();
                break;
        }
    }
}
