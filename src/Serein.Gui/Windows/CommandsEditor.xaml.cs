using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using Serein.Core.Models.Commands;
using Serein.Gui.ViewModels;

namespace Serein.Gui.Windows;

public partial class CommandsEditor : Window
{
    private readonly CommandsEditorViewModel _viewModel;

    public CommandsEditor(ObservableCollection<Command> commands)
    {
        InitializeComponent();
        DataContext = _viewModel = new(commands);
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
