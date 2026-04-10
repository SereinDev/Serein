using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Serein.Core.Models.Network.Web.WebAuthentication;
using Serein.Core.Services.Data;
using Serein.Gui.ViewModels;

namespace Serein.Gui.Windows;

public partial class WebAuthenticationEditor : Window
{
    private readonly WebAuthenticationEditorViewModel _viewModel;

    internal WebAuthenticationEditor(WebAuthenticationProvider webAuthenticationProvider)
    {
        DataContext = _viewModel = new(webAuthenticationProvider);
        InitializeComponent();
    }

    private void WebAuthenticationListView_SelectionChanged(
        object sender,
        SelectionChangedEventArgs e
    )
    {
        if (sender is not ListView listView)
        {
            return;
        }

        _viewModel.UpdateSelection(listView.SelectedItem as AuthenticationBase);
    }

    private void WebAuthenticationListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
    {
        if (sender is not ListView listView || listView.SelectedItem is not AuthenticationBase item)
        {
            return;
        }

        _viewModel.EditCommand.Execute(item);
    }
}
