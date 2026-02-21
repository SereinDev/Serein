using System.Windows;
using Serein.Gui.ViewModels;
using Page = iNKORE.UI.WPF.Modern.Controls.Page;

namespace Serein.Gui.Pages;

public partial class PluginConsolePage : Page
{
    public PluginConsolePage(PluginConsoleViewModel pluginConsoleViewModel)
    {
        ViewModel = pluginConsoleViewModel;
        DataContext = ViewModel;

        InitializeComponent();
        Console.EnableLogLevelHighlight();
        ViewModel.RequestClearConsole += () => Dispatcher.Invoke(Console.Clear);
    }

    public PluginConsoleViewModel ViewModel { get; }
}
