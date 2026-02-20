using System.Linq;
using System.Windows.Controls;
using Serein.Core.Models.Automations;
using Serein.Gui.ViewModels;
using Page = iNKORE.UI.WPF.Modern.Controls.Page;

namespace Serein.Gui.Pages;

public partial class AutomationPage : Page
{
    private readonly AutomationPageViewModel _viewModel;

    public AutomationPage(AutomationPageViewModel viewModel)
    {
        InitializeComponent();
        DataContext = _viewModel = viewModel;
    }

    private void AutomationTasksListView_SelectionChanged(
        object sender,
        SelectionChangedEventArgs e
    )
    {
        if (sender is not ListView listView)
        {
            return;
        }

        _viewModel.UpdateSelection([.. listView.SelectedItems.OfType<AutomationTask>()]);
    }
}
