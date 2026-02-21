using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using Serein.Core.Models.Permissions;
using Serein.Gui.ViewModels;

namespace Serein.Gui.Pages;

public partial class PermissionGroupPage : Page
{
    private readonly PermissionGroupPageViewModel _viewModel;

    public PermissionGroupPage(PermissionGroupPageViewModel viewModel)
    {
        _viewModel = viewModel;
        DataContext = _viewModel;

        InitializeComponent();
    }

    private void GroupListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        _viewModel.UpdateSelection(
            GroupListView.SelectedItem is KeyValuePair<string, Group> kv ? kv : null
        );
    }
}
