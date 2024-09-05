using System.Windows.Navigation;

using iNKORE.UI.WPF.Modern.Controls;

using Serein.Core.Utils.Extensions;

namespace Serein.Plus.Dialogs;

public partial class WelcomeDialog : ContentDialog
{
    public WelcomeDialog()
    {
        InitializeComponent();
    }

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
