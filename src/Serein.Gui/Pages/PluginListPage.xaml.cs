using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using Serein.Core.Models.Plugins;
using Serein.Gui.ViewModels;
using Page = iNKORE.UI.WPF.Modern.Controls.Page;

namespace Serein.Gui.Pages;

public partial class PluginListPage : Page
{
    private readonly PluginListPageViewModel _viewModel;

    public PluginListPage(PluginListPageViewModel viewModel)
    {
        _viewModel = viewModel;
        DataContext = _viewModel;

        InitializeComponent();
    }

    private void PluginListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        _viewModel.UpdateSelection(PluginListView.SelectedItem as KeyValuePair<string, IPlugin>?);
    }
}
