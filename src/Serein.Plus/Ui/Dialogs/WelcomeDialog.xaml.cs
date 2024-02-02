using System.Diagnostics;
using System.Windows.Navigation;

using iNKORE.UI.WPF.Modern.Controls;

namespace Serein.Plus.Ui.Dialogs;

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
            Process.Start(new ProcessStartInfo(uri.ToString()) { UseShellExecute = true });
            e.Handled = true;
        }
    }
}
