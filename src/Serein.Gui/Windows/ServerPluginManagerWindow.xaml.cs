using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Serein.Core.Models.Server;
using Serein.Core.Services.Servers;
using Serein.Gui.ViewModels;

namespace Serein.Gui.Windows;

public partial class ServerPluginManagerWindow : Window
{
    public ServerPluginManagerViewModel ViewModel { get; }

    public ServerPluginManagerWindow(Server server)
    {
        ViewModel = new(server);
        DataContext = ViewModel;

        InitializeComponent();
    }

    private void PluginListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        ViewModel.UpdateSelection(PluginListView.SelectedItems.OfType<ServerPlugin>());
    }
}
