using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using Serein.Gui.ViewModels;
using TriggerBase = Serein.Core.Models.Automations.Triggers.TriggerBase;

namespace Serein.Gui.Windows;

public partial class TriggersEditor : Window
{
    private readonly TriggersEditorViewModel _viewModel;

    public TriggersEditor(ObservableCollection<TriggerBase> triggers)
    {
        InitializeComponent();
        DataContext = _viewModel = new(triggers);
    }

    private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (sender is not ListView listView)
        {
            return;
        }

        _viewModel.UpdateSelection(listView.SelectedItems);
    }
}
